using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure;
using Azure.Core;
using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Models.Dto;
using Clone_Backend_Twitter.Models.Entity;
using Clone_Backend_Twitter.Models.Response;
using Clone_Backend_Twitter.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Slugify;

namespace Clone_Backend_Twitter.Services.Auth;

public class AuthService : IAuthInterface
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly Url _url;

    public AuthService(AppDbContext context, Url url, IConfiguration configuration)
    {
        _context = context;
        _url = url;
        _configuration = configuration;
    }

    public async Task<ResponseModel<AuthResponse>> Signup(SignupDto signupDto)
    {
        ResponseModel<AuthResponse> response = new ResponseModel<AuthResponse>();
        try
        {
            var email = await _context.Users.FirstOrDefaultAsync( u => u.Email == signupDto.Email);
            if(email != null)
            {
                response.Message = "Email já existe!";
                response.Status = false;
                return response;
            }
            
            Boolean genSlug = true;
            
            var slugHelper = new SlugHelper();
            string userSlug = slugHelper.GenerateSlug(signupDto.Name);

            while (genSlug)
            {
                var Slug = _context.Users.FirstOrDefault(u => u.Slug == userSlug);
                if (Slug != null)
                {
                    Random random = new Random();
                    string slugSufix =  random.Next(0, 999999).ToString("D6");
                    userSlug = slugHelper.GenerateSlug(signupDto.Name + slugSufix);
                }
                else
                {
                    genSlug = false;
                }
            }
            
            var HashPassword = BCrypt.Net.BCrypt.HashPassword(signupDto.Password);

            var newUser = new UserModel()
            {
                Slug = userSlug,
                Email = signupDto.Email,
                PasswordHash = HashPassword,
                Name = signupDto.Name
            };
            
            _context.Add(newUser);
            await _context.SaveChangesAsync();
            
            var avatarUrl = _url.GetPublicUrl(newUser.Avatar);
            var token = await GetAccessToken(newUser.Name, newUser.Slug);
            
            var user = new AuthResponse()
            {
                Name = newUser.Name,
                Slug = userSlug,
                Avatar = avatarUrl,
                Token = token
            };
    
            response.Message = "Usuário cadastrado com sucesso!";
            response.Data = user;
            return response;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Status = false;
            return response;
        }
    }

    public async Task<ResponseModel<AuthResponse>> Signin(SigninDto signinDto)
    {
        ResponseModel<AuthResponse> response = new ResponseModel<AuthResponse>();
        try
        {
            var email = await _context.Users.FirstOrDefaultAsync( u => u.Email == signinDto.Email);
            if(email == null)
            {
                response.Message = "Acesso negado!";
                response.Status = false;
                return response;
            }
            
            if (!BCrypt.Net.BCrypt.Verify(signinDto.Password, email.PasswordHash))
            {
                response.Message = "Acesso negado!";
                response.Status = false;
                return response;
            }
            
            var token = await GetAccessToken(email.Name, email.Slug);
            var avatarUrl = _url.GetPublicUrl(email.Avatar);
            var user = new AuthResponse()
            {
                Name = email.Name,
                Slug = email.Slug,
                Avatar = avatarUrl,
                Token = token
            };
            
            response.Message = "Login realizado com sucesso!";
            response.Data = user;
            return response;

        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
            response.Status = false;
            return response;
        }
    }

    public async Task<string> GetAccessToken(string name, string slug)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? String.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(1);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.NameIdentifier, slug),
        };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<UserModel> VerifyJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();
    
        if (!handler.CanReadToken(token))
        {
            return null;
        }
            
        var jwtToken = handler.ReadJwtToken(token);
            
        var name = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var slug = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Slug == slug);
            
        return user;
    }
}















