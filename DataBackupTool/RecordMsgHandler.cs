using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace DataBackupTool
{
    public enum Category
    {
        Device = 1, Equipment, Chamber
    }
    public class RecordMsgHandler
    {

        public void SendWarningMail()
        {
            // device, 提前三天提醒
            string sql = @"
select record.* 
, Loaner.P_Name as [LoanerName], Loaner.P_Email as [LoanerEmail] 
, Reviewer.P_Name as [ReviewerName], Reviewer.P_Email as [ReviewerEmail] 
, summary.s_name as [DeviceName], summary.s_category as [Category] 
from tbl_DeviceBooking as record 
inner join tbl_summary_dev_title as summary on record.Device_ID = summary.s_id 
inner join tbl_Person as [Loaner] on record.Loaner_ID = [Loaner].P_ID 
inner join tbl_Person as [Reviewer] on record.Reviewer_ID = [Reviewer].P_ID 
where (summary.s_category = 1) 
and (record.Status = 2) 

";
            //and (record.Warn_Date >= GetDate()) 
            try
            {
                DataTableCollection results = SystemDAO.SqlHelperEx.GetTableText(sql, null);
                DataTable result = results[0];
                foreach (DataRow row in result.Rows)
                {
                    DateTime date_from = Convert.ToDateTime(row["Loan_DateTime"]);
                    DateTime date_to = Convert.ToDateTime(row["Plan_To_ReDateTime"]);
                    DateTime date_warn = date_to.AddDays(-3);// 警告时间设定为截止日期前三天
                    if (date_warn <= date_from)
                    {// 如果开始时间大于警告时间， 以开始时间为准确的警告时间
                        date_warn = date_from;
                    }

                    if (DateTime.Now >= date_warn && DateTime.Now <= date_to)
                    {// 现在时间介于警告时间和结束时间之间， 发送警告邮件
                        SendMail(row, RecordOvergoingStatus.WARNNING);
                    }
                    else if (DateTime.Now > date_to)
                    {// 现在时间大于截止时间， 发送过期邮件
                        SendMail(row, RecordOvergoingStatus.OVERDUE);
                    }
                }
            }
            catch { }
        }

        void SendMail(DataRow record, RecordOvergoingStatus status)
        {
            string subject = String.Empty;
            StringBuilder body = new StringBuilder();
            // send to reviewer
            switch (status)
            {
                case RecordOvergoingStatus.NORMAL:
                    break;
                case RecordOvergoingStatus.WARNNING:
                    subject = "[Device Borrowing System]--Record Warning";
                    break;
                case RecordOvergoingStatus.OVERDUE:
                    subject = "[Device Borrowing System]--Record Overdue";
                    break;
            }
            body.Append("<h2>Hi, " + record["LoanerName"] + "&" + record["ReviewerName"] + ",</h2>");
            body.Append("<h3>    There is a borrow record need to be update status now!</h3>");
            body.Append("<h3>    Record Status: <font color='red'>" + status.ToString() + "</font></h3>");
            body.Append("<h2>Borrow Information</h2><br/>");
            body.Append(@"
<table border='1'>
<tr>
    <th>Booking_ID</th>
    <th>Device</th>
    <th>Category</th>
    <th>Loaner</th>
    <th>From</th>
    <th>To</th>
    <th>Comment</th>
</tr>");
            body.Append(@"
<tr>
    <td>" + record["Booking_ID"] + @"</td>
    <td>" + record["DeviceName"] + @"</td>
    <td>" + ((Category)Convert.ToInt32(record["Category"])).ToString() + @"</td>
    <td>" + record["LoanerName"] + @"</td>
    <td>" + Convert.ToDateTime(record["Loan_DateTime"]).ToString("yyyy/MM/dd HH:mm") + @"</td>
    <td>" + Convert.ToDateTime(record["Plan_To_ReDateTime"]).ToString("yyyy/MM/dd HH:mm") + @"</td>
    <td>" + record["Comment"] + @"</td>

</tr>");
            body.Append("</table><br/>");
            body.Append("Please sign in the borrowing system and update the item!<br/>");
            body.Append("http://tpeota01.whq.wistron:88/");

            string to = record["LoanerEmail"].ToString() + ";" + record["ReviewerEmail"].ToString();



            string id = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            MailService.AddOneMailService(id, to, "", subject, body.ToString(), true, "");
            Thread.Sleep(1000);
        }

        public void SendCheckMail()
        {
            MailService.AddOneMailService(DateTime.Now.ToString("yyyyMMddHHmmssfff"), "Ivan_JL_Zhang@wistron.com", "", "[Device Borrowing System]----Check", "The tool run OK!", true, "");
        }
    }


}
