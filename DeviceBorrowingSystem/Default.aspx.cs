using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Text;
//using System.Net.Mail;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Redirect("UserLogin.aspx");
        //记录使用信息
        //log4net.ILog log = log4net.LogManager.GetLogger("loginfo");
        //HttpBrowserCapabilities bc = Request.Browser;
        //log.Info("Browser: " + bc.Browser + "; IP: " + Request.UserHostAddress);

    }
    protected void btn_sendMail_Click(object sender, EventArgs e)
    {
        SendMailByWebSMTPServer();
    }

    private void SendMailByLocalSMTPServer()
    {

        MailMessage message = new MailMessage("zjl4023352012@163.com", "zjl4023352013@126.com");
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        message.Subject = "testing of mailing list";
        message.Body = "hi hadi .this email is just for test.";
        message.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient("127.0.0.1");
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;






        {
            //SmtpClient smtp = new SmtpClient();//实例化一个SmtpClient
            //smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;//将smtp的出站方式设为 Network





            //smtp.EnableSsl = false;//smtp服务器是否启用SSL加密
            //smtp.Host = "localhost";//指定 smtp 服务器地址
            //smtp.Port = 25;//指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去
            ////smtp.Credentials = new NetworkCredential("zjl4023352012@163.com", "Ivan_Zhang2012");
            //MailMessage mm = new MailMessage(); //实例化一个邮件类
            //mm.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            //string from = "zjl4023352013@126.com";
            //mm.From = new MailAddress(from, "Sender", Encoding.GetEncoding(936));
            ////收件方看到的邮件来源；
            ////第一个参数是发信人邮件地址
            ////第二参数是发信人显示的名称
            ////第三个参数是 第二个参数所使用的编码，如果指定不正确，则对方收到后显示乱码
            ////936是简体中文的codepage值
            //mm.To.Add(new MailAddress("zjl4023352012@163.com", "Receiver", Encoding.GetEncoding(936)));
            //mm.Subject = "Test"; //邮件标题
            //mm.SubjectEncoding = Encoding.GetEncoding(936);
            //// 这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码。
            //// 936是简体中文的pagecode，如果是英文标题，这句可以忽略不用
            //mm.IsBodyHtml = true; //邮件正文是否是HTML格式
            //mm.BodyEncoding = Encoding.GetEncoding(936);
            ////邮件正文的编码， 设置不正确， 接收者会收到乱码
            //mm.Body = "this is a test mail: " + from;//mm.IsBodyHtml = true时,此处可以是HTML格式
            //邮件正文
            try
            {
                //smtp.Send(mm); //发送邮件，如果不返回异常， 则大功告成了。
                smtpClient.Send(message);
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script>");
                return;
            }
            Response.Write("<script>alert('Great!!!!')</script>");
        }
    }
    private void SendMailByWebSMTPServer()
    {
        {
            SmtpClient smtp = new SmtpClient();//实例化一个SmtpClient
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//将smtp的出站方式设为 Network
            smtp.EnableSsl = false;//smtp服务器是否启用SSL加密
            smtp.Host = "smtp.wistron.com";//指定 smtp 服务器地址
            smtp.Port = 25;//指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去
            smtp.Credentials = new NetworkCredential("BILL_GE@wistron.com", "Wistron1234567890123");
            MailMessage mm = new MailMessage(); //实例化一个邮件类
            mm.Priority = MailPriority.High; //邮件的优先级，分为 Low, Normal, High，通常用 Normal即可
            mm.From = new MailAddress("BILL_GE@wistron.com", "Sender", Encoding.GetEncoding(936));
            //收件方看到的邮件来源；
            //第一个参数是发信人邮件地址
            //第二参数是发信人显示的名称
            //第三个参数是 第二个参数所使用的编码，如果指定不正确，则对方收到后显示乱码
            //936是简体中文的codepage值
            mm.To.Add(new MailAddress("BILL_GE@wistron.com", "Receiver", Encoding.GetEncoding(936)));
            mm.Subject = "Test"; //邮件标题
            mm.SubjectEncoding = Encoding.GetEncoding(936);
            // 这里非常重要，如果你的邮件标题包含中文，这里一定要指定，否则对方收到的极有可能是乱码。
            // 936是简体中文的pagecode，如果是英文标题，这句可以忽略不用
            mm.IsBodyHtml = true; //邮件正文是否是HTML格式
            mm.BodyEncoding = Encoding.GetEncoding(936);
            //邮件正文的编码， 设置不正确， 接收者会收到乱码
            mm.Body = "this is a test mail";//mm.IsBodyHtml = true时,此处可以是HTML格式
            //邮件正文
            try
            {
                smtp.Send(mm); //发送邮件，如果不返回异常， 则大功告成了。
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script>");
                return;
            }
            Response.Write("<script>alert('Great!!!!')</script>");
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

    }
}