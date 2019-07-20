using CheshmebazarIrMyProject.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheshmebazarIrMyProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ApplicationRoleManager rolemngr
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
        }
        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();
        public ActionResult Index()
        {
            List<string> lstrole = new List<string>()
            {
                "Admin","Provider","Customer"
            };
            lstrole.ForEach(x =>
            {
                if (rolemngr.RoleExists(x) == false)
                {
                    IdentityRole role = new IdentityRole(x);
                    rolemngr.Create(role);
                }
            });

            ViewBag.d =
                            db.Stores.Where(x => x.AdminConfirm == true).OrderByDescending(d => d.EditDate).ToList();
            return View();
        }
        public ActionResult Pricing()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContactCustomerToAdmin()
        {

            return View();
        }
        [OutputCache(Duration =30)]
        public ActionResult Search(string name)
        {
            System.Timers.Timer tr = new System.Timers.Timer();
            tr.Enabled = true;
            tr.Interval = 1000;
            tr.Elapsed += Tr_Elapsed;
            tr.Start();


            return View(
                db.Stores.Where(x => x.Name.Contains(name.ToLower()) && 
                x.ManagmentName.Contains(name.ToLower()) && x.Motto.Contains(name.ToLower())&&x.Txt.Contains(name.ToLower())).ToList()
           
            );
            
        }
        private void Tr_Elapsed (object sender,System.Timers.ElapsedEventArgs a)
        {

        }
        public ActionResult SendMsgToAdmin(MsgCustomerToAdmin msg)
        {
            db.MsgCustomerToAdmins.Add(new MsgCustomerToAdmin
            { Datee = DateTime.Now, Email = msg.Email, Name = msg.Name, Txt = msg.Txt });
            db.SaveChanges();
            TempData["msgoksendtoadmin"] = "پیام با موفقیت ارسال شد";
            return RedirectToAction("ContactCustomerToAdmin");

        }

    }
}