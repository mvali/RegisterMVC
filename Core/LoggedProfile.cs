using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ut = Utils;

public class LoggedProfile
{
    [System.ComponentModel.DefaultValue(-1)]
    public int Id { get; set; } =-1;
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DataRow data { get; set; }
    public LoggedProfile(string sEmail = "", string sPassword = "")
    {
        Email = sEmail;
        Password = sPassword;
        Id = -1;
    }
    public LoggedProfile(string sEmail, string sPassword, int iId, string sFirstName, string sLastName, string sPhone, DataRow drData)
    {
        Id = iId;
        Email = sEmail;
        Password = sPassword;
        FirstName = sFirstName;
        LastName = sLastName;
        Phone = sPhone;
        data = drData;
    }
    public string content(string columnName)
    {
        string strRet = "";
        try
        {
            strRet = data[columnName].ToString();
        }
        catch { }
        return strRet;
    }
    public int contenti(string columnName)
    {
        int iRet = ut.Str2Int(content(columnName));
        return iRet;
    }
    public bool contentb(string columnName)
    {
        bool bRet = ut.Str2Bool(content(columnName));
        return bRet;
    }

    public static LoggedProfile LoggedProfileBuild(DataTable dt)
    {
        LoggedProfile lu = null;
        if (dt.Rows.Count < 1)
            return lu;

        lu = new LoggedProfile(dt.Rows[0]["JobEmail"].ToString(),
            dt.Rows[0]["JobPassword"].ToString(),
            ut.Str2Int(dt.Rows[0]["Id_Job"].ToString()),
            dt.Rows[0]["JobFirstName"].ToString(),
            dt.Rows[0]["JobLastName"].ToString(),
            dt.Rows[0]["JobPhone"].ToString(),
            dt.Rows[0]);
        System.Web.HttpContext.Current.Session[LoggedCookieName()] = lu;
        LoggedCookieWrite(lu);
        return lu;
    }
    public static string LoggedCookieName()
    {
        string sRet = "logged";
        sRet += "job";
        return sRet;
    }
    static void LoggedCookieWrite(LoggedProfile lu)
    {
        HttpCookie ck = new HttpCookie(LoggedCookieName());
        ck.Value = ut.Base64Encode("email=" + lu.Email + "&password=" + lu.Password);
        ck.Expires = System.DateTime.Now.AddDays(14);
        ck.Secure = true;
        ck.HttpOnly = true;
        System.Web.HttpContext.Current.Response.Cookies.Add(ck);
    }
    public static LoggedProfile logged
    {
        get
        {
            LoggedProfile lu = null;
            try
            {
                lu = (LoggedProfile)System.Web.HttpContext.Current.Session[LoggedCookieName()];
                if (lu == null)
                {
                    lu = LoggedCookieRead();
                }
            }
            catch (Exception ex)
            {
                lu = LoggedCookieRead();
            }
            if (lu == null)
                lu = new LoggedProfile();
            return lu;
        }set { }
    }
    static LoggedProfile LoggedCookieRead()
    {
        LoggedProfile lu = null;
        HttpCookie ck = System.Web.HttpContext.Current.Request.Cookies[LoggedCookieName()];

        if (ck != null)
        {
            try
            {
                string strContent = ut.Base64Decode(ck.Value);
                CQueryString qs = new CQueryString(strContent);
                string email = qs["email"];
                string password = qs["password"];
                if (!String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(password))
                {// load from db
                    lu = LoggedDbRead(email, password);
                }
            }
            catch (Exception ex)
            {
                Log.AddLogEntry(ex.ToString());
            };
        }
        return lu;
    }
    public static LoggedProfile LoggedDbRead(string email, string password)
    {
        LoggedProfile lu = null;
        DataTable dt = new DataTable();
        int r = new Jobs().Job_Login(email, password, ref dt);
        if (dt.Rows.Count > 0)
        {
            lu = LoggedProfileBuild(dt);
        }else
        {
            lu = new LoggedProfile();
        }
        return lu;
    }
    public static void LoggedDestroy()
    {
        System.Web.HttpContext.Current.Session[LoggedCookieName()] = null;
        System.Web.HttpContext.Current.Session["UserDetails"] = null;

        HttpCookie ck = System.Web.HttpContext.Current.Request.Cookies[LoggedCookieName()];
        if (ck != null)
        {
            ck.Expires = System.DateTime.Now.AddDays(-1);
            ck.Secure = true;
            ck.HttpOnly = true;

            System.Web.HttpContext.Current.Response.Cookies.Add(ck);
        }
    }

}