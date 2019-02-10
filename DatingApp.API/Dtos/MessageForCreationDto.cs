using System;
using DatingApp.API.Models;

namespace DatingApp.API.Dtos
{
    public class MessageForCreationDto
    {
        public int SenderId {get;set;}
        public int RecipientId {get;set;}
        public bool IsApproved {get;set;}
        public string ContentMessage { get; set; }
        public DateTime MessageSent {get;set;}
        public DateTime Date { get; set; }
        public string Room { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }
        
    }
}