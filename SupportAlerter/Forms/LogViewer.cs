using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosMain.Forms
{
    public partial class LogViewer : Form
    {
        public LogViewer()
        {
            InitializeComponent();
            eventLog1.Source = Program.EventLogName;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (eventLog1.Entries.Count > 0)
            {
                dataGridView1.Rows.Clear();
                foreach (System.Diagnostics.EventLogEntry entry in eventLog1.Entries)
                {
                    dataGridView1.Rows.Add(new object[] { entry.Message });
                }
            }
        }
    }
}
