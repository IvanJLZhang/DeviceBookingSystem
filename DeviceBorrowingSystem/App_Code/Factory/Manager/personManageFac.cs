using BLL;
using DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZipHelper;

/// <summary>
/// personManageFac 的摘要说明
/// </summary>
public class personManageFac
{
    public personManageFac()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //


    }

    public ExcelRenderNode GenerateTemplateFile(string tempPath)
    {
        int cat = Int32.Parse("1");
        cl_PersonManage deviceManage = new cl_PersonManage();
        DataTable dataTable = deviceManage.GetDeviceTemplateByCategory();
        ExcelRender xlsApp = new ExcelRender();

        xlsApp.CreateSheet("data Element");
        int nStartCol = 0;
        int nStartRow = 0;
        for (int index = 0; index != dataTable.Columns.Count; index++)
        {
            DataColumn column = dataTable.Columns[index];
            xlsApp.SetCellValue(nStartRow, nStartCol, column.Caption, TypeCode.String);
            xlsApp.SetCellWidth(nStartCol, 40);
            xlsApp.SetRowHeight(nStartRow, 15);
            xlsApp.SetCellStyle(nStartRow, nStartCol, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);

            if (column.Caption == "P_ID")
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Job NO", TypeCode.String);
            }
            else if (column.Caption == "P_Name")
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "User Name", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "P_Location", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Site(WKS/WHC/WCH)", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "P_Department", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Department", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "P_Role", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "0=>Normal User, \n10=>Reviewer", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "P_ExNumber", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "ExNumber", TypeCode.String);
            }
            else
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, column.Caption, TypeCode.String);
            }
            xlsApp.SetCellWidth(nStartCol, 40);
            xlsApp.SetRowHeight(nStartRow + 1, 30);
            xlsApp.SetCellStyle(nStartRow + 1, nStartCol, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);


            nStartCol++;
        }
        nStartRow += 2;
        DataRow row = dataTable.Rows[0];
        int col = 0;
        //string ticksTemp = GlobalClassNamespace.GlobalClass.GetMD5Str(DateTime.Now.Ticks.ToString());
        //Directory.CreateDirectory(tempPath + ticksTemp);
        foreach (DataColumn column in dataTable.Columns)
        {
            if (column.DataType.Name == "DateTime")
            {
                xlsApp.SetCellValue(nStartRow, col, DateTime.Parse(row[column].ToString()).ToString("yyyy/MM/dd HH:mm"), TypeCode.String);
            }
            else if (column.DataType.Name == "Decimal")
            {
                string value = String.Format("{0:0.0}", row[column]);
                xlsApp.SetCellValue(nStartRow, col, value, TypeCode.String);
            }
            else
            {
                xlsApp.SetCellValue(nStartRow, col, row[column].ToString(), TypeCode.String);
            }
            col++;
        }
        nStartRow++;
        MemoryStream ms = new MemoryStream();
        xlsApp._MyWorkbook.Write(ms);

        ExcelRenderNode result = new ExcelRenderNode();
        result.ms = ms;
        result.errMsg = "";
        return result;
    }


    public static DataTable GetChargeOfDptList(DataTable LoginInfo)
    {
        if (LoginInfo == null)
            return null;
        UserRole role = (UserRole)Convert.ToInt32(LoginInfo.Rows[0]["P_Role"]);
        DataTable chargeOfDpt = new DataTable();
        chargeOfDpt.Columns.Add(new DataColumn("DptName", typeof(String)));
        chargeOfDpt.Columns.Add(new DataColumn("DptValue", typeof(String)));
        switch (role)
        {
            case UserRole.LEADER:
            case UserRole.REVIEWER:
                {
                    string[] chargeOfDptArr = LoginInfo.Rows[0]["P_ChargeDepartment"].ToString().Split(',');
                    foreach (var dpt in chargeOfDptArr)
                    {
                        if (dpt != String.Empty)
                        {
                            DataRow row = chargeOfDpt.NewRow();
                            row["DptName"] = dpt;
                            row["DptValue"] = dpt;

                            chargeOfDpt.Rows.Add(row);
                        }
                    }
                    if (!chargeOfDptArr.Contains(LoginInfo.Rows[0]["P_Department"].ToString()))
                    {
                        DataRow row = chargeOfDpt.NewRow();
                        row["DptName"] = LoginInfo.Rows[0]["P_Department"].ToString();
                        row["DptValue"] = LoginInfo.Rows[0]["P_Department"].ToString();
                        chargeOfDpt.Rows.Add(row);
                    }
                }
                break;
            case UserRole.ADMIN:
                chargeOfDpt = GetDepartmentList((int)UserRole.USER);
                break;
            default:
                break;
        }
        return chargeOfDpt;
    }

    private static DataTable GetDepartmentList(int role)
    {
        StringBuilder cmdText = new StringBuilder();
        List<SqlParameter> paramList = new List<SqlParameter>();
        cmdText.Remove(0, cmdText.Length);
        cmdText.Append("Select distinct P_Department as Name, P_Department as Value From tbl_Person where (P_Activate > 0) AND (P_Role >= @Role) AND (P_Name <> 'admin')");

        paramList.Clear();
        paramList.Add(new SqlParameter("@Role", role));
        DataTableCollection result = null;
        try
        {
            result = SystemDAO.SqlHelperEx.GetTableText(cmdText.ToString(), paramList.ToArray());
            if (result.Count > 0)
            {
                DataTable departmentList = result[0];
                DataTable retDptList = new DataTable();
                retDptList.Columns.Add(new DataColumn("DptName", typeof(System.String)));
                retDptList.Columns.Add(new DataColumn("DptValue", typeof(System.String)));
                retDptList.PrimaryKey = new DataColumn[] { retDptList.Columns["DptName"] };
                foreach (DataRow row in departmentList.Rows)
                {
                    DataRow rptRow = retDptList.NewRow();
                    rptRow["DptName"] = row["Name"];
                    rptRow["DptValue"] = row["Value"];
                    retDptList.Rows.Add(rptRow);
                }
                return retDptList;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }


    public static DataTable GetPersonTable(DataTable LoginInfo)
    {
        DataTable chargeOfDpt = GetChargeOfDptList(LoginInfo);
        if (chargeOfDpt == null || chargeOfDpt.Rows.Count <= 0)
            return null;
        string sql = @"
select * from tbl_Person 
where (P_Activate = 0 or P_Activate = 1) ";

        // department filter
        string dptFilter = @"
and (P_Department in ( ";

        foreach (DataRow dpt in chargeOfDpt.Rows)
        {
            dptFilter += @"
'" + dpt["DptName"].ToString() + "',";
        }

        dptFilter = dptFilter.Substring(0, dptFilter.Length - 1);
        dptFilter += @"
)) ";

        sql += dptFilter;

        try
        {
            var results = SystemDAO.SqlHelperEx.GetTableText(sql, null);
            return results[0];
        }
        catch
        {
            return null;
        }
    }
    public static DataTable GetNewPersonTable(DataTable LoginInfo)
    {
        DataTable chargeOfDpt = GetChargeOfDptList(LoginInfo);
        if (chargeOfDpt == null || chargeOfDpt.Rows.Count <= 0)
            return null;
        string sql = @"
select * from tbl_new_person 
where (P_Activate = 0 or P_Activate = 1) ";

        // department filter
        UserRole role;
        string dptFilter = String.Empty;
        var result = Enum.TryParse<UserRole>(LoginInfo.Rows[0]["P_Role"].ToString(), out role);
        if (role != null)
        {
            switch (role)
            {
                case UserRole.ADMIN:
                    break;
                case UserRole.LEADER:
                case UserRole.REVIEWER:
                case UserRole.USER:
                    dptFilter = @"
and (P_Department in ( ";
                    foreach (DataRow dpt in chargeOfDpt.Rows)
                    {
                        dptFilter += @"
'" + dpt["DptName"].ToString() + "',";
                    }
                    dptFilter = dptFilter.Substring(0, dptFilter.Length - 1);
                    dptFilter += @"
)) ";
                    break;
            }
        }
        sql += dptFilter;

        try
        {
            var results = SystemDAO.SqlHelperEx.GetTableText(sql, null);
            return results[0];
        }
        catch
        {
            return null;
        }
    }
    public static int DeletePersonByID(string uid)
    {
        string sql = @"delete from tbl_Person 
where P_ID = @P_ID";

        try
        {
            int result = SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@P_ID", uid) });
            return result;
        }
        catch
        {
            return -1;
        }
    }
    public static int DeletePersonByIDByRole(string uid, UserRole role)
    {
        string sql = @"delete from tbl_Person 
where (P_ID = @P_ID) 
and (P_Role <= @P_Role) ";

        try
        {
            int result = SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@P_ID", uid), new SqlParameter("@P_Role", (Int32)role) });
            return result;
        }
        catch
        {
            return -1;
        }
    }
    public static int DeletePersonByIDByRole(string uid, UserRole role, string oper_id)
    {
        string sql = @"delete from tbl_Person 
where (P_ID = @P_ID) 
and (P_Role < @oper_role) 
and (P_ID != @oper_id) ";

        try
        {
            int result = SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@P_ID", uid), 
                                                                                            new SqlParameter("@oper_role", (Int32)role), 
                                                                                            new SqlParameter("@oper_id", oper_id)});
            return result;
        }
        catch
        {
            return -1;
        }
    }
    public static DataRow GetPersonDetailByID(string uid)
    {
        if (uid == String.Empty)
            return null;
        string sql = @"
select top 1 * from tbl_Person 
where (P_ID = @P_ID) ";

        try
        {
            return SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("P_ID", uid) })[0].Rows[0];
        }
        catch
        {
            return null;
        }
    }
    public static int DeleteNewPersonByID(string uid)
    {
        string sql = @"delete from tbl_new_person 
where P_ID = @P_ID";

        try
        {
            int result = SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@P_ID", uid) });
            return result;
        }
        catch
        {
            return -1;
        }
    }
    public static DataRow GetNewPersonDetailByID(string uid)
    {
        if (uid == String.Empty)
            return null;
        string sql = @"
select top 1 * from tbl_new_person 
where (P_ID = @P_ID) ";

        try
        {
            return SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("P_ID", uid) })[0].Rows[0];
        }
        catch
        {
            return null;
        }
    }

    public static int UpdatePersonInfo(tbl_Person person)
    {
        string sql = @"
update tbl_Person 
set 
  P_Location = @P_Location 
, P_Department = @P_Department 

, P_Name = @P_Name 
, P_Email = @P_Email 
, P_ExNumber = @P_ExNumber 

, P_ChargeDepartment = @P_ChargeDpt 
, P_Role = @P_Role 
, P_Activate = @P_Activate 
where (P_ID = @P_ID) ";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@P_Location", person.P_Location));

        paramss.Add(new SqlParameter("@P_Name", person.P_Name));
        paramss.Add(new SqlParameter("@P_Email", person.P_Email));
        paramss.Add(new SqlParameter("@P_ExNumber", person.P_ExNumber));

        paramss.Add(new SqlParameter("@P_Department", person.P_Department));
        paramss.Add(new SqlParameter("@P_ChargeDpt", person.P_ChargeDepartment));
        paramss.Add(new SqlParameter("@P_Role", person.P_Role));
        paramss.Add(new SqlParameter("@P_Activate", person.P_Activate));
        paramss.Add(new SqlParameter("@P_ID", person.P_ID));

        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
        }
        catch
        {
            return -1;
        }
    }

    public static int UpdatePersonInfo(tbl_Person person, UserRole oper_role, string oper_id)
    {
        string sql = @"
update tbl_Person 
set 
  P_Location = @P_Location 
, P_Department = @P_Department 

, P_Name = @P_Name 
, P_Email = @P_Email 
, P_ExNumber = @P_ExNumber 

, P_ChargeDepartment = @P_ChargeDpt 
, P_Role = @P_Role 
, P_Activate = @P_Activate 
where (P_ID = @P_ID) 
and ((P_Role < @oper_role) or (P_ID = @oper_id)) ";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@P_Location", person.P_Location));

        paramss.Add(new SqlParameter("@P_Name", person.P_Name));
        paramss.Add(new SqlParameter("@P_Email", person.P_Email));
        paramss.Add(new SqlParameter("@P_ExNumber", person.P_ExNumber));

        paramss.Add(new SqlParameter("@P_Department", person.P_Department));
        paramss.Add(new SqlParameter("@P_ChargeDpt", person.P_ChargeDepartment));
        paramss.Add(new SqlParameter("@P_Role", person.P_Role));
        paramss.Add(new SqlParameter("@P_Activate", person.P_Activate));
        paramss.Add(new SqlParameter("@P_ID", person.P_ID));
        paramss.Add(new SqlParameter("@oper_role", (Int32)oper_role));
        paramss.Add(new SqlParameter("@oper_id", oper_id));
        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
        }
        catch
        {
            return -1;
        }
    }

    public static int InsertOnePerson(DataRow person)
    {
        string sql = @"
insert into tbl_Person(P_ID, P_Name, P_Password, P_Department, P_ChargeDepartment, P_Email, P_ExNumber
, P_Role, P_Date, P_Location, P_Activate) 
values(@P_ID, @P_Name, '123456', @P_Department, @P_ChargeDepartment, @P_Email, @P_ExNumber
, @P_Role, GetDate(), @P_Location, @P_Activate)";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@P_ID", person["P_ID"]));
        paramss.Add(new SqlParameter("@P_Name", person["P_Name"]));
        paramss.Add(new SqlParameter("@P_Department", person["P_Department"]));
        paramss.Add(new SqlParameter("@P_ChargeDepartment", person["P_ChargeDepartment"]));
        paramss.Add(new SqlParameter("@P_Email", person["P_Email"]));
        paramss.Add(new SqlParameter("@P_ExNumber", person["P_ExNumber"]));
        paramss.Add(new SqlParameter("@P_Role", person["P_Role"]));
        paramss.Add(new SqlParameter("@P_Location", person["P_Location"]));
        paramss.Add(new SqlParameter("@P_Activate", person["P_Activate"]));

        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
        }
        catch
        {
            return -1;
        }
    }
    /// <summary>
    /// user端新用户注册， 只提供用户UID， Email, Dpt相关信息
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public static int NewUser(tbl_Person person)
    {
        string sql = @"
insert into tbl_new_person (P_ID, P_Email, P_Department)
values (@P_ID, @P_Email, @P_Department) ";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@P_ID", person.P_ID));
        paramss.Add(new SqlParameter("@P_Email", person.P_Email));
        paramss.Add(new SqlParameter("@P_Department", person.P_Department));

        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
        }
        catch
        {
            return -1;
        }
    }
    /// <summary>
    /// 根据不同角色用户列表
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    public static DataTable GetPersonListByRole(UserRole role)
    {
        string sql = @"
select * from tbl_Person 
where (P_Role = @P_Role) 
and (P_ID != 'K1207A49') 
and (P_RegisterStatus = 1) 
and (P_Activate = 1) ";// 排除开发者帐号
        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@P_Role", (Int32)role));
        try
        {
            return SystemDAO.SqlHelperEx.GetTableText(sql, paramss.ToArray())[0];
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// 获取对应权限的用户列表
    /// </summary>
    /// <param name="role">权限值</param>
    /// <param name="department">管辖的部门</param>
    /// <returns></returns>
    public static DataTable GetPersonList(UserRole role, string department)
    {
        string sql = @"
select * from tbl_Person 
where (P_Role = @P_Role) 
and (P_ID != 'K1207A49') "// 排除开发者帐号
+ @"
and (P_RegisterStatus = 1) 
and (P_Activate = 1) 
and ((PATINDEX(@P_Department, P_ChargeDepartment) > 0) or (P_Department = @P_Department)) 
or ((@P_Role >= 20) and (P_ID != 'k1207a49') and (P_Role = @P_Role))  
";
        List<SqlParameter> paramm = new List<SqlParameter>();
        paramm.Add(new SqlParameter("@P_Role", (Int32)role));
        paramm.Add(new SqlParameter("@P_Department", department));

        try
        {
            return SystemDAO.SqlHelperEx.GetTableText(sql, paramm.ToArray())[0];
        }
        catch
        {
            return null;
        }
    }
}