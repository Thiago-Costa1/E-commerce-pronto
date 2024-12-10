using aspnetsite.Repository;
using aspnetsite.Repository.Contract;
using aspnetsite.GerenciaArquivos;
using System.Net;
using aspnetsite.Libraries.Login;
using aspnetsite.Libraries.Middleware;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Adicionado para manipular a sessão
builder.Services.AddHttpContextAccessor();


// Adicionar a interface como serviço
builder.Services.AddScoped<INotebookRepository, NotebookRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();


builder.Services.AddScoped<aspnetsite.Libraries.Sessao.Sessao>();
builder.Services.AddScoped<LoginCliente>();
builder.Services.AddScoped<LoginColaborador>();


// Add Gerenciador Arquivo como serviços
builder.Services.AddScoped<GerenciadorArquivo>();
builder.Services.AddScoped<aspnetsite.Cookie.Cookie>();
builder.Services.AddScoped<aspnetsite.CarrinhoCompra.CookieCarrinhoCompra>();
builder.Services.AddScoped<aspnetsite.Libraries.Login.LoginCliente>();


// Corrigir problema com TEMPDATA
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Definir um tempo para duração. 
    options.IdleTimeout = TimeSpan.FromSeconds(900);
    options.Cookie.HttpOnly = true;
    // Mostrar para o navegador que o cookie e essencial   
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().AddSessionStateTempDataProvider();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDefaultFiles();
app.UseCookiePolicy();
app.UseSession();

app.UseRouting();

app.UseAuthorization();
//app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();