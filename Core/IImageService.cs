using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Core
{
    public interface IImageService
    {
        Task<UploadResult> UploadAsync(IFormFile image);
    }
}