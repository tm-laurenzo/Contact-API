using DTO;
using System.Threading.Tasks;

namespace Core
{
    public interface IAuthentication
    {
        Task<UserResponseDTO> Login(UserRequestDTO userRequest);
        Task<UserResponseDTO> Register(RegistrationRequest registrationRequest);
    }
}