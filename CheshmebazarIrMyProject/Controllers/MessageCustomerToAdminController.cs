using CheshmebazarIrMyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheshmebazarIrMyProject.Controllers
{
    public class MessageCustomerToAdminController : Controller
    {
        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();
        public ActionResult MessageCustomerToAdmin()
        {
            
            return View(db.MsgCustomerToAdmins.OrderByDescending(x=>x.Datee).ToList());
        }
    }
}