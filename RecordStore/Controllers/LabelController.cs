using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecordStore.Data.EF;
using RecordStore.UI.MVC.Models;//Added to access to the Models


namespace RecordStore.UI.MVC.Controllers
{
    [Authorize]
    public class LabelController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        // GET: Labels
        public ActionResult Index()
        {
            var labels = db.Label.OrderBy(a => a.LabelName).ToList();
            return View(labels);
        }

        #region Create
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
        #endregion

        #region Edit
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
        #endregion

        #region Delete
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
        #endregion


        #region AJAX Create, Edit, & Delete, Label
        //******** AJAX *********//
        //GET: Label/Create
        public PartialViewResult AjaxLabelCreate()
        {
            return PartialView();
        }

        //POST: Label/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxLabelCreate(Label label)
        {
            db.Label.Add(label);
            db.SaveChanges();
            return Json(label);
        }

        //GET: Label/Delete
        public PartialViewResult AjaxLabelDelete()
        {
            return PartialView();
        }

        //POST: Label/Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AjaxLabelDelete(int id)
        {
            Label label = db.Label.Find(id);
            db.Label.Remove(label);
            db.SaveChanges();

            string confirmMessage = string.Format($"Deleted {label.LabelName} from the database.");
            return Json(new { id = id, message = confirmMessage });
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
