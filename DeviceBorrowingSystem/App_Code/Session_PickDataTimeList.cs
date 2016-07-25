using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Session_PickDataTimeList 的摘要说明
/// </summary>
public class Session_PickDataTimeList
{
	public Session_PickDataTimeList()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}
    public static void InitPickDataTimeList( ref DataTable pickDataTimeList)
    {
        if (pickDataTimeList == null)
        {
            pickDataTimeList = new DataTable();
            pickDataTimeList.Columns.Add(new DataColumn("id", typeof(int)));
            pickDataTimeList.PrimaryKey = new DataColumn[] { pickDataTimeList.Columns["id"] };
            pickDataTimeList.Columns["id"].AutoIncrement = true;
            pickDataTimeList.Columns["id"].AutoIncrementSeed = 1;
            pickDataTimeList.Columns["id"].AutoIncrementStep = 1;
            pickDataTimeList.Columns.Add(new DataColumn("Device_ID", typeof(String)));
            pickDataTimeList.Columns.Add(new DataColumn("StartDateTime", typeof(DateTime)));
            pickDataTimeList.Columns.Add(new DataColumn("EndDateTime", typeof(DateTime)));
        }
    }
}