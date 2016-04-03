using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusWebDevCor.ViewModels;
using System.Net.Mail;
using Umbraco.Core.Models;


namespace AarhusWebDevCor.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: Default
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

            MailMessage message = new MailMessage();
            message.To.Add("jesper@purup.ninja");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "asmtp.unoeuro.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("jesper@purup.ninja", "password");  
                // send mail
                smtp.Send(message);

                TempData["success"] = true;
            }


            IContent comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "Message");
            comment.SetValue("name", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);

            Services.ContentService.Save(comment);


            return RedirectToCurrentUmbracoPage();
        }

    }
}