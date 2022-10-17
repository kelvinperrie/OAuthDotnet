using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OAuthClientTesting4.Data;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// this setup is from
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-6.0&tabs=visual-studio
// got to put secrets in first for this to work
// https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows
builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection =
       config.GetSection("Authentication:Google");
       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
   })
   .AddFacebook(options =>
   {
       //IConfigurationSection FBAuthNSection =
       //config.GetSection("Authentication:FB");
       //options.ClientId = FBAuthNSection["ClientId"];
       //options.ClientSecret = FBAuthNSection["ClientSecret"];
       options.ClientId = "blah";
       options.ClientSecret = "blah";
   })
   .AddMicrosoftAccount(options =>
   {
       //microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
       //microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
       options.ClientId = "blah";
       options.ClientSecret = "blah";
   })
   .AddTwitter(options =>
   {
       //twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
       //twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
       options.ConsumerKey = "blah";
       options.ConsumerSecret = "blah";
       options.RetrieveUserDetails = true;
   });


var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
