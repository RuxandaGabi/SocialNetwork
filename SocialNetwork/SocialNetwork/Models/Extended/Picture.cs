using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
//using System.Web;

namespace SocialNetwork.Models
{
    [MetadataType(typeof(PictureMetadata))]
    public partial class Picture
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }
    public class PictureMetadata
    {
        public int PictureID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> Comments { get; set; }
        public string Description { get; set; }

        [Display(Name = "Upload image")]
        public string ImagePath { get; set; }

        public Nullable<System.DateTime> DatePosted { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }


    }
}