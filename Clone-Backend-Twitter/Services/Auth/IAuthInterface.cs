using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Response;

namespace Clone_Backend_Twitter.Services.Auth;

public interface IAuthInterface
{
    Task<ResponseModel<AuthResponse>> Signup(SignupDto signupDto);
    Task<ResponseModel<AuthResponse>> Signin(SigninDto signinDto);
    Task<string> GetAccessToken(string name, string slug);
}