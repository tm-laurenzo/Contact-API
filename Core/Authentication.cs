using DTO;
using Commons;
using DTO.Mappings;
using Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Authentication : IAuthentication
    {
        private UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="tokenGenerator"></param>
        public Authentication(UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> Login(UserRequestDTO userRequest)
        {
            User user = await _userManager.FindByEmailAsync(userRequest.Email);
            
            if (user != null)
            {
                CurrentUser.Id = user.Id;


                if (await _userManager.CheckPasswordAsync(user, userRequest.Password))
                {
                    var response = UserMappings.GetUserResponse(user);
                    response.Token = await _tokenGenerator.GenerateTokenAsync(user);

                    return response;
                }
                throw new AccessViolationException("Invalid credentials");

            }

            throw new AccessViolationException("invalid credentials");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationRequest"></param>
        /// <returns></returns>
        public async Task<UserResponseDTO> Register(RegistrationRequest registrationRequest)
        {
            User user = UserMappings.GetUser(registrationRequest);
            IdentityResult result = await _userManager.CreateAsync(user, registrationRequest.Password);
            await _userManager.AddToRoleAsync(user, "Regular");

            if (result.Succeeded)
            {
                return UserMappings.GetUserResponse(user);
            }

            string errors = "";

            foreach (var error in result.Errors)
            {
                errors += error.Description + Environment.NewLine;
            }

            throw new MissingFieldException(errors);
        }

    }

}
