using Amazon.S3;
using PayIP.Infra;
using PayIP.Infra.Interfaces;
using PayIP.Services;
using PayIP.Services.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
builder.Services.AddHttpClient();
builder.Services.AddScoped<IBlipSenderNotification, BlipSenderNotification>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IClientePagamentoService, ClientePagamentoService>();
builder.Services.AddScoped<IMotoristaPagamentoService, MotoristaPagamentoService>();
builder.Services.AddScoped<IPayIpSender, PayIpSender>();
builder.Services.AddAWSService<IAmazonS3>();

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
