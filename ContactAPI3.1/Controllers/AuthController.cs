using Core;
using Commons;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ContactAPI3._1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authentication;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authentication"></param>
        public AuthController(IAuthentication authentication)
        {
            this._authentication = authentication;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] UserRequestDTO userRequest)
        {
            try  
            {
                return Ok(await _authentication.Login(userRequest));
            }
            catch (AccessViolationException)
            {

                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            try
            {
                var result = await _authentication.Register(registrationRequest);
                return Created("", result);
            }
            catch (MissingFieldException msex)
            {

                return BadRequest(msex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }

}
