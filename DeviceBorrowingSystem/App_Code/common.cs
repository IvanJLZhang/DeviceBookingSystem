using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// common 的摘要说明
/// </summary>
public class common
{
	public common()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 获取MD5值32位
    /// </summary>
    /// <param name="strSrc"></param>
    /// <returns></returns>
    public static string GetMD5Str(string strSrc)
    {
        MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] md5Bytes = md5.ComputeHash(Encoding.Default.GetBytes(strSrc));

        StringBuilder sbMd5Str = new StringBuilder();
        for (int index = 0; index != md5Bytes.Length; index++)
        {
            sbMd5Str.Append(md5Bytes[index]);
        }
        return sbMd5Str.ToString();
    }
    /// <summary>
    /// MD5 16位加密 加密后密码为大写
    /// </summary>
    /// <param name="ConvertString"></param>
    /// <returns></returns>
    public static string GetMd5Str16Bit(string ConvertString)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
        t2 = t2.Replace("-", "");
        return t2;
    }

    /// <summary>
    /// 在指定页面上弹出提示信息
    /// </summary>
    /// <param name="page"></param>
    /// <param name="strInfo"></param>
    public static void PopMsg(System.Web.UI.Page page, string msg)
    {
        if (msg == null || msg.CompareTo(String.Empty) == 0)
            return;
        msg = msg.Replace("\r", "\\r");
        msg = msg.Replace("\n", "\\n");
        //msg = msg.Replace("'", "\'");
        StringBuilder sbScript = new StringBuilder();
        sbScript.Append("<script>alert('");
        sbScript.Append(msg);
        sbScript.Append("')</script>");

        if (!page.ClientScript.IsStartupScriptRegistered("OperResult"))
        {
            page.ClientScript.RegisterStartupScript(page.ClientScript.GetType(), "OperaResult", sbScript.ToString());
        }
    }
}