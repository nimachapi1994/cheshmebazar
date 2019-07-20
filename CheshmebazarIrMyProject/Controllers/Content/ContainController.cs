using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CheshmebazarIrMyProject.Models;

namespace CheshmebazarIrMyProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContainController : Controller
    {
        // GET: Contain
        public ActionResult ContainPage()
        {
            return View();
        }
    }
}