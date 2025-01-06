using Microsoft.AspNetCore.Authentication.JwtBearer;
using CheckMate.BLL.Interfaces;
using CheckMate.BLL.Services;
using CheckMate.DAL.Interfaces;
using CheckMate.DAL.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {

    // c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    // Permet d'ajouter le "cadenas" sur les routes
    // - Implémentation simple (Cadenas sur toutes les routes)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    // - Plus d'infos : 
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#add-security-definitions-and-requirements
});

// Dependency Injection
builder.Services.AddTransient<SqlConnection>(c => new SqlConnection(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<ITournamentService, TournamentService>();

builder.Services.AddScoped<ITournamentCategoryRepository, TournamentCategoryRepository>();
builder.Services.AddScoped<ITournamentCategoryService, TournamentCategoryService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MailService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true
    };
});

builder.Services.AddCors(service =>
{
    service.AddPolicy("Angular_Front", policy =>
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("Angular_Front");

app.UseAuthentication();
app.UseAuthorization();


// CORS

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch(ArgumentNullException e)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
    catch (UnauthorizedAccessException e)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
    // TODO : Catch error 403
    catch (ArgumentException e)
    {
        context.Response.StatusCode = 404;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
    catch (Exception e)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(e.Message);
    }
});


app.MapControllers();

app.Run();

// Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjUxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6Im5naWhvdWwiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJuZ2lob3VsQGhvdG1haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJSb2xlIjoiQWRtaW4iLCJleHAiOjE3MzM3NTkwOTIsImlzcyI6IkNoZWNrTWF0ZSIsImF1ZCI6IkFuZ3VsYXJfQ2hlY2tNYXRlX09ubHkifQ.d4RIYCKIxOh9UuGsLbPyf1dtU7uhVkJy3EGKFwUAMik