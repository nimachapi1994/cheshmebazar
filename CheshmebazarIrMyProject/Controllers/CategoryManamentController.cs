using CheshmebazarIrMyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheshmebazarIrMyProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryManamentController : Controller
    {

        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();

        public ActionResult GetParentCategoryAndAddToDb(string name)
        {
            db.CategoryParents.Add(new CategoryParent { Name = name });
            db.SaveChanges();
            return RedirectToAction("AdminCategoryManagement", "Admin");
        }
        public ActionResult GetChildCategoryAndAddToDb(List<string> name, int id)
        {
            foreach (var item in name)
            {
                CategoryChild cat = new CategoryChild();
                cat.Name = item;
                cat.Parent_Id = id;
                db.CategoryChilds.Add(cat);
            }

            db.SaveChanges();
            return RedirectToAction("AdminCategoryManagement", "Admin");
        }
        public ActionResult ShowCategoryByJsonId(int id)
        {
            var find = db.CategoryParents.Find(id);
            var s = db.CategoryChilds.ToList().Select(x =>
            new { name = x.Name, parentid = x.Parent_Id });
            var d = s.Where(x => x.parentid == id);
            string xx = find.Name;

            return Json(d.ToList());
        }
        public ActionResult UpdateParentCategory(CategoryParent p)
        {
            var found = db.CategoryParents.Find(p.Id);
            if (found.Id != null)
            {
                found.Name = p.Name;
            }
            db.SaveChanges();
            TempData["msg"] = "اطلاعات با موفقیت آپدیت شد";
            return RedirectToAction("AdminCategoryManagement", "Admin", new { id = -1 });
        }
        public ActionResult DeleteChildCategory(CategoryChild ch)
        {
            var d = db.CategoryChilds.Find(ch.Id);
            db.DeleteChildCategory(d.Id);
            db.SaveChanges();


            TempData["msg"] = "اطلاعات با موفقیت آپدیت شد";
            return RedirectToAction("AdminCategoryManagement", "Admin", new { idd = -1 });
        }
        public ActionResult UpdateChildCategory(CategoryChild ch)
        {
            var d = db.CategoryChilds.Find(ch.Id);
            d.Name = ch.Name;
            db.SaveChanges();


            TempData["msg"] = "اطلاعات با موفقیت آپدیت شد";
            return RedirectToAction("AdminCategoryManagement", "Admin", new { idd = -1 });
        }
        public ActionResult DeleteParentCategory(CategoryParent cp)
        {
            var found = db.CategoryParents.Find(cp.Id);
            db.CategoryParents.Remove(db.CategoryParents.Find(cp.Id));


            db.SaveChanges();
            TempData["msg"] = "دسته اصلی با موفقیت حذف شد";
            return RedirectToAction("AdminCategoryManagement", "Admin", new { idd = -1 });
        }


    }
}