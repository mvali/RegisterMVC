using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class User
{
    public static string CookieGet(string partnerDetail)
    {
        string retV = "";
        HttpCookie hq = HttpContext.Current.Request.Cookies["p"];
        if (hq != null)
            retV = hq.Values[partnerDetail];

        return retV;
    }


}