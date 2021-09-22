using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core
{
    public interface IUserServices
    {
        Task<bool> DeleteUser(string userId);
        Task<IEnumerable<UserResponseDTO>> GetAllUsers(PaginationParams @params);
        Task<UserResponseDTO> GetUserByEmail(string email);
        Task<UserResponseDTO> GetUserById(string userId);
        Task<IEnumerable<UserResponseDTO>> Search(string searchTerm);
        Task<bool> UpdateUser(string userId, UpdateUserRequest updateUser);
    }
}