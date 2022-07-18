using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;
using OrderAPI.MessageBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHostedService<Subscriber>();
builder.Services.AddSingleton<IPublisher, Publisher>();
builder.Services.AddScoped<IEventMessageProcessor, EventMessageProcessor>();


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

app.MapGet("/", () => { return "Order Service"; });
app.MapControllers();

app.Run();
