using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace Clone_Backend_Twitter.Services.Auth;

public class AuthService : IAuthInterface
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseModel<AuthResponse>> Signup(SignupDto signupDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<AuthResponse>> Signin(SigninDto signinDto)
    {
        throw new NotImplementedException();
    }
}