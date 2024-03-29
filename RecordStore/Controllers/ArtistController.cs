﻿using System;
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
    public class ArtistController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: Artist
        public ActionResult Index()
        {
            return View(db.Artist.ToList());
        }

        // GET: Artist/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artist.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Artist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ArtistID,ArtistName,IsStillActive")] Artist artist)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Artist.Add(artist);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(artist);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistID,ArtistName,IsStillActive")] Artist artist)
        {

            if (ModelState.IsValid)
            {

                #region Create Artist Name For Sorting

                //If ArtistName begins with "The ", remove and place at end of name
                string artistName = artist.ArtistName;

                if (artistName != null)
                {
                    //first param of substring is the starting point, next param is the ending point
                    string firstWord = artistName.Substring(0, artistName.IndexOf(' ')+1);

                    if (firstWord.ToLower() == "the ")
                    {
                        //in this overload of substring, the starting point is after the first space, and through the end
                        artistName = artistName.Substring(artistName.IndexOf(' ')+1) + ", The";
                    }
                    else
                    {
                        artistName = artist.ArtistName; //use original unput
                    }

                }//end if(albumCover != null)

                artist.ArtistNameSort = artistName;

                #endregion


                db.Artist.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }





        // GET: Artist/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artist.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }


        // POST: Artist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID,ArtistName,IsStillActive")] Artist artist)
        {
            if (ModelState.IsValid)
            {

                #region Create Artist Name For Sorting

                //If ArtistName begins with "The ", remove and place at end of name
                string artistName = artist.ArtistName;

                if (artistName != null)
                {
                    //first param of substring is the starting point, next param is the ending point
                    string firstWord = artistName.Substring(0, artistName.IndexOf(' ') + 1);

                    if (firstWord.ToLower() == "the ")
                    {
                        //in this overload of substring, the starting point is after the first space, and through the end
                        artistName = artistName.Substring(artistName.IndexOf(' ') + 1) + ", The";
                    }
                    else
                    {
                        artistName = artist.ArtistName; //use original unput
                    }

                }//end if(albumCover != null)

                artist.ArtistNameSort = artistName;

                #endregion

                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artist/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artist.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artist.Find(id);
            db.Artist.Remove(artist);
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
