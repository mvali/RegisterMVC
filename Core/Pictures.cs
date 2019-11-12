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
            set { }
        }


        public static string SolveImageUrl(int ID, string ImageType, string ImageID, string ImageExt, bool Thumb, string AccessedBy = "url")
        {
            string sRet = "";

            if (ID > 0)
            {
                if (ImageType == "Photo" || ImageType == "PhotoStamp" || ImageType == "PhotoBW" || ImageType == "PhotoStampBW" || ImageType == "PhotoSepia" || ImageType == "PhotoStampSepia")
                {
                    //sRet = BuildJobImageURL(ID.ToString(), ImageType, ImageID, ImageExt, Thumb, AccessedBy);
                }
                else if (ImageType == "Photographer" || ImageType == "Map" || ImageType == "Sample" || ImageType == "Studio")
                {
                    sRet = BuildPHImageURL(ID.ToString(), ImageType, ImageID, ImageExt, Thumb, AccessedBy);
                }
            }
            else
            {
                if (AccessedBy == "url")
                    sRet = imagesUrl + "/" + "noimage.gif";
            }
            if (AccessedBy == "path")
                sRet = sRet.Replace("/", "\\");

            return sRet;
        }
        public static string BuildPHImageURL(string phId, string imageType, string imageId, string imageExt, bool imageThumb, string AccessedBy = "url")
        {
            string sRet = "";
            string sRoot = PathRootGet(AccessedBy, pHImagesURL, Utils.AppSetting("sPHImagesPath"));
            if (!imageThumb)
            {
                if (imageType == "Photographer" || imageType == "Studio")
                    sRet = sRoot + "/" + GetHashValue(Utils.Str2Int(phId)) + phId + "/" + imageType + imageId + "." + imageExt;
                else
                    sRet = sRoot + "/" + GetHashValue(Utils.Str2Int(phId)) + phId + "/" + imageType + imageId + "-Full" + "." + imageExt;
            }
            else
            {
                sRet = sRoot + "/" + GetHashValue(Utils.Str2Int(phId)) + phId + "/" + imageType + imageId + "-Thumb080" + "." + imageExt;
            }
            return sRet;
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

            //iRet = (((upid / 499) + 1) * 500);
            iRet = ((iRet + 1) * 500);

            return iRet.ToString() + sDelimiter;
        }
    }//end class
}