using RecordStore.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web.Mvc;
using RecordStore.UI.MVC.Models;

namespace RecordStore.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }


        [HttpGet]
        public ActionResult Products()
        {

            return View();
        }







        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //POST Request
        [HttpPost]
        [ValidateAntiForgeryToken] //Check if the form in our view was used to submit the cvm object
        public ActionResult Contact(ContactViewModel cvm)
        {
            //Check that the ModelState is valid (they are sending us valid information for our ContactViewModel)
            if (ModelState.IsValid)
            {
                //Send them back to the form with the completed inputs
                return View(cvm);
            }

            //Build the message that will be emailed to us
            string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. Please respond " +
                $"to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

            string body = $"You have received an email from {cvm.Name}. {(string.IsNullOrEmpty(cvm.Subject) ? "No Subject Provided" : cvm.Subject)}.<br/>" +
                $"Please respond to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

            //MailMessage - What sends the email
            MailMessage mm = new MailMessage(

                //FROM
                ConfigurationManager.AppSettings["EmailUser"].ToString(),

                //TO
                ConfigurationManager.AppSettings["EmailTo"].ToString(),

                //SUBJECT
                cvm.Subject,

                //BODY of the email
                body

                );

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
                ViewBag.CustomerMessage = $"We're sorry, but your request could not be completed at this time. " +
                    $"Please try again later.<br /> Error Message: <br />{ex.StackTrace}";

                //Send them back to the View with their completed form data
                return View(cvm);
            }

            //If email was sent successfully, display confirmation message

            return View("EmailConfirmation", cvm);
        }

    }
}
