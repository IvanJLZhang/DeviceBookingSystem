using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// DeviceFilters 的摘要说明
/// </summary>
public class DeviceFilters
{
	public DeviceFilters()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public string Category;


    public string Department;
    public string Chamber;
    public string Site;


    public string Project_ID;
    public string TestCategory_ID;

    public DateTime StartDate;
    public DateTime EndDate;

    public DateTime? StartDateNull;
    public DateTime? EndDateNull;
}