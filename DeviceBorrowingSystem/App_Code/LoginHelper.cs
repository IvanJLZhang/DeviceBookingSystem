using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// 和用户登录、注册等相关的处理类
/// </summary>
public class LoginHelper
{
    public LoginHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        TrimCaptchaTable();
    }
    #region properties

    /// <summary>
    /// 验证码的最大长度
    /// </summary>
    public int MaxLength
    {
        get { return 10; }
    }
    /// <summary>
    /// 验证码的最小长度
    /// </summary>
    public int MinLength
    {
        get { return 1; }
    }

    /// <summary>
    /// 验证码的失效时间间隔（24小时）
    /// </summary>
    public TimeSpan ExpirationDuration
    {
        get { return TimeSpan.FromHours(24); }
    }
    #endregion

    #region private methods
    /// <summary>
    /// 整理存储验证码的数据库表，主要是删除过期的验证码条目
    /// </summary>
    /// <returns></returns>
    private Int32 TrimCaptchaTable()
    {
        string sql = @"
delete from tbl_captcha 
where (cp_expiration < @cp_expiration) ";
        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@cp_expiration", DateTime.Now) });
        }
        catch { return -1; }
    }
    #endregion

    #region public methods

    /// <summary>
    /// 生成随机验证码(数字)明文
    /// </summary>
    /// <param name="length">验证码的长度（默认为4位）</param>
    /// <returns></returns>
    public string GenerateCaptchaCleartext(int length = 4)
    {
        if (length > MaxLength || length < MinLength)
        {
            return null;
        }
        int[] randMembers = new int[length];
        int[] captchaNums = new int[length];
        // 生成起始序列值
        int seekSeek = unchecked((int)DateTime.Now.Ticks);
        Random seekRand = new Random(seekSeek);

        int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
        int[] seeks = new int[length];
        for (int index = 0; index != length; index++)
        {
            beginSeek += 10000;
            seeks[index] = beginSeek;
        }
        // 生成随机数字
        for (int index = 0; index != length; index++)
        {
            Random rand = new Random(seeks[index]);
            int powNum = 1 * (int)Math.Pow(10, length);
            randMembers[index] = rand.Next(powNum, Int32.MaxValue);// 保证生成的随机数字大于等于四位数字
        }
        // 抽取随机数字
        for (int index = 0; index != length; index++)
        {
            string numStr = randMembers[index].ToString();
            int numLength = numStr.Length;
            Random rand = new Random();
            int numPosition = rand.Next(0, numLength - 1);

            captchaNums[index] = Int32.Parse(numStr.Substring(numPosition, 1));
        }

        string capthcaStr = String.Empty;
        // 生成验证码
        for (int index = 0; index != length; index++)
        {
            capthcaStr += captchaNums[index].ToString();
        }
        return capthcaStr;
    }

    /// <summary>
    /// 文本加密
    /// </summary>
    /// <param name="clearText">明文</param>
    /// <param name="identityText">身份信息</param>
    /// <returns></returns>
    public string GenerateCaptchaCiphertext(string clearText, string identityText)
    {
        string str = clearText + identityText;
        return common.GetMD5Str(str);
    }

    /// <summary>
    /// 将验证码保存到数据库中
    /// </summary>
    /// <param name="cleartext"></param>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    public int InsertCaptchaToDB(string cleartext, string ciphertext)
    {
        string sql = @"
insert into tbl_captcha (cp_cleartext, cp_ciphertext, cp_expiration, cp_date)
values (@cp_cleartext, @cp_ciphertext, @cp_expiration, GetDate())";

        List<SqlParameter> paramList = new List<SqlParameter>();
        paramList.Add(new SqlParameter("@cp_cleartext", cleartext));
        paramList.Add(new SqlParameter("@cp_ciphertext", ciphertext));
        paramList.Add(new SqlParameter("@cp_expiration", DateTime.Now.Add(this.ExpirationDuration)));
        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, paramList.ToArray());
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// 根据密文检索出验证码信息
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    public DataRow CheckCaptcha(string ciphertext)
    {
        TrimCaptchaTable();
        string sql = @"
select * from tbl_captcha 
where (cp_ciphertext = @cp_ciphertext) ";

        try
        {
            var results = SystemDAO.SqlHelperEx.GetTableText(sql, new SqlParameter[] { new SqlParameter("@cp_ciphertext", ciphertext) });
            return results[0].Rows[0];
        }
        catch { return null; }
    }
    /// <summary>
    /// 更新验证码的使用状态
    /// </summary>
    /// <param name="ciphertext"></param>
    /// <returns></returns>
    public int UpdateCaptchaTable(string ciphertext)
    {
        TrimCaptchaTable();
        string sql = @"
update tbl_captcha set cp_check = 1 
where (cp_check = 0) 
and (cp_ciphertext = @cp_ciphertext) ";
        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@cp_ciphertext", ciphertext) });
        }
        catch { return -1; }
    }

    /// <summary>
    /// 注册状态更新
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static int RegisterStatusUpdate(string uid)
    {
        string sql = String.Empty;
        sql += @"
update tbl_Person set P_RegisterStatus = 1 
where P_ID = @P_ID";

        try
        {
            return SystemDAO.SqlHelperEx.ExecteNonQueryText(sql, new SqlParameter[] { new SqlParameter("@P_ID", uid) });
        }
        catch
        {
            return -1;
        }
    }
    #endregion
}