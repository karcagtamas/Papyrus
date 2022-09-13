using System.Text;
using AutoMapper;
using KarcagS.Common.Middlewares;
using KarcagS.Common.Tools;
using KarcagS.Common.Tools.Authentication.JWT;
using KarcagS.Common.Tools.HttpInterceptor;
using KarcagS.Common.Tools.HttpInterceptor.Converters;
using KarcagS.Common.Tools.Mongo;
using KarcagS.Common.Tools.Services;
using KarcagS.Common.Tools.Table;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Papyrus.DataAccess;
using Papyrus.DataAccess.Entities;
using Papyrus.Logic.Authorization;
using Papyrus.Logic.Hubs;
using Papyrus.Logic.Mappers;
using Papyrus.Logic.Services;
using Papyrus.Logic.Services.Auth;
using Papyrus.Logic.Services.Auth.Interfaces;
using Papyrus.Logic.Services.Common;
using Papyrus.Logic.Services.Common.Interfaces;
using Papyrus.Logic.Services.Editor;
using Papyrus.Logic.Services.Editor.Interfaces;
using Papyrus.Logic.Services.Groups;
using Papyrus.Logic.Services.Groups.Interfaces;
using Papyrus.Logic.Services.Interfaces;
using Papyrus.Logic.Services.Notes;
using Papyrus.Logic.Services.Notes.Interfaces;
using Papyrus.Middlewares;
using Papyrus.Mongo.DataAccess.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddHttpLogging(opt => { });

// Config
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<UtilsSettings>(builder.Configuration.GetSection("Utils"));

// Add mongo
builder.AddMongo<CollectionConfiguration>((conf) => conf.GetSection("Mongo"));

// Add services to the container.
builder.Services.AddSingleton<IMongoService<CollectionConfiguration>, MongoService<CollectionConfiguration>>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUtilsService<string>, UtilsService<PapyrusContext, string>>();
builder.Services.AddScoped<ILoggerService, LoggerService<string>>();
builder.Services.AddScoped<IJWTAuthService, JWTAuthService>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserTableService, UserTableService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<IActionLogService, ActionLogService>();

builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupRoleService, GroupRoleService>();
builder.Services.AddScoped<IGroupRoleTableService, GroupRoleTableService>();
builder.Services.AddScoped<IGroupMemberService, GroupMemberService>();
builder.Services.AddScoped<IGroupMemberTableService, GroupMemberTableService>();
builder.Services.AddScoped<IGroupActionLogService, GroupActionLogService>();
builder.Services.AddScoped<IGroupActionLogTableService, GroupActionLogTableService>();

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<INoteActionLogService, NoteActionLogService>();
builder.Services.AddScoped<INoteActionLogTableService, NoteActionLogTableService>();
builder.Services.AddScoped<INoteContentService, NoteContentService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<IEditorService, EditorService>();

builder.Services.AddScoped<IFileService, FileService>();

// Mandatory for HTTP Interceptor
builder.Services.AddErrorConverter((conf) =>
{
    conf.AddAgent(new TableErrorConverterAgent());
});

// Add AutoMapper
var mapperConfig = new MapperConfiguration(conf =>
{
    conf.AddProfile<UserMapper>();
    conf.AddProfile<GroupMapper>();
    conf.AddProfile<TagMapper>();
    conf.AddProfile<NoteMapper>();
    conf.AddProfile<LanguageMapper>();
    conf.AddProfile<RoleMapper>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// CORS
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
builder.Services.AddDbContextPool<PapyrusContext>(opt =>
    opt.UseLazyLoadingProxies()
        .UseMySql(connString, ServerVersion.AutoDetect(connString), b => b.MigrationsAssembly("Papyrus")));

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
    .AddEntityFrameworkStores<PapyrusContext>()
    .AddDefaultTokenProviders();

builder.Services.AddModelValidatedControllers();

// Auth Handlers
builder.Services.AddTransient<IAuthorizationHandler, GroupHandler>();
builder.Services.AddTransient<IAuthorizationHandler, NoteHandler>();
builder.Services.AddTransient<IAuthorizationHandler, TagHandler>();

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
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/editor"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(conf =>
{
    conf.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT containing user id claim",
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

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpInterceptor(opt =>
{
    opt.OnlyApi = true;
    opt.IgnoredPaths = new List<string> { @"/api/editor/[a-zA-Z0-9/]*", @"\/api\/File\/[a-z0-9A-Z]*" };
});

if (bool.TrueString.Equals(builder.Configuration["HttpsRedirect"]))
{
    app.UseHttpsRedirection();
}

app.MapHub<EditorHub>("/editor");

app.UseCors("Policy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment() || bool.TrueString.Equals(builder.Configuration["Migration"]))
{
    app.Migrate<PapyrusContext>();
}

app.SeedRoles();

var lf = app.Services.GetRequiredService<ILoggerFactory>();
lf.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\log.txt");

app.UseHttpLogging();

app.Run();