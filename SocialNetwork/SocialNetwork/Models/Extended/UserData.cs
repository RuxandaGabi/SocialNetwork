using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialNetwork.Models.Extended
{
    [MetadataType(typeof(UserDataMetadata))]
    public partial class UserData
    {

    }

    public class UserDataMetadata
    {
        public int Id { get; set; }
        public string ProfileImage { get; set; }
        public string Hobbies { get; set; }
        public string Studies { get; set; }
        public string Job { get; set; }
        public string Description { get; set; }
    }
}