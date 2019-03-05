using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WashZa.Models;

namespace WashZa.Controllers
{
    public class LaundriesController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Laundries
        public ActionResult Index()
        {
            return View(db.Laundries.ToList());
        }

        // GET: Laundries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WashZa.Models.Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return HttpNotFound();
            }
            return View(laundry);
        }

        // GET: Laundries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Laundries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Active,userid,payid,amount,flag_payment,flag_send")] WashZa.Models.Laundry laundry)
        {
            if (ModelState.IsValid)
            {
                db.Laundries.Add(laundry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(laundry);
        }

        // GET: Laundries/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WashZa.Models.Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return HttpNotFound();
            }
            return View(laundry);
        }

        // POST: Laundries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Active,userid,payid,amount,flag_payment,flag_send")] WashZa.Models.Laundry laundry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(laundry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(laundry);
        }

        // GET: Laundries/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WashZa.Models.Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return HttpNotFound();
            }
            return View(laundry);
        }

        // POST: Laundries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            WashZa.Models.Laundry laundry = db.Laundries.Find(id);
            db.Laundries.Remove(laundry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
