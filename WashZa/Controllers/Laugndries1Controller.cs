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
    public class Laugndries1Controller : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Laugndries1
        public ActionResult Index()
        {
            return View(db.Laundries.ToList());
        }

        // GET: Laugndries1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return HttpNotFound();
            }
            return View(laundry);
        }

        // GET: Laugndries1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Laugndries1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Active,userid,payid,amount,flag_payment,flag_send")] Laundry laundry)
        {
            if (ModelState.IsValid)
            {
                db.Laundries.Add(laundry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(laundry);
        }

        // GET: Laugndries1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return HttpNotFound();
            }
            return View(laundry);
        }

        // POST: Laugndries1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Active,userid,payid,amount,flag_payment,flag_send")] Laundry laundry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(laundry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(laundry);
        }

        // GET: Laugndries1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laundry laundry = db.Laundries.Find(id);
            if (laundry == null)
            {
                return HttpNotFound();
            }
            return View(laundry);
        }

        // POST: Laugndries1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Laundry laundry = db.Laundries.Find(id);
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
