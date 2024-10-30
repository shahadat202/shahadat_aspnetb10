using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web;
using DevSkill.Inventory.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Reflection;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure.Extensions;

#region Bootstrap Logger
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
#endregion

try
{
    Log.Information("Application Started...");

    var builder = WebApplication.CreateBuilder(args);

    #region Serilog Configuration...
    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));
    #endregion

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    var migrationAssembly = Assembly.GetExecutingAssembly().FullName;
    
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString, (x) =>x.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDbContext<InventoryDbContext>(options =>
        options.UseSqlServer(connectionString, (x) => x.MigrationsAssembly(migrationAssembly)));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    #region Autofac Configuration....
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new WebModule(connectionString, migrationAssembly));
    });
    #endregion

    //builder.WebHost.UseUrls("http://*:80");

    builder.Services.AddIdentity();
    builder.Services.AddControllersWithViews();

    builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

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

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    
    //app.MapRazorPages();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application.");
}
finally
{
    Log.CloseAndFlush();
}