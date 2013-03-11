﻿using PosMain.Helper;
using PosLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PosMain.Forms;

namespace PosMain
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();

            RegistrySettings.loadValues();


            ReadValues();
            SqlConnection connection = CoreFeature.getInstance().getDataConnection();
            lvAccount.Items.Clear();

            if (connection != null)
            {
                //email accounts
                SqlCommand cmd;
                SqlDataReader rdr;
                cmd = connection.CreateCommand();
                cmd.CommandText = "select name from account order by name";
                cmd.CommandType = CommandType.Text;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lvAccount.Items.Add(rdr.GetString(0));
                }
                cmd.Dispose();
                rdr.Dispose();

                connection.Close();
            }
            cboDatabaseType.SelectedIndex = 0;
        }

        private void ReadValues()
        {
            numEmailCheckInterval.Value = RegistrySettings.emailCheckInterval;
            updateServiceStatus();
            txtHost.Text = RegistrySettings.SqlHost;
            txtDatabase.Text = RegistrySettings.SqlDatabase;
            txtUsername.Text = RegistrySettings.SqlUsername;
            txtPassword.Text = RegistrySettings.SqlPassword;
            cboLogLevel.Text = RegistrySettings.loggingLevel;
        }

        private void updateServiceStatus()
        {
            lblInfoService.Text = ServiceManagement.getServiceStatus();
            if (lblInfoService.Text.Contains("Running"))
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            }
            else if (lblInfoService.Text.Contains("Stopped"))
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            else if (lblInfoService.Text.Contains("not installed"))
            {
                btnStart.Enabled = false;
                btnStop.Enabled = false;
                lblInfoService.Text = lblInfoService.Text + ". Please reinstall application";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveValues();
            RegistrySettings.writeValues();
            RegistrySettings.loadValues();
            CoreFeature.getInstance().resetConnection();
            Close();
        }

        private void SaveValues()
        {

            RegistrySettings.emailCheckInterval = Convert.ToInt32(numEmailCheckInterval.Value);
            RegistrySettings.SqlHost = txtHost.Text;
            RegistrySettings.SqlDatabase = txtDatabase.Text;
            RegistrySettings.SqlUsername = txtUsername.Text;
            RegistrySettings.SqlPassword = txtPassword.Text;
            RegistrySettings.loggingLevel = cboLogLevel.Text;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveValues();
            RegistrySettings.writeValues();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lvAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvAccount.SelectedIndex >= 0)
            {
                panelAccountDetail.Controls.Clear();
                panelAccountDetail.Controls.Add(new EmailAccount(lvAccount.Text, this));

            }
            else
            {
                panelAccountDetail.Controls.Clear();
                panelAccountDetail.Controls.Add(lblAccountInfo);
            }
        }

        private void btnRemoveAccount_Click(object sender, EventArgs e)
        {
            if (lvAccount.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this account?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlConnection connection = CoreFeature.getInstance().getDataConnection();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = "delete from account where name='" + lvAccount.Text + "'";
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    lvAccount.Items.RemoveAt(lvAccount.SelectedIndex);
                    cmd.Dispose();
                    connection.Close();
                   
                }
            }
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            lvAccount.Items.Add("");
            lvAccount.SelectedIndex = lvAccount.Items.Count - 1;
        }

        internal void updateListAccountName(string p)
        {
            lvAccount.Items[lvAccount.SelectedIndex] = p;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            ServiceManagement.startService();
            updateServiceStatus();
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            ServiceManagement.stopService();
            updateServiceStatus();
            btnStart.Enabled = true;
        }

       

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }



        private void btnTestDatabaseConnection_Click(object sender, EventArgs e)
        {
            SaveValues();
            RegistrySettings.writeValues();
            try
            {
                SqlConnection con = new SqlConnection("Server=" + RegistrySettings.SqlHost + ";Database=" + RegistrySettings.SqlDatabase + ";Uid=" + RegistrySettings.SqlUsername + ";Pwd=" + RegistrySettings.SqlPassword);
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    MessageBox.Show("Connection success!");
                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection error : " + ex.Message);
            }
        }

        
    }
}
