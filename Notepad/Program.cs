using Microsoft.EntityFrameworkCore;
using Notepad.Context;
using Notepad.Models;
using Notepad.Middleware;
using Notepad.Interface.Repository;
using Notepad.Implementation.Repository;
using Notepad.Interface.Services;
using Notepad.Implementation.Services;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<INoteRepository, NoteRepository>()
    .AddScoped<INoteServices, NoteServices>();

//builder.Services.AddDbContext<NoteContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("NoteContext"),
//        new MySqlServerVersion(new Version(9, 0, 0))
//    ));


var connectionString = builder.Configuration.GetConnectionString("NotepadConnection");

//Just in case you want to use an env var later, keep this logic flexible
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__NotepadConnection");
}

if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');

    var npgsqlBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? userInfo[1] : "",
        Database = uri.AbsolutePath.Trim('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate= true
    };

    connectionString = npgsqlBuilder.ConnectionString;
}

builder.Services.AddDbContext<NoteContext>(options =>
    options.UseNpgsql(connectionString));


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (builder.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseMiddleware<Notepad.Middleware.DeviceIdMiddleWare>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notes}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NoteContext>();

    try
    {
        context.Database.Migrate();
        Console.WriteLine("Database migrated successfully on startup.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database migration failed: {ex.Message}");
    }
}

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception caught: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
        throw;
    }
});

app.Run();
