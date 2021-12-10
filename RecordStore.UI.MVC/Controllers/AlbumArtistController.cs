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
    public class AlbumArtistController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: AlbumArtist
        public ActionResult Index()
        {
            var albumArtists = db.AlbumArtist.Include(a => a.Album).Include(a => a.Artist);
            return View(albumArtists.ToList());
        }

        // GET: AlbumArtist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumArtist albumArtist = db.AlbumArtist.Find(id);
            if (albumArtist == null)
            {
                return HttpNotFound();
            }
            return View(albumArtist);
        }

        // GET: AlbumArtist/Create
        public ActionResult Create()
        {
            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName");
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName");
            return View();
        }

        // POST: AlbumArtist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumArtistID,AlbumID,ArtistID")] AlbumArtist albumArtist)
        {
            if (ModelState.IsValid)
            {
                db.AlbumArtist.Add(albumArtist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName", albumArtist.AlbumID);
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", albumArtist.ArtistID);
            return View(albumArtist);
        }

        // GET: AlbumArtist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumArtist albumArtist = db.AlbumArtist.Find(id);
            if (albumArtist == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName", albumArtist.AlbumID);
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", albumArtist.ArtistID);
            return View(albumArtist);
        }

        // POST: AlbumArtist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumArtistID,AlbumID,ArtistID")] AlbumArtist albumArtist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albumArtist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumID = new SelectList(db.Album, "AlbumID", "AlbumName", albumArtist.AlbumID);
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", albumArtist.ArtistID);
            return View(albumArtist);
        }

        // GET: AlbumArtist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumArtist albumArtist = db.AlbumArtist.Find(id);
            if (albumArtist == null)
            {
                return HttpNotFound();
            }
            return View(albumArtist);
        }

        // POST: AlbumArtist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlbumArtist albumArtist = db.AlbumArtist.Find(id);
            db.AlbumArtist.Remove(albumArtist);
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
