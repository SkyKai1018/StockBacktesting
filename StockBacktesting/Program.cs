using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StockBacktesting.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// 注册服务和配置
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// 配置HTTP请求管道
ConfigurePipeline(app);

app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Add services to the container.
    services.AddControllersWithViews();

    // Add Swagger services.
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    });

    var serviceProvider = services.BuildServiceProvider();
    var logger = serviceProvider.GetRequiredService<ILogger<Startup>>();

    try
    {
        //// Add DbContext with MySQL
        //services.AddDbContext<TestConnDBContext>(options =>
        //    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
        //    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

        //services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseMySql(configuration.GetConnectionString("GcpConnection"),
        //    ServerVersion.AutoDetect(configuration.GetConnectionString("GcpConnection"))));
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Connect to SQL error");
    }
}

void ConfigurePipeline(WebApplication app)
{
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}
