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
    public class AlbumGenreController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: AlbumGenre
        public ActionResult Index()
        {
            var albumGenres = db.AlbumGenre.Include(a => a.Album).Include(a => a.Genre);
            return View(albumGenres.ToList());
        }

        // GET: AlbumGenre/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumGenre albumGenre = db.AlbumGenre.Find(id);
            if (albumGenre == null)
            {
                return HttpNotFound();
            }
            return View(albumGenre);
        }

        // GET: AlbumGenre/Create
        public ActionResult Create()
        {
            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName");
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName");
            return View();
        }

        // POST: AlbumGenre/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumGenreID,AlbumID,GenreID")] AlbumGenre albumGenre)
        {
            if (ModelState.IsValid)
            {
                db.AlbumGenre.Add(albumGenre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName", albumGenre.AlbumID);
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName", albumGenre.GenreID);
            return View(albumGenre);
        }

        // GET: AlbumGenre/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumGenre albumGenre = db.AlbumGenre.Find(id);
            if (albumGenre == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName", albumGenre.AlbumID);
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName", albumGenre.GenreID);
            return View(albumGenre);
        }

        // POST: AlbumGenre/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumGenreID,AlbumID,GenreID")] AlbumGenre albumGenre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albumGenre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName", albumGenre.AlbumID);
            ViewBag.GenreID = new SelectList(db.Genre, "GenreID", "GenreName", albumGenre.GenreID);
            return View(albumGenre);
        }

        // GET: AlbumGenre/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumGenre albumGenre = db.AlbumGenre.Find(id);
            if (albumGenre == null)
            {
                return HttpNotFound();
            }
            return View(albumGenre);
        }

        // POST: AlbumGenre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlbumGenre albumGenre = db.AlbumGenre.Find(id);
            db.AlbumGenre.Remove(albumGenre);
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
