using System;

namespace DatingApp.API.Models
{
    public class PhotoSchedule
    {

        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMainSched { get; set; }
        public string PublicId { get; set; }
        public User User { get; set; }

        public int UserId { get; set; }
    }
}