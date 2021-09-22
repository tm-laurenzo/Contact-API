using Commons;
using Core;
using DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;

namespace ContactAPI3._1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserServices userService, IImageService imageService, UserManager<User> userManager)
        {
            _userService = userService;
            _imageService = imageService;
            _userManager = userManager;
        }   
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetUserById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.GetUserAsync(User);
            try
            {
                return Ok(await _userService.GetUserById(userId));

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetUserByEmail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                return Ok(await _userService.GetUserByEmail(email));

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateUserRequest"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("Update")]
        [Authorize(Roles = "Regular")]
        public async Task<IActionResult> Update(UpdateUserRequest updateUserRequest)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userService.UpdateUser(userId, updateUserRequest);
                return NoContent();

            }
            catch (MissingMemberException mmex)
            {

                return BadRequest(mmex.Message);
            }
            catch (ArgumentException argex)
            {

                return BadRequest(argex.Message);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        [HttpDelete]
        [Route("Delete")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                await _userService.DeleteUser(userId);
                return Ok();

            }
            catch (MissingMemberException mmex)
            {

                return BadRequest(mmex.Message);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin")] 

        public IActionResult GetAllUsers([FromQuery] PaginationParams @params)
        {
            try
            {
                return Ok( _userService.GetAllUsers(@params));
                
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("Search")]
        [Authorize(Roles = "Admin")]
        public  IActionResult Search(string searchTerm)
        {
            try
            {
                return Ok( _userService.Search(searchTerm));

            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
         }


        [HttpPost]
        [Route("UploadImage")]
        [Authorize(Roles = "Regular")]
        public async Task<IActionResult> UploadImage([FromForm] AddImageDTO imageDTO)
        {
            try
            {
                var upload = await _imageService.UploadAsync(imageDTO.Image);
                var result = new ImageAddedDTO()
                
                {
                    PublicId = upload.PublicId,
                    Url = upload.Url.ToString()
                };
                var userId = CurrentUser.Id;
                User user = await _userManager.FindByIdAsync(userId);
                user.ImageUrl = result.Url.ToString();
                await _userManager.UpdateAsync(user);
                
                return Ok(result);

                //result.Uri.ToString();
                //var user.Image = result.Uri.ToString();
            }
            catch (ArgumentException e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}
