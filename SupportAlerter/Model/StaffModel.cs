using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PosMain
{
    class StaffModel
    {
        private int _staffno;
        private string _scode;
        private string _sname;
        private string _password;

        public int staffNo
        {
            get { return _staffno; }
            set { _staffno = value; }
        }

        public string staffCode
        {
            get { return _scode; }
            set { _scode = value; }
        }

        public string staffName
        {
            get { return _sname; }
            set { _sname = value; }
        }

        public string password
        {
            get { return _password; }
            set { _password = value; }
        }

        public StaffModel(int staffno, string sname, string password)
        {
            this._staffno = staffno;
            this._sname = sname;
            this._password = password;
        }

        public int Value
        {
            get { return _staffno; }
            set { _staffno = value; }
        }
        public string Name
        {
            get { return _sname; }
            set { _sname = value; }
        }
    }
}
