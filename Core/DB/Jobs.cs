using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public class Jobs
{
    public Jobs() { }
    public int Job_AddPayment(int Id_Job, string JobPaypalAmount, string JobPaypalAmount_USD, string JobPaypalVariables)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@JobPaypalAmount", SqlDbType.VarChar, JobPaypalAmount.Length, JobPaypalAmount),
data.MakeInParam("@JobPaypalAmount_USD", SqlDbType.VarChar, JobPaypalAmount_USD.Length, JobPaypalAmount_USD),
data.MakeInParam("@JobPaypalVariables", SqlDbType.VarChar, JobPaypalVariables.Length, JobPaypalVariables)
                                   };
            int r = data.RunProc("N4_spJob_AddPayment", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_GetByEmail(string JobEmail, ref DataTable dt)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@JobEmail", SqlDbType.VarChar, JobEmail.Length, JobEmail)
                                   };
            int r = data.RunProc("N4_spJob_GetByEmail", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_AddCheck(int Id_Job, int job_id_parent, string JobEmail, string JobZIP, string JobState, string services_id_list_topay, int country_id)
    {
        try
        {
            DataTable dt = new DataTable();
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@JobEmail", SqlDbType.VarChar, JobEmail.Length, JobEmail),
data.MakeInParam("@JobZIP", SqlDbType.VarChar, JobZIP.Length, JobZIP),
data.MakeInParam("@JobState", SqlDbType.VarChar, JobState.Length, JobState),
data.MakeInParam("@jobtype", SqlDbType.Int, 4, 0),
data.MakeInParam("@services_id_list_topay", SqlDbType.VarChar, services_id_list_topay.Length, services_id_list_topay),
data.MakeInParam("@country_id", SqlDbType.Int, 4, country_id),
data.MakeInParam("@job_id_parent", SqlDbType.Int, 4, job_id_parent)
                                   };
            int r = data.RunProc("N4_spJob_AddCheck", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_Add(int Id_Job, string JobFirstName, string JobLastName, string JobEmail, string JobPassword, string JobAddress, int fk_country, string JobCity, string JobState, string JobZIP, string JobPhone, string JobPromoCode, 
        string JobSessionDate, int Id_Photographer, string referer, int fk_c_id, decimal c_value, decimal Price, int pk_id, string visitDate, int bannerId, int job_mailinglist, decimal pc_discountAmount, int job_id_parent, 
        string JobAddressHome, string JobCityHome, string JobZIPHome, string JobStateHome, int fk_countryHome)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@JobFirstName", SqlDbType.VarChar, JobFirstName.Length, JobFirstName),
data.MakeInParam("@JobLastName", SqlDbType.VarChar, JobLastName.Length, JobLastName),
data.MakeInParam("@JobEmail", SqlDbType.VarChar, JobEmail.Length, JobEmail),
data.MakeInParam("@JobPassword", SqlDbType.VarChar, JobPassword.Length, JobPassword),
data.MakeInParam("@JobAddress", SqlDbType.Text, Utils.StrLen(JobAddress), JobAddress),
data.MakeInParam("@fk_country", SqlDbType.Int, 4, fk_country),
data.MakeInParam("@JobCity", SqlDbType.VarChar, JobCity.Length, JobCity),
data.MakeInParam("@JobState", SqlDbType.VarChar, JobState.Length, JobState),
data.MakeInParam("@JobZIP", SqlDbType.VarChar, JobZIP.Length, JobZIP),
data.MakeInParam("@JobPhone", SqlDbType.VarChar, JobPhone.Length, JobPhone),
data.MakeInParam("@JobPromoCode", SqlDbType.VarChar, JobPromoCode!=null?JobPromoCode.Length:1, JobPromoCode),
data.MakeInParam("@JobSessionDate", SqlDbType.VarChar, JobSessionDate.Length, JobSessionDate),
data.MakeInParam("@Id_Photographer", SqlDbType.Int, 4, Id_Photographer),
data.MakeInParam("@UsersId", SqlDbType.Int, 4, User.Id_get()),
data.MakeInParam("@referer", SqlDbType.VarChar, referer.Length, referer),
data.MakeInParam("@fk_c_id", SqlDbType.Int, 4, fk_c_id),
data.MakeInParam("@c_value", SqlDbType.Money, 4, c_value),
data.MakeInParam("@Price", SqlDbType.Money, 4, Price),
data.MakeInParam("@pk_id", SqlDbType.Int, 4, pk_id),
data.MakeInParam("@visitDate", SqlDbType.VarChar, visitDate.Length, visitDate),
data.MakeInParam("@bannerId", SqlDbType.Int, 4, bannerId),
data.MakeInParam("@job_mailinglist", SqlDbType.Int, 4, job_mailinglist),
data.MakeInParam("@pc_discountAmount", SqlDbType.Money, 4, pc_discountAmount),
data.MakeInParam("@job_id_parent", SqlDbType.Int, 4, job_id_parent),
data.MakeInParam("@JobAddressHome", SqlDbType.VarChar, Utils.StrLen(JobAddressHome), JobAddressHome),
data.MakeInParam("@JobCityHome", SqlDbType.VarChar, Utils.StrLen(JobCityHome), JobCityHome),
data.MakeInParam("@JobZIPHome", SqlDbType.VarChar, Utils.StrLen(JobZIPHome), JobZIPHome),
data.MakeInParam("@JobStateHome", SqlDbType.VarChar, Utils.StrLen(JobStateHome), JobStateHome),
data.MakeInParam("@fk_countryHome", SqlDbType.Int, 4, fk_countryHome)
                                   };
            int r = data.RunProc("N4_spJob_Add", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_AssignPhotographer2(int Id_Job, int Id_Photographer, decimal JobTotal)
    {
        try
        {
            DataTable dt = new DataTable();
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@Id_Photographer", SqlDbType.Int, 4, Id_Photographer),
data.MakeInParam("@SiteAction", SqlDbType.Int, 4, 1),
data.MakeInParam("@JobTotal", SqlDbType.Money, 4, JobTotal)
                                   };
            int r = data.RunProc("N4_spJob_AssignPhotographer2", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_UpdatePasswords(int Id_Job)
    {
        try
        {
            DataTable dt = new DataTable();
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job)
                                   };
            int r = data.RunProc("N4_spJob_UpdatePasswords", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_UpdateTotal(int Id_Job, decimal JobTotal)
    {
        if (Id_Job <= 0)
            return 0;
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job),
data.MakeInParam("@JobTotal", SqlDbType.Money, 4, JobTotal)
                                   };
            int r = data.RunProc("N4_spJob_UpdateTotal2", prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_Login(string JobEmail, string JobPassword, ref DataTable dt)
    {
        try
        {
            Db data = new Db();
            SqlParameter[] prams = {
data.MakeInParam("@JobEmail", SqlDbType.VarChar, JobEmail.Length, JobEmail),
data.MakeInParam("@JobPassword", SqlDbType.VarChar, JobPassword.Length, JobPassword)
                                   };
            int r = data.RunProc("N4_spJob_Login", out dt, prams);
            return r;
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
            return -100;
        }
    }
    public int Job_GetByID(int Id_Job, ref DataTable dt)
    {
        Db data = new Db();
        SqlParameter[] prams = {
                                       data.MakeInParam("@Id_Job", SqlDbType.Int, 4, Id_Job)
                               };
        int r = data.RunProc("N4_spJob_GetByID", out dt, prams);
        return r;
    }


}