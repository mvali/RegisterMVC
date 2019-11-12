using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class Job
    {
        public int Id { get; set; }
        public int PhotographerId { get; set; }
        public List<int> ServicesId { get; set; }
        public int CountryId { get; set; }
        public int PackageId { get; set; }
        //public Address AddressHome { get; set; }
        public ZipDetails Zip { get; set; }
        //public ZipDetails ZipHome { get; set; }
        public DateTime SessionDateTime { get; set; }
        public int SessionTime { get; set; }
        public JobInfo JobPersonalInfo { get; set; }
        public PromocodeResponse Promocode { get; set; }
        public bool Paid { get; set; }
        public string Currency { get; set; }
        public decimal CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }
        public int CurrencyId { get; set; }

        public Job() {
            CountryId = -1;
            Zip = new ZipDetails();
            ServicesId = new List<int>();
            JobPersonalInfo = new JobInfo();
            Paid = false;
            CurrencyValue = 1;
            CurrencyId = -1;
        }
        public static void Update(Job ju)
        {
            //Job j = Session();
            //j.PackageId = packageId;
            System.Web.HttpContext.Current.Session["Job"] = ju;
        }
        public static Job Session()
        {
            if (System.Web.HttpContext.Current.Session["Job"] != null)
            {
                return (Job)System.Web.HttpContext.Current.Session["Job"];
            }
            else
                return new Job();
        }
        public static bool Authorized(string controller = null)
        {
            return Authorized(null, controller);
        }
        public static bool Authorized(System.Web.Mvc.ActionExecutingContext filterContext, string controller=null)
        {
            string ctrl = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            if (!String.IsNullOrWhiteSpace(controller))
                ctrl = controller;

            bool bRet = true;
            Job jb = Session();

            switch (ctrl)
            {
                case "Address"://true allways
                    break;
                case "Packages":
                    if (jb.Zip != null)
                    {
                        if (String.IsNullOrWhiteSpace(jb.Zip.ZipCode)|| String.IsNullOrWhiteSpace(jb.Zip.State) || String.IsNullOrWhiteSpace(jb.Zip.City))
                            bRet = false;
                    }
                    else
                        bRet = false;
                    break;
                case "Services":
                    if (String.IsNullOrWhiteSpace(jb.Zip.ZipCode) || String.IsNullOrWhiteSpace(jb.Zip.State) || String.IsNullOrWhiteSpace(jb.Zip.City))
                        bRet = false;

                    if (bRet)
                    {
                        var pkId = 0;
                        if (filterContext != null)
                            if (filterContext.ActionParameters.ContainsKey("pkid"))
                            {
                                pkId = Utils.Str2Int(filterContext.ActionParameters["pkid"].ToString());
                            }
                        if (jb.PackageId <= 0 && pkId <= 0)
                            bRet = false;
                    }
                    break;
                case "SessionDateTime":
                    if (jb.PackageId <= 0)
                        bRet = false;
                    break;
                case "Order":
                    if (jb.SessionDateTime.Year < 2000)
                        bRet = false;
                    break;
                case "Payment":
                    if (String.IsNullOrWhiteSpace(jb.JobPersonalInfo.FirstName) || String.IsNullOrWhiteSpace(jb.JobPersonalInfo.LastName) || String.IsNullOrWhiteSpace(jb.JobPersonalInfo.JobEmail)
                        || String.IsNullOrWhiteSpace(jb.Zip.City) || String.IsNullOrWhiteSpace(jb.Zip.State) || String.IsNullOrWhiteSpace(jb.Zip.ZipCode))
                        bRet = false;
                    break;
                default:
                    break;
            }
            return bRet;
        }

        public static decimal AmountTotal()
        {
            var sl = new List<Service>();
            var p = new Package();
            decimal promoCodeAmount = 0;
            return AmountTotal(false, ref sl, ref p, ref promoCodeAmount);
        }
        public static decimal AmountTotal(bool getLists, ref List<Service> services, ref Package pack, ref decimal promoCodeAmount) {
            decimal retV = 0;
            Package p = new Package().Get();
            retV = p.Price;
            Job j = Job.Session();

            decimal servicesPrice = 0;
            if (j.ServicesId != null && p.ServiceListAll != null)
                foreach (int s in j.ServicesId)
                {
                    foreach (var sx in p.ServiceListAll)
                    {
                        if (s == sx.Id)
                        {
                            if (!sx.IncludedInPackage)
                            {
                                servicesPrice += sx.Price;
                                if (getLists) services.Add(sx);
                            }
                            break;
                        }
                    }
                }
            retV += servicesPrice;

            if (getLists)
            {
                pack = p;
            }
            promoCodeAmount = PromoCode.PromoDiscount(j, retV, servicesPrice);

            return retV- promoCodeAmount;//.ToString("0.00");//"C 0.00"
        }

        public static decimal ServiceUpdate(int serviceId, int action)
        {
            Job j = Job.Session();
            List<int> s = j.ServicesId;

            if (action == 1)
            {
                if (!s.Contains(serviceId))
                {
                    s.Add(serviceId);
                    if (j.Id > 0 && j.PackageId > 0)
                        new Services().JobServices_addnew(j.Id, j.PackageId, serviceId);

                }
            }
            else
            {
                if (s.Contains(serviceId))
                {
                    s.Remove(serviceId);
                    if (j.Id > 0 && j.PackageId > 0)
                        new Services().JobServices_Delete(j.Id, j.PackageId, serviceId);
                }
            }

            j.ServicesId = s;
            Job.Update(j);
            decimal retV = AmountTotal();
            int r5 = new Jobs().Job_UpdateTotal(j.Id, retV);//services_price_all

            return retV;
        }
        public static bool ServicePicked(int serviceId)
        {
            bool retV = false;
            List<int> s = Job.Session().ServicesId;
            if (s.Contains(serviceId))
                retV = true;

            return retV;
        }

        public static int SessionDateTimeUpdate(string sessionDate)
        {
            var retV = 0;
            //try{
            Job j = Job.Session();
            DateTime sd = DateTime.Parse(sessionDate);
            //DateTime sd = DateTime.ParseExact(sessionDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            j.SessionDateTime = sd;
            Job.Update(j);
            retV = 1;
            //}catch (Exception ex) { }

            return retV;
        }
        public static int SessionTimeUpdate(int sessionTime)
        {
            var retV = 0;
            Job j = Job.Session();
            j.SessionTime = sessionTime;
            Job.Update(j);
            retV = 1;

            return retV;
        }
        public static void JobUpdate()
        {
            Job j = Job.Session();
            int r = new Jobs().Job_AddCheck(j.Id, LoggedProfile.logged.Id, j.JobPersonalInfo.JobEmail, j.Zip.ZipCode, j.Zip.StateCode, "services", j.Zip.CountryId);
        }

        public static string RefererGet()
        {
            string retV = "";
            HttpCookie hq = HttpContext.Current.Request.Cookies["lboreferer"];
            if (hq != null)
                retV = hq.Value;

            return retV;
        }
        public static int BannerGet()
        {
            int retV = -1;
            HttpCookie hq = HttpContext.Current.Request.Cookies["banner"];
            if (hq != null)
                retV = Utils.Str2Int(hq.Values["callId"]);

            return retV;
        }
        public static string VisitorSignupCookieGet(string visitorDetail)
        {
            string retV = "";
            HttpCookie hq = HttpContext.Current.Request.Cookies["visitorsignup"];
            if (hq != null)
                retV = hq.Values[visitorDetail];

            return retV;
        }
        public static Job j { get
            {
                return Session();
            }
            set { } }

    }
}