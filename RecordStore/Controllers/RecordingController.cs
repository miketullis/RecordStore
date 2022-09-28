using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecordStore.Data.EF;

namespace RecordStore.UI.MVC.Controllers
{
    [Authorize]
    public class RecordingController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: Recordings
        public ActionResult Index()
        {
            return View(db.Recording.ToList());
        }

        #region Create
        // GET: Recordings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recordings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordingID,RecordingType")] Recording recording)
        {
            if (ModelState.IsValid)
            {
                db.Recording.Add(recording);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(recording);
        }
        #endregion

        #region Edit
        // GET: Recordings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recording recording = db.Recording.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }
            return View(recording);
        }

        // POST: Recordings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordingID,RecordingType")] Recording recording)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recording).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recording);
        }
        #endregion

        #region Delete

        // GET: Recordings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recording recording = db.Recording.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }
            return View(recording);
        }

        // POST: Recordings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recording recording = db.Recording.Find(id);
            db.Recording.Remove(recording);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


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
