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
        [HandleError]
        public ActionResult Index()
        {
            return View("Unresolved");
        }

        //Basic "Default Custom Error Page" 
        public ActionResult Unresolved() 
        {
            return View();
        }

        //Basic "Default 404 Error Page" 
        public ActionResult NotFound() 
        {
            
            return HttpNotFound();
        }



    }
}