using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Model
{
    public class SessionData
    {
        public ObjectId _id { get; set; }

        [Required]
        public string PublicName { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }
        public string ProfileImageName { get; set; }

        [Required]
        public string Category { get; set; }
        public VisibilitySettingModel Privacy { get; set; }
    }
}
