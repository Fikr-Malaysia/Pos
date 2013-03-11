using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.ModifyRegistry;

namespace PosLibrary
{
    public class RegistrySettings
    {
        private static RegistrySettings instance = null;
        private static ModifyRegistry reg = new ModifyRegistry();
        
        public static string dbType;
        public static bool windowsLogin;
        public static bool serverLogin;
        public static string SqlHost;
        public static string SqlDatabase;
        public static string SqlUsername;
        public static string SqlPassword;
        public static string loggingLevel;

        private RegistrySettings()
        {
            reg.SubKey = "SOFTWARE\\Fikr Malaysia\\Pos";
            loadValues();
        }

        public static RegistrySettings getInstance()
        {
            if (instance == null)
            {
                instance = new RegistrySettings();
            }
            return instance;

        }

        public static void loadValues()
        {
            windowsLogin = ((int)reg.Read("windowsLogin", 1)==1);
            serverLogin = ((int)reg.Read("serverLogin", 1) == 1);
            dbType = (string)reg.Read("dbType", "MSSQL");
            SqlHost = (string) reg.Read("SqlHost", "localhost");
            SqlDatabase = (string)reg.Read("SqlDatabase", "email2sms");
            SqlUsername = (string)reg.Read("SqlUsername", "root");
            SqlPassword = Cryptho.Decrypt((string)reg.Read("SqlPassword", Cryptho.Encrypt("adminadmin")));
            loggingLevel = (string)reg.Read("loggingLevel", "Normal");
        }

        public static void writeValues()
        {
            reg.Write("windowsLogin", Convert.ToInt32(windowsLogin));
            reg.Write("serverLogin", Convert.ToInt32(serverLogin));
            reg.Write("dbType", dbType);
            reg.Write("SqlHost", SqlHost);
            reg.Write("SqlDatabase", SqlDatabase);
            reg.Write("SqlUsername", SqlUsername);
            reg.Write("SqlPassword", Cryptho.Encrypt(SqlPassword));
            reg.Write("loggingLevel", loggingLevel);
        }
    }
}
