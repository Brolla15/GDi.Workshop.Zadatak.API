using GDi.Workshop.Zadatak.BM.Models.SignalR;
using GDi.Workshop.Zadatak.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<WorkshopDbContext>
    (configure => configure.UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultConnection"), options =>
    {
        options.MigrationsAssembly(typeof(WorkshopDbContext).Assembly.FullName);
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials();
});
app.UseAuthorization();

app.MapControllers();

app.MapHub<AppHub>("/appHub");


app.Run();
