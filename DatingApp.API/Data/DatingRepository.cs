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
              var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // returns list of users
            var users = _context.Users.Include(p => p.Photos).
            Include(z => z.PhotoSchedules).
            OrderByDescending(u => u.LastActive).
            AsQueryable();
            users = users.Where(u => u.Id != userParams.UserId); // to not view 
            users = users.Where(u => u.Type == userParams.Type);
            users = users.Where( u => u.Department == userParams.Department);
           // users = users.Where(u => u.Department == userParams.Department);


        //section 15 lecture 152






            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge -1);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);
                users = users.
                Where(u => u.DateofBirth >= minDob && u.DateofBirth <= maxDob);
            }
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch(userParams.OrderBy)
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
           public async Task<IEnumerable<User>> GetUsersByDept()
        {
            // returns list of users
            var users = await _context.Users.Where(p => p.Department == "CCIS?").Include(p => p.Photos).Include(p => p.PhotoSchedules).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
            // > 0 because returns boolean
            // if = 0, nothing saves back to database, so return false and nothing do to database.
        }

      
    }
}