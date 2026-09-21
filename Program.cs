using MaisGuinchos;
using MaisGuinchos.Hubs;
using MaisGuinchos.Middlewares;
using MaisGuinchos.Repositorys;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ILocationRepo, LocationRepo>();
builder.Services.AddScoped<IGuinchoService, GuinchoService>();
builder.Services.AddScoped<IGuinchoRepo, GuinchoRepo>();
builder.Services.AddScoped<ITowRequestService, TowRequestService>();
builder.Services.AddScoped<ITowRequestRepo, TowRequestRepo>();
builder.Services.AddScoped<ITowTravelRepo, TowTravelRepo>();
builder.Services.AddScoped<ITravelService, TravelService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpClient<IMapsService, MapsService>();
builder.Services.AddControllers();          
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173",
                                "http://localhost:4173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"])
        ),
        ClockSkew = TimeSpan.Zero
    };
});

var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseSecretKey = builder.Configuration["Supabase:SecretKey"];

if (string.IsNullOrWhiteSpace(supabaseUrl))
    throw new InvalidOperationException("Supabase URL não configurada.");

if (string.IsNullOrWhiteSpace(supabaseSecretKey))
    throw new InvalidOperationException("Supabase Secret Key não configurada.");

builder.Services.AddSingleton<Supabase.Client>(provider =>
{
    var options = new Supabase.SupabaseOptions();

    return new Supabase.Client(
        supabaseUrl,
        supabaseSecretKey,
        options
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<TowHub>("/towhub");

app.Run();


