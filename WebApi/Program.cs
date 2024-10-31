using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infra.repository.generics;
using Infra.repository;
using dominio.Interfaces.Generics;
using dominio.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using webApi.Token;
using Entities.Context;
using Domain.Interfaces.IUsuario;
using Domain.Interfaces.ICliente;
using Domain.Interfaces.IFornecedor;
using Domain.Interfaces.IHistoricoAcao;
using Domain.Interfaces.IPedido;
using Domain.Interfaces.IPedidoProduto;
using Microsoft.OpenApi.Models;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API - Sistema Estoque Fiotec", Version = "v1" });

   


});

builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// INTERFACE E REPOSITORIO
builder.Services.AddScoped(typeof(InterfaceGeneric<>), typeof(RepositorioGeneric<>));

// INTERFACE E REPOSITORIO
builder.Services.AddScoped(typeof(InterfaceGeneric<>), typeof(RepositorioGeneric<>));

builder.Services.AddScoped<InterfaceUsuario, RepositorioUsuario>();
builder.Services.AddScoped<InterfaceCliente, RepositorioCliente>();
builder.Services.AddScoped<InterfaceFornecedor, RepositorioFornecedor>();
builder.Services.AddScoped<InterfaceHistoricoAcao, RepositorioHistoricoAcao>();
builder.Services.AddScoped<InterfacePedido, RepositorioPedido>();
builder.Services.AddScoped<InterfacePedidoProduto, RepositorioPedidoProduto>();
builder.Services.AddScoped<InterfaceUsuario, RepositorioUsuario>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
