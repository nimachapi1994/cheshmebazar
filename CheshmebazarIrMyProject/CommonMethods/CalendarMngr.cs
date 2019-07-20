using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
namespace CheshmebazarIrMyProject.CommonMethods
{
    public class CalendarMngr
    {
        public static DateTime PerToEng(string st)
        {
            object d;
         
            
                PersianCalendar pcal = new PersianCalendar();
                string[] parts = st.Split('/');
                d = pcal.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]),0,0,0,0);
                
            
            return (DateTime)d;
        }
        public static string EngToPer(DateTime dt)
        {
            PersianCalendar pcal = new PersianCalendar();
            string datetime = $"{pcal.GetYear(dt)}/{pcal.GetMonth(dt)},{pcal.GetDayOfMonth(dt)}";
            return datetime;
        }
    }
}