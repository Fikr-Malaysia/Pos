using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosLibrary.model;
using OpenPop.Mime;
using System.Text.RegularExpressions;

namespace PosLibrary
{
    public class CoreFeature
    {
        private readonly Pop3Client pop3Client = new Pop3Client();
        private SqlConnection  dataConnection = null;
        private static CoreFeature instance = null;
        
        private CoreFeature()
        {
            RegistrySettings.loadValues();
            resetConnection();
        }

        public void resetConnection()
        {
            if (dataConnection != null)
            {
                if (dataConnection.State == ConnectionState.Open) dataConnection.Close();
                dataConnection.Dispose();
            }
            //Data Source=(localdb)\v11.0;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False
            dataConnection = new SqlConnection("Server=" + RegistrySettings.SqlHost + ";Database=" + RegistrySettings.SqlDatabase + ";Uid=" + RegistrySettings.SqlUsername + ";Pwd=" + RegistrySettings.SqlPassword);
        }

        public static CoreFeature getInstance()
        {
            if (instance == null)
            {
                instance = new CoreFeature();
            }
            return instance;
        }

        public Pop3Client getPop3Client()
        {
            return pop3Client;
        }

        public SqlConnection getDataConnection()
        {
            if (dataConnection.State == ConnectionState.Closed)
            {
                try
                {
                    dataConnection.Open();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Database connection error : " + ex.Message);
                    LogActivity(LogLevel.Normal, "Database connection error : " + ex.Message,EventLogEntryType.Error);
                    return null;
                }
            }
            return dataConnection;
        }

        public bool Connect(string name, string server, int port, bool use_ssl, string username, string password)
        {
            try
            {
                if (pop3Client.Connected) pop3Client.Disconnect();
                pop3Client.Connect(server, port, use_ssl);
                pop3Client.Authenticate(username, password);
                return true;
            }
            catch (InvalidLoginException)
            {
                LogActivity(LogLevel.Debug, "[POP3 Server Authentication] for " + name + ". The server did not accept the user credentials!",  EventLogEntryType.FailureAudit);
            }
            catch (PopServerNotFoundException)
            {
                LogActivity(LogLevel.Debug, "[POP3 Retrieval] for " + name + ". The server could not be found", EventLogEntryType.FailureAudit);
            }
            catch (PopServerLockedException)
            {
                LogActivity(LogLevel.Debug, "[POP3 Account Locked] for " + name + ". The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?", EventLogEntryType.FailureAudit);
            }
            catch (LoginDelayException)
            {
                LogActivity(LogLevel.Debug, "[POP3 Account Login Delay] for " + name + ". Login not allowed. Server enforces delay between logins. Have you connected recently?", EventLogEntryType.FailureAudit);
            }
            catch (Exception e)
            {
                LogActivity(LogLevel.Debug, "[POP3 Retrieval] for " + name + ". Error occurred retrieving mail. " + e.Message, EventLogEntryType.FailureAudit);
            }
            return false;
        }

        //focusing first on gMail account using recent: in username
        public void FetchRecentMessages(Account emailAccount, bool isFetchLast30days)
        {
            SqlConnection connection = null;
            SqlCommand cmd = null;
            string emailUsername = null;

            if (isFetchLast30days)
            {
                if(emailAccount.server.Contains("gmail.com"))
                    emailUsername = "recent:" + emailAccount.username;
                else
                    emailUsername = emailAccount.username;
                CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Fetching *last 30 days* message", EventLogEntryType.Information);
            }
            else
            {
                emailUsername = emailAccount.username;
                CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Fetching *new* message", EventLogEntryType.Information);
            }

            if (PosLibrary.CoreFeature.getInstance().Connect(emailAccount.name, emailAccount.server, emailAccount.port, emailAccount.use_ssl, emailUsername, emailAccount.password))
            {
                int count = PosLibrary.CoreFeature.getInstance().getPop3Client().GetMessageCount();
                for (int i = 1; i <= count; i++)
                {
                    //Regards to : http://hpop.sourceforge.net/exampleSpecificParts.php
                    OpenPop.Mime.Message message = PosLibrary.CoreFeature.getInstance().getPop3Client().GetMessage(i);
                    MessagePart messagePart = message.FindFirstPlainTextVersion();
                    if (messagePart == null) messagePart = message.FindFirstHtmlVersion();
                    string messageBody = null;
                    if (messagePart != null) messageBody = messagePart.GetBodyAsText();

                    messageBody = Regex.Replace(messageBody, "<.*?>", string.Empty);
                    //save to appropriate inbox
                    connection = CoreFeature.getInstance().getDataConnection();
                    string sql = "insert into inbox(account_name,sender,subject,body,date, sender_ip,[to]) values (@account_name,@sender,@subject,@body,@date,@sender_ip,@to)";
                    cmd = new SqlCommand(sql, connection);
                    cmd.Parameters.Add(new SqlParameter("account_name", emailAccount.name.ToString()));
                    cmd.Parameters.Add(new SqlParameter("sender", message.Headers.From.ToString()));
                    cmd.Parameters.Add(new SqlParameter("subject", message.Headers.Subject.ToString()));
                    cmd.Parameters.Add(new SqlParameter("body", messageBody.ToString()));
                    cmd.Parameters.Add(new SqlParameter("date", message.Headers.Date.ToString()));
                    cmd.Parameters.Add(new SqlParameter("sender_ip", message.Headers.Received[message.Headers.Received.Count - 1].Raw.ToString()));
                    cmd.Parameters.Add(new SqlParameter("to", message.Headers.To[message.Headers.To.Count-1].ToString()));
                    try
                    {
                        int rowAffected = cmd.ExecuteNonQuery();
                        CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Inserting email inbox from " + message.Headers.From + ", subject=" + message.Headers.Subject + ", body=" + messageBody, EventLogEntryType.Information);
                    }
                    catch (Exception ex)
                    {
                        CoreFeature.getInstance().LogActivity(LogLevel.Debug, "[Internal Application Error] FetchRecentMessages " + ex.Message, EventLogEntryType.Information);
                    }                    
                    cmd.Dispose();
                    connection.Close();
                } 

                // delete if there any message received from the server
                if (count > 0 && !emailAccount.server.Contains("gmail.com"))
                {
                    pop3Client.DeleteAllMessages();
                    pop3Client.Disconnect();
                }
            }
            else
            {
                CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Unable to login to your email", EventLogEntryType.Information);
            }
        }

        public void LogActivity(LogLevel logLevel, string message, EventLogEntryType eventLogEntryType)
        {
            if (RegistrySettings.loggingLevel.Equals("None")) return;
            if (logLevel == LogLevel.Debug && RegistrySettings.loggingLevel.Equals("Debug"))
            {
                EventLog.WriteEntry(Program.EventLogName, message, eventLogEntryType);
            }
            else if (logLevel == LogLevel.Normal && RegistrySettings.loggingLevel.Equals("Normal"))
            {
                EventLog.WriteEntry(Program.EventLogName, message, eventLogEntryType);
            }
         }
    }

    public enum LogLevel
    {
        Normal,
        Debug
    }
}
