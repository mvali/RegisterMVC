using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Register.Core.DB
{
    public class Photographer
    {
        public int Photographer_GetByID(int Id_Photographer, ref DataTable dt)
        {
            try
            {
                Db data = new Db();
                SqlParameter[] prams = {
data.MakeInParam("@Id_Photographer", SqlDbType.Int, 4, Id_Photographer),
                                   };
                int r = data.RunProc("N4_spPhotographer_GetByID", out dt, prams);
                return r;
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
                return -100;
            }
        }

    }

}