using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models.Extended
{
    [MetadataType(typeof(FriendDataMetadata))]
    public partial class Friend
    {

    }

    public class FriendDataMetadata
    {
        public int Id { get; set; }
        public Nullable<int> UserId1 { get; set; }
        public Nullable<int> UserId2 { get; set; }
        public Nullable<int> Status { get; set; }
        public string User1Name { get; set; }
        public string User2Name { get; set; }
    }
}