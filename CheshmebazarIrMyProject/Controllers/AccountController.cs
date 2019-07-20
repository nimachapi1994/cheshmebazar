using CheshmebazarIrMyProject.Models;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheshmebazarIrMyProject.Controllers
{
    public class AccountController : Controller
    {
        DbCheshmeBazarIrMyProjectOkey db = new DbCheshmeBazarIrMyProjectOkey();
    

        public ApplicationUserManager usermngr
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
        }
        public ApplicationSignInManager singinmnger
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

        }
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            string userid = (string)Session["userid"].ToString();
            string token = (string)Session["token"].ToString();
            Session.Remove("userid");
            Session.Remove("token");
            var result = usermngr.ResetPassword(userid, token, model.Password);
            if (result.Succeeded)
            {
                TempData["msgsuccess"] = "تبریک رمز عبور شما با موفقیت تغییر کرد میتوانید وارد شوید" +
                    "تیم مدیریت چشم بازار";
            }
            else
            {
              
                TempData["msgdanger"] = "خطا در تغییر رمز عبور لطفا دوباره تلاش فرمایید "
                    + "تیم مدیریت چشم بازار";
            }
            return RedirectToAction("login", "account");
        }
        public ActionResult PasswordRecoveryConfirm(string userid,string token)
        {
            Session["userid"] = userid;
            Session["token"] = token;
            return RedirectToAction("newpass");
        }
        public ActionResult newpass()
        {
            return View("~/Views/account/PasswordRecoveryConfirm.cshtml");
        }

        public ActionResult PasswordRecovery(string email)
        {
            var user = usermngr.FindByEmail(email);
            if (user==null)
            {
                TempData["msgdanger"] = "کاربر محترم ایمیل وارد شده صحیح نمی باشد ";
                return RedirectToAction("PassRecoveryPage");
            }
            else
            {
                string token = usermngr.GeneratePasswordResetToken(user.Id);
                string emaillink = Url.Action("PasswordRecoveryConfirm", "Account",
                    new { userid=user.Id ,token=token }, Request.Url.Scheme);
                usermngr.SendEmail(user.Id, "تغییر رمز عبور چشم بازار",

       $"<h2 dir='rtl'>سلام خدمت شما دوست عزیز</h2></br>" +
    $"<h4 dir='rtl'> جهت تغییر رمز حساب خود بر روی لینک زیر کلیک نمایید</h4></br>" +
    $"<div dir='rtl'><a href = '{emaillink}' >تغییر رمز عبور</a></hr></div>" +
    $"<h4 dir = 'rtl'> با تشکر تیم مدیریت چشم بازار </h4 ></ br >"

    );





                TempData["msgsuccess"] = "کاربر محترم لینک تغییر رمز کاربری به ایمیل شما ارسال شد"
                + "با تشکر تیم مدیریت چشم بازار";
            }
            return RedirectToAction("Index","Home");
        }
        public ActionResult PassRecoveryPage()
        {
            return View();
        }
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("login");



        }


        public ActionResult LoginConfirm(LoginViewModel model)
        {


            var userconfirm = usermngr.FindByEmail(model.Email);
            if (usermngr.FindByEmail(model.Email)==null)
            {
                TempData["msgdanger"] = "کاربری با این ایمیل در سیستم وجود ندارد";
                return RedirectToAction("login");
            }
            else
            {
                if (userconfirm.EmailConfirmed == true)
                {
                    var user = singinmnger.PasswordSignIn(model.Email, model.Password, model.RememberMe, true);
                    if (user==SignInStatus.Success)
                    {
                        if (userconfirm.EmailConfirmed == true)
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                    else
                    {
                        TempData["msgdanger"] = "نام کاربری یا رمز عبور اشتباه می باشد";
                        return RedirectToAction("login");
                    }
                   
                }
                
                if (userconfirm.EmailConfirmed == false)
                {

                    if (userconfirm.EmailConfirmed == false)
                    {
                        TempData["msgdanger"] = " لطفا ایمیل خود باز کرده و حساب کاربری تان را تایید نمایید"
                        + "با تشکر تیم مدیریت چشم بازار ";
                        return RedirectToAction("index", "home");
                    }
                }
            }
           

            return RedirectToAction("index", "Home");





            
           
        }
        public ActionResult Login()
        {
            var user = usermngr.FindByEmail("admin@admin.com");
            if (user==null)
            {
                ApplicationUser admin = new ApplicationUser()
                {
                    UserName = "admin@admin.com",
                    PhoneNumber = "09178134699",
                    Email= "admin@admin.com",
                    EmailConfirmed=true,

                };
              var usersuccess=  usermngr.Create(admin, "nimaNIMA12.@");
                if (usersuccess.Succeeded==true)
                {
                       usermngr.AddToRole(admin.Id, "admin");

                }
            }
            return View();
        }
        public ActionResult RegisterConfirm(RegisterViewModel model)
        {
            var confirm = usermngr.FindByEmail(model.Email);
            if (confirm == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.Email
                    ,
                    Email = model.Email

                };
              var d=  usermngr.Create(user, model.Password);


                if (d.Succeeded == true)
                {
                    string token = usermngr.GenerateEmailConfirmationToken(user.Id);
                    string emaillink = Url.Action("ConfirmEmail", "Account",
                        new { userid = user.Id, token = token }, Request.Url.Scheme);
                  
                  usermngr.SendEmail(user.Id,"تایید حساب",
    $"<h2 dir='rtl'>سلام خدمت شما دوست عزیز</h2></br>" +
    $"<h4 dir='rtl' > جهت تایید حساب کاربری خود بر روی لینک زیر کلیک نمایید</h4></br>" +
    $"<div dir='rtl'><a href = '{emaillink}'  >تایید حساب</a></hr></div>" +
    $"<h4 dir = 'rtl' > با تشکر تیم مدیریت چشم بازار </h4 ></ br >"

    );

                    usermngr.AddToRole(user.Id, "store");
                    TempData["msgsuccess"] = " تبریک حساب کاربری شما با موفقیت ساخته شد جهت تایید به ایمیل خود مراجعه نمایید." +
                        " تیم مدیریت چشم بازار  ";



                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["msgdanger"] = "کاربر عزیز حساب شما ساخته نشد لطفا مجددا تلاش نمایید"
                   + "با تشکر تیم مدیریت چشم بازار  ";
                    return RedirectToAction("Register");
                }
            }
            else
            {
                TempData["msgdanger"] = "کاربر محترم ایمیل شما در صندوق چشم بازار وجود دارد لطفا وارد شوید در صورتی که رمز خود را فراموش کرده اید میتوانید با استفاده از فراموشی رمز ,رمز خود را بازیابی کنید "
               + "با تشکر تیم مدیریت چشم بازار";
                return RedirectToAction("Login");
            }
           
            return RedirectToAction("Login");
        }
        public ActionResult Register()
        {
            
            return View();
        }
        public ActionResult ConfirmEmail(string userid,string token)
        {
            IdentityResult result = usermngr.ConfirmEmail(userid, token);
            if (result.Succeeded==true)
            {
                AspNetUser user = new AspNetUser();
                user.EmailConfirmed = true;
                TempData["msgsuccess"] = "کاربر عزیز حساب شما با موفقیت تایید شد میتوانید لایگین شوید";
                return RedirectToAction("login");
            }
            else
            {
                TempData["msg"] = "کاربر عزیز حساب شما تایید نشد لطفا مجددا تلاش نمایید";
                return RedirectToAction("index", "home");
            }
        }
       
        
    }
}