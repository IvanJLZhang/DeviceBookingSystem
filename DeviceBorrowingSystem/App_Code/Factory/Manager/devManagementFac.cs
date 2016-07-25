using BarCodeHelper;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using SystemDAO;
using ZipHelper;

/// <summary>
/// devManagementFac 的摘要说明
/// </summary>
public class devManagementFac
{
    public devManagementFac()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }


    public DataTable GetDeviceTable(List<Status> deviceStatus)
    {
        string sql = @"select device.d_id as id, base.s_name as [Device], device.d_customid as [Custom ID], Owner.P_Name as [Owner], Owner.P_Department as Department 
, device.d_class Class, device.d_interface Interface, Owner.P_Location as Site, base.s_status, device.d_status as BorrowStatus 
from tbl_device_detail as device
inner join tbl_summary_dev_title as base on device.d_id = base.s_id 
inner join tbl_Person as [Owner] on base.s_ownerid = Owner.P_ID 
where (base.s_category = 1) ";

        string filterStr = String.Empty;
        filterStr += @"
and (base.s_status in ( ";
        foreach (Status status in deviceStatus)
        {
            filterStr += (Int32)status + ",";
        }
        filterStr = filterStr.Substring(0, filterStr.Length - 1);
        filterStr += @"
)) ";

        sql += filterStr;
        try
        {
            DataTableCollection results = SystemDAO.SqlHelperEx.GetTableText(sql, null);

            DataTable retTable = results[0];
            return retTable;
        }
        catch
        {
            return null;
        }

    }
    public DataTable GetEquipmentTable(List<Status> deviceStatus)
    {
        string sql = @"select base.s_id as id, base.s_name as [Device Name], Owner.P_Name as [Owner Name], Owner.P_Department as Department 
, Owner.P_Location as Site, base.s_image_url as ImageUrl, base.s_status 
, base.s_note as Note 
from tbl_summary_dev_title as base 
inner join tbl_Person as [Owner] on base.s_ownerid = Owner.P_ID 
inner join tbl_equipment_detail equipment on base.s_id = equipment.e_id 
where (base.s_category = 2) ";

        string filterStr = String.Empty;
        filterStr += @"
and (base.s_status in ( ";
        foreach (Status status in deviceStatus)
        {
            filterStr += (Int32)status + ",";
        }
        filterStr = filterStr.Substring(0, filterStr.Length - 1);
        filterStr += @"
)) ";

        sql += filterStr;
        try
        {
            DataTableCollection results = SystemDAO.SqlHelperEx.GetTableText(sql, null);

            DataTable retTable = results[0];
            return retTable;
        }
        catch
        {
            return null;
        }

    }
    public bool DeleteDevice(string id)
    {
        string sql = @"delete from tbl_device_detail where d_id = @id";
        SqlParameter[] para = new SqlParameter[] { 
            new SqlParameter("@id", id)
        };

        try
        {
            int ret = SqlHelper.ExecteNonQueryText(sql, para);
            if (ret >= 1)
            {
                sql = @"delete from tbl_summary_dev_title where s_id = @id";
                SqlParameter[] para1 = new SqlParameter[] { 
            new SqlParameter("@id", id)
        };
                SqlHelper.ExecteNonQueryText(sql, para1);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    public bool DeleteEquipment(string id)
    {
        string sql = @"delete from tbl_equipment_detail where e_id = @id ";
        SqlParameter[] para = new SqlParameter[] { 
            new SqlParameter("@id", id)
        };

        try
        {
            int ret = SqlHelper.ExecteNonQueryText(sql, para);
            if (ret >= 1)
            {
                sql = @"delete from tbl_summary_dev_title where s_id = @id ";
                SqlParameter[] para1 = new SqlParameter[] { 
            new SqlParameter("@id", id)
        };
                SqlHelper.ExecteNonQueryText(sql, para1);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
    public static string errMsg;
    private DataTable GetDeviceListByCategory(string category)
    {
        StringBuilder cmdText = new StringBuilder();
        int cat = Int32.Parse(category);
        switch (cat)
        {
            case 1:
                cmdText.Remove(0, cmdText.Length);
                cmdText.Append(@"select S.s_name [Device Name], S.s_category [Category], S.s_assetid [Asset ID], D.d_customid [Custom ID], D.d_class [Class], 
D.d_interface [Interface], S.s_ownerid [Owner], OwnerTable.P_Name as [Owner Name], OwnerTable.P_Department, S.s_vender [Vender], S.s_cost [Cost], S.s_status [Status],
 '' [Image], S.s_note [Description], S.s_date [Last Update Date] from tbl_device_detail as D 
inner join tbl_summary_dev_title as S on D.d_id = S.s_id ");
                break;
            case 2:
                cmdText.Remove(0, cmdText.Length);
                cmdText.Append(@"select S.s_name [Device Name], S.s_category [Category], S.s_assetid [Asset ID], S.s_ownerid [Owner], OwnerTable.P_Name as [Owner Name], OwnerTable.P_Department, 
E.e_testing_time [Testing Time], E.e_avg_hr [Avg_Hr], E.e_loan_day [Loan Day], E.e_lab_location [Lab Location], 
S.s_vender [Vender], S.s_cost [Cost], S.s_status [Status],
 '' [Image], S.s_note [Description], S.s_date [Last Update Date] 
 from tbl_equipment_detail as E  
inner join tbl_summary_dev_title as S on E.e_id = S.s_id ");
                break;
            case 3:
                cmdText.Remove(0, cmdText.Length);
                cmdText.Append(@"select S.s_name [Device Name], S.s_category [Category], S.s_assetid [Asset ID], S.s_ownerid [Owner], OwnerTable.P_Name as [Owner Name], OwnerTable.P_Department, 
C.c_avg_hr [Avg_Hr], C.c_loan_day [Loan Day], C.c_lab_location [Lab Location], 
S.s_vender [Vender], S.s_cost [Cost], S.s_status [Status],
 '' [Image], S.s_note [Description], S.s_date [Last Update Date] 
 from tbl_chamber_detail as C  
inner join tbl_summary_dev_title as S on C.c_id = S.s_id ");
                break;
        }
        cmdText.Append(@"
inner join tbl_Person as OwnerTable on S.s_ownerid = OwnerTable.P_ID ");
        DataTableCollection result = null;
        try
        {
            result = SqlHelper.GetTableText(cmdText.ToString(), null);
        }
        catch (Exception ex)
        {
            errMsg = ex.Message;
            return null;
        }

        if (result.Count <= 0)
        {
            errMsg = "No data!";
            return null;
        }
        return result[0];
    }

    public ExcelRenderNode PrintDeviceTable(string category)
    {
        int cat = Int32.Parse(category);
        DataTable dataTable = GetDeviceListByCategory(category);

        ExcelRenderNode result = new ExcelRenderNode();
        ExcelRender xlsApp = new ExcelRender();

        xlsApp.CreateSheet("data Element");
        int nStartCol = 0;
        int nStartRow = 0;
        int customidCol = 0;
        for (int index = 0; index != dataTable.Columns.Count; index++)
        {
            DataColumn column = dataTable.Columns[index];
            xlsApp.SetCellValue(nStartRow, nStartCol, column.Caption, TypeCode.String);
            if (column.Caption == "Status")
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "1=>Usable,\r\n2=>Broken,\n3=>Lost,\n4=>Return to Custumer,\n5=>NotForCirculation", TypeCode.String);
            }
            else if (column.Caption == "Owner")
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Owner ID", TypeCode.String);
            }
            else if (column.Caption == "P_Department")
            {
                xlsApp.SetCellValue(nStartRow, nStartCol, "Owner Department", TypeCode.String);
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Owner Department", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Avg_hr", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Working Hours Daily", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Image", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Image Name", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Category", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "1=>Device,\n2=>Equipment,\n3=>Chamber", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Custom id", StringComparison.CurrentCultureIgnoreCase))
            {
                customidCol = index;
                nStartCol++;
                // barcode列
                xlsApp.SetCellValue(nStartRow, nStartCol, "Custom id barcode", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "borrow_status", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "0=>Borrow_Out, \n1=>OK", TypeCode.String);
            }
            else
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, column.Caption, TypeCode.String);
            }
            nStartCol++;
        }

        for (int index = 0; index != nStartCol; index++)
        {
            xlsApp.SetCellWidth(index, 40);
            xlsApp.SetRowHeight(nStartRow, 15);
            xlsApp.SetCellStyle(nStartRow, index, NPOI.HSSF.Util.HSSFColor.DarkYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);

            xlsApp.SetRowHeight(nStartRow + 1, 30);
            xlsApp.SetCellStyle(nStartRow + 1, index, NPOI.HSSF.Util.HSSFColor.LightYellow.Index, NPOI.SS.UserModel.HorizontalAlignment.Center, true);
        }
        nStartRow += 2;

        foreach (DataRow row in dataTable.Rows)
        {
            int col = 0;
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
                else if (column.Caption == "Custom ID")
                {
                    string customid = row["Custom ID"].ToString();
                    xlsApp.SetCellValue(nStartRow, col, row[column].ToString(), TypeCode.String);

                    col++;
                    if (customid != String.Empty)
                    {
                        string barcodefile = GetBarCode(customid);
                        xlsApp.SetRowHeight(nStartRow, 55);
                        xlsApp.SetCellWidth(col, 35);
                        xlsApp.AddPicture_Ex(barcodefile, nStartRow, col);

                    }
                }
                else
                {
                    xlsApp.SetCellValue(nStartRow, col, row[column].ToString(), TypeCode.String);
                }
                col++;
            }
            nStartRow++;
        }

        {// 删除Temp文件夹中的内容
            string[] files = Directory.GetFiles(this.serverPath + "Temp\\");
            for (int index = 0; index != files.Length; index++)
            {
                File.Delete(files[index]);
            }
        }
        MemoryStream ms = new MemoryStream();
        xlsApp._MyWorkbook.Write(ms);

        result.ms = ms;
        result.errMsg = "";
        return result;
    }


    bool PreUploadCheck(DataTable deviceTable)
    {
        int cat = Int32.Parse(deviceTable.Rows[0]["Category"].ToString());
        for (int index = 0; index != deviceTable.Rows.Count; index++)
        {
            string cate = deviceTable.Rows[index]["Category"].ToString();
            if (cate == String.Empty)
                continue;
            int cat1 = Int32.Parse(cate);
            if (cat != cat1)
                return false;
        }
        switch ((Category)cat)
        {
            case Category.Device:
                {
                    try
                    {
                        if (deviceTable.Columns["Custom ID"] == null)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                break;
            case Category.Equipment:
                {
                    try
                    {
                        if (deviceTable.Columns["Testing Time"] == null)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                break;
            case Category.Chamber:
                {
                    try
                    {
                        if (deviceTable.Columns["Avg_Hr"] == null)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
                break;

        }
        return true;
    }

    public DataTable LoadDataAnalyze(string loadFileName, string tempPath)
    {
        UnZipClass unZip = new UnZipClass();
        string temp = GlobalClassNamespace.GlobalClass.GetMD5Str(DateTime.Now.Ticks.ToString()) + "\\";
        string unzipPath = Path.Combine(tempPath, temp);
        unZip.UnZip(loadFileName, unzipPath);


        ExcelRender xlsApp = new ExcelRender();
        DirectoryInfo dir = new DirectoryInfo(unzipPath);
        FileInfo[] deviceDataFiles = dir.GetFiles("*.xls*", SearchOption.AllDirectories);
        if (deviceDataFiles.Length <= 0)
            return null;
        var deviceDataFile = deviceDataFiles[0];
        FileStream fs = new FileStream(deviceDataFile.FullName, FileMode.Open, FileAccess.Read);
        DataTable deviceTable = ExcelRender.RenderFromExcel(fs, 0);

        if (deviceTable != null && deviceTable.Rows.Count > 1)
        {
            deviceTable.Rows.RemoveAt(0);// 第一行为title 的comment 行， 故删去

            if (!PreUploadCheck(deviceTable))
            {
                return null;
            }

            int cat = Int32.Parse(deviceTable.Rows[0]["Category"].ToString());
            DataTable genDevice = GenDeviceTable((Category)cat);
            for (int index = 0; index != deviceTable.Rows.Count; index++)
            {
                DataRow row = deviceTable.Rows[index];
                DataRow genRow = genDevice.NewRow();

                string id = GenID((Category)cat) + index.ToString();

                genRow["id"] = id;
                DataTable person = GetPersonName(row["Owner"].ToString());
                if (person == null)
                {
                    genRow["s_ownerid"] = String.Empty;
                    genRow["owner"] = String.Empty;
                    genRow["site"] = String.Empty;
                }
                else
                {
                    genRow["s_ownerid"] = person.Rows[0]["P_ID"];
                    genRow["owner"] = person.Rows[0]["P_Name"];
                    genRow["site"] = person.Rows[0]["P_Location"];
                }

                genRow["s_name"] = row["Device Name"];
                genRow["s_assetid"] = row["Asset ID"] == null ? String.Empty : row["Asset ID"];
                genRow["s_category"] = cat;
                genRow["s_vender"] = row["Vender"] == null ? String.Empty : row["Vender"];
                genRow["s_cost"] = row["Cost"] == null ? String.Empty : row["Cost"];
                genRow["s_status"] = row["Status"] == null ? String.Empty : row["Status"];
                genRow["s_image_url"] = GetImageUrl(row["Image"].ToString(), tempPath, temp);
                genRow["s_note"] = row["Description"] == null ? String.Empty : row["Description"];

                switch (cat)
                {
                    case 1:
                        genRow["d_customid"] = row["Custom ID"] == null ? String.Empty : row["Custom ID"];
                        genRow["d_class"] = row["Class"] == null ? String.Empty : row["Class"];
                        genRow["d_interface"] = row["Interface"] == null ? String.Empty : row["Interface"];
                        break;
                    case 2:
                        TimeSpan testtime = TimeSpan.Zero;
                        try
                        {
                            testtime = TimeSpan.Parse(row["Testing Time"].ToString());

                        }
                        catch
                        {

                        }
                        genRow["e_testing_time"] = testtime;
                        //genRow["e_testing_time"] = row["Testing Time"] == null ? String.Empty : row["Testing Time"];

                        genRow["e_avg_hr"] = (row["Avg_Hr"] == null || row["Avg_Hr"].ToString() == String.Empty) ? 24 : Int32.Parse(row["Avg_Hr"].ToString());
                        genRow["e_loan_day"] = (row["Loan Day"] == null || row["Loan Day"].ToString() == String.Empty) ? 0 : Int32.Parse(row["Loan Day"].ToString());
                        genRow["e_lab_location"] = row["Lab Location"] == null ? String.Empty : row["Lab Location"];
                        break;
                    case 3:
                        genRow["c_avg_hr"] = (row["Avg_Hr"] == null || row["Avg_Hr"].ToString() == String.Empty) ? 24 : Int32.Parse(row["Avg_Hr"].ToString());
                        genRow["c_loan_day"] = (row["Loan Day"] == null || row["Loan Day"].ToString() == String.Empty) ? 0 : Int32.Parse(row["Loan Day"].ToString());
                        genRow["c_lab_location"] = row["Lab Location"] == null ? String.Empty : row["Lab Location"];
                        break;
                }
                genDevice.Rows.Add(genRow);
            }

            return genDevice;
        }
        return null;
    }

    public ExcelRenderNode GenerateTemplateFile(string category, string tempPath, string deviceImagesPath)
    {
        int cat = Int32.Parse(category);
        cl_DeviceManage deviceManage = new cl_DeviceManage();
        DataTable dataTable = deviceManage.GetDeviceTemplateByCategory(category);
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

            if (column.Caption == "Status")
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "1=>Usable,\n2=>Broken,\n3=>Lost,\n4=>Return to Custumer,\n5=>NotForCirculation", TypeCode.String);
            }
            else if (column.Caption == "Owner")
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Owner ID", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Avg_hr", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Working Hours Daily", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Image", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "Image Name", TypeCode.String);
            }
            else if (String.Equals(column.Caption, "Category", StringComparison.CurrentCultureIgnoreCase))
            {
                xlsApp.SetCellValue(nStartRow + 1, nStartCol, "1=>Device,\n2=>Equipment,\n3=>Chamber", TypeCode.String);
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

        //foreach (DataRow row in dataTable.Rows)
        //{
        DataRow row = dataTable.Rows[0];
        int col = 0;
        string ticksTemp = GlobalClassNamespace.GlobalClass.GetMD5Str(DateTime.Now.Ticks.ToString());
        Directory.CreateDirectory(tempPath + ticksTemp);
        // 取出图片文件名
        if (row["Image"].ToString() != String.Empty)
        {
            string image = row["Image"].ToString();
            image = image.Substring(image.LastIndexOf('/') + 1);
            row["Image"] = image;
            FileInfo imageFile = new FileInfo(deviceImagesPath + image);
            string imageNewPath = tempPath + ticksTemp + "\\DeviceImgs\\";
            Directory.CreateDirectory(imageNewPath);
            imageNewPath += image;
            if (imageFile.Exists)
            {
                imageFile.CopyTo(imageNewPath, true);
            }
        }

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
        // }


        string fileName = Enum.GetName(typeof(Category), cat) + "_Data_" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
        fileName = tempPath + ticksTemp + "\\" + fileName;
        MemoryStream ms = new MemoryStream();
        xlsApp._MyWorkbook.Write(ms);
        ExcelRender.SaveToFile(ms, fileName);
        ms.Close();

        string zipfile = tempPath + ticksTemp + ".zip";
        ZipClass.ZipFolder(tempPath + ticksTemp, zipfile);

        ExcelRenderNode result = new ExcelRenderNode();

        FileStream fs = new FileStream(zipfile, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        fs.Close();

        ms = new MemoryStream(bytes);
        result.ms = ms;
        result.errMsg = ticksTemp;
        return result;
    }
    string GetImageUrl(string imageName, string tempPath, string tempp)
    {
        if (imageName == String.Empty)
            return String.Empty;
        string imagePath = Path.Combine(tempPath, tempp, imageName);
        if (File.Exists(imagePath))
            return Path.Combine("~\\Temp\\", tempp, imageName);
        return String.Empty;
    }

    private DataTable GetPersonName(string userID)
    {
        if (userID == String.Empty)
            return null;
        cl_PersonManage personManage = new cl_PersonManage();
        DataTable personInfo = personManage.GetPersonInfoByID(userID);
        if (personInfo != null && personInfo.Rows.Count > 0)
        {
            return personInfo;
        }
        return null;
    }
    /// <summary>
    /// 生成device ID
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    public static string GenID(Category category)
    {
        string id = String.Empty;
        switch (category)
        {

            case Category.Device:
                id += "DT";
                break;
            case Category.Equipment:
                id += "ET";
                break;
            case Category.Chamber:
                id += "CT";
                break;
        }

        id += DateTime.Now.ToString("yyyyMMddss");
        return id;
    }

    DataTable GenDeviceTable(Category cat)
    {

        DataTable device = new DataTable();
        device.Columns.Add("id", typeof(String));
        device.Columns.Add("s_ownerid", typeof(String));
        device.Columns.Add("owner", typeof(String));
        device.Columns.Add("site", typeof(String));
        device.Columns.Add("s_name", typeof(String));
        device.Columns.Add("s_assetid", typeof(String));
        device.Columns.Add("s_category", typeof(int));
        device.Columns.Add("s_vender", typeof(String));
        device.Columns.Add("s_cost", typeof(float));
        device.Columns.Add("s_status", typeof(int));
        device.Columns.Add("s_image_url", typeof(String));
        device.Columns.Add("s_note", typeof(String));
        device.Columns.Add("errnote", typeof(String));

        switch (cat)
        {
            case Category.Device:
                device.Columns.Add("d_customid", typeof(String));
                device.Columns.Add("d_class", typeof(String));
                device.Columns.Add("d_interface", typeof(String));
                break;
            case Category.Equipment:
                device.Columns.Add("e_testing_time", typeof(TimeSpan));
                device.Columns.Add("e_avg_hr", typeof(int));
                device.Columns.Add("e_loan_day", typeof(int));
                device.Columns.Add("e_lab_location", typeof(String));
                break;
            case Category.Chamber:
                device.Columns.Add("c_avg_hr", typeof(int));
                device.Columns.Add("c_loan_day", typeof(int));
                device.Columns.Add("c_lab_location", typeof(String));
                break;
        }

        return device;
    }


    public string serverPath = String.Empty;
    string GetBarCode(string customid)
    {
        Code128Auto _code = new Code128Auto(customid);
        Image img = _code.GetBarCodeImage();
        if (this.serverPath != String.Empty)
        {
            string barCodeImgfile = this.serverPath + "Temp\\" + customid + ".gif";
            img.Save(barCodeImgfile);

            return barCodeImgfile;
        }

        return String.Empty;

    }


    public static DataTable GetDeviceList(RecordFilter filter)
    {
        StringBuilder cmdText = new StringBuilder();
        cmdText.Remove(0, cmdText.Length);
        cmdText.Append(@"select summary.s_id as Device_ID, summary.s_name as Device_Name 
from tbl_summary_dev_title as summary 
inner join tbl_device_detail device on summary.s_id = device.d_id 
inner join tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID 
where (summary.s_category = @category) ");

        #region do filter
        string filterStr = string.Empty;
        List<SqlParameter> paramss = new List<SqlParameter>();
        if (filter.deviceFilter)
        {
            if (filter.d_site != string.Empty)
            {
                filterStr += @"
and ([Owner].P_Location = @Location) ";

                paramss.Add(new SqlParameter("@Location", filter.d_site));
            }

            if (filter.d_department != string.Empty)
            {
                filterStr += @"
and ([Owner].P_Department = @Department) ";
                paramss.Add(new SqlParameter("@Department", filter.d_department));
            }
        }
        cmdText.Append(filterStr);
        paramss.Add(new SqlParameter("@category", (int)filter.category));
        #endregion
        DataTableCollection result = null;
        try
        {
            result = SystemDAO.SqlHelperEx.GetTableText(cmdText.ToString(), paramss.ToArray());
        }
        catch (Exception ex)
        {
            errMsg = ex.Message;
            return null;
        }
        if (result.Count <= 0)
        {
            errMsg = "can not get Device list";
            return null;
        }
        return result[0];
    }

    public static DataRow GetDeviceInfo(string id, Category cat)
    {
        string sql = @"
select summary.* 
, [Owner].P_Name as OwnerName 
, D.* ";
        sql += @"
from tbl_summary_dev_title as summary 
inner join tbl_Person as [Owner] on summary.s_ownerid = [Owner].P_ID ";

        switch (cat)
        {
            case Category.Device:
                sql += @"
inner join tbl_device_detail as D on summary.s_id = D.d_id ";
                break;
            case Category.Equipment:
                sql += @"
inner join tbl_equipment_detail as D on summary.s_id = D.e_id ";
                break;
            case Category.Chamber:
                sql += @"
inner join tbl_chamber_detail as D on summary.s_id = D.c_id ";
                break;
            default:
                break;
        }

        sql += @"
where summary.s_id = @id ";
        try
        {
            var tables = SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("@id", id) });
            return tables[0].Rows[0];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 获取summary的表结构
    /// </summary>
    /// <param name="cat"></param>
    /// <returns></returns>
    public static DataTable GetSummaryTableStruct(Category cat)
    {
        string sql = @"
select top 1 summary.* from tbl_summary_dev_title as summary ";

        string catFilter = String.Empty;
        switch (cat)
        {
            case Category.Device:
                catFilter += @"
inner join tbl_device_detail as D on summary.s_id = D.d_id ";
                break;
            case Category.Equipment:
                catFilter += @"
inner join tbl_equipment_detail as D on summary.s_id = D.e_id ";
                break;
            case Category.Chamber:

                catFilter += @"
inner join tbl_chamber_detail as D on summary.s_id = D.c_id ";
                break;
        }

        sql += catFilter;
        sql += @"
where summary.s_category = " + (Int32)cat;

        try
        {
            var results = SystemDAO.SqlHelperEx.GetTableText(sql, null);
            return results[0].Clone();
        }
        catch
        {
            return null;
        }
    }

    public static int UpdateDeviceInfo(DataRow deviceInfo)
    {
        Category cat = (Category)Convert.ToInt32(deviceInfo["s_category"]);

        string sql = @"
update tbl_summary_dev_title set s_ownerid = @s_ownerid, s_name = @s_name, s_assetid = @s_assetid, s_vender = @s_vender, 
s_cost = @s_cost, s_status = @s_status, s_image_url = @s_image_url, s_note = @s_note, s_date = GetDate() 
where s_id = @s_id";

        List<SqlParameter> paramList = new List<SqlParameter>();
        paramList.Add(new SqlParameter("@s_id", deviceInfo["s_id"]));
        paramList.Add(new SqlParameter("@s_assetid", deviceInfo["s_assetid"]));
        //paramList.Add(new SqlParameter("@", summary.s_category));
        paramList.Add(new SqlParameter("@s_cost", deviceInfo["s_cost"]));
        paramList.Add(new SqlParameter("@s_image_url", deviceInfo["s_image_url"]));
        paramList.Add(new SqlParameter("@s_name", deviceInfo["s_name"]));
        paramList.Add(new SqlParameter("@s_note", deviceInfo["s_note"]));
        paramList.Add(new SqlParameter("@s_ownerid", deviceInfo["s_ownerid"]));
        paramList.Add(new SqlParameter("@s_status", deviceInfo["s_status"]));
        paramList.Add(new SqlParameter("@s_vender", deviceInfo["s_vender"]));

        try
        {
            int ret1 = SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramList.ToArray());
            if (ret1 >= 1)
            {
                switch (cat)
                {
                    case Category.Device:
                        sql = @"
update tbl_device_detail set d_customid = @d_customid, d_class = @d_class, d_interface = @d_interface, d_date = GetDate() 
where d_id = @d_id";

                        paramList.Clear();
                        paramList.Add(new SqlParameter("@d_id", deviceInfo["d_id"]));
                        paramList.Add(new SqlParameter("@d_customid", deviceInfo["d_customid"]));
                        paramList.Add(new SqlParameter("@d_class", deviceInfo["d_class"]));
                        paramList.Add(new SqlParameter("@d_interface", deviceInfo["d_interface"]));
                        break;
                    case Category.Equipment:
                        sql = @"
update tbl_equipment_detail set e_testing_time = @e_testing_time, e_avg_hr = @e_avg_hr, e_loan_day = @e_loan_day, e_lab_location = @e_lab_location, e_date = GetDate() 
where e_id = @e_id ";

                        paramList.Clear();
                        paramList.Add(new SqlParameter("@e_id", deviceInfo["e_id"]));
                        paramList.Add(new SqlParameter("@e_testing_time", deviceInfo["e_testing_time"]));
                        paramList.Add(new SqlParameter("@e_avg_hr", deviceInfo["e_avg_hr"]));
                        paramList.Add(new SqlParameter("@e_loan_day", deviceInfo["e_loan_day"]));
                        paramList.Add(new SqlParameter("@e_lab_location", deviceInfo["e_lab_location"]));
                        break;
                    case Category.Chamber:
                        sql = @"
update tbl_chamber_detail set c_avg_hr = @c_avg_hr, c_loan_day = @c_loan_day, c_lab_location = @c_lab_location, e_date = GetDate() 
where c_id = @c_id ";

                        paramList.Clear();
                        paramList.Add(new SqlParameter("@c_id", deviceInfo["c_id"]));
                        paramList.Add(new SqlParameter("@c_avg_hr", deviceInfo["c_avg_hr"]));
                        paramList.Add(new SqlParameter("@c_loan_day", deviceInfo["c_loan_day"]));
                        paramList.Add(new SqlParameter("@c_lab_location", deviceInfo["c_lab_location"]));
                        break;
                }

                ret1 = SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramList.ToArray());
            }
            return ret1;
        }
        catch
        {
            return -1;
        }
    }

}