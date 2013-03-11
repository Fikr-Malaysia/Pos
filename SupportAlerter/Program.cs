using OpenPop.Pop3;
using PosMain.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using PosLibrary;

namespace PosMain
{
    public class Program : Form
    {
        public const string EventLogName = "Pos Fikr-Malaysia";
        public static LoginDialog loginDialog;

        [STAThread]
        static void Main()
        {
            RegistrySettings.getInstance(); //prepare registry
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            loginDialog = new LoginDialog();
            loginDialog.Show(); 
            
            Application.Run(new Program());
            
            
        }

        
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        internal static StaffModel loginUser;

        public Program()
        {
            

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("&Settings", OnSettings);
            trayMenu.MenuItems[0].DefaultItem = true;
            trayMenu.MenuItems.Add("&Log viewer", OnLogViewer);
            trayMenu.MenuItems.Add("&Show inbox", OnShowInbox);
            trayMenu.MenuItems.Add("&About", OnAbout);
            trayMenu.MenuItems.Add("-");
            trayMenu.MenuItems.Add("E&xit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "eMail to MSSQL";
            trayIcon.Icon = new Icon("icon/sms-32.ico");
            trayIcon.DoubleClick += new EventHandler(OnSettings); 
            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnSettings(object sender, EventArgs e)
        {
            Settings frmSettings = new Settings();
            frmSettings.ShowDialog();
            frmSettings.Dispose();
        }

        private void OnAbout(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
            about.Dispose();
        }

        private void OnLogViewer(object sender, EventArgs e)
        {
            LogViewer logViewer = new LogViewer();
            logViewer.ShowDialog();
            logViewer.Close();
            logViewer.Dispose();
        }

        private void OnShowInbox(object sender, EventArgs e)
        {
            InboxViewer inboxViewer = new InboxViewer();
            inboxViewer.ShowDialog();
            inboxViewer.Close();
            inboxViewer.Dispose();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
