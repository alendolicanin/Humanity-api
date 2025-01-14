using Humanity.API.Middleware;
using Humanity.Application.Interfaces;
using Humanity.Application.Mapping;
using Humanity.Application.Services;
using Humanity.Domain.Models;
using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;
using Humanity.Repository.Repositories;
using Humanity.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Humanity.Repository.MongoDB.Interfaces.MongoDB;
using Humanity.Repository.MongoDB.Repositories.MongoDB;
using Humanity.Repository.MongoDB;
using Humanity.Infrastructure.MongoDB;
using MongoDB.Driver;
using Humanity.Repository.Neo4jDB.Interfaces.Neo4jDB;
using Humanity.Repository.Neo4jDB;
using Humanity.Infrastructure.Neo4jDB;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddScoped<IDistributedDonationRepository, DistributedDonationRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IThankYouNoteRepository, ThankYouNoteRepository>();
builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories.MongoDB
builder.Services.AddScoped<IMongoDonationRepository, MongoDonationRepository>();
builder.Services.AddScoped<IMongoDistributedDonationRepository, MongoDistributedDonationRepository>();
builder.Services.AddScoped<IMongoReceiptRepository, MongoReceiptRepository>();
builder.Services.AddScoped<IMongoThankYouNoteRepository, MongoThankYouNoteRepository>();
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();

// Repositories.Neo4jDB
builder.Services.AddScoped<INeo4jThankYouNoteRepository>(sp => sp.GetRequiredService<INeo4jUnitOfWork>().ThankYouNoteRepository);
builder.Services.AddScoped<INeo4jUnitOfWork, Neo4jUnitOfWork>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IDistributedDonationService, DistributedDonationService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IThankYouNoteService, ThankYouNoteService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Services.MongoDB
builder.Services.AddScoped<Humanity.Application.Services_MongoDB.DonationService>();
builder.Services.AddScoped<Humanity.Application.Services_MongoDB.DistributedDonationService>();
builder.Services.AddScoped<Humanity.Application.Services_MongoDB.ReceiptService>();
builder.Services.AddScoped<Humanity.Application.Services_MongoDB.ThankYouNoteService>();

// Services.Neo4jDB
builder.Services.AddScoped<Humanity.Application.Services_Neo4jDB.ThankYouNoteService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// MediatR
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining(typeof(Program)));

// IP Whitelist
var whitelistedIPs = builder.Configuration.GetSection("WhitelistedIPs").Get<string[]>();
builder.Services.AddIPWhitelist(whitelistedIPs);

// Configure DbContext for relational database
builder.Services.AddDbContext<PlutoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure MongoDB
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddSingleton<IMongoDatabase>(sp => sp.GetRequiredService<MongoDbContext>().Database);

// Configure Neo4j
builder.Services.Configure<Neo4jSettings>(builder.Configuration.GetSection("Neo4j"));
builder.Services.AddSingleton<Neo4jContext>();
builder.Services.AddScoped<IAsyncSession>(sp => sp.GetRequiredService<Neo4jContext>().GetSession());

// Configure JSON options to handle reference loops
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<PlutoContext>()
        .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIPWhitelist();

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
