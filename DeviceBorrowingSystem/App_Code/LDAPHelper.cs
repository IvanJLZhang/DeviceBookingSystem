using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.Data;
using SystemDAO;
using System.DirectoryServices.ActiveDirectory;
//using DataBaseModel;
public class UserProperties
{
    public string cn;
    public string sn;
    public string mail;
    public string telephoneNumber;
    public string sAMAccountName;
}


/// <summary>
/// 
/// </summary>
public class LDAPHelper
{
    private DirectoryEntry _objDirectoryEntry;

    public bool bDirectoryConnected { private set; get; }
    public UserProperties user;
    public LDAPHelper()
    {
        this.bDirectoryConnected = false;
        config_ldap = new authentication_ldap();
        //this.user = new UserProperties();
    }
    ~LDAPHelper()
    {
        closeConnection();
    }
    private authentication_ldap config_ldap;


    #region public methods
    /// <summary>
    /// Login method
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="pwd"></param>
    /// <param name="errMsg"></param>
    /// <param name="isFirstLogin"></param>
    /// <returns></returns>
    public DataRow UserLogin(string uid, string pwd, out string errMsg, bool isFirstLogin = true)
    {
        errMsg = "login failed";
        DataRow userInfo = this.CheckOneUserInDB(uid);
        if (userInfo == null)
        {
            errMsg = "Invalid uid or uname.";
            return null;
        }
        //isFirstLogin = true;
        foreach (KeyValuePair<string, string> ldap_server in config_ldap.ldap_server)
        {
            string uFilterId = uid + config_ldap.ldap_server_dc[ldap_server.Key];
            if (isFirstLogin)
            {
                queryUserProperties(ldap_server.Value, uFilterId, pwd, out errMsg);
                if (this.user != null)
                {
                    DataBaseModel.tbl_Person person = new DataBaseModel.tbl_Person();
                    person.P_ID = uid;

                    person.P_Email = this.user.mail;
                    person.P_ExNumber = this.user.telephoneNumber;
                    person.P_Name = this.user.cn;

                    person.P_Department = userInfo["P_Department"].ToString();
                    person.P_ChargeDepartment = userInfo["P_ChargeDepartment"].ToString();
                    person.P_Role = Convert.ToInt32(userInfo["P_Role"]);
                    person.P_Location = userInfo["P_Location"].ToString();
                    person.P_Activate = Convert.ToBoolean(userInfo["P_Activate"]);
                    int result = personManageFac.UpdatePersonInfo(person);
                    if (result >= 1)
                    {
                        userInfo["P_Email"] = person.P_Email;
                        userInfo["P_ExNumber"] = person.P_ExNumber;
                        userInfo["P_Name"] = person.P_Name;
                        return userInfo;
                    }
                }
            }
            else
            {
                if (this.bind(ldap_server.Value, uid, pwd, out errMsg))
                {
                    return userInfo;
                }
            }
        }
        return null;
    }
    #endregion

    /// <summary>
    /// 打开连接
    /// </summary>
    /// <param name="LADPath">ldap的地址，例如"LDAP://***.***.48.110:389/dc=***,dc=com"</param>
    /// <param name="authUserName">连接用户名，例如"cn=root,dc=***,dc=com"</param>
    /// <param name="authPWD">连接密码</param>
    public bool OpenConnection(string LADPath, string authUserName, string authPWD)
    {    //创建一个连接 
        _objDirectoryEntry = new DirectoryEntry(LADPath, authUserName, authPWD, AuthenticationTypes.None);


        if (null == _objDirectoryEntry)
        {
            return false;
        }
        else if (_objDirectoryEntry.Properties != null && _objDirectoryEntry.Properties.Count > 0)
        {
            this.bDirectoryConnected = true;
            return true;
        }
        return false;
    }
    /// <summary>
    /// connection to the ldap server
    /// </summary>
    /// <param name="ldap_server_ip"></param>
    /// <returns></returns>
    private bool connection(string ldap_server_ip)
    {
        if (ldap_server_ip == String.Empty)
            return false;

        try
        {
            string ldap_server = "LDAP://" + ldap_server_ip;
            _objDirectoryEntry = new DirectoryEntry(ldap_server);
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// bind user to ldap server
    /// </summary>
    /// <param name="ldap_server_ip"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="errMsg"></param>
    /// <returns></returns>
    private bool bind(string ldap_server_ip, string userName, string password, out string errMsg)
    {
        errMsg = "OK";
        if (connection(ldap_server_ip))
        {
            _objDirectoryEntry.Username = userName;
            _objDirectoryEntry.Password = password;
            try
            {
                if (_objDirectoryEntry.Properties != null && _objDirectoryEntry.Properties.Count > 0)
                    return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
        }
        return false;
    }
    /// <summary>
    /// query user properties
    /// </summary>
    /// <param name="ldap_server_ip"></param>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="errMsg"></param>
    private void queryUserProperties(string ldap_server_ip, string userName, string password, out string errMsg)
    {
        errMsg = "OK";
        if (bind(ldap_server_ip, userName, password, out errMsg))
        {
            string uid = userName.Substring(0, userName.IndexOf('@'));
            DirectorySearcher searcher = new DirectorySearcher(this._objDirectoryEntry);
            string filter = "(&(sAMAccountName=" + uid + "))";

            searcher.Filter = filter;
            searcher.SearchScope = SearchScope.Subtree;
            try
            {
                var objSearResult = searcher.FindOne();
                if (objSearResult != null && objSearResult.Properties.Count > 0)
                {
                    this.user = new UserProperties();
                    this.user.cn = objSearResult.Properties["cn"][0].ToString();
                    this.user.sn = objSearResult.Properties["sn"][0].ToString();
                    this.user.mail = objSearResult.Properties["mail"][0].ToString();
                    this.user.telephoneNumber = objSearResult.Properties["telephoneNumber"][0].ToString();
                    this.user.sAMAccountName = objSearResult.Properties["sAMAccountName"][0].ToString();
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
        }
    }

    /// <summary>
    /// 检测一个用户和密码是否正确
    /// </summary>
    /// <param name="strLDAPFilter">(|(uid= {0})(cn={0}))</param>
    /// <param name="TestUserID">testuserid</param>
    /// <param name="TestUserPwd">testuserpassword</param>
    /// <param name="ErrorMessage"></param>
    /// <returns></returns>
    public bool CheckUidAndPwd(string TestUserID, string TestUserPwd, ref string ErrorMessage)
    {
        bool blRet = false;
        foreach (var ldap_server in this.config_ldap.ldap_server)
        {
            string site = ldap_server.Key;
            string server = ldap_server.Value;

            string ldapath = CombineLDAPath(server, this.config_ldap.ldap_port, this.config_ldap.ldap_root_dn[site]);
            OpenConnection(ldapath, this.config_ldap.ldap_bind_dn[site], this.config_ldap.ldap_bind_passwd[site]);
            if (!this.bDirectoryConnected)
            {
                return false;
            }

            #region
            try
            {
                //创建一个检索
                DirectorySearcher deSearch = new DirectorySearcher(_objDirectoryEntry);
                deSearch.ReferralChasing = ReferralChasingOption.All;
                //过滤名称是否存在
                string strLDAPFilter = String.Format("(&(sAMAccountName={0}))", TestUserID);
                deSearch.Filter = strLDAPFilter;
                deSearch.SearchScope = SearchScope.Subtree;

                //find the first instance 
                var objSearResult = deSearch.FindOne();
                if (null != objSearResult && !string.IsNullOrEmpty(objSearResult.Path))
                {
                    //获取用户名路径对应的用户uid
                    int pos = objSearResult.Path.LastIndexOf('/');
                    string uid = objSearResult.Path.Remove(0, pos + 1);
                    DirectoryEntry objUserEntry = new DirectoryEntry(objSearResult.Path, uid, TestUserPwd, AuthenticationTypes.None);
                    if (null != objUserEntry && objUserEntry.Properties.Count > 0)
                    {
                        this.user = new UserProperties();
                        this.user.cn = objSearResult.Properties["cn"][0].ToString();
                        this.user.sn = objSearResult.Properties["sn"][0].ToString();
                        this.user.mail = objSearResult.Properties["mail"][0].ToString();
                        blRet = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "" + ex.Message;
            }
            finally
            {

                closeConnection();
            }
            #endregion
        }

        return blRet;
    }


    string CombineLDAPath(string server, int port, string root_dn)
    {
        //ldap的地址，例如"LDAP://***.***.48.110:389/dc=***,dc=com"
        string LDAPath = String.Empty;
        //LDAPath += "LDAP://" + server;
        LDAPath += "GC://" + server;
        LDAPath += ":" + port + "/";
        LDAPath += root_dn;
        return LDAPath;
    }

    /// <summary>
    /// 关闭连接
    /// </summary>
    public void closeConnection()
    {
        if (null != _objDirectoryEntry)
        {
            _objDirectoryEntry.Close();
            this.bDirectoryConnected = false;
        }
    }

    public bool SearchUser(string TestUserID, ref string ErrorMessage)
    {
        bool blRet = false;
        foreach (var ldap_server in this.config_ldap.ldap_server)
        {
            string site = ldap_server.Key;
            string server = ldap_server.Value;

            string ldapath = CombineLDAPath(server, this.config_ldap.ldap_port, this.config_ldap.ldap_root_dn[site]);
            OpenConnection(ldapath, this.config_ldap.ldap_bind_dn[site], this.config_ldap.ldap_bind_passwd[site]);
            if (!this.bDirectoryConnected)
            {
                //ErrorMessage = Resources.Resource.msg_user_ldap_login_error;
                return false;
            }

            #region
            try
            {
                //创建一个检索
                DirectorySearcher deSearch = new DirectorySearcher(_objDirectoryEntry);
                //过滤名称是否存在
                string genFilter = "(&(sAMAccountName=" + TestUserID + "))";
                deSearch.Filter = genFilter;
                deSearch.SearchScope = SearchScope.Subtree;

                //find the first instance 
                SearchResult objSearResult = deSearch.FindOne();
                if (null != objSearResult && !string.IsNullOrEmpty(objSearResult.Path))
                {
                    this.user = new UserProperties();
                    this.user.cn = objSearResult.Properties["cn"][0].ToString();
                    this.user.sn = objSearResult.Properties["sn"][0].ToString();
                    this.user.mail = objSearResult.Properties["mail"][0].ToString();
                    this.user.telephoneNumber = objSearResult.Properties["telephoneNumber"][0].ToString();
                    this.user.sAMAccountName = objSearResult.Properties["sAMAccountName"][0].ToString();
                    blRet = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "" + ex.Message;
            }
            finally
            {
                closeConnection();
            }
            #endregion
        }

        return blRet;
    }

    public DataTable CheckUserInDB(string TestUserID)
    {
        string sql = @"select top 1 users.* from tbl_Person as users 
where (users.P_ID = @userid 
or users.P_Name = @userid) 
and P_Activate = 1 ";
        DataTableCollection result = null;
        try
        {
            result = SqlHelperEx.GetTableText(sql, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@userid", TestUserID) });
            return result[0];
        }
        catch
        {
            return null;
        }
    }
    public DataRow CheckOneUserInDB(string TestUserID)
    {
        string sql = @"select top 1 users.* from tbl_Person as users 
where (users.P_ID = @userid 
or users.P_Name = @userid) 
and P_Activate = 1 ";
        DataTableCollection result = null;
        try
        {
            result = SqlHelperEx.GetTableText(sql, new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@userid", TestUserID) });
            return result[0].Rows[0];
        }
        catch
        {
            return null;
        }
    }
}