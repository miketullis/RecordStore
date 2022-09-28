using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecordStore.Data.EF;//added for access to the Domain/Entity models
using System.ComponentModel.DataAnnotations; //added to validation and display of metadata


namespace RecordStore.UI.MVC.Models
{
    public class CartItemViewModel
    {

        [Range(1, int.MaxValue)] //ensures values are greater than zero
        public int Qty { get; set; }

        public Album Product { get; set; }

        //Fully-Qualified Constructor
        public CartItemViewModel(int qty, Album product)
        {
            Qty = qty;
            Product = product;
        }



    }
}