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
data.MakeInParam("@EmailAddress", SqlDbType.VarChar, EmailAddress.Length, EmailAddress),
data.MakeInParam("@EmailSubject", SqlDbType.VarChar, EmailSubject.Length, EmailSubject),
data.MakeInParam("@EmailText", SqlDbType.Text, EmailText.Length, EmailText)
                                   };
            int r = data.RunProc("N4_spEmails_AddNew", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
}