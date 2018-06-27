using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models.Extended
{
    [MetadataType(typeof(CommentMetaData))]
    public partial class Comment
    {

    }

    public class CommentMetaData
    {
        public int Id { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Comment1 { get; set; }
        public Nullable<System.DateTime> DatePosted { get; set; }
    }
}