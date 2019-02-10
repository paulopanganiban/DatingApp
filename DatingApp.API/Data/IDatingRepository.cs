using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Collections.Generic;
using DatingApp.API.Helpers;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        // save changes to database
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<PhotoSchedule> GetPhotoSchedule(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
        Task<PhotoSchedule> GetMainPhotoForUserSchedule (int userId);
        Task<Like> GetLike(int userId, int recipientId);
        // Task<Message> GetMessage(int id);
        // Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        // // start here
        // Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}