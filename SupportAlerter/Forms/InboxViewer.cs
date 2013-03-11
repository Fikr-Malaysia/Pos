using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using PosLibrary;

namespace PosMain.Forms
{
    public partial class InboxViewer : Form
    {
        public InboxViewer()
        {
            InitializeComponent();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            String sql = "select * from inbox_plain";
            SqlCommand cmd = new SqlCommand(sql, CoreFeature.getInstance().getDataConnection());
            SqlDataReader rdr = cmd.ExecuteReader();
            dataGridView1.Rows.Clear();
            int i = 0;
            while (rdr.Read())
            {
                DataGridViewButtonColumn buttonBody = new DataGridViewButtonColumn();
                buttonBody.Text = "View";
                dataGridView1.Rows.Add(new object[] { ++i, rdr.GetString(rdr.GetOrdinal("date")), rdr.GetString(rdr.GetOrdinal("sender")), rdr.GetString(rdr.GetOrdinal("sender_ip")), rdr.GetString(rdr.GetOrdinal("to")), rdr.GetString(rdr.GetOrdinal("subject")), "View", rdr.GetInt32(rdr.GetOrdinal("idinbox"))});
            }
            rdr.Close();
            cmd.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                String sql = "select body from inbox where idinbox=@idinbox";
                SqlCommand cmd = new SqlCommand(sql, CoreFeature.getInstance().getDataConnection());
                cmd.Parameters.Add(new SqlParameter("idinbox", dataGridView1.Rows[e.RowIndex].Cells[7].Value));
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    MessageBox.Show(rdr.GetString(rdr.GetOrdinal("body")));
                }
                rdr.Close();
                cmd.Dispose();

            }
        }
    }
}
