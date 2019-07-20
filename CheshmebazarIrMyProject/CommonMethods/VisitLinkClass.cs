using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheshmebazarIrMyProject.CommonMethods
{
    public class VisitLinkClass : Controller
    {
        public static string visitlink(string actionname, string controllername, int id)
        {
            string visitlink = $"/{controllername}/{actionname}?id={id}";
            return visitlink;

        }
    }
}