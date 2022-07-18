using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.MessageBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<Subscriber>();
builder.Services.AddSingleton<IPublisher, Publisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.MapGet("/", () => { return "Product Service"; });
app.MapControllers();
app.Run();

