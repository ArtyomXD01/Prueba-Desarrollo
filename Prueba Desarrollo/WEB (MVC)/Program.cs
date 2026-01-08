var builder = WebApplication.CreateBuilder(args);

// Cambiar a MVC
builder.Services.AddControllersWithViews();

// Configurar HttpClient
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Ruta MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Region}/{action=Index}/{id?}");

app.Run();
