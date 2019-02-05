using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Department { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt {get;set;}

        public string Gender { get; set; }  

        public DateTime DateofBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public string Introduction { get; set; }

        public string LookingFor { get; set; }

        public string Interests { get; set; }

        public string City { get; set; }
        // city is faculty ok?

        public string Country { get; set; }
        public string Type {get;set;}

        public ICollection<Photo> Photos { get; set; }

        
    }
}