using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web.Mvc;
using System.Web.UI.WebControls;

public static class Utils
{
    public static object TempDataU(string keyName, System.Web.Mvc.TempDataDictionary dict, object defaultValue)
    {
        var retV = defaultValue;
        try
        {
            var keyValue = dict[keyName];
            if (keyValue != null)
                retV = keyValue;
        }
        catch { }
        return retV;
    }

    public static string TempData(string keyName, System.Web.Mvc.TempDataDictionary dict, string defaultValue = "")
    {
        return TempDataU(keyName, dict, defaultValue).ToString();
    }
    public static int Null2Int(int? a)
    {
        var retV = 0;
        if (a != null)
        {
            retV = a.GetValueOrDefault();
        }
        return retV;
    }
    public static string Null2Str(string a)
    {
        var retV = "";
        if (string.IsNullOrWhiteSpace(a))
        {
            retV = a;
        }
        return retV;
    }





    public static string Base64Encode(string m_enc)
    {
        //return m_enc;
        byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(m_enc);
        string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
        returnValue = Base64Scramble(returnValue);
        return returnValue;
    }
    public static string Base64Decode(string m_enc)
    {
        string returnValue = "";
        try
        {
            m_enc = Base64UnScramble(m_enc);
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(m_enc);
            returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
        }
        return returnValue;
    }
    public static string Base64Scramble(string s)
    {
        if (s == null) return null;
        char[] charArray = s.ToCharArray();
        string char1 = "", char2 = "", char3 = "", char4 = "";
        int j = 0;
        for (int i = 0; i < charArray.Length; i++)
        {
            j++;
            switch (j)
            {
                case 1: char1 += charArray[i]; break;
                case 2: char2 += charArray[i]; break;
                case 3: char3 += charArray[i]; break;
                case 4: char4 += charArray[i]; break;
            }
            if (j == 4)
                j = 0;
        }
        return char4 + char3 + char2 + char1;
    }
    public static string Base64UnScramble(string s)
    {
        if (s == null) return null;
        string char1 = "", char2 = "", char3 = "", char4 = "";

        double fraction = (double)s.Length / 4;
        int cntAll = (int)System.Math.Ceiling(fraction);
        int cntFirst = (int)System.Math.Floor(fraction);
        int cntRound = (int)System.Math.Round(fraction, 0);
        int h = cntRound == cntAll ? cntAll : cntFirst;
        char4 = s.Substring(0, cntFirst);
        char3 = s.Substring(cntFirst, h);
        char2 = s.Substring(cntFirst + h);
        char1 = s.Substring(cntFirst + 2 * h);

        s = "";
        for (int i = 0; i < cntAll; i++)
        {
            if (char1.Length > i)
                s += char1.Substring(i, 1);
            if (char2.Length > i)
                s += char2.Substring(i, 1);
            if (char3.Length > i)
                s += char3.Substring(i, 1);
            if (char4.Length > i)
                s += char4.Substring(i, 1);
        }
        return s;
    }
    public static string MapPath(string path)
    {
        try
        {
            return System.Web.Hosting.HostingEnvironment.MapPath(path);
        }
        catch (Exception ex)
        {
            return path;
        }
    }
    public static string SessionGet(string key, string defaultVal = "")
    {
        string retValue = defaultVal;
        try
        {
            if (System.Web.HttpContext.Current.Session[key] != null)
                retValue = System.Web.HttpContext.Current.Session[key].ToString();
        }
        catch { }
        return retValue;
    }
    public static void SessionRemove(string key)
    {
        try
        {
            if (System.Web.HttpContext.Current.Session[key] != null)
            {
                System.Web.HttpContext.Current.Session.Remove(key);
            }
        }
        catch { }
    }
    public static int Str2Int(object source, int defaultVal = -1)
    {
        decimal tmp;
        int retVal = decimal.TryParse(source.ToString(), out tmp) ? (int)tmp : defaultVal;
        return retVal;
    }
    public static decimal Str2Decimal(object source, decimal defaultVal = -1)
    {
        decimal retVal = defaultVal;
        if (Decimal.TryParse(source.ToString(), out retVal))
            return retVal;
        return defaultVal;
    }

    public static bool Str2Bool(string strValue, bool bDefaultValue = false)
    {
        bool bRet = bDefaultValue;
        try
        {
            bRet = Convert.ToBoolean(strValue);
        }
        catch
        {
            if (strValue == "1" || strValue.ToLower() == "true")
                bRet = true;
            if (strValue == "0" || strValue.ToLower() == "false")
                bRet = false;
        }
        return bRet;
    }

    /// <summary>
    /// System.Configuration.ConfigurationManager.AppSettings[appSettingsName].ToString()
    /// </summary>
    /// <param name="appSettingsName"></param>
    /// <returns>string</returns>
    public static string AppSetting(string appSettingsName)
    {
        string sRet = null;
        try
        {
            sRet = System.Configuration.ConfigurationManager.AppSettings[appSettingsName].ToString();
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
        }
        return sRet;
    }
    /// <summary>
    /// "_"+ Response.Write(strValue)+"|" only if AppSetting("responseWrite") in web.config is set to "on"
    /// </summary>
    /// <param name="strValue">text to be writen on page</param>
    /// <param name="backNewLine">Add new line after text</param>
    /// <param name="frontNewLine">Add new line in front of text</param>
    public static void rw(string strValue, Boolean backNewLine = false, Boolean frontNewLine = false)
    {
        string masterWrite = AppSetting("responseWrite");
        if (masterWrite == "on")
        {
            string strRet = "_" + strValue + "|";
            if (frontNewLine)
            {
                strRet = "</br>" + strRet;
            }
            if (backNewLine)
            {
                strRet = strRet + "</br>";
            }
            System.Web.HttpContext.Current.Response.Write(strRet);
        }
    }

    public static bool ControlerViewIs(string controlerName, string actionName, ViewContext vc)
    {
        bool retV = false;
        if (string.Equals(vc.RouteData.Values["controller"].ToString(), controlerName, StringComparison.CurrentCultureIgnoreCase))
        {
            if (string.Equals(vc.RouteData.Values["action"].ToString(), actionName, StringComparison.CurrentCultureIgnoreCase)) retV = true;
            else
                if (string.IsNullOrWhiteSpace(actionName)) retV = true;
        }
        return retV;
    }
    public static decimal MathRound(decimal dValue, int iDecimals = 2)
    {
        decimal dReturn = dValue;
        try
        {
            dReturn = Math.Round(dValue, iDecimals);
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
        }
        return dReturn;
    }
    public static decimal MathRound(string sValue, int iDecimals = 2)
    {
        decimal dReturn = Str2Decimal(sValue);
        if (sValue.Length == 0)
            dReturn = 0;

        return MathRound(dReturn, iDecimals);
    }
    public static string rq(string keyName)
    {
        string sRetValue = "";
        string sKeyname = keyName.ToLower();
        System.Web.HttpRequest r = System.Web.HttpContext.Current.Request;
        System.Collections.Specialized.NameValueCollection n = r.QueryString;
        if (n.HasKeys())
        {
            for (int i = 0; i < n.Count; i++)
            {
                if (n.GetKey(i) != null)
                {
                    string key = n.GetKey(i).ToLower();
                    if (key == sKeyname)
                    {
                        sRetValue = n.Get(i);
                        break;
                    }
                }
            }
        }
        return sRetValue;
    }
    public static T IsNull<T>(this T testValue, T defaultValue)
    {
        return testValue == null? defaultValue: testValue;
    }

    /// <summary>
    /// Request Object Form Value
    /// </summary>
    /// <param name="FormObject">WebControls, HtmlControls</param>
    /// <param name="sDefaultValue"></param>
    /// <returns>String</returns>
    public static string rf(object FormObject, string sDefaultValue = null)
    {
        string sRet = sDefaultValue;
        if (FormObject != null)
        {
            try
            {
                TextBox obj = (TextBox)FormObject;
                sRet = obj.Text;
            }
            catch { }
            finally { }
            try
            {
                System.Web.UI.HtmlControls.HtmlInputText obj = (System.Web.UI.HtmlControls.HtmlInputText)FormObject;
                sRet = obj.Value;
            }
            catch { }
            try
            {
                System.Web.UI.HtmlControls.HtmlInputHidden obj = (System.Web.UI.HtmlControls.HtmlInputHidden)FormObject;
                sRet = obj.Value;
            }
            catch { }
            try
            {
                System.Web.UI.HtmlControls.HtmlTextArea obj = (System.Web.UI.HtmlControls.HtmlTextArea)FormObject;
                sRet = obj.InnerText;
            }
            catch { }
            try
            {
                DropDownList obj = (DropDownList)FormObject;
                sRet = obj.SelectedItem.Value;
            }
            catch { }
            try
            {
                System.Web.UI.HtmlControls.HtmlSelect obj = (System.Web.UI.HtmlControls.HtmlSelect)FormObject;
                sRet = obj.Value;
            }
            catch { }
            try
            {
                RadioButtonList obj = (RadioButtonList)FormObject;
                sRet = obj.SelectedItem.Value;
            }
            catch { }
            try
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox obj = (System.Web.UI.HtmlControls.HtmlInputCheckBox)FormObject;
                sRet = obj.Value;
            }
            catch { }
            try
            {
                System.Web.UI.HtmlControls.HtmlInputRadioButton obj = (System.Web.UI.HtmlControls.HtmlInputRadioButton)FormObject;
                sRet = obj.Value;
            }
            catch { }
        }
        return sRet;
    }
    public static string rf(string ObjectName)
    {
        string sDefaultValue = null;
        string sRet = sDefaultValue;
        if (!String.IsNullOrEmpty(ObjectName))
        {
            try
            {
                sRet = System.Web.HttpContext.Current.Request.Form[ObjectName];
            }
            catch { }
        }
        return sRet;
    }

    /// <summary>
    /// Request Object true/false Flag
    /// </summary>
    /// <param name="FormObject">CheckBox, HtmlInputCheckBox</param>
    /// <param name="sDefaultValue"></param>
    /// <returns>boolean</returns>
    public static Boolean rfb(object FormObject, Boolean sDefaultValue = false)
    {
        Boolean bRet = sDefaultValue;
        if (FormObject != null)
        {
            try
            {
                CheckBox obj = (CheckBox)FormObject;
                bRet = obj.Checked;
            }
            catch { }
            try
            {
                System.Web.UI.HtmlControls.HtmlInputCheckBox obj = (System.Web.UI.HtmlControls.HtmlInputCheckBox)FormObject;
                bRet = obj.Checked;
            }
            catch { }
        }
        return bRet;
    }
    public static string MathDecimals(object sValue, int iDecimals = 2)
    {
        decimal dbl = Str2Decimal(sValue.ToString());
        return string.Format("{0:N2}", dbl);
    }
    /// <summary>
    /// Used in DB for string parameter length
    /// </summary>
    /// <param name="sVal"></param>
    /// <returns></returns>
    public static int StrLen(string sVal)
    {
        if (string.IsNullOrWhiteSpace(sVal))
            return 0;
        return sVal.Length;
    }
    public static DateTime? Str2Date(string strValue, DateTime? dDefaultValue = null, string cultureName = "en-US")
    {
        DateTime? dd = dDefaultValue;
        System.Globalization.CultureInfo culture = null;
        try
        {
            try
            {
                culture = new System.Globalization.CultureInfo(cultureName);
            }
            catch { }
            if (culture == null)
            {
                dd = Convert.ToDateTime(strValue);
            }
            else
            {
                dd = Convert.ToDateTime(strValue, culture);
            }
        }
        catch { }
        return dd;
    }
    public static DateTime Str2DateNN(string strValue)
    {
        return Str2DateNN(strValue, "");
    }
    public static DateTime Str2DateNN(string strValue, string cultureName = "en-US")
    {
        DateTime dd = DateTime.Now;
        System.Globalization.CultureInfo culture = null;
        try
        {
            if (cultureName.Length == 0)
            {
                dd = Convert.ToDateTime(strValue);
            }
            else
            {
                try
                {
                    culture = new System.Globalization.CultureInfo(cultureName);
                }
                catch { }
                if (culture == null)
                {
                    dd = Convert.ToDateTime(strValue);
                }
                else
                {
                    dd = Convert.ToDateTime(strValue, culture);
                }
            }
        }
        catch { }
        return dd;
    }
    public static string Str2Date2Str(string strValue, string ToStringFormat = "MM/dd/yyyy HH:MM")
    {
        string strRet = "";
        if (Str2Date(strValue) != null)
        {
            strRet = Str2DateNN(strValue).ToString(ToStringFormat);
        }
        return strRet;
    }


}