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
    public class ChannelController2 : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: Channels
        public ActionResult Index()
        {
            return View(db.Channel.ToList());
        }

        #region Create
        // GET: Channels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Channels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChannelID,ChannelType")] Channel channel)
        {
            if (ModelState.IsValid)
            {
                db.Channel.Add(channel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(channel);
        }
        #endregion

        #region Edit
        // GET: Channels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Channel channel = db.Channel.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        // POST: Channels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChannelID,ChannelType")] Channel channel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(channel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(channel);
        }
        #endregion

        #region Delete

        // GET: Channels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Channel channel = db.Channel.Find(id);
            if (channel == null)
            {
                return HttpNotFound();
            }
            return View(channel);
        }

        // POST: Channels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Channel channel = db.Channel.Find(id);
            db.Channel.Remove(channel);
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