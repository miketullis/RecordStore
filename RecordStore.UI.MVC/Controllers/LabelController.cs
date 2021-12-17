using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecordStore.Data.EF;
using PagedList;
using PagedList.Mvc;
using RecordStore.UI.MVC.Models;//Added to access to the Models



namespace RecordStore.UI.MVC.Controllers
{
    [Authorize]
    public class LabelController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: Labels

        public ActionResult Index(string searchString, int page = 1)
        {
            int pageSize = 10;

            var labels = db.Label.OrderBy(a => a.LabelName).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                labels = (from a in labels
                          where a.LabelName.ToLower().Contains(searchString.ToLower()) ||
                          a.Country.ToLower().Contains(searchString.ToLower()) 
                          select a).ToList();
            }
            return View(labels.ToPagedList(page, pageSize));
        }


        // GET: Labels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Label.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // GET: Labels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LabelID,LabelName,Country,IsStillDistributing")] Label label)
        {
            if (ModelState.IsValid)
            {
                db.Label.Add(label);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(label);
        }

        // GET: Labels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Label.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LabelID,LabelName,Country,IsStillDistributing")] Label label)
        {
            if (ModelState.IsValid)
            {
                db.Entry(label).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(label);
        }

        // GET: Labels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Label.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Label label = db.Label.Find(id);
            db.Label.Remove(label);
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
