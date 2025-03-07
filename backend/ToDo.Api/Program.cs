using Microsoft.EntityFrameworkCore;
using ToDo.Core.Context;
using ToDo.Infrastructure.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();