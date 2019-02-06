using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photoschedule")]
    [ApiController]
    public class PhotoScheduleController : ControllerBase
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private Cloudinary _cloudinary;
        public PhotoScheduleController(IDatingRepository repo,
        IMapper mapper,
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account
            (
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhotoSchedule")]
        public async Task<IActionResult> GetPhotoSchedule(int id)
        {
            var photoFromRepo = await _repo.GetPhotoSchedule(id);
            var photoSchedule = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photoSchedule);
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUserSchedule(int userId,
        [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userFromRepo = await _repo.GetUser(userId);
            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").
                        Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photoSchedule = _mapper.Map<PhotoSchedule>(photoForCreationDto);

            if(!userFromRepo.PhotoSchedules.Any(u => u.IsMainCurrentSchedule))
                photoSchedule.IsMainCurrentSchedule = true;
            userFromRepo.PhotoSchedules.Add(photoSchedule);

            
            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photoSchedule);
                return CreatedAtRoute("GetPhotoSchedule", new { id = photoSchedule.Id},
                photoToReturn);
            }
            return BadRequest("Could not add the photo");
        }
    }
}


