global using TModelId = int;
using InventariumAPI;
using InventariumAPI.Data;
using InventariumAPI.Interfaces;
using InventariumAPI.Middleware;
using InventariumAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAutoMapper(c =>
{
    var profile = new MapperProfile();
    c.AddProfile(profile);
});
builder.Services.AddScoped<IObjectRepository, ObjectRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILendoutsRepository, LendoutsRepository>();
if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IDebugRepository, DebugRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();
app.UseErrorHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.EnableValidator());
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

