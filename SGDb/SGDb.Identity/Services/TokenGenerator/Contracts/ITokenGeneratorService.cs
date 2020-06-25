using System.Threading.Tasks;

namespace SGDb.Identity.Services.TokenGenerator.Contracts
{
    public interface ITokenGeneratorService
    {
        Task<string> GenerateToken(string email, string password);
    }
}