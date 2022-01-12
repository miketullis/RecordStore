using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecordStore.Data.EF;
using System.Drawing; //Added for access to the Image class
using RecordStore.UI.MVC.Utilities; //Added for access to Image Utilities
using RecordStore.UI.MVC.Models;//Added to access to the Models


namespace RecordStore.UI.MVC.Controllers
{
    public class AlbumController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();

        public ActionResult Index()
        {
            var albums = db.Album.ToList();
            return View(albums);
        }


        #region Details
        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }
        #endregion

        #region Create
        // GET: Albums/Create
        public ActionResult Create()
        {
            var albums = db.Album.ToList();
            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName");
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType");
            ViewBag.LabelID = new SelectList(db.Label.OrderBy(x => x.LabelName), "LabelID", "LabelName");

            var albumArtist = db.AlbumArtist.ToList();
            ViewBag.PrimaryArtist = new SelectList(db.AlbumArtist, "PrimaryArtist");

            //Since going through linking tables to get this info, pass a list of artists then loop through them 
            ViewBag.Artist = db.Artist.OrderBy(x => x.ArtistName).ToList();
            ViewBag.Genre = db.Genre.OrderBy(x => x.GenreName).ToList();


            return View();
        }

        // POST: Albums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumID,AlbumName,ReleaseYear,ArtistID,GenreID,Description,LabelID,CompilationAlbum,CatalogNum,Price,IsInPrint,FormatID,UnitsInStock,AlbumStatusID,AlbumImage,Num,Tracks,Year")] Album album, HttpPostedFileBase albumCover, int[] artists, int[] genres, int primaryArtistId)
        {
            if (ModelState.IsValid)
            {
                #region Upload AlbumImage File

                string file = "noImage.jpg";

                if (albumCover != null)
                {
                    //reassign varible holding default image name to uploaded image name
                    file = albumCover.FileName;
                    //grab file extension using Substring()
                    string ext = file.Substring(file.LastIndexOf('.'));
                    //declare list of valid file extension
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //is extension valid and filesize under 4mb
                    if (validExts.Contains(ext.ToLower()) && albumCover.ContentLength <= 4194304)
                    {
                        file = Guid.NewGuid() + ext; //Create a unique new filename for the file (using a GUID)

                        #region Resize Image Functionality
                        string savePath = Server.MapPath("~/Content/assets/images/albumImages/"); //Define image save path
                        Image convertedImage = Image.FromStream(albumCover.InputStream); //image uploaded by user to input
                        int maxImageSize = 500; //max image size in pixels
                        int maxThumbSize = 100; //max thumbnail size in pixels
                        ImageUtility.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize); //Pass data to ResizeImage() utility
                        #endregion

                        if (album.AlbumImage != null && album.AlbumImage != "noImage.jpg")
                        {
                            string path = Server.MapPath("~/Content/assets/images/albumImages/"); //Define image save path
                            ImageUtility.Delete(path, album.AlbumImage); //Delete existing image
                        }

                    }
                }//end if(albumCover != null)

                //Set image property of album whether image was uploaded or not
                album.AlbumImage = file;

                #endregion
                db.Album.Add(album);
                db.SaveChanges();

                //create AlbumArtist record for each checkbox that was checked
                if (artists != null)
                {
                    foreach (var artistId in artists)
                    {
                        var ar = db.Artist.Find(artistId);
                        AlbumArtist aa = new AlbumArtist();
                        aa.AlbumID = album.AlbumID;
                        aa.ArtistID = ar.ArtistID;
                        // primaryArtist check and set true/false
                        if (artistId == primaryArtistId)
                        {
                            aa.PrimaryArtist = true;
                        }
                        else
                        {
                            aa.PrimaryArtist = false;
                        }
                        db.AlbumArtist.Add(aa);
                    }
                    db.SaveChanges();
                }

                //create AlbumGenre record for each checkbox that was checked
                if (genres != null)
                {
                    foreach (var genreId in genres)
                    {
                        var g = db.Genre.Find(genreId);
                        AlbumGenre ag = new AlbumGenre();
                        ag.AlbumID = album.AlbumID;
                        ag.GenreID = g.GenreID;
                        db.AlbumGenre.Add(ag);
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName", album.AlbumStatusID);
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType", album.FormatID);
            ViewBag.LabelID = new SelectList(db.Label, "LabelID", "LabelName", album.LabelID);
            ViewBag.PrimaryArtist = new SelectList(db.AlbumArtist, "PrimaryArtist");

            //Since going through linking tables to get this info, pass a list of artists then loop through them 
            ViewBag.Artist = db.Artist.ToList();
            ViewBag.Genre = db.Genre.ToList();

            return View(album);
        }

        #endregion


        #region Edit
        // GET: Albums/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            var albums = db.Album.ToList();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }

            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName", album.AlbumStatusID);
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType", album.FormatID);
            ViewBag.LabelID = new SelectList(db.Label.OrderBy(x => x.LabelName), "LabelID", "LabelName", album.LabelID);

            var albumArtist = db.AlbumArtist.ToList();
            ViewBag.PrimaryArtist = new SelectList(db.AlbumArtist, "PrimaryArtist");

            //Since going through linking tables to get this info, pass a list of artists then loop through them 
            ViewBag.Artist = db.Artist.OrderBy(x => x.ArtistName).ToList();
            ViewBag.Genre = db.Genre.OrderBy(x => x.GenreName).ToList();

            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumID,AlbumName,ReleaseYear,ArtistID,Description,LabelID,CompilationAlbum,CatalogNum,Price,IsInPrint,FormatID,UnitsInStock,AlbumStatusID,AlbumImage,Num,Tracks,Year")] Album album, HttpPostedFileBase albumCover, int[] artists, int[] genres, int primaryArtistId, int? id)
        {
            RecordStoreEntities context = new RecordStoreEntities();

            if (ModelState.IsValid)
            {
                #region Upload AlbumImage File

                //AlbumImage = existing AlbumImage
                //This way AlbumImage stays the same unless a new image is uploaded
                string file = album.AlbumImage;

                if (albumCover != null)
                {
                    //reassign varible holding default image name to uploaded image name
                    file = albumCover.FileName;
                    //grab file extension using Substring()
                    string ext = file.Substring(file.LastIndexOf('.'));
                    //declare list of valid file extension
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //is extension valid and filesize under 4mb
                    if (validExts.Contains(ext.ToLower()) && albumCover.ContentLength <= 4194304)
                    {
                        file = Guid.NewGuid() + ext; //Create a unique new filename for the file (using a GUID)

                        #region Resize Image Functionality
                        string savePath = Server.MapPath("~/Content/assets/images/albumImages/"); //Define image save path
                        Image convertedImage = Image.FromStream(albumCover.InputStream); //image uploaded by user to input
                        int maxImageSize = 500; //max image size in pixels
                        int maxThumbSize = 100; //max thumbnail size in pixels
                        ImageUtility.ResizeImage(savePath, file, convertedImage, maxImageSize, maxThumbSize); //Pass data to ResizeImage() utility
                        #endregion

                        if (album.AlbumImage != null && album.AlbumImage != "noImage.jpg")
                        {
                            string path = Server.MapPath("~/Content/assets/images/albumImages/"); //Define image save path
                            ImageUtility.Delete(path, album.AlbumImage); //Delete existing image
                        }
                    }
                }//end if(albumCover != null)
                #endregion

                //Updated image file to name of images saved to DB
                album.AlbumImage = file;

                //Delete existing AlbumArtist record 
                var albumArtists = context.AlbumArtist.Where(x => x.AlbumID == id);
                context.AlbumArtist.RemoveRange(albumArtists);
                context.SaveChanges();

                //create new AlbumArtist record for each checkbox that was checked
                if (artists != null)
                {
                    foreach (var artistId in artists)
                    {
                        AlbumArtist aa = new AlbumArtist();
                        aa.AlbumID = album.AlbumID;
                        aa.ArtistID = artistId;
                        // primaryArtist check and set true/false
                        if (artistId == primaryArtistId)
                        {
                            aa.PrimaryArtist = true;
                        }
                        else
                        {
                            aa.PrimaryArtist = false;
                        }

                        var existingAAs = context.AlbumArtist.Where(x => x.AlbumID == album.AlbumID && x.ArtistID == artistId).Count();
                        if (context.AlbumArtist.Where(x => x.AlbumID == album.AlbumID && x.ArtistID == artistId).Count() == 0)
                        {
                            context.AlbumArtist.Add(aa);
                        }
                    }
                }
                context.SaveChanges();
                
                //delete existing AlbumGenre record 
                var albumGenres = context.AlbumGenre.Where(x => x.AlbumID == id);
                context.AlbumGenre.RemoveRange(albumGenres);
                context.SaveChanges();

                //create new AlbumGenre record for each checkbox that was checked
                if (genres != null)
                {
                    foreach (var genreId in genres)
                    {
                        AlbumGenre ag = new AlbumGenre();
                        ag.AlbumID = album.AlbumID;
                        ag.GenreID = genreId;
                        if (context.AlbumGenre.Where(x => x.AlbumID == album.AlbumID && x.GenreID == genreId).Count() == 0)
                        {
                            context.AlbumGenre.Add(ag);
                        }
                    }
                }
                context.SaveChanges();
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");


            }

            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName", album.AlbumStatusID);
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType", album.FormatID);
            ViewBag.LabelID = new SelectList(db.Label, "LabelID", "LabelName", album.LabelID);
            ViewBag.PrimaryArtist = new SelectList(db.AlbumArtist, "PrimaryArtist");

            //Since going through linking tables to get this info, pass a list of artists then loop through them 
            ViewBag.Artist = db.Artist.ToList();
            ViewBag.Genre = db.Genre.ToList();

            return View(album);
        }
        #endregion

        #region Delete
        // GET: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var albums = db.Album.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var album = db.Album.Find(id);

            //delete album image if album is deleted, but not if it is the default image file
            if (album.AlbumImage != null && album.AlbumImage != "noImage.jpg")
            {
                string path = Server.MapPath("~/Content/assets/images/albumImages/");
                ImageUtility.Delete(path, album.AlbumImage);
            }

            //delete AlbumArtist & AlbumGerne records if album is deleted
            var albumArtists = db.AlbumArtist.Where(x => x.AlbumID == id);
            db.AlbumArtist.RemoveRange(albumArtists);
            var albumGenres = db.AlbumGenre.Where(x => x.AlbumID == id);
            db.AlbumGenre.RemoveRange(albumGenres);
            db.SaveChanges();

            db.Album.Remove(album);
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
        #endregion

        #region AJAX Create Artist & Label
        //******** AJAX CREATE *********//
        // Create a new record and return data as JSON
        // GET: Artist/Create
        public PartialViewResult ArtistCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ArtistCreate(Artist artist)
        {
            db.Artist.Add(artist);
            db.SaveChanges();
            return Json(artist);
        }

        // GET: Label/Create
        public PartialViewResult LabelCreate()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LabelCreate(Label label)
        {
            db.Label.Add(label);
            db.SaveChanges();
            return Json(label);
        }
        #endregion


        #region AJAX Delete
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AjaxDelete(int id)
        {
            var album = db.Album.Find(id);
            //delete album image if album is deleted, but not if it is the default image file
            if (album.AlbumImage != null && album.AlbumImage != "noImage.jpg")
            {
                string path = Server.MapPath("~/Content/assets/images/albumImages/");
                ImageUtility.Delete(path, album.AlbumImage);
            }

            //delete AlbumArtist & AlbumGenre records if album is deleted
            var albumArtists = db.AlbumArtist.Where(x => x.AlbumID == id);
            db.AlbumArtist.RemoveRange(albumArtists);
            var albumGenres = db.AlbumGenre.Where(x => x.AlbumID == id);
            db.AlbumGenre.RemoveRange(albumGenres);
            db.SaveChanges();
            db.Album.Remove(album);
            db.SaveChanges();

            string confirmMessage = string.Format($"Album {album.AlbumName} had been deleted.");
            return Json(new { id = id, message = confirmMessage });
        }
        #endregion




        #region AddToCart Functionality
        public ActionResult AddToCart(int qty, int albumID)
        {
            //create empty shell for the LOCAL shopping cart variable
            Dictionary<int, CartItemViewModel> shoppingCart = null;
            if (Session["cart"] != null)
            {
                //If a session cart exists -- put its items in the local verison
                shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            }
            else
            {
                //If a session cart doesn't exist, create one
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }

            //Find selected product by its ID
            Album product = db.Album.Where(a => a.AlbumID == albumID).FirstOrDefault();

            if (product == null)
            {
                //If product ID is null, send user back to page to try again
                return RedirectToAction("Index");
            }
            else
            {
                //If product ID is valid, add item to cart
                CartItemViewModel item = new CartItemViewModel(qty, product);

                //Put item in the local cart. If Item is already present in cart, update quanity. 
                if (shoppingCart.ContainsKey(product.AlbumID))
                {
                    shoppingCart[product.AlbumID].Qty += qty;
                }
                else
                {
                    shoppingCart.Add(product.AlbumID, item);
                }

                //Update session cart using local version.
                Session["cart"] = shoppingCart;
                ViewBag.Message = null;
            }

            //If product was added to cart, send user to shopping cart view.
            return RedirectToAction("Index", "ShoppingCart");
        }
        #endregion

    }
}