# OAuthClientTesting4
 

Signing in to an app using OAuth2 / credentials from external auth providers like google, facebook etc
https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-6.0&tabs=visual-studio

This lets the user login, but doesn’t do any authorization so is presumably just doing OpenID stuff? Everything is super abstracted away, so you can’t see how anything is working or what URLs are being called etc.

•	This uses Microsoft.AspNetCore.Authentication and needs a nuget package for each authentication provider e.g. Microsoft.AspnetCore.Authentication.Twitter
•	This is .net 6 and higher?
•	I used razor pages, but should work with MVC?

Process is:
•	Select the ASP.NET Core Web App template. Select OK.
•	In the Authentication type input, select Individual Accounts.
•	There’s no database yet!
	o	Run the app and select the Register link.
	o	Enter the email and password for the new account, and then select Register.
	o	Follow the instructions to apply migrations.
•	After the builder.Services.AddRazorPages(); add into forward headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
	Also add in app.UserForwardedHeaders(); which must be before app.UseHsts();
•	Go to each login provider and setup access; get a Client ID and a Client Secret for each one, put those some where secure
https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows
•	You have to add nuget packages for the authentication providers e.g. Microsoft.AspnetCore.Authentication.Facebook
•	Add the authentication options for each authentication provider to the Program.cs e.g.
builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection =
       config.GetSection("Authentication:Google");
       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
   })


•	All the Identity/login pages etc are added by calling AddDefaultIdentity in the Program.cs, no files will exist in the project. To customize them you need to add them, when the file exists it will then take precedence.
“Right-click your project in the Solution Explorer and choose Add > New Scaffolded Item. There's a tab there for Identity, with one scaffold. When you choose this, you'll be presented with a list of the various Razor Pages that can be scaffolded in. Select one or more you'd like to customize (or just include everything, if you like).”
https://stackoverflow.com/questions/55377364/change-layout-of-facebook-authentication-button-in-asp-net-core-mvc
