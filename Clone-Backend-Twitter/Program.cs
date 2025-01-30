using System.Text;
using Clone_Backend_Twitter.Data;
using Clone_Backend_Twitter.Services.Auth;
using Clone_Backend_Twitter.Services.Feed;
using Clone_Backend_Twitter.Services.Search;
using Clone_Backend_Twitter.Services.Trend;
using Clone_Backend_Twitter.Services.Tweet;
using Clone_Backend_Twitter.Services.User;
using Clone_Backend_Twitter.Utils;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblies([typeof(Program).Assembly]);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = 401;
            return context.Response.WriteAsync("Acesso negado");
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            return context.Response.WriteAsync("Acesso negado");
        }
    };
});

builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<ITweetInterface, TweetService>();
builder.Services.AddScoped<ITrendInterface, TrendService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddScoped<IFeedInterface, FeedService>();
builder.Services.AddScoped<ISearchInterface, SearchService>();
builder.Services.AddScoped<Url>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();