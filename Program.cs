using TelegrammService.model;
using TelegrammService.service;
using WebApplication1.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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

app.UseDefaultFiles();
app.UseStaticFiles();

TelegrammSenderService telegrammService = new TelegrammSenderService();
app.MapPost("/api/autentificationClient", (Autentification source) =>
{
    return telegrammService.autentificationClientAsync(source.apiId, source.apiHash, source.phoneNumber);
});
app.MapPost("/api/autentificationSetCode", (Autentification source) =>
{
    return telegrammService.autentificationSetCodeAsync(source.apiId, source.phoneNumber, source.code);
});
app.MapPost("/api/sendMessage", (SendMessage source) =>
{
    return telegrammService.sendMessage(source.apiId, source.chatId, source.message);
});
app.Run();
