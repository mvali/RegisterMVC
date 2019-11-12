using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Register.Models
{
    public class PhAvailabilityDay
    {
        public int DayId { get; set; }
        public string DayWeek { get; set; }
        public bool Available { get; set; }
        public int HourStart { get; set; }
        public int HourEnd { get; set; }

        public PhAvailabilityDay(int dayId, bool available, int hstart, int hend)
        {//DB: 1=sunday...7=saturday
            DayId = dayId;
            DayWeek = GetDayOfWeek(dayId).ToString();
            Available = available;
            HourStart = hstart;
            HourEnd = hend;
        }
        public DayOfWeek GetDayOfWeek(int DayValue)
        {
            if (Enum.IsDefined(typeof(DayOfWeek), DayValue))
            {
                return (DayOfWeek)DayValue;
            }
            else
            {
                throw new Exception("WaWaOoops!");
            }
        }
        public string HourPM(int hourt)
        {
            var retV = hourt.ToString();
            if (hourt == 0 || hourt == 12)
            {
                if (hourt == 0) retV = "12 AM";
                else retV = "12 PM";
            }
            else
            {
                retV = hourt < 12 ? hourt.ToString() + " AM" : (hourt - 12).ToString() + " PM";
            }
            return retV;
        }
        public static string HourPMS(int hourt)
        {
            var retV = hourt.ToString();
            if (hourt == 0 || hourt == 12)
            {
                if (hourt == 0) retV = "12 AM";
                else retV = "12 PM";
            }
            else
            {
                retV = hourt < 12 ? hourt.ToString() + " AM" : (hourt - 12).ToString() + " PM";
            }
            return retV;
        }
    }
}