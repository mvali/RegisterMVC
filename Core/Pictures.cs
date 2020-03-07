using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Core
{
    public class Pictures
    {
        public static string _imagesUrl="";
        public static string imagesUrl
        {
            get
            {
                return Utils.AppSetting("sImagesUrl");
            }
            set { _imagesUrl = value; }
        }
        public static string pHImagesURL
        {
            get { return Utils.AppSetting("sPHImagesURL"); }
        }


        public static string PathRootGet(string AccessedBy, string url, string path)
        {
            string sRoot = "";
            try
            {
                switch (AccessedBy)
                {
                    case "url": sRoot = url; break;
                    case "path": sRoot = System.Web.Hosting.HostingEnvironment.MapPath(path); break;
                }
                return sRoot;
            }
            catch (Exception ex)
            {
                switch (AccessedBy)
                {
                    case "url": sRoot = url; break;
                    case "path": sRoot = path; break;
                }
                return sRoot;
            }
        }
        public static string GetHashValue(int upid, string sDelimiter = "/")
        {
            double dupid = upid;
            double iRet = 0;
            if (upid <= 0)
                return "error";
            double dFolders = dupid / 500;
            iRet = Math.Round(dFolders, 0);
            if (iRet * 500 > dupid)
                iRet = iRet - 1;

            iRet = ((iRet + 1) * 500);

            return iRet.ToString() + sDelimiter;
        }
    }//end class
}