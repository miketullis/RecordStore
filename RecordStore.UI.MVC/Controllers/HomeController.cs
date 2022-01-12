using RecordStore.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using RecordStore.UI.MVC.Models;
using RecordStore.Data.EF;
//May not be needed
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing; //Added for access to the Image class
using RecordStore.UI.MVC.Utilities; //Added for access to Image Utilities
using PagedList; //Added for access to the PagedList base classes
using PagedList.Mvc; //Added for access to the PagedList.MVC functionality


namespace RecordStore.UI.MVC.Controllers
//namespace RecordStore.Controllers
{
    public class HomeController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        public ActionResult Index(string searchFilter, int page = 1)
        {
            int pageSize = 12; //We will use this value to set how many records/objects per page

            var albums = db.Album.OrderBy(x => x.AlbumID).ToList();
            //return View(albums.ToPagedList(page, pageSize));

            var artist = db.Artist.ToList();
            var albumArtist = db.AlbumArtist.ToList();
            var genre = db.Genre.ToList();
            var albumGenre = db.AlbumGenre.ToList();



            if (!String.IsNullOrEmpty(searchFilter))
            {
                albums = (from a in albums join aa in albumArtist on a.AlbumID equals aa.AlbumID
                          join ar in artist on aa.ArtistID equals ar.ArtistID
                          join ag in albumGenre on a.AlbumID equals ag.AlbumID
                          join g in genre on ag.GenreID equals g.GenreID
                          where a.AlbumName.ToLower().Contains(searchFilter.ToLower()) ||
                          a.Label.LabelName.ToLower().Contains(searchFilter.ToLower()) ||
                          a.Year.ToString().Contains(searchFilter) || 
                          ar.ArtistName.ToLower().Contains(searchFilter.ToLower()) ||
                          g.GenreName.ToLower().Contains(searchFilter.ToLower()) ||
                          a.CatalogNum.ToLower().Contains(searchFilter.ToLower()) ||
                          a.Label.Country.ToLower().Contains(searchFilter.ToLower())
                          select a).Distinct().ToList();
            }
            return View(albums.ToPagedList(page, pageSize));
        }



        #region About Page
        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            return View();
        }
        #endregion


        [HttpGet]
        public ActionResult Products()
        {
            return View();
        }


        #region Ajax Contact Form

        //POST Request
        [HttpPost]
        public ActionResult ContactAjax(ContactViewModel cvm)
        {
            //Lead-in Message 
            string body = $"You have received an email from {cvm.Name}, with a subject of: {cvm.Subject}. Please respond " +
                $"to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

            //MailMessage
            MailMessage mm = new MailMessage(
                //FROM
                ConfigurationManager.AppSettings["EmailUser"].ToString(),
                //TO
                ConfigurationManager.AppSettings["EmailTo"].ToString(),
                //SUBJECT
                cvm.Subject,
                //BODY of the email
                body);

            //MailMessage properties
            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            mm.ReplyToList.Add(cvm.Email);

            //SmtpClient - SMTP (Secure Mail Transfer Protocol)
            //host info that allows email to be sent
            SmtpClient client = new SmtpClient(
                ConfigurationManager.AppSettings["EmailClient"].ToString());

            //Client credentials - Email Login information
            client.Credentials = new NetworkCredential(
                 ConfigurationManager.AppSettings["EmailUser"].ToString(),
                 ConfigurationManager.AppSettings["EmailPass"].ToString());

            //Client Properties
            client.Port = 25;
            client.EnableSsl = false;

            //Try to send the email
            try
            {
                //Attempt to send the email
                client.Send(mm);
            }
            catch (Exception ex)
            {
                //Format an error message for the user
                ViewBag.Message = $"We're sorry, but your request could not be completed at this time. " +
                    $"Please try again later.<br /> Error Message: <br />{ex.StackTrace}";

                //Send them back to the View with their completed form data
                return Json(cvm);
            }

            //If email was sent successfully, display confirmation message
            return View("EmailConfirmation", cvm);
        }


        #endregion


        #region Original Contact Form

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        ////POST Request
        //[HttpPost]
        //[ValidateAntiForgeryToken] //Check if the form in our view was used to submit the cvm object
        //public ActionResult Contact(ContactViewModel cvm)
        //{
        //    //Check that the ModelState is valid (they are sending us valid information for our ContactViewModel)
        //    if (ModelState.IsValid)
        //    {
        //        //Send them back to the form with the completed inputs
        //        return View(cvm);
        //    }

        //    //Build the message that will be emailed to us
        //    string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. Please respond " +
        //        $"to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

        //    string body = $"You have received an email from {cvm.Name}. {(string.IsNullOrEmpty(cvm.Subject) ? "No Subject Provided" : cvm.Subject)}.<br/>" +
        //        $"Please respond to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

        //    //MailMessage - What sends the email
        //    MailMessage mm = new MailMessage(

        //        //FROM
        //        ConfigurationManager.AppSettings["EmailUser"].ToString(),

        //        //TO
        //        ConfigurationManager.AppSettings["EmailTo"].ToString(),

        //        //SUBJECT
        //        cvm.Subject,

        //        //BODY of the email
        //        body

        //        );

        //    //MailMessage properties
        //    mm.IsBodyHtml = true;
        //    mm.Priority = MailPriority.High;
        //    mm.ReplyToList.Add(cvm.Email);

        //    //SmtpClient - SMTP (Secure Mail Transfer Protocol)
        //    //host info that allows email to be sent
        //    SmtpClient client = new SmtpClient(
        //        ConfigurationManager.AppSettings["EmailClient"].ToString());

        //    //Client credentials - Email Login information
        //    client.Credentials = new NetworkCredential(
        //         ConfigurationManager.AppSettings["EmailUser"].ToString(),
        //         ConfigurationManager.AppSettings["EmailPass"].ToString());


        //    //Client Properties
        //    client.Port = 25;
        //    client.EnableSsl = false;


        //    //Try to send the email
        //    try
        //    {
        //        //Attempt to send the email
        //        client.Send(mm);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Format an error message for the user
        //        ViewBag.CustomerMessage = $"We're sorry, but your request could not be completed at this time. " +
        //            $"Please try again later.<br /> Error Message: <br />{ex.StackTrace}";

        //        //Send them back to the View with their completed form data
        //        return View(cvm);
        //    }

        //    //If email was sent successfully, display confirmation message

        //    return View("EmailConfirmation", cvm);
        //}
        #endregion

    }
}
