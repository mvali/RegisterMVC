using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class PromoCodes
{
    public PromoCodes() { }
    public int PromoCodeV2_get_byname(string pc_name, ref DataTable dt)
    {
        string UsersUsername = "";
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@pc_name", SqlDbType.VarChar, pc_name.Length, pc_name),
data.MakeInParam("@usersid", SqlDbType.Int, 4, User.Id_get()),
data.MakeInParam("@UsersUsername", SqlDbType.VarChar, UsersUsername.Length, UsersUsername)
                                   };
            int r = data.RunProc("N4_spPromoCodeV2_get_byname", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
}
