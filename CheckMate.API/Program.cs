using ChackMate.BLL.Interfaces;
using ChackMate.BLL.Services;
using CheckMate.BLL.Interfaces;
using CheckMate.BLL.Services;
using CheckMate.DAL.Interfaces;
using CheckMate.DAL.Repositories;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddTransient<SqlConnection>(c => new SqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentService, TournamentService>();

builder.Services.AddScoped<ITournamentCategoryRepository, TournamentCategoryRepository>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
