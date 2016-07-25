using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SystemDAO;
namespace DataBackupTool
{
    class MailService
    {
        public MailService()
        {
        }
        public string errMsg = String.Empty;
        public bool SendMailAuto()
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("Select * From tbl_Message ORDER BY CreateDataTime");

            DataTableCollection tables = null;
            try
            {
                tables = SqlHelperEx.GetTableText(cmdText.ToString(), null);
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }

            DataTable result = tables[0];

            if (result.Rows.Count <= 0)
            {
                errMsg = "no data";
                return false;
            }
            ArrayList sendedItem = new ArrayList();
            for (int index = 0; index != result.Rows.Count; index++)
            {
                DataRow row = result.Rows[index];
                string to = row["MailTo"].ToString();
                string subject = row["Subject"].ToString();
                string body = row["Body"].ToString() + "\r\n\r\n\r\n" + row["Signature"].ToString();
                string cc = row["CC"].ToString();
                bool isHtmlBody = bool.Parse(row["IsHtmlBody"].ToString());

                if (SendMailByOutlook(to, subject, body, cc, isHtmlBody))
                {
                    sendedItem.Add(row["id"]);
                }
            }

            DeleteSendedItemInDataBase(sendedItem);
            return false;
        }
        private void DeleteSendedItemInDataBase(ArrayList sendedItem)
        {
            for (int index = 0; index != sendedItem.Count; index++)
            {
                StringBuilder cmdText = new StringBuilder();
                cmdText.Append("Delete From tbl_Message Where id=@ID");

                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ID", sendedItem[index]);

                int result = 0;
                try
                {
                    result = SqlHelperEx.ExecteNonQueryText(cmdText.ToString(), param);
                }
                catch (System.Exception ex)
                {
                    errMsg = ex.Message;
                    continue;
                }
            }
        }
        private bool SendMailByOutlook(string to, string subject, string body, string cc, bool isHtmlBody)
        {
            Application outObjt = new Application();
            MailItem mailItem = (MailItem)outObjt.CreateItem(OlItemType.olMailItem);

            mailItem.To = to;
            mailItem.Subject = subject;
            if (isHtmlBody)
            {
                mailItem.HTMLBody = body;
            }
            else
            {
                mailItem.Body = body;
            }
            mailItem.CC = cc;
            try
            {
                ((_MailItem)mailItem).Send();
            }
            catch (System.Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }


        public static bool AddOneMailService(string id, string to, string cc, string subject, string body, bool ishtmlbody, string signature)
        {
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("Insert into tbl_Message values(@ID, @MailTo, @CC, @Subject, @Body,@IsHtmlBody, @Signature, GetDate(), 0)");

            SqlParameter[] param = new SqlParameter[7];

            param[0] = new SqlParameter("@ID", id);
            param[1] = new SqlParameter("@MailTo", to);
            param[2] = new SqlParameter("@CC", cc);
            param[3] = new SqlParameter("@Subject", subject);
            param[4] = new SqlParameter("@Body", body);
            param[5] = new SqlParameter("@Signature", signature);
            param[6] = new SqlParameter("@IsHtmlBody", ishtmlbody);
            try
            {
                SqlHelperEx.ExecteNonQueryText(cmdText.ToString(), param);
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
