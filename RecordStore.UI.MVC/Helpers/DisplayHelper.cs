using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecordStore.UI.MVC.Helpers
{
    public static class DisplayHelper
    {

         //HTML HELPER CLASS created to format album name for sorting in album index

        public static string AlbumSwap(this HtmlHelper helper, string albumName)
        {
            string albumFormatted = "";

            if (!String.IsNullOrEmpty(albumName))
            {
                string firstWord = albumName.IndexOf(' ') > 0 ? albumName.Substring(0, albumName.IndexOf(' ')+1) : "";

                if (firstWord.Length > 0 && firstWord.ToLower() == "the ")
                {
                    albumName = albumName.Substring(albumName.IndexOf(' ')+1) + ", The";
                    albumFormatted = albumName;
                }
                else
                {
                    albumFormatted = albumName;
                }
            }
            return albumFormatted;
        }


    }
}