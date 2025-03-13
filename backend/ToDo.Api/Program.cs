using Microsoft.EntityFrameworkCore;
using ToDo.Core.Context;
using ToDo.Infrastructure.Mappings;
using ToDo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<ITasksRepository, TasksRepository>();

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "https://localhost:3000"));

app.MapControllers();
app.UseHttpsRedirection();

app.Run();