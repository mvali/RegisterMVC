using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class User
{
    public static int Id_get()
    {
        int retV = -1;
        retV = Utils.Str2Int(CookieGet("UsersId"));
        retV = retV < 0 ? 1 : retV;
        return retV;
    }
    public static string CookieGet(string partnerDetail)
    {
        string retV = "";
        HttpCookie hq = HttpContext.Current.Request.Cookies["lbopartner"];
        if (hq != null)
            retV = hq.Values[partnerDetail];

        return retV;
    }


}