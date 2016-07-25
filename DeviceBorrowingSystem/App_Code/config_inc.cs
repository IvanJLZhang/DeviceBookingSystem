using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;




public enum Category
{
    Device = 1, Equipment, Chamber
}

public enum Status
{
    Usable = 1, Broken, Lost, ReturnToCustomer, NotForCirculation
}


public enum BorrowStatus
{
    Borrow_Out = 0, OK

}

public enum RecordStatus
{
    REJECT = -1, NEW_NOSUBMIT, NEW_SUBMIT, APPROVE_NORETURN, RETURN
}

public enum WarningLevel
{
    NORMAL, WARNING, OVERDU
}

public enum ProjectStageStatus
{
    ACTIVATE, NON_ACTIVATE
}

public enum UserRole
{
    USER = 0, REVIEWER = 10, LEADER = 11, ADMIN = 20
}

public enum ViewType
{
    VIEW, EDIT, DETELE, ADD, NULL
}

public enum RegularType
{
    NONE = -1, EveryDay = 0,
    MONDAY,
    TUESDAY,
    WEDNESDAY,
    THURSDAY,
    FRIDAY,
    SATURDAY,
    SUNDAY
}

public struct ReturnResult
{
    public string returnMsg;
    public object returnCode;
}

public struct RecordFilter
{

    public Category category;

    public bool deviceFilter;
    public string d_site;
    public string d_department;

    public bool recordFilter;
    public string r_project;
    public string r_purpose;

    public bool durationFilter;
    public int df_year;
    public DateTime df_start;
    public DateTime df_end;
}
/// <summary>
/// ldap登录参数设置
/// </summary>
public class authentication_ldap
{
    public Dictionary<string, string> ldap_server = new Dictionary<string, string>() { 
        {"WKS", "10.42.22.7" }
       ,{"WHQ", "10.34.1.220"}
       ,{"WIH", "10.37.31.17"}
       ,{"WZS", "10.37.31.19"}
       ,{"WPH", "10.45.45.23"}
       ,{"WMX", "10.49.41.91"}
       ,{"WCZ", "10.82.33.21"}
    };

    public Dictionary<string, string> ldap_server_dc = new Dictionary<string, string>()
    {
        {"WKS", "@wkscn.wistron"}
       ,{"WHQ", "@whq.wistron"}
       ,{"WIH", "@wih.wistron"}
       ,{"WZS", "@wzs.wistron"}
       ,{"WPH", "@wph.wistron"}
       ,{"WMX", "@wmx.wistron"}
       ,{"WCZ", "@wcz.wistron"}
    };

    public Dictionary<string, string> ldap_suffix = new Dictionary<string, string>() { 
        {"WKS", "dc=wkscn,dc=wistron"}
       ,{"WHQ", "dc=whq,dc=wistron"}
       ,{"WIH", "dc=wih,dc=wistron"}
       ,{"WZS", "dc=wzs,dc=wistron"}
       ,{"WPH", "dc=wph,dc=wistron"}
       ,{"WMX", "dc=wmx,dc=wistron"}
       ,{"WCZ", "dc=wcz,dc=wistron"}
    };

    public int ldap_port = 389;
    public int ldap_version = 3;

    public Dictionary<string, string> ldap_root_dn = new Dictionary<string, string>() { 
        {"WKS", "dc=wkscn,dc=wistron"},
        {"WHQ", "dc=whq,dc=wistron"}
    };

    public Dictionary<string, string> ldap_bind_dn = new Dictionary<string, string>() { 
        {"WKS", "CN=Ivan JL Zhang,OU=NTSK00,OU=Clients,OU=WKS,DC=wkscn,DC=wistron"},
        {"WHQ", "CN=CSBG Android,OU=Public_Account,DC=whq,DC=wistron"}
    };

    public Dictionary<string, string> ldap_bind_passwd = new Dictionary<string, string>() { 
        {"WKS", "TodayIs4thday"},
        {"WHQ", "G4fAb8Y!"}
    };
}

public struct SiteColorNode
{
    public int ColorIndex;
    public string siteName;
}
public struct ProjectStageColorNode
{
    public int ColorIndex;
    public string pj_StageName;
}


/// <summary>
/// config_inc 的摘要说明
/// </summary>
public class config_inc
{
    public config_inc()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public DataTable GetCategorys()
    {
        DataTable category = new DataTable();
        category.Columns.Add(new DataColumn("category", typeof(String)));
        category.Columns.Add(new DataColumn("value", typeof(Int32)));

        DataRow row = category.NewRow();
        row["category"] = Enum.GetName(typeof(Category), 1);
        row["value"] = 1;
        category.Rows.Add(row);


        row = category.NewRow();
        row["category"] = Enum.GetName(typeof(Category), 2);
        row["value"] = 2;
        category.Rows.Add(row);


        row = category.NewRow();
        row["category"] = Enum.GetName(typeof(Category), 3);
        row["value"] = 3;
        category.Rows.Add(row);

        return category;
    }

    public DataTable GetStatus()
    {
        DataTable status = new DataTable();
        status.Columns.Add(new DataColumn("status", typeof(String)));
        status.Columns.Add(new DataColumn("value", typeof(Int32)));
        for (int index = 1; index <= Enum.GetNames(typeof(Status)).Length; index++)
        {
            DataRow row = status.NewRow();
            row["status"] = Enum.GetName(typeof(Status), index);
            row["value"] = index;
        }

        return status;
    }

    public static int ConvertToCategoryEnum(string cat)
    {
        if (cat == "Equipment")
            return 2;
        else if (cat == "Device")
            return 1;
        else if (cat == "Chamber")
            return 3;
        else
            return 0;
    }
}