using ExpressiveAnnotations.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class SessionDate
    {
        public DateTime minDate {
            get { return DateTime.Now.AddDays(3); }
            set { }
        }
        [AssertThat("PhotoSessionDate >= minDate", ErrorMessage = "You can book only with 3 days in advance")]
        public DateTime PhotoSessionDate { get; set; }
        public string PhotoSessionDateS { get; set; }
        public int PhotoSessionTime { get; set; }

        public SessionDate()
        {
            
            DateTime sd = Job.Session().SessionDateTime;
            if (sd.Year <= 2000)
                sd = DateTime.Now;
            PhotoSessionDate = sd;
        }
        public SessionDate(string sessionDate)
        {
            PhotoSessionDate = DateTime.Parse(sessionDate);
            PhotoSessionDateS = sessionDate;
        }
        public static DateTime Session()
        {
            DateTime sd = Job.Session().SessionDateTime;
            if (sd.Year <= 2000)
            {
                sd = DateTime.Now.AddDays(3);
                for (int i = 1; i <= 7; i++)
                {
                    if (Photographer.AvailableDate(sd.AddDays(i)))
                    {
                        sd = sd.AddDays(i);
                        break;
                    }
                }
            }
            return sd;
        }
        public static int SessionTime()
        {
            int sd = Job.Session().SessionTime;
            if (sd <= 0)
                sd = 12;
            return sd;
        }
        public static bool Active(int testHour)
        {
            bool retV = false;
            int sessHour = SessionTime();
            if (sessHour == testHour)
                retV = true;

            return retV;
        }

    }
}