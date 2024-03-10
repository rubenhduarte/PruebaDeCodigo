using Test.Shared.Entities.DTO;

namespace Test.Client.Interfaces
{
    public interface IAuthenticationService
    {
        Task<RegisterResponse> RegisterUser(UserRegisterRequest userAuthentication);
        Task<AuthenticationResponse> Login(UserAuthenticationRequest userAuthentication);
        Task Logout();
    }
}
