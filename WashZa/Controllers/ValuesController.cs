using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using WashZa.Models;
using WashZa;
using System.Threading.Tasks;

namespace WashZa.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        // GET: Values
        Laundry cd = new Laundry();
        Entities1 db = new Entities1();
        // GET api/values
        public IEnumerable<Laundry> Get()
        {
            return db.Laundries;
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            var ss = db.Laundries.Where(x => x.Id.Equals("00001"));
           
            return Ok(ss);
        }

        // POST api/values
        [System.Web.Mvc.HttpPost]
        public async Task<IHttpActionResult> Post(Laundry item)
        {
            db.Laundries.Add(item);
            await db.SaveChangesAsync();
            var ss = db.Laundries.Where(x => x.Id.Equals(item.Id));
            return Ok(ss);
        }
        // PUT api/values/5
        public async Task<IHttpActionResult> Put(string id, Laundry item)
        {
            string sr = id;//.ToString("00000");
            var rr = db.Laundries.Where(x => x.Id.Equals(sr));
            var srr  = rr.FirstOrDefault();
            srr.Active = item.Active;
            db.Entry(srr).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok(srr);
        }
        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
        public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

    }
}