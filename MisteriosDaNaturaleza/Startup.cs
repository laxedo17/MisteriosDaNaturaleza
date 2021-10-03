using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MisteriosDaNaturaleza.AccesoDatos.Data;
using MisteriosDaNaturaleza.AccesoDatos.Data.Inicializador;
using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio;
using MisteriosDaNaturaleza.AccesoDatos.Data.Repositorio.IRepositorio;

using MisteriosDaNatureza.Utilidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MisteriosDaNaturaleza
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Este metodo usa Dependency Injection, que era opcional en ASP.NET clasico, en ASP.NET Core e parte integral
        /// <summary>
        /// Este metodo engade servicios a aplicacion para que esten disponibles e configurar a nosa app. E un metodo que se usa bastante na creacion de webs con ASP.NET
        /// </summary>
        /// <param name="servicios">Usa un obxeto IServiceCollection que se inxecta como parametro, para construir os servicios que estaran disponible na aplicacion: Identify Framework, MVC, Razor Pages etc. Se necesitamos agregar algunha funcionalidade facemolo aqui/param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true) //o codigo orixinal aqui, pide confirmacion de que a conta rexistrada sexa true, pero non queremos esa funcionalidade agora, co cal quitamos o que hai entre parentesis
            //Quitamos tamen DefaultIdentity porque se no futuro queremos facer cambios a unha identidade, DefaultIdentity non o permite
            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //AddDefaultTokenProviders hai que agregalo cando quitamos DefaultIdentity xa que con DefaultIdentity estaba incluido por defecto.
            //AddDefaultTokenProviders envia un autentificador para axudar o usuario a identificar unha conta se olvidou o password

            //AddDefaultUI(UIFramework.Bootstrap4) poderia intentar engadirse como outro servicio, pero non e necesario de .NET Core 3.0 en adiante

            services.AddSingleton<IEmailSender, EnviarEmail>();
            services.AddScoped<IDbInicializador, DbInicializador>();//para iniciar a creacion de usuarios na Base de Datos cando se carga a web por 1ª vez. Despois no metodo Configure usamos estos obxetos
            services.AddScoped<IUnidadeDeTraballo, UnidadeDeTraballo>();

            //estas opcions de abaixo son necesarias para a Sesion e queiramos usar o carro
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews().AddNewtonsoftJson().AddRazorRuntimeCompilation();//o metodo AddRazorRuntimeCompilation e un paquete Nuget que permite actualizar a paxina en tempo real mentres a probamos. Antes estba por defecto, en NET Core 3.0 deixouse opcional para usar cantos menos paquetes por defecto millor co cal hai que descargala en Tools-->Nuget Packages
            services.AddRazorPages();
        }

        /// <summary>
        /// Configura o pipeline HTTP de ASP.NET. O pipeline manexa como a aplicacion debe responder as peticions HTTP. O pipeline esta composto de partes individuales chamadas middleware. Cando a aplicacion recibe esa peticion do navegador, recorre o pipeline e volve.
        /// 
        /// Se hai cousas no pipeline que nunca usaremos, solo se incrementa o overhead (sobrecarga)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDbInicializador dbInic)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //todos estos son middlewares, que reciben unha peticion e responden segun pase entre as diferentes "tuberias" do pipeline, que se poden personalizar
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();//con esto activado usamos as opcions da Sesion configuradas nos Servicios
            app.UseCookiePolicy();

            app.UseRouting();
            dbInic.Inicializar();
            app.UseAuthentication();
            app.UseAuthorization();

            //endpoint routing e un routing que se agregou novo en ASP.NET Core 3, que permite que o routing sexa parte dun middleware, asi que antes de que o proxecto vaia os puntos finales, basado no routing podemos tomar algunhas decisions, co cal isto podemos cambialo
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=Cliente}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //O routing en ASP.NET Core e un sistema de pattern matching -equivalencia de patrons- que realizan as accions definidas nos Controllers para mostrar a paxina adecuada e equivalente a url que lle pasamos. Se se atopa o recurso pedido, usa o Controller e o Action Method e mostrase esa paxina da nosa web en particular
            //Usando endpoint routing inyectamos middleware a nosa aplicacion
            //o patron por defecto (default pattern) e controller=Home que seria a paxina principal e un indice con unha id que pode ser nullable (poder ter un valor ou ser null)
        }
    }
}
