using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class Photographer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public List<PhAvailabilityDay> PhAvailability { get; set; }

        public Photographer() {
            Id = 0; FirstName = ""; LastName = ""; PhAvailability = new List<PhAvailabilityDay>();
        }
        public Photographer(int id, string firstName, string lastName, List<PhAvailabilityDay> phA)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhAvailability = phA;
        }
        public static Photographer Session()
        {
            if (System.Web.HttpContext.Current.Session["Photographer"] != null)
            {
                return (Photographer)HttpContext.Current.Session["Photographer"];
            }
            else
                return new Photographer();
        }
        public static void Update(int idPhotographer)
        {
            //var phi = Utils.rq("ph");

            int idPh = Session().Id;
            if (idPh == idPhotographer)
                return;

            DataTable dt = new DataTable();
            int r = new Core.DB.Photographer().Photographer_GetByID(idPhotographer, ref dt);

            if (r < 0) return;
            if (dt.Rows.Count == 0)return;

            Photographer ph = Session();
            ph.Id = idPhotographer;
            ph.FirstName = dt.Rows[0]["PhotographerFirstName"].ToString();
            ph.LastName = dt.Rows[0]["PhotographerLastName"].ToString();
            ph.Email = dt.Rows[0]["PhotographerEmail"].ToString();
            string phImage = dt.Rows[0]["PhotographerImage"].ToString();
            if (!string.IsNullOrWhiteSpace(phImage))
                ph.Image = Core.Pictures.SolveImageUrl(idPhotographer, "Photographer", idPhotographer.ToString(), phImage, true);
            else
                ph.Image = Core.Pictures.SolveImageUrl(-1, "", "", "", false);

            List<PhAvailabilityDay> phal = new List<PhAvailabilityDay>();
            for(int i=1; i<=7; i++)
            {
                PhAvailabilityDay pha = new PhAvailabilityDay(i-1, Utils.Str2Bool(dt.Rows[0]["PhotographerAvailability"+i.ToString()+"Day"].ToString())
                    ,Utils.Str2Int(dt.Rows[0]["PhotographerAvailability" + i.ToString() + "From_d"].ToString())
                    ,Utils.Str2Int(dt.Rows[0]["PhotographerAvailability"+i.ToString()+"To_d"].ToString()));
                phal.Add(pha);
            }
            ph.PhAvailability = phal;

            System.Web.HttpContext.Current.Session["Photographer"] = ph;

            var j = Models.Job.Session();
            j.PhotographerId = idPhotographer;
            Models.Job.Update(j);
        }
        public static bool AvailableDate(DateTime testDate)
        {
            bool retV = false;
            int weekdayt = (int)testDate.DayOfWeek; //weekdayt = weekdayt == 0 ? 7 : weekdayt;
            foreach(var x in Session().PhAvailability)
            {
                if (!x.Available)//not available is NOT checked
                {
                    if (x.DayId == weekdayt)
                    {
                        retV = true; ;
                        break;
                    }
                }
            }

            return retV;
        }
        public static bool AvailableTime(int hourt)
        {
            bool retV = false;
            int weekdayt = (int)Job.Session().SessionDateTime.DayOfWeek; //weekdayt = weekdayt == 0 ? 7 : weekdayt;
            foreach (var x in Session().PhAvailability)
            {
                if (x.DayId == weekdayt)
                {
                    if (hourt >= x.HourStart && hourt <= x.HourEnd)
                        retV = true;
                    break;
                }
            }
            return retV;
        }
        public static int Hour(string timeSpan)
        {
            int retV = 0;
            int dayc = (int)Job.Session().SessionDateTime.DayOfWeek;
            foreach (var x in Session().PhAvailability)
            {
                if (x.DayId == dayc)
                {
                    if (timeSpan == "s")
                        retV = x.HourStart;
                    else
                        retV = x.HourEnd;
                    break;
                }
            }
            return retV;
        }
    }//end class
}