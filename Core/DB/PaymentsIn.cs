using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class PaymentsIn
{
    public int PaymentsIn_Add(int fk_idjob, string pi_FirstName, string pi_LastName, string pi_Email, decimal pi_amount, string pi_currency, string pi_TransactionCode, string pi_livemode)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@fk_idjob", SqlDbType.Int, 4, fk_idjob),
data.MakeInParam("@pi_FirstName", SqlDbType.VarChar, pi_FirstName.Length, pi_FirstName),
data.MakeInParam("@pi_LastName", SqlDbType.VarChar, pi_LastName.Length, pi_LastName),
data.MakeInParam("@pi_Email", SqlDbType.VarChar, pi_Email.Length, pi_Email),
data.MakeInParam("@pi_amount", SqlDbType.Money, 4, pi_amount),
data.MakeInParam("@pi_currency", SqlDbType.VarChar, pi_currency.Length, pi_currency),
data.MakeInParam("@pi_TransactionCode", SqlDbType.VarChar, pi_TransactionCode.Length, pi_TransactionCode),
data.MakeInParam("@pi_livemode", SqlDbType.VarChar, pi_livemode.Length, pi_livemode)
                                   };
            int r = data.RunProc("N4_spPaymentsIn_Add", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    /// <summary>
    /// Add paimentIn details
    /// </summary>
    /// <param name="id_job"></param>
    /// <param name="pi_id">paymentIn Id</param>
    /// <param name="pid_type">1:package, 2:service</param>
    /// <param name="pid_typeId">package/service Id</param>
    /// <returns></returns>
    public int PaymentsInDetails_Add(int id_job, int pi_id, int pid_type, int pid_typeId)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@id_job", SqlDbType.Int, 4, id_job),
data.MakeInParam("@pi_id", SqlDbType.Int, 4, pi_id),
data.MakeInParam("@pid_type", SqlDbType.Int, 4, pid_type),
data.MakeInParam("@pid_typeId", SqlDbType.Int, 4, pid_typeId)
                                   };
            int r = data.RunProc("N4_spPaymentsInDetails_Add", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
}