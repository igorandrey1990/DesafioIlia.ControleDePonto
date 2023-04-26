using DesafioIlha.ControleDePonto.Controllers;
using DesafioIlha.ControleDePonto.DAL;
using DesafioIlha.ControleDePonto.Services;
using DesafioIlha.ControleDePonto.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DesafioIlha.ControleDePonto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MomentoContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("MomentoContext")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Controle de Ponto API",
                    Description = "Desafio da Ília: Bater ponto e gerar relatório.",
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            builder.Services.AddScoped<IMomentoService, MomentoService>();
            builder.Services.AddScoped<IFolhaDePontoService, FolhaDePontoService>();

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
        }
    }
}