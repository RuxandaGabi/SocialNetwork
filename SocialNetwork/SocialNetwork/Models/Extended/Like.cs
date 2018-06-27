using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Models.Extended
{
    [MetadataType(typeof(LikeMetaData))]
    public partial class Like
    {

    }


    public class LikeMetaData
    {
        public int Id { get; set; }
        public Nullable<int> PostId { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}