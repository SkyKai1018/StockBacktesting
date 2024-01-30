using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StockBacktesting;
using StockBacktesting.Models;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Swagger services.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

//create
using (var context = new TestConnDB())
{
    var newConnection = new Connection { ConnStr = "new_conn_string", Remark = "new_remark" };
    context.Connection.Add(newConnection);
    context.SaveChanges();
}


//read
using (var context = new TestConnDB())
{
    var connections = context.Connection.ToList();
    foreach (var conn in connections)
    {
        Console.WriteLine($"ConnStr: {conn.ConnStr}, Remark: {conn.Remark}");
    }
}

//update
using (var context = new TestConnDB())
{
    var connection = context.Connection.FirstOrDefault(c => c.ConnectionId == 1); // 假設要更新 ConnectionId 為 1 的記錄
    if (connection != null)
    {
        connection.ConnStr = "updated_conn_string";
        connection.Remark = "updated_remark";
        context.SaveChanges();
    }
}

//delete
using (var context = new TestConnDB())
{
    var connection = context.Connection.FirstOrDefault(c => c.ConnectionId == 1);
    if (connection != null) {
        context.Connection.Remove(connection);
        context.SaveChanges();
    }

}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

app.Run();
