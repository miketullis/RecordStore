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
    public class AlbumStatusController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: AlbumStatus
        public ActionResult Index()
        {
            return View(db.AlbumStatus.ToList());
        }

        // GET: AlbumStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumStatus albumStatus = db.AlbumStatus.Find(id);
            if (albumStatus == null)
            {
                return HttpNotFound();
            }
            return View(albumStatus);
        }

        // GET: AlbumStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlbumStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumStatusID,AlbumStatusName,StatusNotes")] AlbumStatus albumStatus)
        {
            if (ModelState.IsValid)
            {
                db.AlbumStatus.Add(albumStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(albumStatus);
        }

        // GET: AlbumStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumStatus albumStatus = db.AlbumStatus.Find(id);
            if (albumStatus == null)
            {
                return HttpNotFound();
            }
            return View(albumStatus);
        }

        // POST: AlbumStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumStatusID,AlbumStatusName,StatusNotes")] AlbumStatus albumStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albumStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(albumStatus);
        }

        // GET: AlbumStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumStatus albumStatus = db.AlbumStatus.Find(id);
            if (albumStatus == null)
            {
                return HttpNotFound();
            }
            return View(albumStatus);
        }

        // POST: AlbumStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlbumStatus albumStatus = db.AlbumStatus.Find(id);
            db.AlbumStatus.Remove(albumStatus);
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
