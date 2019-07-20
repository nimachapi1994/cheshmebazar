using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheshmebazarIrMyProject.Models;
using System.Drawing;
using System.Drawing.Imaging;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace CheshmebazarIrMyProject.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
        public ApplicationUserManager usermngr
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
        }
        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();
        public ActionResult GetDiscountStoresByGategory()
        {
            return View();
        }
        public ActionResult GetLikedStoresByGategory()
        {
            return View();
        }
        public ActionResult GetSpecialStoresByGategory()
        {
            return View();
        }
        public ActionResult GetAllStoresByGategory()
        {
            return View();
        }
        public ActionResult NewStores()
        {
            return View();
        }
        public ActionResult DiscountStores()
        {
            ViewBag.showDiscountStores = db.sm_ShowDicountStores().OrderByDescending(x => x.EditDate).ToList();
            return View();
        }
        public ActionResult LikedStores()
        {
            
            return View();
        }
       public ActionResult GetDiscountForThisStore()
        {
            return View();
        }
        public ActionResult GetDiscountForThis()
        {
            return View();
        }
        public ActionResult GetDiscountForThisProduct()
        {
            return View();
        }
        public ActionResult StoreView(int id)
        {
            if (Session["id"]!=null)
            {
                Session.Remove("id");
            }
            if (id==null)
            {
                return RedirectToAction("home", "index");
            }
            var find = db.Stores.Find(id);
            //ViewBag.id
            //      = db.Stores.Where(x => x.Id == find.Id).Select(x => x.Id).ToList();
            ViewBag.showStore = db.Stores.Where(x => x.Id == find.Id).ToList();
            ViewBag.showProducts = db.Products.Where(x => x.Store_Id == find.Id  && x.AdminConfirm==true).ToList();
            ViewBag.ShowWorkHours = db.WorkHours.Where(x => x.Store_Id == find.Id).ToArray();
            ViewBag.showkeywords = db.Stores.Where
                (x => x.Id == find.Id);
            find.TotalVistor = find.TotalVistor + 1;
            ViewBag.showcomments = db.SComments.
                Where(x => x.Store_Id == find.Id && x.AdminConfirm == true).OrderByDescending(x => x.Date).ToList();
            Session["id"] = id;
            return View();
        }
        public ActionResult Product(int id)
        {
            if (Session["pid"]!=null)
            {
                Session.Remove("pid");
            }
           
            var find = db.Products.Find(id);
            ViewBag.showproduct = db.Products.Where(x => x.Id == find.Id).ToList();
            foreach (var item in db.Stores.ToList())
            {
                if (item.Id==find.Store_Id)
                {
                    ViewBag.id = item.Id;
                    ViewBag.showOtheProducts = db.Products.Where(x => x.Store_Id == item.Id&&x.AdminConfirm==true).ToList().
                        Except(db.Products.Where(x => x.Id == find.Id)).OrderByDescending(x => x.Edit_Date);
                    
                }
            }
            ViewBag.showproductsComment =
                         db.PCommentOKs.Where(x => x.Product_Id == find.Id && x.AdminConfirm == true).OrderByDescending(x => x.Date).ToList();

            Session["pid"] = id;




            return View();
        }
        //store control
        public ActionResult  GetMessageSinceCustomersToStore(MsgToStore msg)
        {
            int id = (int)Session["id"];
            var find = db.Stores.Find(id);

            db.MsgToStores.Add(new MsgToStore
            {
                Store_Id = find.Id,
                Name = msg.Name,
                Family = msg.Family,
                Date =DateTime.Now,
                EmailorPhoneNumber = msg.EmailorPhoneNumber,
                Txt = msg.Txt
                
            });
            db.SaveChanges();

            return Json("ok");
        }
        public ActionResult GetCommentForStore(SComment sc)
        {
            int id = (int)Session["id"];
            var find = db.Stores.Find(id);
            db.SComments.Add(new SComment
            {
                NameAndFname = sc.NameAndFname,
                Date = DateTime.Now,
                AdminConfirm = false,
                Txt = sc.Txt,
                Email = sc.Email,
                Store_Id = find.Id
            });
            db.SaveChanges();
            return RedirectToAction("storeview",new { id=find.Id});
        }
        public ActionResult GetCommentForProduct(PCommentOK pc)
        {
            int id = (int)Session["pid"];
            var find = db.Products.Find(id);
            db.PCommentOKs.Add(new PCommentOK
            {
                Email = pc.Email,
                NameAndFname = pc.NameAndFname,
                Date = DateTime.Now
            ,
                AdminConfirm = false,
                Product_Id = find.Id
                ,Txt=pc.Txt
            });
            db.SaveChanges();
            return RedirectToAction("product", new { id = find.Id });
        }
        

    }
}