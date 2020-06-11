namespace SGDb.Server.Services.Users.Contracts
{
    // TODO [GM]: inherit from Implement IBaseService
    public interface IUsersService
    {
        string GenerateJwtToken(string username, string password);
    }
}