using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
           return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
              var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            // returns list of users
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }
           public async Task<IEnumerable<User>> GetUsersByDept()
        {
            // returns list of users
            var users = await _context.Users.Where(p => p.Department == "CCIS?").Include(p => p.Photos).ToListAsync();
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