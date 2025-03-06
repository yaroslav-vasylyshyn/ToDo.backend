using Microsoft.EntityFrameworkCore;
using ToDo.Core.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();