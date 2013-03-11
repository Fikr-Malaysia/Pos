using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using PosLibrary;

namespace PosMain.Forms
{
    public partial class LoginDialog : Form
    {
        List<StaffModel> listStaff;
        bool graceClose = false;

        public LoginDialog()
        {
            InitializeComponent();
            SqlCommand cmd = new SqlCommand("select userid, username from users where [isactive]=1 order by username", CoreFeature.getInstance().getDataConnection());
            SqlDataReader rdr = cmd.ExecuteReader();

            listStaff = new List<StaffModel>();
            AutoCompleteStringCollection autoCompleteUser = new AutoCompleteStringCollection();

            while (rdr.Read())
            {
                listStaff.Add(new StaffModel(rdr.GetInt32(rdr.GetOrdinal("userid")), rdr.GetString(rdr.GetOrdinal("username")), ""));
                autoCompleteUser.Add(rdr.GetString(rdr.GetOrdinal("username")));
            }

            cboUsername.AutoCompleteCustomSource = autoCompleteUser;
            cboUsername.Items.Clear();
            cboUsername.DisplayMember = "Name";
            cboUsername.DataSource = listStaff;
            rdr.Close();
            cmd.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string sql = "select * from [users] where [userid]=@userid and [password]=@password";
            SqlCommand cmd = new SqlCommand(sql, CoreFeature.getInstance().getDataConnection());
            cmd.Parameters.AddWithValue("userid", ((StaffModel)cboUsername.SelectedValue).Value);
            cmd.Parameters.AddWithValue("password", txtPassword.Text);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                if (rdr.GetByte(rdr.GetOrdinal("isactive"))==0)
                {
                    MessageBox.Show("Your account has been deactivated. Please consult your administration regarding this");
                    rdr.Close();
                    cmd.Dispose();
                    return;
                }
                Program.loginUser = new StaffModel(rdr.GetInt32(rdr.GetOrdinal("userid")), rdr.GetString(rdr.GetOrdinal("username")), "");
                rdr.Close();
                cmd.Dispose();
                graceClose = true;
                Close();
            }
            else
            {
                MessageBox.Show("You have supply a wrong credential", Application.ProductName);
                rdr.Close();
                cmd.Dispose();
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            graceClose = false;
            Close();
        }

        private void LoginDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (graceClose) return;
            if (MessageBox.Show("Sure you want to quit?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                graceClose = true;
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
