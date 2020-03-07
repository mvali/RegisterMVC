using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for DB
/// </summary>
public class Db : IDisposable
{
    String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
    private SqlConnection con;
    public SqlConnection Connection
    {
        get
        {
            return con;
        }
        set
        {
            con = value;
        }
    }

    /// <summary>
    /// Open the connection.
    /// </summary>
    public void CreateCon()
    {
        // open connection
        if (con == null)
        {
            con = new SqlConnection(strConnString);
            //con.Open();
        }
    }

    /// <summary>
    /// Close the connection.
    /// </summary>
    public void CloseCon()
    {
        if (con != null)
        {
            con.Close();
            con.Dispose();
            con = null;
        }
    }

    public void Dispose()
    {
        // make sure connection is closed
        if (con != null)
        {
            con.Dispose();
            con = null;
        }
    }

    //-------------BEGIN Generic Sql access commands --------------------

    /// <summary>
    /// Create command object used to call stored procedure.
    /// </summary>
    /// <param name="procName">Name of stored procedure.</param>
    /// <param name="prams">Params to stored procedure.</param>
    /// <returns>Command object.</returns>
    private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
    {
        CreateCon();

        SqlCommand cmd = new SqlCommand(procName, con);
        cmd.CommandType = CommandType.StoredProcedure;

        if (prams != null)
        {
            foreach (SqlParameter parameter in prams)
            {
                cmd.Parameters.Add(parameter);
            }
        }

        cmd.Parameters.Add(
            new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null)
            );

        con.Open();

        return cmd;
    }

    /// <summary>
    /// Make input param.
    /// </summary>
    /// <param name="ParamName">Name of param.</param>
    /// <param name="DbType">Param type.</param>
    /// <param name="Size">Param size.</param>
    /// <param name="Value">Param value.</param>
    /// <returns>New parameter.</returns>
    public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
    {
        return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
    }

    /// <summary>
    /// Make input param.
    /// </summary>
    /// <param name="ParamName">Name of param.</param>
    /// <param name="DbType">Param type.</param>
    /// <param name="Size">Param size.</param>
    /// <returns>New parameter.</returns>
    public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
    {
        return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
    }
    /// <summary>
    /// Make stored procedure param.
    /// </summary>
    /// <param name="ParamName">Name of param.</param>
    /// <param name="DbType">Param type.</param>
    /// <param name="Size">Param size.</param>
    /// <param name="Direction">Parm direction.</param>
    /// <param name="Value">Param value.</param>
    /// <returns>New parameter.</returns>
    public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
    {
        SqlParameter param;
        if (Size > 0)
            param = new SqlParameter(ParamName, DbType, Size);
        else
            param = new SqlParameter(ParamName, DbType);

        param.Direction = Direction;
        if (!(Direction == ParameterDirection.Output && Value == null))
            param.Value = Value;

        return param;
    }

    /// <summary>
    /// Get Initial time before db procedure start
    /// </summary>
    /// <returns>integer miliseconds</returns>
    int TrackQueryInit()
    {
        string TrackQuery = System.Configuration.ConfigurationManager.AppSettings["TrackQuery"].ToString();
        if (TrackQuery != null && TrackQuery.ToLower() == "yes")
        {
            return (System.DateTime.Now.Minute * 60 * 1000) + (System.DateTime.Now.Second * 1000) + System.DateTime.Now.Millisecond;
        }
        return 0;
    }
    /// <summary>
    /// Show the query execution time only for IP=QueryIPShow
    /// </summary>
    /// <param name="procName">Procedure name</param>
    /// <param name="timeInit">Initial time given by TrackQueryInit</param>
    void TrackQuery(string procName, int timeInit)
    {
        string TrackQuery = System.Configuration.ConfigurationManager.AppSettings["TrackQuery"].ToString();
        int iTime2 = (System.DateTime.Now.Minute * 60 * 1000) + (System.DateTime.Now.Second * 1000) + System.DateTime.Now.Millisecond;
        if (TrackQuery != null && TrackQuery.ToLower() == "yes")
        {
            bool bshow = true;
            if (System.Configuration.ConfigurationManager.AppSettings["QueryIPShow"].ToString().Length > 0)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() != System.Configuration.ConfigurationManager.AppSettings["QueryIPShow"].ToString())
                {
                    bshow = false;
                }
            }
            if (bshow)
            {
                int idatediff = iTime2 - timeInit;
                if (idatediff < 0) idatediff = idatediff + 60000;
                System.Web.HttpContext.Current.Response.Write("Executing " + procName + "... in " + (idatediff).ToString() + "</br>");
            }
        }
    }





    /// <summary>
    /// Run stored procedure.
    /// </summary>
    /// <param name="procName">Name of stored procedure.</param>
    /// <returns>Stored procedure return value.</returns>
    public int RunProc(string procName, SqlParameter[] prams = null)
    {
        try
        {
            int timeInit = TrackQueryInit();

            SqlCommand cmd = CreateCommand(procName, prams);

            cmd.ExecuteNonQuery();
            //cmd.ExecuteReader();

            this.CloseCon();

            TrackQuery(procName, timeInit);

            return (int)cmd.Parameters["ReturnValue"].Value;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            CloseCon();
        }
    }
    public int RunProc(string procName, out DataTable dtResults, SqlParameter[] prams = null)
    {
        dtResults = new DataTable();

        try
        {
            int timeInit = TrackQueryInit();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = CreateCommand(procName, prams);
            sda.Fill(dtResults);

            this.CloseCon();

            TrackQuery(procName, timeInit);

            return (int)sda.SelectCommand.Parameters["ReturnValue"].Value;
        }
        catch (Exception ex)
        {
            Utils.rw(procName + "-" + ex.ToString());
            Log.LogException(ex, procName);
            //throw ex;
            return -1;
        }
        /*finally
        {
            try
            {
                CloseCon();
            }catch{}
        }*/
    }
    public int RunProc(string procName, out DataSet ds, SqlParameter[] prams = null)
    {
        try
        {
            int timeInit = TrackQueryInit();

            ds = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.SelectCommand = CreateCommand(procName, prams);
            sda.Fill(ds);

            this.CloseCon();

            TrackQuery(procName, timeInit);

            return (int)sda.SelectCommand.Parameters["ReturnValue"].Value;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            CloseCon();
        }
    }
    //-------------END Generic Sql access commands --------------------

    //--------- BEGIN SAMPLE procedure ----------------
    int sample_sp(int SqlProcParameterInt, string SqlProcParameterVarchar, ref DataTable dt)
    {
        Db data = new Db();
        SqlParameter[] prams = {
                                       data.MakeInParam("@adminId", SqlDbType.Int, 4, SqlProcParameterInt),
                                       data.MakeInParam("@adminUsername", SqlDbType.VarChar, 5000, SqlProcParameterVarchar),
                                   };
        int r = data.RunProc("spSqlProcedureName", out dt, prams);
        return r;
    }
    //--------- END SAMPLE procedure ----------------









    void test_sp_orig(int SqlProcParameter)
    {

        System.Data.SqlClient.SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = "SqlProcedureName";
        cmd.Parameters.Add("@SqlProcParameter", SqlDbType.Int).Value = SqlProcParameter;
        cmd.Connection = con;
        try
        {
            con.Open();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            con.Close();
            con.Dispose();
        }
    }


}// end class
