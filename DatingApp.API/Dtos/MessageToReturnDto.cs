using System;
using DatingApp.API.Models;

namespace DatingApp.API.Dtos
{
    public class MessageToReturnDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderKnownAs { get; set; }
        public int RecipientId { get; set; }
        public string RecipientKnownAs { get; set; }
        //   public Content Content {get;set;}
        public string ContentMessage { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string RecipientPhotoSchedUrl { get; set; }
        // appointment details
        public bool IsApproved { get; set; }
        public DateTime Date { get; set; }
        public string Room { get; set; }

        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}