using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Desafio.Data;
using Desafio.Services;


var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<ContaService>();
builder.Services.AddScoped<TransacaoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EFCoreContext>(options => options.UseLazyLoadingProxies().UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"))); // This is the connection string from appsettings.json

builder.Services.AddControllers(); // This is required to use the [ApiController] attribute


var app = builder.Build();

// Ativa o CORS
app.UseCors("AllowAll");

app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.UseHttpsRedirection();

app.Run();
