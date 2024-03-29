using Microsoft.EntityFrameworkCore;
using ReportAPI.Data;
using ReportAPI.MessageBus;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<Subscriber>();
builder.Services.AddScoped<IEventMessageProcessor, EventMessageProcessor>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

DbSeed.RunDBSeed(app);


app.UseAuthorization();

app.MapGet("/", () => { return "Report Service"; });
app.MapControllers();

app.Run();
