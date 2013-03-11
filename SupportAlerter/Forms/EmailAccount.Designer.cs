﻿namespace PosMain
{
    partial class EmailAccount
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.btnSaveTest = new System.Windows.Forms.Button();
            this.chkUseSSL = new System.Windows.Forms.CheckBox();
            this.labelServerAddress = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.chkActive);
            this.panelTop.Controls.Add(this.txtServer);
            this.panelTop.Controls.Add(this.labelPassword);
            this.panelTop.Controls.Add(this.btnSaveTest);
            this.panelTop.Controls.Add(this.chkUseSSL);
            this.panelTop.Controls.Add(this.labelServerAddress);
            this.panelTop.Controls.Add(this.txtPassword);
            this.panelTop.Controls.Add(this.labelServerPort);
            this.panelTop.Controls.Add(this.txtPort);
            this.panelTop.Controls.Add(this.txtName);
            this.panelTop.Controls.Add(this.labelUsername);
            this.panelTop.Controls.Add(this.txtUsername);
            this.panelTop.Controls.Add(this.btnSave);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(518, 418);
            this.panelTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Account name";
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(130, 219);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 14;
            this.chkActive.Text = "&Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(130, 82);
            this.txtServer.Name = "txtServer";
            this.txtServer.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtServer.Size = new System.Drawing.Size(150, 20);
            this.txtServer.TabIndex = 1;
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(66, 193);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(64, 23);
            this.labelPassword.TabIndex = 8;
            this.labelPassword.Text = "Password";
            // 
            // btnSaveTest
            // 
            this.btnSaveTest.Location = new System.Drawing.Point(71, 17);
            this.btnSaveTest.Name = "btnSaveTest";
            this.btnSaveTest.Size = new System.Drawing.Size(96, 23);
            this.btnSaveTest.TabIndex = 6;
            this.btnSaveTest.Text = "Test &Connection";
            this.btnSaveTest.UseVisualStyleBackColor = true;
            this.btnSaveTest.Click += new System.EventHandler(this.btnSaveTest_Click);
            // 
            // chkUseSSL
            // 
            this.chkUseSSL.AutoSize = true;
            this.chkUseSSL.Location = new System.Drawing.Point(130, 139);
            this.chkUseSSL.Name = "chkUseSSL";
            this.chkUseSSL.Size = new System.Drawing.Size(68, 17);
            this.chkUseSSL.TabIndex = 2;
            this.chkUseSSL.Text = "Use SSL";
            this.chkUseSSL.UseVisualStyleBackColor = true;
            // 
            // labelServerAddress
            // 
            this.labelServerAddress.Location = new System.Drawing.Point(18, 82);
            this.labelServerAddress.Name = "labelServerAddress";
            this.labelServerAddress.Size = new System.Drawing.Size(112, 23);
            this.labelServerAddress.TabIndex = 1;
            this.labelServerAddress.Text = "POP Server Address";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(130, 193);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(150, 20);
            this.txtPassword.TabIndex = 4;
            // 
            // labelServerPort
            // 
            this.labelServerPort.Location = new System.Drawing.Point(99, 113);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(31, 23);
            this.labelServerPort.TabIndex = 3;
            this.labelServerPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(130, 111);
            this.txtPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(120, 20);
            this.txtPort.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(130, 56);
            this.txtName.Name = "txtName";
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtName.Size = new System.Drawing.Size(150, 20);
            this.txtName.TabIndex = 0;
            // 
            // labelUsername
            // 
            this.labelUsername.Location = new System.Drawing.Point(66, 162);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(64, 23);
            this.labelUsername.TabIndex = 6;
            this.labelUsername.Text = "Username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(130, 162);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(150, 20);
            this.txtUsername.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(17, 17);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(45, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // EmailAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelTop);
            this.Name = "EmailAccount";
            this.Size = new System.Drawing.Size(518, 418);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown txtPort;
        private System.Windows.Forms.CheckBox chkUseSSL;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.Label labelServerAddress;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnSaveTest;
        private System.Windows.Forms.CheckBox chkActive;
    }
}
