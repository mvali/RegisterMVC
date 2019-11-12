using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

public class CQueryString : NameValueCollection
{
    public CQueryString(NameValueCollection qs)
        : base(qs)
    {
    }

    public CQueryString()
        : base()
    {
    }

    public CQueryString(string qs)
        : base()
    {
        if (string.IsNullOrWhiteSpace(qs))
            return;

        string[] arr = qs.Split('?');

        string qs1 = qs;
        if (arr.Length == 2)
            qs1 = arr[1];
        string[] name_value = qs1.Split('&');

        for (int i = 0; i < name_value.Length; i++)
        {
            string[] name_val = name_value[i].Split('=');
            if (name_val.Length == 1 || name_val.Length == 2)
            {
                string name = name_val[0];

                string val = "";
                if (name_val.Length == 2)
                    val = name_val[1];

                base.Add(name, val);
            }
        }
    }

    public string SerializeQueryString()
    {
        return SerializeQueryString(false);
    }

    public string SerializeQueryString(bool UrlEncode)
    {
        string str = "";
        IEnumerator en = base.GetEnumerator();

        while (en.MoveNext())
        {
            string key = null;
            try
            {
                key = en.Current.ToString();
            }
            catch (Exception ex)
            {
                continue;
            }

            string val = base.Get(key);
            if (val != null)
            {
                if (str.Length > 0)
                    str += "&";

                if (UrlEncode)
                {
                    str += key + "=" + System.Web.HttpUtility.UrlEncode(val);
                }
                else
                    str += key + "=" + val;
            }
        }

        return str;
    }

    public int GetUintFromQS(string code)
    {

        int i;
        try
        {
            if (base[code] == null)
                return -1;

            i = Convert.ToInt32(base[code]);

            if (i >= 0)
                return i;
            else
                return -1;
        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    public string GetStringFromQS(string code)
    {
        if (base[code] == null)
            return "";

        string strReturn = "";
        try
        {
            strReturn = Convert.ToString(base[code].ToString());
        }
        catch
        {
            strReturn = "";
        }
        return strReturn;
    }

    public CQueryString Copy()
    {
        CQueryString NewQueryString = new CQueryString();

        for (int i = 0; i < this.Count; i++)
            NewQueryString.Add(this.Keys[i], this[i]);

        return NewQueryString;

    }

    public new CQueryString Add(string name, string val)
    {
        base.Add(name, val);

        return this;
    }

    public CQueryString Add(string name, string val, bool delete)
    {
        if (delete && base[name] != null)
            base.Remove(name);

        base.Add(name, val);

        return this;
    }

    public string FormateNewMessageUrl(string StringToFormate, string nametoadd, string nametoaddValue)
    {
        string[] arr = StringToFormate.Split('?');

        string qs1 = "";
        if (arr.Length == 2)
            qs1 = arr[1];

        CQueryString qs = new CQueryString(qs1);
        qs.Add(nametoadd, nametoaddValue, true);

        return arr[0] + "?" + qs.SerializeQueryString();

    }

    public string FormateNewMessageUrl(string StringToFormate, string nametoremove)
    {
        string[] arr = StringToFormate.Split('?');

        string qs1 = "";
        if (arr.Length == 2)
            qs1 = arr[1];

        CQueryString qs = new CQueryString(qs1);

        qs.Remove(nametoremove);
        return arr[0] + (qs1.Length > 0 ? "?" + qs.SerializeQueryString() : "");
    }

    public void UrlVariableExists(string parameters, string nametocheck, string valuetocheck, ref bool retValue)
    {
        if (!retValue)
        {
            if (parameters.Length > 0)
            {
                string[] arr = parameters.Split('?');

                string qs1 = "";
                if (arr.Length > 0)
                {
                    if (arr.Length == 2)
                        qs1 = arr[1];
                    else
                        qs1 = arr[0];
                }

                CQueryString qs = new CQueryString(qs1);
                string qsvalue = qs[nametocheck];

                //System.Web.HttpContext.Current.Response.Write("|" + qsvalue.ToLower().Trim() +"="+ valuetocheck.ToLower() + "<BR>");

                if (qsvalue.ToLower().Trim() != valuetocheck.ToLower().Trim())
                    retValue = true;
                else
                    retValue = false;
            }
            else
                retValue = false;
        }
    }

    public void UrlVariablesCheck(string oldparameters, string nametocheck, string valuetocheck, ref bool changeExist, ref string currentparameters)
    {
        if (!changeExist)
        {
            UrlVariableExists(oldparameters, nametocheck, valuetocheck, ref changeExist);
        }

        currentparameters = FormateNewMessageUrl(currentparameters, nametocheck, valuetocheck);
    }

    public void UrlVariablesCheck2(string oldparameters, string nametocheck, string valuetocheck, ref bool changeExist, ref string currentparameters)
    {
        if (!changeExist)
            UrlVariableExists(oldparameters, nametocheck, valuetocheck, ref changeExist);

        currentparameters = FormateNewMessageUrl(currentparameters, nametocheck, valuetocheck);
    }



}
