using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADDemo
{
    public partial class Form1 : Form
    {
        public class UserProperties
        {
            public string cn;
            public string sn;
            public string mail;
            public string telephoneNumber;
            public string sAMAccountName;
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //LDAPHelper ldap = new LDAPHelper();
            //string errMsg = String.Empty;
            //ldap.CheckUidAndPwd("8902047", "", ref errMsg);
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://10.42.22.7");
            directoryEntry.Username = "K1207A49@wkscn.wistron";
            directoryEntry.Password = "TodayIs4thday1";
            directoryEntry.Close();
            //if (directoryEntry.Properties != null && directoryEntry.Properties.Count > 0)
            //    return;
            DirectorySearcher searcher = new DirectorySearcher(directoryEntry);
            string genFilter = "(&(sAMAccountName=" + "k1207a49" + "))";
            searcher.Filter = genFilter;
            searcher.SearchScope = SearchScope.Subtree;
            //find the first instance 
            SearchResult objSearResult = searcher.FindOne();
        }
    }
}
