using Microsoft.EntityFrameworkCore;
using WebCameraControl.Core;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IMvcBuilder mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx => new BackgroundTaskQueue(100));

string connectionString = builder.Configuration.GetConnectionString("SqliteConnection")!;

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.UseWhen(httpContext => !httpContext.Request.Path.Value?.StartsWith("/api", StringComparison.CurrentCultureIgnoreCase) == true, application =>
{
    application.UseMiddleware<ExceptionHandlingMiddleware>();
});

app.Run();
