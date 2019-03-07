using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using WashZa.Models;
namespace WashZa.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult Checkwash()
        {
            Entities1 db = new Entities1();
            WashzaEntities1 aa = new WashzaEntities1();
            var item = (from s in aa.AspNetUsers
                        select new WashLaundry
                        {
                            userid = s.Id,
                            First_Name = s.First_Name,
                            Last_Name = s.Last_Name,
                            Address = s.Address
                        }).ToList();
            var item2 = (from s in db.Laundries
                         select new WashLaundry
                         {
                             Id = s.Id,
                             userid = s.userid,
                             flag_payment = string.IsNullOrEmpty(s.flag_payment) || s.flag_payment.Equals("0") ? false : true,
                             flag_send = string.IsNullOrEmpty(s.flag_send) || s.flag_send.Equals("0") ? false : true,
                             amount = s.amount,
                             Active = s.Active
                         }).ToList();
            var query = (from s in item2
                         join b in item on s.userid equals b.userid
                         select new WashLaundry
                         {
                             Id = s.Id,
                             amount = s.amount,
                             First_Name = b.First_Name,
                             Last_Name = b.Last_Name,
                             flag_payment = s.flag_payment,
                             flag_send = s.flag_send,
                             Active = s.Active,
                             Address = b.Address,
                             payid = s.payid,
                             userid = b.userid
                         }).ToList();
            string ss = "";
            return View(query);
        }

        [AllowAnonymous]
        public ActionResult Checkwashzz()
        {
            Entities1 db = new Entities1();
            WashzaEntities1 aa = new WashzaEntities1();
            var item = (from s in aa.AspNetUsers
                        select new WashLaundry
                        {
                            userid = s.Id,
                            First_Name = s.First_Name,
                            Last_Name = s.Last_Name,
                            Address = s.Address
                        }).ToList();
            var item2 = (from s in db.Laundries
                         select new WashLaundry
                        {
                            Id = s.Id,
                            userid= s.userid,
                             flag_payment = string.IsNullOrEmpty(s.flag_payment) || s.flag_payment.Equals("0") ? false : true,
                             flag_send = string.IsNullOrEmpty(s.flag_send) || s.flag_send.Equals("0") ? false : true,
                             amount = s.amount,
                             Active = s.Active
                        }).ToList();
            var query = (from s in item2
                         join b in item on s.userid equals b.userid
                        select new WashLaundry
                        {
                            Id = s.Id,
                            amount = s.amount,
                            First_Name = b.First_Name,
                            Last_Name = b.Last_Name,
                            flag_payment = s.flag_payment,
                            flag_send = s.flag_send,
                            Active = s.Active,
                            Address = b.Address,
                            payid = s.payid,
                            userid = b.Id
                        }).ToList();
            string ss = "";
            return View(query);
        }
        [Authorize]
        public ActionResult Payment()
        {

            ViewBag.Message = "";

            return View();
        }
        
        [HttpPost]
        public ActionResult Payment(HttpPostedFileBase file)
        {
            string fullName = "";
            
            if (User != null)
            {
                var context = new ApplicationDbContext();
                var username = User.Identity.Name;

                if (!string.IsNullOrEmpty(username))
                {
                    var user = context.Users.SingleOrDefault(u => u.UserName == username);
                    fullName = string.Concat(new string[] { user.Email.Split('@')[0], "" });
                    
                }
            }
            if (file.ContentLength > 0 && file.ContentType.Contains("image"))
            {
                ViewBag.Message = "อัพโหลดไฟล์สำเร็จ";

                var fileName = Path.GetFileName(file.FileName);
                if (!Directory.Exists(Server.MapPath("~/App_Data/uploads/" + fullName)))
                {
                    Directory.CreateDirectory(Server.MapPath("~/App_Data/uploads/" + fullName));
                }
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads/"+ fullName), fileName);
               // SendEmail(path, file.ContentType, fullName);
                file.SaveAs(path);
            }
            else
            {
                ViewBag.Message = "อัพโหลดไฟล์รูปภาพเท่านั้น";
                return View();
            }
            return View();
           
        }
        public void SendEmail(string path, string type,string username)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("phutisuk96@gmail.com", "phutisuk96"));

                // From
                mailMsg.From = new MailAddress("admin@washza.com", "washza");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "มีไฟล์บิลจ่ายเงินถูกส่งมา"+username;
                string text = "text body";
                string html = @"<p>มีไฟล์บิลจ่ายเงินถูกส่งมา โดย "+username+"</p><img src="+path+"/>";
                Attachment atc = new Attachment(path,type);
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                mailMsg.Attachments.Add(atc);
                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }

        }
    }
}