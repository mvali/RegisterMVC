using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class Packages
{
    public Packages() { }
    public int Packages_get_byCountryId(int country_id, ref DataTable dt)
    {
        string page = "register";
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@country_id", SqlDbType.Int, 4, country_id),
data.MakeInParam("@fk_users", SqlDbType.Int, 4, User.Id_get()),
data.MakeInParam("@jobtype", SqlDbType.Int, 4, 1),
data.MakeInParam("@page", SqlDbType.VarChar, page.Length, page)
                                   };
            int r = data.RunProc("N4_spPackages_get_byCountryId", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int JobPackageService_Add(int Id_Job, int fk_country, decimal c_value, int pk_id)
    {
        if (Id_Job <= 0)
            return 0;
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@fk_country", SqlDbType.Int, 4, fk_country),
data.MakeInParam("@UsersId", SqlDbType.Int, 4, User.Id_get()),
data.MakeInParam("@c_value", SqlDbType.Money, 4, c_value),
data.MakeInParam("@pk_id", SqlDbType.Int, 4, pk_id)
                                   };
            int r = data.RunProc("N4_spJobPackageService_Add", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int PackagesEmails_Get(int pk_id, int fk_users, ref DataTable dt)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
                                       data.MakeInParam("@pk_id", SqlDbType.Int, 4, pk_id),
                                       data.MakeInParam("@fk_users", SqlDbType.Int, 4, fk_users)
                               };
            int r = data.RunProc("N4_spPackagesEmails_Get", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public string PackagesEmails_GetOrder(int pk_id)
    {
        string ret = "";
        DataTable dt = new DataTable();
        int r = PackagesEmails_Get(pk_id, User.Id_get(), ref dt);
        if (r >= 0 && dt.Rows.Count > 0)
        {
            ret = dt.Rows[0]["pe_Order"].ToString();
        }
        return ret;
    }


}