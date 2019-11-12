using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Services
{
    public int JobServices_addnew(int Id_Job, int package_id, int fk_services_id)
    {
        if (Id_Job <= 0)
            return 0;
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@package_id", SqlDbType.Int, 4, package_id),
data.MakeInParam("@fk_services_id", SqlDbType.Int, 4, fk_services_id)
                                   };
            int r = data.RunProc("N4_spJobServices_addnew", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int JobServices_Delete(int Id_Job, int package_id, int fk_services_id)
    {
        if (Id_Job <= 0)
            return 0;
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@package_id", SqlDbType.Int, 4, package_id),
data.MakeInParam("@fk_services_id", SqlDbType.Int, 4, fk_services_id)
                                   };
            int r = data.RunProc("N4_spJobServices_Delete", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Services_get_bylist_IdJob(string services_id_list, int Id_Job, int packageId, string orderby, string dir, ref DataTable dt)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@services_id_list", SqlDbType.VarChar, services_id_list.Length, services_id_list),
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@packageId", SqlDbType.Int, 4, packageId),
data.MakeInParam("@orderby", SqlDbType.VarChar, orderby.Length, orderby),
data.MakeInParam("@dir", SqlDbType.VarChar, dir.Length, dir)
                                   };
            int r = data.RunProc("N4_spServices_get_bylist_IdJob", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }

}
