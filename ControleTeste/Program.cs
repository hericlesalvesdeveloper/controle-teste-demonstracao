using ControleTeste.Context;
using ControleTeste.Middleware;
using ControleTeste.Repositories;
using ControleTeste.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var defaultUrls = builder.Configuration["Application:Urls"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://localhost:7069";
builder.WebHost.UseUrls(defaultUrls);

builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ControleTesteContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<IAlteracaoRepository, AlteracaoRepository>();

builder.Services.AddScoped<IAlteracaoService,AlteracaoService>();

builder.Services.AddScoped<ControleTeste.Repositories.IUserRepository, ControleTeste.Repositories.UserRepository>();
builder.Services.AddScoped<ControleTeste.Services.IUserService, ControleTeste.Services.UserService>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// JWT configuration
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtKey = Encoding.UTF8.GetBytes(jwtSection["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin", policy => policy.RequireClaim("isAdmin", "true"));
});

var app = builder.Build();


await ControleTeste.Data.SeedData.EnsureAdminAsync(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", async (HttpContext http) =>
{
    if (http.User?.Identity?.IsAuthenticated == true)
    {
        return Results.Redirect("/Alteracoes");
    }
    return Results.Redirect("/Account/Login");
});

app.MapControllers();
app.MapRazorPages();

app.Run();