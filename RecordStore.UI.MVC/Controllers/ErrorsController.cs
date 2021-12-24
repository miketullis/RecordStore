using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordStore.UI.MVC.Controllers
{
    public class ErrorsController : Controller
    {
        // GET: Errors
        public ActionResult Index()
        {
            return View();
        }

        //Basic "Default Custom Error Page" 
        public ActionResult Unresolved() 
        {
            return View();
        }



    }
}