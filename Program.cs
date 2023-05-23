using TelegrammService.model;
using TelegrammService.model.rest;
using TelegrammService.service;
using WebApplication1.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:8033");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
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

MessageCounterService messageCounterService = new MessageCounterService();
TelegrammSenderService telegrammService = new TelegrammSenderService();
telegrammService.setMessageCounterService(messageCounterService);

app.MapPost("/api/autentification/start", (Autentification source) =>
{
    return telegrammService.autentificationClientAsync(source.apiId, source.apiHash, source.phoneNumber, source.sessionPath);
});
app.MapPost("/api/message/send", (SendMessage source) =>
{
    return telegrammService.sendMessage(source.apiId, source.login, source.message);
});
app.MapGet("/api/statistic", () =>
{
    return messageCounterService.getClientStatistic();
});
app.MapPost("/api/client/limit/setup", (ClientLimitSetup clientLimitSetup) =>
{
    return messageCounterService.setLimit(clientLimitSetup.apiId, clientLimitSetup.limit);
});
app.MapPost("/api/client/counter/reset", (MyClient client) =>
{
    return messageCounterService.resetCounter(client.apiId);
});
app.Run();