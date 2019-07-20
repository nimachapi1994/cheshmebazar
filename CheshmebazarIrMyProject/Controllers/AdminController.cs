using CheshmebazarIrMyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheshmebazarIrMyProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();
        public ActionResult ShowActiveProduct()
        {
            ViewBag.ShowConfirmProducts = db.Products.Where(x => x.AdminConfirm == true).ToArray();
            return View();
        }
        
        public ActionResult AdminStoreCommentDelete(int id)
        {
            var findstore = db.SComments.Find(id);
            if (findstore.Id!=null)
            {
                db.SComments.Remove(findstore);
            }
            db.SaveChanges();
            return Json("ok");
        }
        public ActionResult AdminDeleteProduct(int id)
        {
            var find = db.Products.Find(id);
            db.SliderForProducts.RemoveRange(db.SliderForProducts.Where(x => x.product_Id == find.Id));
            db.Products.Remove(find);
            db.SaveChanges();
            return RedirectToAction("AdminProductsManagment");
        }
        public ActionResult AdminProductConfirm(int id)
        {
            var find = db.Products.Find(id);
            find.AdminConfirm = true;
            db.SaveChanges();
            return RedirectToAction("AdminProductsManagment");
        }
     
        public ActionResult AdminDeleteStore(int id)
        {
            db.sp_AdminDeleteStore(id);
            db.SaveChanges();
            return RedirectToAction("AdminControlPanel");
        }
        public ActionResult AdminCityManagment()
        {
            //ViewBag.d = db.Cities.ToList();
          
            return View();
        }
        public ActionResult AdminControlPanel()
        {

            return View();
        }
        public ActionResult AdminShowStore(int id)
        {
            ViewBag.showConfirmStore = db.Stores.Where(x => x.Id == id).ToList();
            return View();
        }
        public ActionResult AdminStoresCommentManagment()
        {
            ViewBag.showstorecomments = db.SComments.Where(x => x.AdminConfirm == false).OrderByDescending(x => x.Date).ToList();

            return View();

        }
        public ActionResult AdminConfirmStore(int id)
        {
            var find = db.Stores.Find(id);
            find.AdminConfirm = true;
            db.SaveChanges();
            return RedirectToAction("admincontrolpanel");
        }
        public ActionResult AdminStoresCommentConfirm(int id)
        {
             var d=db.SComments.Find(id);
           
            
                d.AdminConfirm = true;
                
            
            db.SaveChanges();

            return Json("ok");

        }
        public ActionResult AdminProductCommentConfirm(int id)
        {
            var d = db.PCommentOKs.Find(id);


            d.AdminConfirm = true;


            db.SaveChanges();

            return Json("ok");

        }
        public ActionResult adminProductCommentDelete(int id)
        {
            db.PCommentOKs.Remove(db.PCommentOKs.Find(id));
            db.SaveChanges();
            return Json("ok");
        }
        public ActionResult AdminProductCommentManagment()
        {
            ViewBag.showproductcomments= db.PCommentOKs.Where(x => x.AdminConfirm == false).OrderByDescending(x => x.Date).ToList();
            return View();
        }
        public ActionResult AdminProductsManagment()
        {
            ViewBag.showNotConfirmProducts = db.Products.Where(x => x.AdminConfirm == false).ToArray();
            return View();
        }
        public ActionResult AdminShowproduct(int id)
        {

            ViewBag.d = db.Products.Where(x => x.Id == id).ToList();
            return View();
        }
        public ActionResult AdminActiveStores()
        {
            return View();
        }
        public ActionResult AdminCategoryManagement(int id = -1, int idd = -1)
        {
            ViewBag.dd = id;
            ViewBag.r = idd;
            return View(ViewBag.d = db.CategoryParents.ToList());
        }
       
      
    }
}










