using PosLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using OpenPop.Mime;
using PosLibrary.model;

namespace PosService
{
    public partial class CoreService : ServiceBase
    {
        private Timer timer;
        public CoreService()
        {
            InitializeComponent();
        }

        public void DoStart()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {
            RegistrySettings.loadValues();
            CoreFeature.getInstance().LogActivity(LogLevel.Normal, "The service was started successfully. Will checking email every " + RegistrySettings.emailCheckInterval + " second(s)", EventLogEntryType.Information);
            timer = new Timer(RegistrySettings.emailCheckInterval * 1000);// in seconds
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Start(); // <- important
        }

        protected override void OnStop()
        {
            CoreFeature.getInstance().LogActivity(LogLevel.Normal, "The service was stopped successfully.", EventLogEntryType.Information);
        }

        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            string sql;
            CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Begin timed activity : Logging to email account and processing the rules", EventLogEntryType.Information);
            try
            {   
                //1. LOOP ALL EMAIL ACCOUNTS
                sql = "select name, server,port,use_ssl,username,password,active from account where active=1";
                SqlCommand cmdAccount = new SqlCommand(sql, CoreFeature.getInstance().getDataConnection());
                SqlDataReader rdrAccount = cmdAccount.ExecuteReader();
                List<Account> listAccount = new List<Account>();
                while (rdrAccount.Read())
                {
                    Account emailAccount = new Account(rdrAccount.GetString(rdrAccount.GetOrdinal("name")), rdrAccount.GetString(rdrAccount.GetOrdinal("server")), rdrAccount.GetInt32(rdrAccount.GetOrdinal("port")), rdrAccount.GetString(rdrAccount.GetOrdinal("username")), Cryptho.Decrypt(rdrAccount.GetString(rdrAccount.GetOrdinal("password"))), rdrAccount.GetByte(rdrAccount.GetOrdinal("use_ssl")) == 1, rdrAccount.GetByte(rdrAccount.GetOrdinal("active")) == 1);
                    listAccount.Add(emailAccount);
                    CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Will use active email " + emailAccount.name, EventLogEntryType.Information);
                }
                rdrAccount.Close();
                            
                //2. LOOP ALL NEW MESSAGES
                foreach (Account emailAccount in listAccount)
                {
                    CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Logging in to email account " + emailAccount.name, EventLogEntryType.Information);
                    SqlDataReader rdrInbox = null;
                    sql = "SELECT count(*) FROM inbox i, account a where i.account_name = a.name and a.name=@name";
                    SqlCommand cmdInbox = new SqlCommand(sql, CoreFeature.getInstance().getDataConnection());
                    cmdInbox.Parameters.AddWithValue("name", emailAccount.name);
                    rdrInbox = cmdInbox.ExecuteReader();

                    if (rdrInbox.Read())
                    {
                        if (rdrInbox.GetInt32(0) == 0)
                        {
                            CoreFeature.getInstance().LogActivity(LogLevel.Debug, "No messages in inbox, try to fetch all last 30 days message to test the rule", EventLogEntryType.Information);
                            rdrInbox.Dispose();
                            CoreFeature.getInstance().FetchRecentMessages(emailAccount, true);
                        }
                        else
                        {
                            CoreFeature.getInstance().LogActivity(LogLevel.Debug, "Inbox already consisted of previous fetched message, now only fetch new message", EventLogEntryType.Information);
                            rdrInbox.Dispose();
                            CoreFeature.getInstance().FetchRecentMessages(emailAccount, false);
                        }
                    }
                    rdrInbox.Close();
                    cmdInbox.Dispose();
                }
            }
            catch (Exception ex)
            {
                CoreFeature.getInstance().LogActivity(LogLevel.Debug, "[Internal Application Error] " + ex.Message, EventLogEntryType.Error);
                if(CoreFeature.getInstance().getDataConnection()!=null) CoreFeature.getInstance().getDataConnection().Close();
            }
            timer.Start();
        }

        internal void DoStop()
        {
            Stop();
        }
    }
}
