using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    { // Section 8 Lecture 72
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {

            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await _context.Likes.
            FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }
        public async Task<PhotoSchedule> GetMainPhotoForUserSchedule(int userId)
        {
            return await _context.PhotoSchedules.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMainSched);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }
        public async Task<PhotoSchedule> GetPhotoSchedule(int id)
        {
            var photoSchedule = await _context.PhotoSchedules.FirstOrDefaultAsync(p => p.Id == id);
            return photoSchedule;
        }

        public async Task<User> GetUser(int id)
        {
            // photoschedule being retrieved
            var user = await _context.Users.Include(p => p.Photos).Include(p => p.PhotoSchedules).
            FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // returns list of users
            var users = _context.Users.Include(z => z.Photos).
            Include(z => z.PhotoSchedules).
            OrderByDescending(u => u.LastActive).
            AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId); // to not view 
            users = users.Where(u => u.Type == userParams.Type);
            users = users.Where(u => u.Department == userParams.Department);
            // users = users.Where(u => u.Department == userParams.Department);
            //section 15 lecture 152
            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
        return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _context.Users.
            Include(x => x.Liker).
            Include(x => x.Likee).
            FirstOrDefaultAsync(u => u.Id == id);
            if (likers)
            {
                return user.Liker.Where(u => u.LikeeId == id).
                Select(i => i.LikerId);
            }
            else
            {
                return user.Likee.Where(u => u.LikerId == id).
                Select(i => i.LikeeId);
            }
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
            // > 0 because returns boolean
            // if = 0, nothing saves back to database, so return false and nothing do to database.
        }
    //     public async Task<Message> GetMessage(int id)
    //     {
    //         return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
    //     }

    //     public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
    //     {
    //         var messages = _context.Messages
    //         .Include(u => u.Sender).ThenInclude(p => p.Photos)
    //         .Include(u => u.Recipient).ThenInclude(p => p.Photos)
    //         .AsQueryable();

    //         switch (messageParams.MessageContainer)
    //         {
    //             case "Inbox":
    //             messages = messages.Where(u => u.RecipientId == messageParams.UserId);
    //             break;
    //             case "Outbox":
    //             messages = messages.Where(u => u.SenderId == messageParams.UserId);
    //             break;
    //             default:
    //             messages = messages.
    //             Where(u => u.RecipientId == messageParams.UserId && u.IsRead == false);
    //             break;
    //         }
    //         messages = messages.OrderByDescending(d => d.MessageSent);
    //         return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    //     }

    //     public Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
    //     {
    //         throw new NotImplementedException();
    //     }
     }
}