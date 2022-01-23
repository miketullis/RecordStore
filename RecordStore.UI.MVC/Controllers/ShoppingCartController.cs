using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecordStore.UI.MVC.Models; //added for access to CartItemViewModel class

namespace RecordStore.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Index
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //Pull the session-base cart into to the local variable
            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                // New or empty shopping cart -- display message stating cart is empty
                ViewBag.Message = "There are no items currently in your cart.";

                //Create empty shopping cart
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                // If cart contains item(s), clear out the Viewbag.Message variable
                ViewBag.Message = null;
            }

            return View(shoppingCart);
        }
        #endregion

        #region Update Cart
        public ActionResult UpdateCart(int albumID, int qty)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            //target the correct item using ID
            shoppingCart[albumID].Qty = qty;

            //Return the LOCAL cart to session (GLOBAL)
            Session["cart"] = shoppingCart;


            //if (shoppingCart == null || shoppingCart.Count == 0)
            //{
            //    // New or empty shopping cart -- display message stating cart is empty
            //    ViewBag.Message = "There are no items currently in your cart.";

            //    //Create empty shopping cart
            //    shoppingCart = new Dictionary<int, CartItemViewModel>();
            //}
            //else
            //{
            //    // If cart contains item(s), clear out the Viewbag.Message variable
            //    ViewBag.Message = null;
            //}

            return RedirectToAction("Index");
        }
        #endregion

        #region Remove From Cart
        public ActionResult RemoveFromCart(int id)
        {

            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            shoppingCart.Remove(id);

            Session["cart"] = shoppingCart;

            return RedirectToAction("Index");
        }
        #endregion

        #region Checkout
        [HttpGet]
        public ActionResult Checkout()
        {
            return View();
        }

        #endregion

    }
}