using Test.Client.Interfaces;
using Test.Client.Services;
using Test.Server.Configurations;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);


var levelSwitch = new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Information);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.ControlledBy(levelSwitch)
    .Enrich.WithProperty("InstanceId", Guid.NewGuid().ToString("n"))
    .WriteTo.File("logError.txt")
    .CreateLogger();

/* this is used instead of .UseSerilog to add Serilog to providers */
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));


builder.Services.ConfigureServices(builder.Configuration);


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddHttpClient();

var app = builder.Build();


// Configurar el manejo de errores centralizado
app.UseExceptionHandler("/Error");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
