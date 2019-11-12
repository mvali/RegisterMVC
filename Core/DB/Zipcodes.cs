using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class Zipcodes
{
    public Zipcodes() { }
    public int Zipcode_GetCitiesCountry(string ZipCode, ref DataTable dt)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@ZipCode", SqlDbType.VarChar, ZipCode.Length, ZipCode)
                                   };
            int r = data.RunProc("N4_spZipcode_GetCitiesCountry", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }

}