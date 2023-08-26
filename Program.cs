using TelegrammService.model;
using TelegrammService.model.rest;
using TelegrammService.service;
using WebApplication1.Controllers;


ConfigService configService = new ConfigService();
Config config = configService.getConfig();

var builder = WebApplication.CreateBuilder(args);
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls(config.url);
}
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

app.MapPost("/api/do/login", (DoLogin doLogin) =>
{
    return telegrammService.doLogin(doLogin.data);
});
app.MapPost("/api/client/reset", () =>
{
    return telegrammService.clientReset();
});

app.MapPost("/api/init", (ApiHashSetup source) =>
{
    return telegrammService.init(source.apiId, source.apiHash);
});
app.MapPost("/api/message/send", (SendMessage source) =>
{
    return telegrammService.sendMessage(source.apiId, source.login, source.message);
});
app.MapPost("/api/check/channel/subscribe", (CheckSubscribe source) =>
{
    return telegrammService.checkSubscriber(source.apiId, source.channelId);
});
app.MapPost("/api/message/list/send", (SendMessages source) =>
{
    return telegrammService.sendMessages(source.apiId, source.logins, source.message);
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