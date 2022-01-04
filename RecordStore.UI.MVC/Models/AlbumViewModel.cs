using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecordStore.UI.MVC.Models
{
    public class AlbumViewModel
    {
        //Album Properties
        public int AlbumID { get; set; }
        public string AlbumName { get; set; }
        //public Nullable<DateTime> ReleaseYear { get; set; }
        public Nullable<int> ReleaseYear { get; set; }

        public string Description { get; set; }
        public Nullable<int> LabelID { get; set; }
        public bool CompilationAlbum { get; set; }
        public string CatalogNum { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsInPrint { get; set; }
        public Nullable<int> FormatID { get; set; }
        public Nullable<int> UnitsInStock { get; set; }
        public int AlbumStatusID { get; set; }
        public string AlbumImage { get; set; }
        public Nullable<int> Num { get; set; }
        public Nullable<int> Tracks { get; set; }

        //Genre Properties
        public int GenreID { get; set; }
        public string GenreName { get; set; }

        //Artist Properties
        public int ArtistID { get; set; }
        public string ArtistName { get; set; }

        //AlbumStatus Properties
        public string AlbumStatusName { get; set; }

        //Label Properties
        public string LabelName { get; set; }
        
        //Format Properties
        public string FormatType { get; set; }

        //AlbumArtist Properties
        public bool PrimaryArtist { get; set; }

        //Constructor
        public AlbumViewModel() { }


    }
}