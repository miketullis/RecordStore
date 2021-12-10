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
using PagedList;
using PagedList.Mvc;
using RecordStore.UI.MVC.Models;//Added to access to the Models


namespace RecordStore.UI.MVC.Controllers
{
    public class AlbumController : Controller
    {
        private RecordStoreEntities db = new RecordStoreEntities();


        public ActionResult Index()
        {

            AlbumViewModel avm = new AlbumViewModel();

            var albums = db.Album.OrderBy(a => a.AlbumName).ToList();

            return View(albums.ToList());

        }



        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName");
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType");
            ViewBag.LabelID = new SelectList(db.Label, "LabelID", "LabelName");
            //ViewBag.ArtistID = new SelectList(db.Artists, "ArtistID", "ArtistName");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumID,AlbumName,ReleaseYear,ArtistID,Description,LabelID,CompilationAlbum,CatalogNum,Price,IsInPrint,FormatID,UnitsInStock,AlbumStatusID,AlbumImage,Num")] Album album, HttpPostedFileBase albumCover)
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
                    //is extension valis and filesizre under 4mb
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

                        //Updated image file to name of images saved to DB
                        album.AlbumImage = file;
                    }
                }
                #endregion

                db.Album.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName", album.AlbumStatusID);
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType", album.FormatID);
            ViewBag.LabelID = new SelectList(db.Label, "LabelID", "LabelName", album.LabelID);
            return View(album);
        }













        // GET: Albums/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName", album.AlbumStatusID);
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType", album.FormatID);
            ViewBag.LabelID = new SelectList(db.Label, "LabelID", "LabelName", album.LabelID);
            //return View(album);

            return View(album);
        }



        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumID,AlbumName,ReleaseYear,ArtistID,Description,LabelID,CompilationAlbum,CatalogNum,Price,IsInPrint,FormatID,UnitsInStock,AlbumStatusID,AlbumImage,Num")] Album album, HttpPostedFileBase albumCover)
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
                    //declare list of valid file extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    //is extension valis and filesize under 4mb
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
                    }

                    //Updated image file to name of images saved to DB
                    album.AlbumImage = file;
                }
                #endregion

                db.Album.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumStatusID = new SelectList(db.AlbumStatus, "AlbumStatusID", "AlbumStatusName", album.AlbumStatusID);
            ViewBag.FormatID = new SelectList(db.Format, "FormatID", "FormatType", album.FormatID);
            ViewBag.LabelID = new SelectList(db.Label, "LabelID", "LabelName", album.LabelID);
            return View(album);
        }





        // GET: Albums/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Album.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //delete album image if album is deleted
            Album album = db.Album.Find(id);
            string path = Server.MapPath("~/Content/assets/images/albumImages/");
            ImageUtility.Delete(path, album.AlbumImage);

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




        public ActionResult AlbumViewModel()
        {
            //var albums = db.Album.Include(a => a.Format).Include(a => a.Label).Include(a => a.AlbumStatus).Include(a => a.AlbumGenre).Include(a => a.AlbumArtist); 
            //variable including adjacent tables with Album table

            var albums = db.Album;

            var genre = db.Genre;

            var label = db.Label;

            var format = db.Format;

            var artist = db.Artist;

            var albumArtist = db.AlbumArtist;

            var albumGenre = db.AlbumGenre;

            var albumStatus = db.AlbumStatus;

            var albumView = from a in albums
                            join ag in albumGenre
                            on a.AlbumID equals ag.AlbumID
                            join g in genre
                            on ag.GenreID equals g.GenreID
                            join aa in albumArtist
                            on a.AlbumID equals aa.AlbumID
                            join ar in artist
                            on aa.ArtistID equals ar.ArtistID
                            join al in albumStatus
                            on a.AlbumStatusID equals al.AlbumStatusID
                            join l in label
                            on a.LabelID equals l.LabelID
                            join f in format
                            on a.FormatID equals f.FormatID
                            select new AlbumViewModel()
                            {
                                AlbumID = a.AlbumID,
                                AlbumName = a.AlbumName,
                                ReleaseYear = a.ReleaseYear,
                                Description = a.Description,
                                LabelID = a.LabelID,
                                LabelName = l.LabelName,
                                CompilationAlbum = a.CompilationAlbum,
                                Price = a.Price,
                                IsInPrint = a.IsInPrint,
                                FormatID = a.FormatID,
                                FormatType = f.FormatType,
                                UnitsInStock = a.UnitsInStock,
                                AlbumStatusID = a.AlbumStatusID,
                                AlbumStatusName = al.AlbumStatusName,
                                AlbumImage = a.AlbumImage,
                                Num = a.Num,
                                Tracks = a.Tracks,
                                GenreName = g.GenreName,
                                ArtistName = ar.ArtistName

                            };

            return View(albumView.ToList());
        }

    }
}