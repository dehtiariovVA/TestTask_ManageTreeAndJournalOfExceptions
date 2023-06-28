using TestTask_ManageTreeAndJournalOfExceptions.Web.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TestTask_ManageTreeAndJournalOfExceptions.Data.EFDatabaseContext;
using TestTask_ManageTreeAndJournalOfExceptions.Data.Repositories;
using TestTask_ManageTreeAndJournalOfExceptions.Domain.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connectionString));

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
dbContext.Database.Migrate();

builder.Services.AddScoped<IJournalRepository, JournalRepository>();
builder.Services.AddScoped<INodeRepository, NodeRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
