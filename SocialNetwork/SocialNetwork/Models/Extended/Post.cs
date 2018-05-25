using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models
{
    [MetadataType(typeof(PostMetadata))]
    public partial class Post
    {
        public HttpPostedFileBase ImageFile { get; set; }
    }

    public class PostMetadata
    {
        public int PostID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> Comments { get; set; }

        [Display(Name ="Write something!")]
        public string Description { get; set; }

        [Display(Name = "Upload image")]
        public string ImagePath { get; set; }

        public Nullable<System.DateTime> DatePosted { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }


    }
}