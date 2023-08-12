using BugTrackingBL;
using BugTrackingBL.Managers.Developers;
using BugTrackingBL.Managers.Manager;
using BugTrackingDAL;
using BugTrackingDAL.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NuGet.Protocol;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

#region Default Configuration
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region DB Configuration
var connectionString = builder.Configuration.GetConnectionString("Ticket_ConString");
builder.Services.AddDbContext<BugContext>(options =>
    options.UseSqlServer(connectionString));
#endregion

#region Identity Configuration
builder.Services.AddIdentity<GeneralUser, IdentityRole>( options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<BugContext>();
#endregion

#region Admin configuration
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IAdminManager, AdminManager>();
builder.Services.AddScoped<ILoginManager, LoginManager>();
#endregion

#region Cors Policy

builder.Services.AddCors(options =>
{
    options.AddPolicy("notify", p =>
    {
        p.AllowAnyMethod()
        .AllowAnyHeader()
        //for signalR
        .AllowCredentials()
        .SetIsOriginAllowed(x => true);
    });
});

#endregion

#region Manager Configuration
builder.Services.AddScoped<IManagerManager,ManagerManager>();
builder.Services.AddScoped<IManagerRepo,ManagerRepo >();
#endregion

#region Developer and Tester Configuration
builder.Services.AddScoped<IDeveloper, DeveloperRepo>();
builder.Services.AddScoped<IDeveloperManager, DeveloperManager>();
builder.Services.AddScoped<ITester, TesterRepo>();
builder.Services.AddScoped<ITesterManager, TesterManager>();
#endregion

#region Authentication Configuration

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "cool";
    options.DefaultChallengeScheme = "cool";
}).AddJwtBearer("cool", options =>
{
    var stringKey = builder.Configuration.GetValue<string>("secretkey") ?? string.Empty;
    var bytesKey = Encoding.ASCII.GetBytes(stringKey);
    var key = new SymmetricSecurityKey(bytesKey);
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

#endregion

#region Authorization Configuration

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Manager", policy => policy
        .RequireClaim(ClaimTypes.Role, "Manager")
        .RequireClaim(ClaimTypes.NameIdentifier));

    options.AddPolicy("Developer", policy => policy
        .RequireClaim(ClaimTypes.Role, "Developer")
        .RequireClaim(ClaimTypes.NameIdentifier));

    options.AddPolicy("Tester", policy => policy
        .RequireClaim(ClaimTypes.Role, "Tester")
        .RequireClaim(ClaimTypes.NameIdentifier));

    options.AddPolicy("Admin", policy => policy
        .RequireClaim(ClaimTypes.Role, "Admin")
        .RequireClaim(ClaimTypes.NameIdentifier));
});

#endregion

#region Swagger Configuration

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Define the security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Require authorization for all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();

app.UseCors("notify");

//app.UseAuthorization();

app.MapControllers();

app.Run();
