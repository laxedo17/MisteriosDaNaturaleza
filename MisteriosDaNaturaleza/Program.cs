using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();//punto de entrada da app. Ten un createhostbuilder (un metodo na clase Program que devolve un obxeto de tipo IHostBuilder), despois construe a web e lanzaa. Empeza como unha app de console e despois pasa a ser unha app de ASP.NET, configurando a app para usar Kestrel, que e un servidor web interno, o cal implica que esta embebido na aplicacion. Tamen lle indica app que use IIS, que e o servidor web externo e a peticion sera "proxied" entre o servidor externo e o interno, tamen manexa as opcions do arquivo appsettings.json e configura a ruta directamente
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>(); //aqui podese ver que usa o arquivo Startup.cs
                });
    }
}
