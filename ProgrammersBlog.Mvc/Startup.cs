using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Data.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Mvc.AutoMapper.Profiles;
using ProgrammersBlog.Mvc.Helpers;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Mvc.Helpers.Concrete;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;

namespace ProgrammersBlog.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //mvc uygulamasý olduðunu bu kod ile belirtiyoruz.
            //Add json options ekleme nedenimiz controllerdan  viewa model dönerken javascriptin bu modeli tanýmasý için json formata çevirmemiz gerekmesi.
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=>
                opt.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter())
            );
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile),typeof(UserProfile), typeof(ViewModelsProfile));

            //Bi<im service/Extensions kýsmýnda oluþturduðumuz dosya. Ýnterface leri vs vermek için.
            services.LoadMyServices(Configuration.GetConnectionString(name:"LocalDB"));
            services.AddScoped<IImageHelper, ImageHelper>();
            //Cookie bilgilerini veriyoruz.
            services.ConfigureApplicationCookie(options =>
            {
                //Kullanýcý giriþ yapmadan bir yere ulaþmak istediðinde bu adrese logine yönlendiriyor kullanýcýyý.
                options.LoginPath = new PathString("/Admin/User/Login");
                options.LogoutPath = new PathString("/Admin/User/Logout");

                options.Cookie = new CookieBuilder
                {
                    Name =  "ProgrammersBlog",
                    //Gelen isteklerin sadece http üzerinden olmasýný saðlýyor. Yazýlan js kodu ile cookie bilgilerine ulaþýlmasý engelleniyor bu sayede.
                    HttpOnly=true,
                    //Gelen isteklerin sadece kendi ssitemiz üzerinden gelenlerini kabul ediyor. Cookileri ele geçirilen birinin bilgileriyle baþka bir adresden istek gelmesi engelleniyor.
                    SameSite=SameSiteMode.Strict,
                    //Normalde .Always olmalý gelen bütün istekler https üzerinden olmalý ama geliþtirme aþamasý olduðu için SameAsRequest yaptýk http isteklerinide kabul edecek.
                    SecurePolicy=CookieSecurePolicy.SameAsRequest,
                };
                //Kullanýcý giriþ yaptýktan sonra belirle süre hesabý açýk kalacak mý ne kadar açýk kalacak. 7 gün yaptýk.
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
                //Giriþ yapmýþ kullanýcý yetkisi olmayan bir yere ulaþmaya çalýþtýðýnda bu sayfaya yönlenirilecek burda hata vereceðiz.
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied");
            });

            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //Bulunmayan bir viewe gittiðimizde bize bunu belirterek yardýmcý oluyor.
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Admin area ile article sayfasýna girdiðimizde articels üzerinde deðiþiklik yapabileceðiz. Fakat normal kullanýcý girdiðinde sadece okuyabilecek.
                endpoints.MapAreaControllerRoute(
                    name:"Admin",
                    areaName:"Admin",
                    pattern:"Admin/{controller=Home}/{action=Index}/{id?}"
                    );
                //Baþlangýçta home indexinden açýlýþý saðlýyor.
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
