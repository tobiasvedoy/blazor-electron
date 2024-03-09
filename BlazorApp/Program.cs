using BlazorApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ElectronNET.API;
using ElectronNET.API.Entities;
 
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddElectron();
builder.WebHost.UseElectron(args);

if (HybridSupport.IsElectronActive)
{
    //Open Electron Window
    Task.Run(async () =>
    {
        var browserWindowOptions = new BrowserWindowOptions
        {
            Title = "MyBlazorElectronApp",
            AutoHideMenuBar = true,
            MinHeight = 600,
            MinWidth = 800,
            Frame = false,
        };

        var window = await Electron.WindowManager.CreateWindowAsync(browserWindowOptions);

        window.OnClosed += () => {
            Electron.App.Quit();
        };
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
