using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class SiteEmails
{
    public int Emails_GetByName(string EmailName, string sitesName, ref DataTable dt)
    {
        string UsersUsername = "";
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@EmailName", SqlDbType.VarChar, EmailName.Length, EmailName),
data.MakeInParam("@SiteID", SqlDbType.Int, 4, 1),
data.MakeInParam("@fk_sitesId", SqlDbType.Int, 4, 1),
data.MakeInParam("@sitesName", SqlDbType.VarChar, sitesName.Length, sitesName),
data.MakeInParam("@UsersID", SqlDbType.Int, 4, User.Id_get()),
data.MakeInParam("@UsersUsername", SqlDbType.VarChar, UsersUsername.Length, UsersUsername)
                                   };
            int r = data.RunProc("N4_spEmails_GetByName", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
}