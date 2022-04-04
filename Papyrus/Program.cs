using System.Text;
using AutoMapper;
using Karcags.Common.Middlewares;
using Karcags.Common.Tools.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Config
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("Jwt"));

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUtilsService, UtilsService<NoteWebContext>>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
//builder.Services.AddScoped<ITokenService, TokenService>();
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<IUserService, UserService>();

// Add AutoMapper
var mapperConfig = new MapperConfiguration(conf =>
{
    //conf.AddProfile<UserMapper>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// Cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Policy", cb =>
    {
        cb.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(builder.Configuration["Client:Secure"], builder.Configuration["Client:Basic"]);
    });
});

// Add EF Core
var connString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContextPool<NoteWebContext>(opt => opt.UseLazyLoadingProxies().UseMySql(connString, ServerVersion.AutoDetect(connString), builder => builder.MigrationsAssembly("Papyrus")));

// Add Identity
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1d);
    options.Lockout.MaxFailedAccessAttempts = 5;
})
.AddRoles<Role>()
.AddEntityFrameworkStores<NoteWebContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();

// Auth
builder.Services
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(conf =>
{
    conf.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT containing userid claim",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    conf.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                UnresolvedReference = true
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseCors("Policy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Migrate<NoteWebContext>();
}

app.Run();
