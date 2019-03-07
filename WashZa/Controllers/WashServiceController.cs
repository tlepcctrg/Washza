using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WashZa.Models;

namespace WashZa.Controllers
{
    public class WashServiceController : ApiController
    {
        Laundry cd = new Laundry();
        Entities1 db = new Entities1();
        // PUT api/washService/5
        public async Task<IHttpActionResult> Put(string id, Laundry item)
        {
            string sr = id.Replace("chksend_","").Replace("chkpayment_","");//.ToString("00000");
            var rr = db.Laundries.Where(x => x.Id.Equals(sr));
            var srr = rr.FirstOrDefault();
            srr.flag_send = string.IsNullOrEmpty(item.flag_send) ? srr.flag_send : item.flag_send;
            srr.flag_payment = string.IsNullOrEmpty(item.flag_payment) ? srr.flag_payment : item.flag_payment;
            db.Entry(srr).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(srr);
        }
    }
}
