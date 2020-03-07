using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Variables
{
    public int Admin_Variable_GetByName(string name, ref DataTable dt)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@n", SqlDbType.VarChar, name.Length, name),
data.MakeInParam("@s", SqlDbType.Int, 4, 1)
                                   };
            int r = data.RunProc("aa", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public static string VariableGetByName(string name)
    {
        // just a fix from stone age programming
        string sRet = "";
        System.Data.DataTable dt = new System.Data.DataTable();
        int r = new Variables().Admin_Variable_GetByName(name, ref dt);

        if (dt.Rows.Count > 0)
            sRet = dt.Rows[0]["Valor"].ToString().Trim();

        sRet = sRet.Replace(System.Environment.NewLine, "");
        return sRet.Trim();
    }

}