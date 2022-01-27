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
using System.Data;
using PagedList; //Added for access to the PagedList base classes
using PagedList.Mvc; //Added for access to the PagedList.MVC functionality

namespace RecordStore.UI.MVC.Controllers
//namespace RecordStore.Controllers
{
    public class HomeController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        #region Index
        [HandleError]
        public ActionResult Index(string searchFilter, int page = 1)
        {
            int pageSize = 12; //We will use this value to set how many records/objects per page

            var albums = db.Album.OrderBy(x => x.AlbumID).ToList();
  
            var artist = db.Artist.ToList();
            var albumArtist = db.AlbumArtist.ToList();
            var genre = db.Genre.ToList();
            var albumGenre = db.AlbumGenre.ToList();
            var recording = db.Recording.ToList();
            var albumRecording = db.AlbumRecording.ToList();

            #region SearchFilter
            if (!String.IsNullOrEmpty(searchFilter))
            {
                albums = (from a in albums
                          join aa in albumArtist on a.AlbumID equals aa.AlbumID
                          join ar in artist on aa.ArtistID equals ar.ArtistID
                          join ag in albumGenre on a.AlbumID equals ag.AlbumID
                          join g in genre on ag.GenreID equals g.GenreID
                          join art in albumRecording on a.AlbumID equals art.AlbumID
                          join rt in recording on art.RecordingID equals rt.RecordingID
                          where (a.AlbumName != null && a.AlbumName.ToLower().Contains(searchFilter.ToLower())) ||
                          (a.Label.LabelName != null && a.Label.LabelName.ToLower().Contains(searchFilter.ToLower())) ||
						  (a.Year != null && a.Year.ToString().Contains(searchFilter)) ||
						  (ar.ArtistName != null && ar.ArtistName.ToLower().Contains(searchFilter.ToLower())) ||
						  (g.GenreName != null && g.GenreName.ToLower().Contains(searchFilter.ToLower())) ||
						  (a.CatalogNum != null && a.CatalogNum.ToLower().Contains(searchFilter.ToLower())) ||
						  (a.Label.Country != null && a.Label.Country.ToLower().Contains(searchFilter.ToLower())) ||
						  (a.Description != null && a.Description.ToLower().Contains(searchFilter.ToLower())) ||
						  (rt.RecordingType != null && rt.RecordingType.ToLower().Contains(searchFilter.ToLower()))
                          select a).Distinct().ToList();
            }
            #endregion
            return View(albums.ToPagedList(page, pageSize));
        }
        #endregion

        #region Contact Page
        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion

        #region Ajax Contact Form
        //POST Request
        [HttpPost]
        public JsonResult ContactAjax(ContactViewModel cvm)
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

            //SmtpClient
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());

            //Client credentials
            client.Credentials = new NetworkCredential(
                 ConfigurationManager.AppSettings["EmailUser"].ToString(),
                 ConfigurationManager.AppSettings["EmailPass"].ToString());

            //Client Properties
            client.Port = 8889;

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
            }

            //Send them back to the View with their completed form data
            return Json(cvm);
        }


        public PartialViewResult EmailConfirmation(ContactViewModel cvm)
        {
            return PartialView("Emailconfirmation", cvm);
        }

        #endregion

    }
}
