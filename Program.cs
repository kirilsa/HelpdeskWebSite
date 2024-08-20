global using HelpDeskWebSite.Services.EmailService;

using HelpDeskWebSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HelpDeskWebSite;
using Microsoft.AspNetCore.Identity.UI.Services;
using HelpDeskWebSite.Models;
using HelpDeskWebSite.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

//builder.Services.AddScoped<IEmailService, EmailService>();    //test implementation
//email sending
builder.Services.AddScoped<ISendMailgunEmail, SendMailgunEmail>(); //send Mailgun email

builder.Services.Configure<EmailSettings>
   (options => builder.Configuration.GetSection("EmailSettings").Bind(options));
builder.Services.AddSingleton<IEmailService, EmailService>();


//receiving emails
//builder.Services.AddSingleton<IEmailReceiverService, EmailReceiverService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseDeveloperExceptionPage();

 app.Run();
