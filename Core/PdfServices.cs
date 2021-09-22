using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DTO;
using Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core
{
    public class PdfServices
    {
        private readonly IConfiguration _configuration;
        private readonly Cloudinary cloudinary;
        private readonly ImageUploadSettings _accountSettings;
        private readonly UserManager<User> _userManager;

        public PdfServices(IConfiguration configuration, IOptions<ImageUploadSettings> accountSettings, UserManager<User> userManager)
        {
            _configuration = configuration;
            _accountSettings = accountSettings.Value;
            cloudinary = new Cloudinary(new Account(_accountSettings.CloudName, _accountSettings.ApiKey, _accountSettings.ApiSecret));
            _userManager = userManager;

        }

        public async Task<UploadResult> UploadAsync(IFormFile book)
        {

            var pictureFormat = false;
            var listOfImageExtensions = _configuration.GetSection("PhotoSettings:Formats").Get<List<string>>();

            foreach (var item in listOfImageExtensions)
            {
                if (book.FileName.EndsWith(item))
                {
                    pictureFormat = true;
                    break;
                }
            }

            if (pictureFormat == false)
            {
                throw new ArgumentException("File format not supported");
            }

            var uploadResult = new ImageUploadResult();


            using (var imageStream = book.OpenReadStream())
            {
                string filename = Guid.NewGuid().ToString() + book.FileName;
                uploadResult = await cloudinary.UploadAsync(new ImageUploadParams()
                {
                    File = new FileDescription(filename, imageStream)
                    //Transformation = new Transformation().Crop("thumb").Gravity("face").Width(150).Height(150)
                });
            }



            return uploadResult;
        }
    }
}
