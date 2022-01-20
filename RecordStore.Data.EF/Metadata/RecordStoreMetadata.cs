using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //Added for access to DataAnnotations

namespace RecordStore.Data.EF//.Metadata
{
    #region Album Metadata
    public class AlbumMetadata
    {
        //public int AlbumID { get; set; }

        [Required(ErrorMessage = "* Album Name is required *")]
        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters *")]
        [Display(Name = "Album")]
        public string AlbumName { get; set; }

        [UIHint("MultilineText")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string Description { get; set; }

        [Display(Name = "Compilation Album")]
        public bool CompilationAlbum { get; set; }

        [Display(Name = "Catalog No.")] //Cat# often have a mixture a letters and numbers
        [StringLength(25, ErrorMessage = "* Cannot exceed 25 characters *")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string CatalogNum { get; set; }

        [Required(ErrorMessage = "* Price is required *")]
        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger *")]
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "[-N/A-]")]
        public Nullable<decimal> Price { get; set; }

        [Display(Name = "In Print")]
        public bool IsInPrint { get; set; }

        [Display(Name = "Units In Stock")]
        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger *")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public Nullable<int> UnitsInStock { get; set; }

        [StringLength(100, ErrorMessage = "* Cannot exceed 25 characters")]
        [Display(Name = "Cover")]
        public string AlbumImage { get; set; }

        [Display(Name = "Number")]
        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger *")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public Nullable<int> Num { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger *")]
        public Nullable<int> Tracks { get; set; }

        [Required(ErrorMessage = "* Label ID is required *")]
        [Display(Name = "Label")]
        public int LabelID { get; set; }

        [Required(ErrorMessage = "* Format is required *")]
        [Display(Name = "Format")]
        public int FormatID { get; set; }

        [Required(ErrorMessage = "* Status is required *")]
        [Display(Name = "Status")]
        public int AlbumStatusID { get; set; }

        [Range(1900, 9999, ErrorMessage = "* Value must be a valid year *")]
        public Nullable<int> Year { get; set; }

    }

    [MetadataType(typeof(AlbumMetadata))]
    public partial class Album
    {
        public static object OrderBy(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Album Artist Metadata
    public class AlbumArtistMetadata
    {
        //public int AlbumArtistID { get; set; }

        [Required(ErrorMessage = "* Album ID is required *")]
        public int AlbumID { get; set; }

        [Required(ErrorMessage = "* Artist ID is required *")]
        public int ArtistID { get; set; }
    }

    [MetadataType(typeof(AlbumArtistMetadata))]
    public partial class AlbumArtist
    {

    }
    #endregion

    #region Artist Metadata
    public class ArtistMetadata
    {
        //public int ArtistID { get; set; }

        [Required(ErrorMessage = "* Artist Name is required *")]
        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters *")]
        [Display(Name = "Artist Name")]
        public string ArtistName { get; set; }

        [Display(Name = "Is Artist Still Active?")]
        public bool IsStillActive { get; set; }
    }

    [MetadataType(typeof(ArtistMetadata))]
    public partial class Artist
    {

    }
    #endregion

    #region Genre Metadata
    public class GenreMetadata
    {
        //public int GenreID { get; set; }
        [Required(ErrorMessage = "* Genre is required *")]
        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters *")]
        [Display(Name = "Genre")]
        public string GenreName { get; set; }
    }

    [MetadataType(typeof(GenreMetadata))]
    public partial class Genre
    {

    }
    #endregion

    #region Label Metadata
    public class LabelMetadata
    {
        //public int LabelID { get; set; }

        [Required(ErrorMessage = "* Label Name is required *")]
        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters *")]
        [Display(Name = "Label")]
        public string LabelName { get; set; }

        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters *")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string Country { get; set; }

        [Display(Name = "Still In Distribution")]
        public bool IsStillDistributing { get; set; }
    }

    [MetadataType(typeof(LabelMetadata))]
    public partial class Label
    {

    }
    #endregion


    #region Format Metadata
    public class FormatMetadata
    {
        //public int FormatID { get; set; }

        [Required(ErrorMessage = "* Format Type is required *")]
        [StringLength(25, ErrorMessage = "* Cannot exceed 25 characters *")]
        [Display(Name = "Format")]
        public string FormatType { get; set; }

        [Display(Name = "Multi-Format Availible")]
        public bool MultiFormat { get; set; }
    }

    [MetadataType(typeof(FormatMetadata))]
    public partial class Format
    {

    }
    #endregion


    #region Recording Metadata
    public class RecordingMetadata
    {
        //public int RecordingID { get; set; }

        [Required(ErrorMessage = "* Recording Type is required *")]
        [StringLength(25, ErrorMessage = "* Cannot exceed 25 characters *")]
        [Display(Name = "Recording Type")]
        public string RecordingType{ get; set; }
    }

    [MetadataType(typeof(RecordingMetadata))]
    public partial class Recording
    {

    }
    #endregion


    #region Album Status Metadata
    public class AlbumStatusMetadata
    {
        //public int AlbumStatusID { get; set; }

        [Required(ErrorMessage = "* Status is required *")]
        [Display(Name = "Status")]
        public int AlbumStatusName { get; set; }

        [Display(Name = "Status Notes")]
        public int StatusNotes { get; set; }
    }

    [MetadataType(typeof(AlbumStatusMetadata))]
    public partial class AlbumStatus
    {

    }
    #endregion


    #region Employee Metadata
    public class EmployeeMetadata
    {
        //public int EmployeeID { get; set; }

        [Required(ErrorMessage = "* First Name is required *")]
        [StringLength(20, ErrorMessage = "* Cannot exceed 20 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* Lastst Name is required *")]
        [StringLength(25, ErrorMessage = "* Cannot exceed 25 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(25, ErrorMessage = "* Cannot exceed 25 characters")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string Position { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger")]
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "[-N/A-]")]
        public Nullable<decimal> PayRate { get; set; }

        [Display(Name = "Full Time Employee")]
        public bool IsFullTime { get; set; }

        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "* Cannot exceed 50 characters")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string City { get; set; }

        [StringLength(2, ErrorMessage = "* Value must be 2 characters")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string State { get; set; }

        [StringLength(10, ErrorMessage = "* Cannot exceed 10 characters")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", NullDisplayText = "[-N/A-]")]
        public string Phone { get; set; }

        [DisplayFormat(DataFormatString = "{0:M/d/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Birthday { get; set; }

        [DisplayFormat(DataFormatString = "{0:M/d/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> HireDate { get; set; }

        //public int DirectReportID { get; set; }
    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {

        public string NamePlusPosition
        {
            get { return Position + ": " + FirstName + " " + LastName; }
        }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public string FullAddress
        {
            get { return Address + ", " + City + ", " + State + ", " + ZipCode; }
        }
    }
    #endregion



    #region Department Metadata
    public class DepartmentMetadata
    {
        //public int DepartmentID { get; set; }

        [Required(ErrorMessage = "* Dept. Name is required *")]
        [StringLength(15, ErrorMessage = "* Cannot exceed 15 characters *")]
        [Display(Name = "Dept. Name")]
        public string DepartmentName { get; set; }

        [StringLength(100, ErrorMessage = "* Cannot exceed 100 characters *")]
        [Display(Name = "Dept. Description")]
        [DisplayFormat(NullDisplayText = "[-N/A-]")]
        public string DeptDescription { get; set; }
    }

    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {

    }
    #endregion


}

