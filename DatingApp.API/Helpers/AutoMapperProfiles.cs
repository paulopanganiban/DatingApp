using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>() 
            //gets the values from user database <User,
            //pass it to UserForListDto to transform the data to return to client.
            .ForMember(dest => dest.PhotoUrl, opt =>
            {
                //dest.PhotoUrl is in UserForListDto
                opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                //PINASA YUNG IS MAIN SA DEST.PHOTOURL FROM USELIST DTO

                //src.src is from User.cs public ICollection<Photo> Photos { get; set; }
                // the PhotoUrl both from User and UserForListDto
                //finds from Photos column and get IsMainUrl
            })
            .ForMember(dest => dest.Age, opt =>
            {
                opt.ResolveUsing(d => d.DateofBirth.CalculateAge()); //transform the data source 
                //and CalculateAge returns the value, and pass it to dest.Age
            })
            .ForMember(dest => dest.PhotoUrlSched, opt =>
            {
                opt.MapFrom(src => src.PhotoSchedules.FirstOrDefault(p => p.IsMainSched).Url);
            });


            CreateMap<User, UserForDetailedDto>()
              .ForMember(dest => dest.PhotoUrl, opt =>
              {
                  opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
              })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateofBirth.CalculateAge());
                })// added this 14:12
                .ForMember(dest => dest.PhotoUrlSched, opt =>
                {
                    opt.MapFrom(src => src.PhotoSchedules.FirstOrDefault(p => p.IsMainSched).Url);
                });
                      // CreateMap<Go from, Go to>
                    CreateMap<Photo, PhotosForDetailedDto>(); // return photosfordetail
                    CreateMap<UserForUpdateDto, User>();
                    CreateMap<Photo, PhotoForReturnDto>();
                    CreateMap<PhotoForCreationDto, Photo>();
                    CreateMap<UserForRegisterDto, User>();
                    // schedulke
                    
                    CreateMap<PhotoSchedule, PhotoForReturnDto>();
                    CreateMap<PhotoSchedule, PhotosForDetailedDto>();
                    CreateMap<PhotoForCreationDto, PhotoSchedule>();
                    CreateMap<MessageForCreationDto, Message>().ReverseMap();
                    CreateMap<Message, MessageToReturnDto>()
                    .ForMember(m => m.SenderPhotoUrl, opt => opt.MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                    .ForMember(m => m.RecipientPhotoUrl, opt => opt.MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}