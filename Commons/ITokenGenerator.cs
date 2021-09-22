using Models;
using System.Threading.Tasks;

namespace Commons
{
    public interface ITokenGenerator
    {
        Task<string> GenerateTokenAsync(User user);
    }
}