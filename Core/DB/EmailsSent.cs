using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class EmailsSent
{
    public int Emails_AddNew(string EmailAddress, string EmailSubject, string EmailText)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@ea", SqlDbType.VarChar, EmailAddress.Length, EmailAddress),
data.MakeInParam("@es", SqlDbType.VarChar, EmailSubject.Length, EmailSubject),
data.MakeInParam("@et", SqlDbType.Text, EmailText.Length, EmailText)
                                   };
            int r = data.RunProc("aaa", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
}