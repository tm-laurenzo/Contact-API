using DTO;
using DTO.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class UserServices : IdentityUser, IUserServices
    {
        private readonly UserManager<User> _userManager;
        /// <summary>
        /// A Constructor for creating a new UserService objec
        /// </summary>
        /// <param name="userManager"></param>
        public UserServices(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        #region 
        /// <summary>
        /// A method to update a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUser"></param>
        /// <returns>Task<bool></returns>
        public async Task<bool> UpdateUser(string userId, UpdateUserRequest updateUser)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.FirstName = string.IsNullOrWhiteSpace(updateUser.FirstName) ? user.FirstName : updateUser.FirstName;
                user.LastName = string.IsNullOrWhiteSpace(updateUser.LastName) ? user.LastName : updateUser.LastName;
                user.PhoneNumber = string.IsNullOrWhiteSpace(updateUser.PhoneNumber) ? user.PhoneNumber : updateUser.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }

                string errors = "";

                foreach (var error in result.Errors)
                {
                    errors += error;
                }
                throw new MissingMemberException(errors);
            }

            throw new ArgumentException("Resource Not Found");

        }
        #endregion
        /// <summary>
        /// A method to delete a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUser"></param>
        /// <returns>Task<bool></returns>
        public async Task<bool> DeleteUser(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                string errors = "";

                foreach (var error in result.Errors)
                {
                    errors += error;
                }
                throw new MissingMemberException(errors);
            }

            throw new ArgumentException("Resource Not Found");

        }
        /// <summary>
        /// Gets a user based on the Id parsed
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUser"></param>
        /// <returns> Task<UserResponseDTO></returns>
        public async Task<UserResponseDTO> GetUserById(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return UserMappings.GetUserResponse(user);
            }

            throw new ArgumentException("Resource Not Found");
        }
        /// <summary>
        /// Get Auser based on the email provided
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Task<UserResponseDTO></returns>
        public async Task<UserResponseDTO> GetUserByEmail(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return UserMappings.GetUserResponse(user);
            }

            throw new ArgumentException("Resource Not Found");
        }

        /// <summary>
        /// Crawls the database to get all users
        /// </summary>
        /// <returns>an IEnumerable of UserResposDTO</returns>
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers(PaginationParams @params)
        {
            var users = await _userManager.Users.Skip((@params.Page - 1) * @params.ItemsPerPage)
                                                               .Take(@params.ItemsPerPage).ToListAsync();

            List<UserResponseDTO> allUsers = new List<UserResponseDTO>();
            if (users != null)
            {
                foreach (var user in users)
                {

                    var result = new UserResponseDTO
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Id = user.Id

                    };
                    allUsers.Add(result);

                }
                return allUsers;
            }
            throw new ArgumentException("Resource Not Found");
        }
        /// <summary>
        /// Crawls the database gets the users that match a given search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>Task<IEnumerable<UserResponseDTO>></returns>
        public async Task<IEnumerable<UserResponseDTO>> Search(string searchTerm)
        {

            var users = await _userManager.Users.ToListAsync();
            var searchTermMatch = users.Where(user => user.FirstName.Contains(searchTerm)
                                                                            || user.LastName.Contains(searchTerm)
                                                                            || user.Email.Contains(searchTerm)
                                                                            || user.PhoneNumber.Contains(searchTerm)
                                                                            || user.UserName.Contains(searchTerm));



            List<UserResponseDTO> matchUsers = new List<UserResponseDTO>();
            if (users != null)
            {
                foreach (var user in searchTermMatch)
                {

                    var result = new UserResponseDTO
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Id = user.Id
                    };
                    matchUsers.Add(result);

                }
                return matchUsers;
            }
            throw new ArgumentException("Resource Not Found");
        }

    }
}
 