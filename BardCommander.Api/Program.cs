using BardCommander.Api.Middlewares;
using Service.BoardCommander.DataBuilder;
using Service.BoardCommander.TcpCommunication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITcpCommunication, TcpCommunication>();
builder.Services.AddSingleton<IDataBuilderService, DataBuilderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseErrorHandler();

app.MapControllers();

app.Run();
