using DataBaseModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SystemDAO;

/// <summary>
/// settingsHandler 的摘要说明
/// </summary>
public abstract class settingsHandler
{
    public settingsHandler()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    #region project handler
    public static DataTable GetProjectTable()
    {
        string sql = @"select * from tbl_Project 
order by PJ_Code ";
        try
        {
            DataTableCollection result = SqlHelperEx.GetTableText(sql, null);
            DataTable projectTable = result[0];
            projectTable.Columns.Add("pj_stage");
            foreach (DataRow project in projectTable.Rows)
            {
                project["pj_stage"] = GetActivateProjectStage(project["pj_code"].ToString());
            }
            return projectTable;
        }
        catch
        {
            return null;
        }
    }
    public static DataTable GetProjectTable(string pj_code)
    {
        string sql = @"select * from tbl_Project 
where PJ_Code = @PJ_Code 
order by PJ_Code ";
        try
        {
            DataTableCollection result = SqlHelperEx.GetTableText(sql, new SqlParameter[]{
                new SqlParameter("PJ_Code", pj_code)
            });
            DataTable projectTable = result[0];
            projectTable.Columns.Add("pj_stage");
            foreach (DataRow project in projectTable.Rows)
            {
                project["pj_stage"] = GetActivateProjectStage(project["pj_code"].ToString());
            }
            return projectTable;
        }
        catch
        {
            return null;
        }
    }
    public static int DeleteProject(string pj_code)
    {
        string sql = @"delete from tbl_Project 
where PJ_Code = @PJ_Code ";
        try
        {
            int result = SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[]{
                new SqlParameter("PJ_Code", pj_code)
            });

            return result;
        }
        catch
        {
            return -1;
        }
    }

    public static int UpdateProject(tbl_Project oldValues, tbl_Project newValues)
    {
        List<SqlParameter> paramss = new List<SqlParameter>();
        string sql = @"update tbl_Project set ";
        string setValues = String.Empty;

        if (oldValues.PJ_Name != newValues.PJ_Name)
        {
            setValues += ", PJ_Name = @PJ_Name ";
            paramss.Add(new SqlParameter("@PJ_Name", newValues.PJ_Name));
        }

        if (oldValues.Cust_Name != newValues.Cust_Name)
        {
            setValues += ", Cust_Name = @Cust_Name ";
            paramss.Add(new SqlParameter("@Cust_Name", newValues.PJ_Name));
        }

        if (setValues != String.Empty)
        {
            setValues = setValues.Substring(1);
            sql += setValues;

            sql += @" where PJ_Code = @PJ_Code ";
            paramss.Add(new SqlParameter("@PJ_Code", newValues.PJ_Code));

            try
            {
                return SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
            }
            catch
            {// 更新失败
                return -1;
            }
        }
        return 0;
    }

    public static int InitNewRow(tbl_Project newValues)
    {
        string sql = @"Insert into tbl_Project(PJ_Code, PJ_Name, Cust_Name, Date) 
values(@PJ_Code, @PJ_Name, @Cust_Name, GetDate())";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@PJ_Code", newValues.PJ_Code));
        paramss.Add(new SqlParameter("@PJ_Name", newValues.PJ_Name));
        paramss.Add(new SqlParameter("@Cust_Name", newValues.Cust_Name));

        try
        {
            var result = SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
            return result;
        }
        catch
        {
            return -1;
        }
    }
    #endregion

    #region purpose handler
    public static DataTable GetPurposeTable()
    {
        string sql = @"select * from tbl_TestCategory 
order by ID ";
        try
        {
            DataTableCollection result = SqlHelperEx.GetTableText(sql, null);

            return result[0];
        }
        catch
        {
            return null;
        }
    }
    public static int DeletePurpose(string id)
    {
        string sql = @"delete from tbl_TestCategory 
where ID = @ID ";
        try
        {
            int result = SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[]{
                new SqlParameter("@ID", id)
            });

            return result;
        }
        catch
        {
            return -1;
        }
    }

    public static int UpdatePurse(tbl_TestCategory oldValues, tbl_TestCategory newValues)
    {
        if (oldValues.Name == newValues.Name)
            return 0;
        List<SqlParameter> paramss = new List<SqlParameter>();
        string sql = @"update tbl_TestCategory set ";
        string setValues = String.Empty;
        if (oldValues.Name != newValues.Name)
        {
            setValues += ", Name = @Name ";
            paramss.Add(new SqlParameter("@Name", newValues.Name));
        }
        if (setValues != String.Empty)
        {
            setValues = setValues.Substring(1);
            sql += setValues;
        }
        else
        {
            // 没有可更新的数据
            return 0;
        }
        sql += "where ID = @ID ";
        paramss.Add(new SqlParameter("@ID", newValues.ID));

        try
        {
            return SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
        }
        catch
        {// 更新失败
            return -1;
        }
    }

    public static int InitNewRow(tbl_TestCategory newValues)
    {
        string sql = @"Insert into tbl_TestCategory(ID, Name, Date) 
values(@ID, @Name, GetDate())";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@ID", newValues.ID));
        paramss.Add(new SqlParameter("@Name", newValues.Name));

        try
        {
            var result = SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
            return result;
        }
        catch
        {
            return -1;
        }
    }
    #endregion

    #region project stage handler
    public static DataTable GetProjectStage(string pj_code)
    {
        string sql = @"select * from tbl_project_stage 
where ps_pj_id = @PJ_Code ";
        try
        {
            var result = SqlHelperEx.GetTableText(sql, new SqlParameter[]{
                new SqlParameter("@PJ_Code", pj_code)
            });

            return result[0];
        }
        catch
        {
            return null;
        }
    }
    public static string GetActivateProjectStage(string pj_code)
    {
        string sql = @"select * from tbl_project_stage 
where ps_pj_id = @PJ_Code ";
        try
        {
            var result = SqlHelperEx.GetTableText(sql, new SqlParameter[]{
                new SqlParameter("@PJ_Code", pj_code)
            });
            var pjTable = result[0];
            foreach (DataRow pj in pjTable.Rows)
            {
                DateTime start = Convert.ToDateTime(pj["ps_from"]);
                DateTime to = Convert.ToDateTime(pj["ps_to"]);
                if (DateTime.Now >= start && DateTime.Now <= to)
                {
                    return pj["ps_stage"].ToString();
                }
            }
        }
        catch
        {
        }
        return "NULL";
    }

    public static int DeleteProjectStage(string ps_id)
    {
        string sql = @"delete from tbl_project_stage 
where ps_id = @ps_id ";
        try
        {
            int result = SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[]{
                new SqlParameter("@ps_id", ps_id)
            });

            return result;
        }
        catch
        {
            return -1;
        }
    }

    public static int UpdateProjectStage(tbl_project_stage oldValues, tbl_project_stage newValues)
    {
        List<SqlParameter> paramss = new List<SqlParameter>();
        string sql = @"update tbl_project_stage set ";
        string setValues = String.Empty;

        if (oldValues.ps_stage != newValues.ps_stage)
        {
            setValues += ", ps_stage = @ps_stage";
            paramss.Add(new SqlParameter("@ps_stage", newValues.ps_stage));
        }

        if (oldValues.ps_from != newValues.ps_from)
        {
            setValues += ", ps_from = @ps_from";
            paramss.Add(new SqlParameter("@ps_from", newValues.ps_from));
        }

        if (oldValues.ps_to != newValues.ps_to)
        {
            setValues += ", ps_to = @ps_to";
            paramss.Add(new SqlParameter("@ps_to", newValues.ps_to));
        }

        if (setValues != String.Empty)
        {
            setValues = setValues.Substring(1);
            sql += setValues;

            sql += @" where ps_id = @ps_id ";
            paramss.Add(new SqlParameter("@ps_id", newValues.ps_id));

            try
            {
                return SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
            }
            catch
            {// 更新失败
                return -1;
            }
        }
        else
        {
            return 0;
        }
    }
    public static int InitNewRow(tbl_project_stage newValues)
    {
        string sql = @"Insert into tbl_project_stage(ps_pj_id, ps_stage, ps_from, ps_to, date) 
values(@ps_pj_id, @ps_stage, @ps_from, @ps_to, GetDate())";

        List<SqlParameter> paramss = new List<SqlParameter>();
        paramss.Add(new SqlParameter("@ps_pj_id", newValues.ps_pj_id));
        paramss.Add(new SqlParameter("@ps_stage", newValues.ps_stage));
        paramss.Add(new SqlParameter("@ps_from", newValues.ps_from));
        paramss.Add(new SqlParameter("@ps_to", newValues.ps_to));
        try
        {
            var result = SqlHelperEx.ExecteNonQueryText(sql, paramss.ToArray());
            return result;
        }
        catch
        {
            return -1;
        }
    }
    #endregion

    #region site table handler
    public static DataTable GetSiteList()
    {
        string sql = @"select * from tbl_site ";
        try
        {
            var result = SqlHelperEx.GetTableText(sql, null);
            return result[0];
        }
        catch
        {
            return null;
        }
    }
    #endregion

}