using Chinobod.Api.Brokers.Storages;
using Chinobod.Api.Services.Foundations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

AddBrokers(builder);

AddServices(builder);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddBrokers(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<StorageBroker>();
    builder.Services.AddTransient<IStorageBroker, StorageBroker>();
}

static void AddServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<INewsService, NewsService>();
}