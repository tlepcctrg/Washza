using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
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
        string orderNo = DateTime.Now.Ticks.ToString().Substring(0, 6);
        string orderDate = DateTime.Now.ToString("dd MMM yyyy");
        decimal totalAmtStr;
        string accountNo = "0123456789012";
        string accountName = "John Willion";
        string branch = "Phahon Yothin Branch";
        string bank = "Kasikorn Bank";
        static DataTable dt = new DataTable();
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
                             userid = b.Id
                         }).ToList();
            string ss = "";
            return View(query);
        }
        [AllowAnonymous]
        public ActionResult Print_Invoicxe() {
            dt.Columns.Add("NO", Type.GetType("System.String"));
            dt.Columns.Add("ITEM", Type.GetType("System.String"));
            dt.Columns.Add("QUANTITY", Type.GetType("System.String"));
            dt.Columns.Add("AMOUNT", Type.GetType("System.Decimal"));

            for (int i = 0; i < 10; ++i)
            {
                dt.Rows.Add();
                dt.Rows[i]["NO"] = (i + 1).ToString();
                dt.Rows[i]["ITEM"] = "Item " + i.ToString();
                dt.Rows[i]["QUANTITY"] = (i + 1).ToString();
                dt.Rows[i]["AMOUNT"] = (i + 1).ToString();
                totalAmtStr += (i + 1);
                //dt.Rows.Add();
            }
            string ss = dt.Rows[0]["NO"].ToString();
            return View();
        }
        [HttpPost]
        public ActionResult Print_Invoicxe(string ss)
        {
            DownloadPDF(CreatePDF());
            return View();
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
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads/" + fullName), fileName);
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
        public void SendEmail(string path, string type, string username)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("phutisuk96@gmail.com", "phutisuk96"));

                // From
                mailMsg.From = new MailAddress("admin@washza.com", "washza");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "มีไฟล์บิลจ่ายเงินถูกส่งมา" + username;
                string text = "text body";
                string html = @"<p>มีไฟล์บิลจ่ายเงินถูกส่งมา โดย " + username + "</p><img src=" + path + "/>";
                Attachment atc = new Attachment(path, type);
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
        [AllowAnonymous]
        public ActionResult Print_Invoice()
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
                             userid = b.Id
                         }).ToList();
            string ss = "";
            return View(query);
        }
        protected MemoryStream CreatePDF()
        {
            // Create a Document object
            Document document = new Document(PageSize.A4, 70, 70, 70, 70);

            //MemoryStream
            MemoryStream PDFData = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, PDFData);

            // First, create our fonts
            var titleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Rectangle pageSize = writer.PageSize;

            // Open the Document for writing
            document.Open();
            //Add elements to the document here

            #region Top table
            // Create the header table 
            PdfPTable headertable = new PdfPTable(3);
            headertable.HorizontalAlignment = 0;
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 4, 2, 4 });  // then set the column's __relative__ widths
            headertable.DefaultCell.Border = Rectangle.NO_BORDER;
            //headertable.DefaultCell.Border = Rectangle.BOX; //for testing
            headertable.SpacingAfter = 30;
            PdfPTable nested = new PdfPTable(1);
            nested.DefaultCell.Border = Rectangle.BOX;
            PdfPCell nextPostCell1 = new PdfPCell(new Phrase("WashZa Co.,Ltd", bodyFont));
            nextPostCell1.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            nested.AddCell(nextPostCell1);
            PdfPCell nextPostCell2 = new PdfPCell(new Phrase("111/206 Moo 9, Ramkhamheang Road,", bodyFont));
            nextPostCell2.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            nested.AddCell(nextPostCell2);
            PdfPCell nextPostCell3 = new PdfPCell(new Phrase("Nonthaburi 11120", bodyFont));
            nextPostCell3.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            nested.AddCell(nextPostCell3);
            PdfPCell nesthousing = new PdfPCell(nested);
            nesthousing.Rowspan = 4;
            nesthousing.Padding = 0f;
            headertable.AddCell(nesthousing);

            headertable.AddCell("");
            PdfPCell invoiceCell = new PdfPCell(new Phrase("INVOICE", titleFont));
            invoiceCell.HorizontalAlignment = 2;
            invoiceCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(invoiceCell);
            PdfPCell noCell = new PdfPCell(new Phrase("No :", bodyFont));
            noCell.HorizontalAlignment = 2;
            noCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(noCell);
            headertable.AddCell(new Phrase(orderNo, bodyFont));
            PdfPCell dateCell = new PdfPCell(new Phrase("Date :", bodyFont));
            dateCell.HorizontalAlignment = 2;
            dateCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(dateCell);
            headertable.AddCell(new Phrase(orderDate, bodyFont));
            PdfPCell billCell = new PdfPCell(new Phrase("Bill To :", bodyFont));
            billCell.HorizontalAlignment = 2;
            billCell.Border = Rectangle.NO_BORDER;
            headertable.AddCell(billCell);
            headertable.AddCell(new Phrase("Customer Name" + "\n" + "Address", bodyFont));
            document.Add(headertable);
            #endregion

            #region Items Table
            //Create body table
            PdfPTable itemTable = new PdfPTable(4);
            itemTable.HorizontalAlignment = 0;
            itemTable.WidthPercentage = 100;
            itemTable.SetWidths(new float[] { 10, 40, 20, 30 });  // then set the column's __relative__ widths
            itemTable.SpacingAfter = 40;
            itemTable.DefaultCell.Border = Rectangle.BOX;
            PdfPCell cell1 = new PdfPCell(new Phrase("NO", boldTableFont));
            cell1.HorizontalAlignment = 1;
            itemTable.AddCell(cell1);
            PdfPCell cell2 = new PdfPCell(new Phrase("สินค้า/บริการ", boldTableFont));
            cell2.HorizontalAlignment = 1;
            itemTable.AddCell(cell2);
            PdfPCell cell3 = new PdfPCell(new Phrase("QUANTITY", boldTableFont));
            cell3.HorizontalAlignment = 1;
            itemTable.AddCell(cell3);
            PdfPCell cell4 = new PdfPCell(new Phrase("จำนวนเงิน(บาท)", boldTableFont));
            cell4.HorizontalAlignment = 1;
            itemTable.AddCell(cell4);

            foreach (DataRow row in dt.Rows)
            {
                PdfPCell numberCell = new PdfPCell(new Phrase(row["NO"].ToString(), bodyFont));
                numberCell.HorizontalAlignment = 0;
                numberCell.PaddingLeft = 10f;
                numberCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(numberCell);

                PdfPCell descCell = new PdfPCell(new Phrase("บริการซัก เรียน", bodyFont));
                descCell.HorizontalAlignment = 0;
                descCell.PaddingLeft = 10f;
                descCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(descCell);

                PdfPCell qtyCell = new PdfPCell(new Phrase(row["QUANTITY"].ToString(), bodyFont));
                qtyCell.HorizontalAlignment = 0;
                qtyCell.PaddingLeft = 10f;
                qtyCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(qtyCell);

                PdfPCell amtCell = new PdfPCell(new Phrase(row["AMOUNT"].ToString(), bodyFont));
                amtCell.HorizontalAlignment = 1;
                amtCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                itemTable.AddCell(amtCell);

            }
            // Table footer
            PdfPCell totalAmtCell1 = new PdfPCell(new Phrase(""));
            totalAmtCell1.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell1);
            PdfPCell totalAmtCell2 = new PdfPCell(new Phrase(""));
            totalAmtCell2.Border = Rectangle.TOP_BORDER; //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            itemTable.AddCell(totalAmtCell2);
            PdfPCell totalAmtStrCell = new PdfPCell(new Phrase("Total Amount", boldTableFont));
            totalAmtStrCell.Border = Rectangle.TOP_BORDER;   //Rectangle.NO_BORDER; //Rectangle.TOP_BORDER;
            totalAmtStrCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtStrCell);
            PdfPCell totalAmtCell = new PdfPCell(new Phrase(totalAmtStr.ToString("#,###.00"), boldTableFont));
            totalAmtCell.HorizontalAlignment = 1;
            itemTable.AddCell(totalAmtCell);

            PdfPCell cell = new PdfPCell(new Phrase("*** Please note that ABC Co., Ltd’s bank account is USD Bank Account ***", bodyFont));
            cell.Colspan = 4;
            cell.HorizontalAlignment = 1;
            itemTable.AddCell(cell);
            document.Add(itemTable);
            #endregion

            Chunk transferBank = new Chunk("Your Bank Account:", boldTableFont);
            transferBank.SetUnderline(0.1f, -2f); //0.1 thick, -2 y-location
            document.Add(transferBank);
            document.Add(Chunk.NEWLINE);

            // Bank Account Info
            PdfPTable bottomTable = new PdfPTable(3);
            bottomTable.HorizontalAlignment = 0;
            bottomTable.TotalWidth = 300f;
            bottomTable.SetWidths(new int[] { 90, 10, 200 });
            bottomTable.LockedWidth = true;
            bottomTable.SpacingBefore = 20;
            bottomTable.DefaultCell.Border = Rectangle.NO_BORDER;
            bottomTable.AddCell(new Phrase("Account No", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountNo, bodyFont));
            bottomTable.AddCell(new Phrase("Account Name", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(accountName, bodyFont));
            bottomTable.AddCell(new Phrase("Branch", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(branch, bodyFont));
            bottomTable.AddCell(new Phrase("Bank", bodyFont));
            bottomTable.AddCell(":");
            bottomTable.AddCell(new Phrase(bank, bodyFont));
            document.Add(bottomTable);

            //Approved by
            PdfContentByte cb = new PdfContentByte(writer);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(pageSize.GetLeft(300), 200);
            cb.ShowText("Approved by,");
            cb.EndText();
            //Image Singature
            //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/Bill_Gates2.png"));
            //logo.SetAbsolutePosition(pageSize.GetLeft(300), 140);
            //document.Add(logo);

            cb = new PdfContentByte(writer);
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
            cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(pageSize.GetLeft(70), 100);
            cb.ShowText("Thank you for your business! If you have any questions about your order, please contact us at 800-555-NORTH.");
            cb.EndText();

            writer.CloseStream = false; //set the closestream property
                                        // Close the Document without closing the underlying stream
            document.Close();
            return PDFData;
        }
        protected void DownloadPDF(System.IO.MemoryStream PDFData)
        {
            // Clear response content & headers
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.Charset = string.Empty;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", orderNo));
            Response.OutputStream.Write(PDFData.GetBuffer(), 0, PDFData.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();
        }


    }
}
