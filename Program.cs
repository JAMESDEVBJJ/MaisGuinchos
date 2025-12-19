using MaisGuinchos;
using MaisGuinchos.Services.Interfaces;
using MaisGuinchos.Services;
using Microsoft.EntityFrameworkCore;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Repositorys;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ILocationRepo, LocationRepo>();
builder.Services.AddHttpClient<IMapsService, MapsService>();
builder.Services.AddControllers();          
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapControllers();

app.Run();


