using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheshmebazarIrMyProject.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using CheshmebazarIrMyProject.CommonMethods;

namespace CheshmebazarIrMyProject.Controllers
{
    [Authorize(Roles = "Store")]
    public class ControlpanelController : Controller
    {
        public ApplicationUserManager usermngr
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
        }
        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();



        //start store control--------------------------------------
        public ActionResult WhenFisrtMakeStore(string name,string managmentname,HttpPostedFileBase img)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            byte[] b;
            if (img != null)
            {
                try
                {
                    b = new byte[img.ContentLength];
                    img.InputStream.Read(b, 0, b.Length);
                }
                catch (BadImageFormatException ex)
                {
                    TempData["msgbadimgformat"] = "لطفا فرمت تصویر را بررسی نمایید";
                    throw;
                }
                foreach (var item in db.Stores.ToList())
                {
                    if (item.User_id == user.Id)
                    {
                        var find = db.Stores.Find(item.Id);
                        string link = CheshmebazarIrMyProject.CommonMethods.VisitLinkClass.visitlink("storeview", "store", find.Id);
                        db.sp_StoreUpdatePartmakestorePart1(managmentname, name, b, link,find.Id);
                        
                        db.SaveChanges();
                        find.VisitLink = link;
                        find.Logo = b;
                        find.Name = name;
                        find.AdminConfirm = false;
                        find.EditDate = DateTime.Now;

                    }

                }
            }
               

        
            
              
          

            
            db.SaveChanges();
            TempData["msgWhenMakeOrEditStore"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("BaseDataAddStore");
            
        }

        public ActionResult CategoryGet(int id)
        {

            var s = db.CategoryChilds.ToList().Select(x =>
            new { name = x.Name, parentid = x.Parent_Id,id=x.Id });
            var d = s.Where(x => x.parentid == id);
          
            
           
            return Json(d.ToList());

        }
        public ActionResult GetDataAddStorePart2(Store st, int categoryparent)
        {            
            var findparent = db.CategoryParents.Find(categoryparent);
 
            var user = usermngr.FindById(User.Identity.GetUserId());
            

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id==user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    find.Txt = st.Txt;
                    find.Motto = st.Motto;
                    find.ParentCategory_Id = findparent.Id;
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                }
            }            
            db.SaveChanges();
            TempData["msgBaseDataAddStore"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("BaseDataAddStore");
        }
        public ActionResult GetDataAddStorePart3Discount(string startdate,string enddate,int perdis)
        {

            var user = usermngr.FindById(User.Identity.GetUserId());

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                    db.sp_StoreUpdateDiscount(startdate, enddate, perdis, find.Id);

                }
            }
            db.SaveChanges();
            TempData["msgDsicountStore"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("BaseDataAddStore");
        }
        public ActionResult GetDataAddStorePart5StoreInfo(Store st)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    db.sp_StoreUpdateComplatedata_Cityproviance_part3
                       (st.Province, st.PostalCode, st.Address, st.City, find.Id);
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                }
            }
            db.SaveChanges();
            TempData["msgCompateStorePart1"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("ComplateDataAddStore");
           
        }
        public ActionResult GetDataAddStorePart6Contacts(Store st)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    
                    db.sp_StoreUpdate_phonenumber_part4
                        (st.WebSite, st.PhoneOne, st.PhoneTwo, st.PhoneThree, st.Mobile,find.Id);
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                    
                }
            }
            db.SaveChanges();
            TempData["msgComaplateStorePart2"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("ComplateDataAddStore");

        }
        enum MyEnum
        {

        }
        public ActionResult GetDataAddStorePart7SocialsId(Store st)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    db.sp_StoreUpdate_social_Part5(st.Facebook, st.GooglePlus, st.LikedIn,
                        st.Instagram, st.Twitter, st.TelegramChannel, st.Whsup, find.Id);
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                }
            }
            db.SaveChanges();
            TempData["msgSocialNeteworks"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("ComplateDataAddStore");

        }
        public ActionResult GetDataAddStorePart8KeyWords(Store st)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    db.sp_StoreUpdate_keywords_Part6(st.KeyWords, find.Id);
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                }
            }
            db.SaveChanges();
            TempData["msgStoreKeyWords"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("ComplateDataAddStore");

        }
        public ActionResult GetDataAddStorePart9Images(List<HttpPostedFileBase> img)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    find.AdminConfirm = false;
                    find.EditDate = DateTime.Now;
                    if (img != null)
                    {
                        foreach (var item1 in img)
                        {
                            byte[] b1;
                            b1 = new byte[item1.ContentLength];
                            item1.InputStream.Read(b1, 0, b1.Length);
                            db.SliderForStores.Add(new SliderForStore
                            { Store_Id = find.Id, Pic = b1 });
                        }

                    }
                   

                }
            }
            db.SaveChanges();
            TempData["msgStoreImages"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("ImgandVideoAddStore");

        }
        public ActionResult GetDataAddStorePart10VideoLink(Store st)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            var store = db.Stores.ToList();
            foreach (var item in store)
            {
                if (item.User_id==user.Id)
                {
                    var findstoreid = db.Stores.Find(item.Id);
                    db.sp_StoreUpdatePartGetVideoLinkString(st.Video, findstoreid.Id);
                    findstoreid.AdminConfirm = false;
                    findstoreid.EditDate = DateTime.Now;
                }
            }
            db.SaveChanges();
            TempData["msgStoreVideo"] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction("ImgandVideoAddStore");
        }
       

   
        public ActionResult DeleteImagesAndShowAddStore(int id)
        {
            db.sp_DeletesliderImageStore(id);
            db.SaveChanges();
           

            return Json("true");
        }


        public ActionResult GetWorkHours(MyWorkHours wkh)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id==user.Id)
                {
                    var find = db.Stores.Find(item.Id);

                    foreach (var item1 in db.WorkHours.ToList())
                    {
                        if (item1.Store_Id==find.Id)
                        {
                            var findwhk = db.WorkHours.Find(item1.Id);
                            findwhk.ShiftOneStart0 = wkh.shanbe2 + " : " + wkh.shanbe1;
                            findwhk.shiftOneEnd0 = wkh.shanbe4 + " : " + wkh.shanbe3;
                            findwhk.ShiftTwoStart0 = wkh.shanbe6 + " : " + wkh.shanbe5;
                            findwhk.ShiftTwoEnd0 = wkh.shanbe8 + " : " + wkh.shanbe7;
                            //--------------------------------------------------------                        
                            findwhk.ShiftOneStart1 = wkh.yekshanbe2 + " : " + wkh.yekshanbe1;
                            findwhk.shiftOneEnd1 = wkh.yekshanbe4 + " : " + wkh.yekshanbe3;
                            findwhk.ShiftTwoStart1 = wkh.yekshanbe6 + " : " + wkh.yekshanbe5;
                            findwhk.ShiftTwoEnd1 = wkh.yekshanbe8 + " : " + wkh.yekshanbe7;
                            //-------------------------------------------------------- 
                            findwhk.ShiftOneStart2 = wkh.doshanbe2 + " : " + wkh.doshanbe1;
                            findwhk.shiftOneEnd2 = wkh.doshanbe4 + " : " + wkh.doshanbe3;
                            findwhk.ShiftTwoStart2 = wkh.doshanbe6 + " : " + wkh.doshanbe5;
                            findwhk.ShiftTwoEnd2 = wkh.doshanbe8 + " : " + wkh.doshanbe7;
                            //-------------------------------------------------------- 
                            findwhk.ShiftOneStart3 = wkh.seshanbe2 + " : " + wkh.seshanbe1;
                            findwhk.shiftOneEnd3 = wkh.seshanbe4 + " : " + wkh.seshanbe3;
                            findwhk.ShiftTwoStart3 = wkh.seshanbe6 + " : " + wkh.seshanbe5;
                            findwhk.ShiftTwoEnd3 = wkh.seshanbe8 + " : " + wkh.seshanbe7;
                            //-------------------------------------------------------- 
                            findwhk.ShiftOneStart4 = wkh.charshanbe2 + " : " + wkh.charshanbe1;
                            findwhk.shiftOneEnd4 = wkh.charshanbe4 + " : " + wkh.charshanbe3;
                            findwhk.ShiftTwoStart4 = wkh.charshanbe6 + " : " + wkh.charshanbe5;
                            findwhk.ShiftTwoEnd4 = wkh.charshanbe8 + " : " + wkh.charshanbe7;
                            //-------------------------------------------------------- 
                            findwhk.ShiftOneStart5 = wkh.panjshanbe2 + " : " + wkh.panjshanbe1;
                            findwhk.shiftOneEnd5 = wkh.panjshanbe4 + " : " + wkh.panjshanbe3;
                            findwhk.ShiftTwoStart5 = wkh.panjshanbe6 + " : " + wkh.panjshanbe5;
                            findwhk.ShiftTwoEnd5 = wkh.panjshanbe8 + " : " + wkh.panjshanbe7;
                            //-------------------------------------------------------- 
                            findwhk.ShiftOneStart6 = wkh.jome2 + " : " + wkh.jome1;
                            findwhk.shiftOneEnd6 = wkh.jome4 + " : " + wkh.jome3;
                            findwhk.ShiftTwoStart6 = wkh.jome6 + " : " + wkh.jome5;
                            findwhk.ShiftTwoEnd6 = wkh.jome8 + " : " + wkh.jome7;
                        }
                    }
                  
                   
                }
            }
           
         
            db.SaveChanges();
            return RedirectToAction("BaseDataAddStore");
        }  
       
        //end store control------------------------------------------------------
        //start store pages------------------------------------------------------
        public ActionResult OrdersPage()
        {
            return View();
        }
        public ActionResult MessageCustomersToStore()
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);
                    ViewBag.showmessage = db.sp_ShowMsgCustomerToStore(find.Id).OrderByDescending(x => x.Date).ToList();
                    var d = db.sp_ShowMsgCustomerToStore(find.Id).Select(x => x.Date);
                  


                }
            }
            return View();
        }
        public ActionResult deletecomment(int id)
        {
            db.SComments.Remove(db.SComments.Find(id));
            db.SaveChanges();
            return Json("ok");
        }
        public ActionResult MessageToAdmin()
        {
            return View();
        }
        public ActionResult FirstControlPanelPage()
        {


            var user = usermngr.FindById(User.Identity.GetUserId());

            var d = db.Stores.Where(x => x.User_id == user.Id).Select(x => x.Id);

            if (d.Count() == 0)
            {
                db.Stores.Add(new Store { User_id = user.Id, Create_date = DateTime.Now,AdminConfirm=false });
                db.SaveChanges();

                if (db.Stores.Where(x => x.User_id == user.Id).Count() == 1)
                {
                    ViewBag.data = db.ShowCompaleStore(user.Id);
                    ViewBag.data1 = db.ShowCompaleStore(user.Id);
                }
            }
            else
            {
                ViewBag.data = db.ShowCompaleStore(user.Id);
                ViewBag.data1 = db.ShowCompaleStore(user.Id);
            }
            var store = db.Stores.ToList();
            foreach (var item in store)
            {
                if (item.User_id == user.Id)
                {
                    var findstoreid = db.Stores.Find(item.Id);
                    ViewBag.showproducts =
                        db.Products.Where(x => x.Store_Id == findstoreid.Id).ToList();
                }
            }
            return View();
        }


        public ActionResult BaseDataAddStore()
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            ViewBag.dd = db.ShowCompaleStore(user.Id);
            ViewBag.cp = db.CategoryParents.ToList();
            ViewBag.cc = db.CategoryChilds.ToList();
            ViewBag.selectedCategory =
                        db.CategoryParents.ToList();


            return View();
        }


        public ActionResult ImgandVideoAddStore()
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            ViewBag.dd = db.ShowCompaleStore(user.Id); 

            ViewBag.getimg= db.sp_getallimages(user.Id);
            
            return View();
        }

        public ActionResult ComplateDataAddStore()
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            ViewBag.dd = db.ShowCompaleStore(user.Id);

            return View();
        }
        public ActionResult SelectLocation()
        {
            var user = usermngr.FindById(User.Identity.GetUserId());

            ViewBag.dd = db.ShowCompaleStore(user.Id);
            return View();
        }
        public ActionResult CommentManagmentAddStore()
        {
          
            var user = usermngr.FindById(User.Identity.GetUserId());
            ViewBag.dd = db.ShowCompaleStore(user.Id);
            

         
            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id==user.Id)
                {

                    ViewBag.showcomment= db.SComments.Where(x => x.Store_Id == item.Id).OrderByDescending(x => x.Date).ToList();


                }

            }

            return View();
        }
        
    

        //end store pages--------------------------------------------------

        //Start Product Control---------------------------------------------
      public ActionResult AddProductConfirmPartFirst(string name,HttpPostedFileBase img)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            byte[] b = null;
       
            if (img!=null)
            {
                b = new byte[img.ContentLength];
                img.InputStream.Read(b, 0, b.Length);



            }
            foreach (var item in db.Stores.ToList())
            {
                if (item.User_id == user.Id)
                {
                    var find = db.Stores.Find(item.Id);                                     
                        db.Products.Add(new Product { Store_Id = find.Id, Name = name,Logo=b,AdminConfirm=false,Create_Date=DateTime.Now });                  
                }
            }
            db.SaveChanges();
          
          
            db.SaveChanges();
            return RedirectToAction("firstControlPanelPage");


        }
        public ActionResult DeleteProduct(int id)
        {
            db.Products.Remove(db.Products.Find(id));
            db.SaveChanges();
            return Json("true");
        }
        public ActionResult AddProductGetLogo(HttpPostedFileBase logo1)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            int id = (int)Session["getid1"];
            var findprouctid = db.Products.Find(id);
            if (logo1!=null)
            {
                byte[] b = new byte[logo1.ContentLength];
                logo1.InputStream.Read(b, 0, b.Length);
                findprouctid.Logo = b;
                findprouctid.AdminConfirm = false;
                findprouctid.Edit_Date = DateTime.Now;
            }
            db.SaveChanges();
           
            return Json("true");
        }


        public ActionResult DeleteProductImages(int id)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            foreach (var item in db.Stores.ToList())
            {
                if (user.Id==item.User_id)
                {
                    foreach (var item1 in db.Products.ToList())
                    {
                        if (item1.Store_Id==item.Id)
                        {
                            foreach (var item2 in item1.SliderForProducts.ToList())
                            {
                                if (item2.product_Id==item1.Id)
                                {
                                    if (item2.Id==id)
                                    {
                                        var find = db.SliderForProducts.Find(item2.Id);
                                        db.SliderForProducts.Remove(find);
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
            db.SaveChanges();
            return Json("true");
        }
        public ActionResult DeleteProductLogo(int id)
        {
            var user = usermngr.FindById(User.Identity.GetUserId());
            foreach (var item in db.Stores.ToList())
            {
                if (user.Id == item.User_id)
                {
                    foreach (var item1 in db.Products.ToList())
                    {
                        if (item1.Store_Id == item.Id)
                        {
                            if (item1.Id==id)
                            {
                                var find = db.Products.Find(item1.Id);
                                find.Logo = null;

                            }
                        }
                    }
                }
            }
            db.SaveChanges();
            return Json("true");
        }
        public ActionResult AddProductConfirmPart1(Product p ,
            List<HttpPostedFileBase> img,HttpPostedFileBase img1)
        {
            int id = (int)Session["getid"];            
            var findprouctid = db.Products.Find(id);           
            var user = usermngr.FindById(User.Identity.GetUserId());
            var store = db.Stores.ToList();
            foreach (var item in store)
            {
                if (item.User_id == user.Id)
                {                    
                    var findstoreid = db.Stores.Find(item.Id);                 
                    findprouctid.Store_Id = findstoreid.Id;
                    findprouctid.Name = p.Name;
                    findprouctid.Txt = p.Txt;
                    findprouctid.Price = p.Price;
                    findprouctid.DiscountPesend = p.DiscountPesend;
                    findprouctid.OfPrice = p.OfPrice;
                    findprouctid.StartDateDiscount = p.StartDateDiscount;
                    findprouctid.ExpiredDateDiscount = p.ExpiredDateDiscount;
                    findprouctid.KeyWords = p.KeyWords;
                    findprouctid.AdminConfirm = false;
                    findprouctid.Edit_Date = DateTime.Today;
                }
            }
            db.SaveChanges();
            if (img!=null)
            {
                foreach (var item in img)
                {
                    byte[] b1;                                                           
                        b1 = new byte[item.ContentLength];
                        item.InputStream.Read(b1, 0, b1.Length);
                        db.SliderForProducts.Add(new SliderForProduct
                        { product_Id = findprouctid.Id, pic = b1 });                    
                }

        }
            if (img1 != null)
            {               
                    byte[] b1;                
                    b1 = new byte[img1.ContentLength];
                    img1.InputStream.Read(b1, 0, b1.Length);
                    db.SliderForProducts.Add(new SliderForProduct
                    { product_Id = findprouctid.Id, pic = b1 });                
            }            
            db.SaveChanges();
            TempData["msgSaveCompateProduct"] ="اطلاعات با موفیت ذخیره شد";
            Session.Remove("getid");          
            return Redirect($"/controlpanel/addproduct?id={id}");//Nima Chapi Technology

        }
     
        //End Product Control-----------------------------------------------

        //start product Pages--------------------------------------------------

        public ActionResult AddProduct(int id)
        {
            var find = db.Products.Find(id);
            ViewBag.showproduct1=db.ShowOneProduct(find.Id);
            ViewBag.showimg = db.SliderForProducts.Where(x => x.product_Id == find.Id).ToArray();
            Session["getid"] = id;
            Session["getid1"] = id;
           
            return View();
        }
        public ActionResult AddProductComments()
        {
            return View();
        }
      
        //end product Pages--------------------------------------------------

     
    }
}