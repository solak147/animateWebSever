using Animate.Data;
using Animate.Data.Service;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri (builder.Configuration["BaseUrl"])});
builder.Services.AddSingleton<ITypedClientConfig, TypedClientConfig>();

builder.Services.AddHttpClient<ITypedClient, TypedClient>()
    .ConfigureHttpClient((serviceProvider, httpClient) =>
    {
        var clientConfig = serviceProvider.GetRequiredService<ITypedClientConfig>();
        httpClient.BaseAddress = clientConfig.BaseUrl;
        httpClient.Timeout = TimeSpan.FromSeconds(clientConfig.Timeout);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "BlahAgent");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .SetHandlerLifetime(TimeSpan.FromMinutes(5))    // Default is 2 mins
    .ConfigurePrimaryHttpMessageHandler(x =>
        new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            UseCookies = false,
            AllowAutoRedirect = false,
            UseDefaultCredentials = true,
        });

//jwt token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

//≈Á√“®≠§¿
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
