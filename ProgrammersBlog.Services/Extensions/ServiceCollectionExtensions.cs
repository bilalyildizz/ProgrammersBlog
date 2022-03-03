using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concrete;
using ProgrammersBlog.Data.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;

namespace ProgrammersBlog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {

        //IdentityDbContext kullanmak için microsoft aspnetcore identity eklemeliyiz. Ve burda configuration yapmalıyı
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection, string connectionString)
        {
            //No tracking ile include ettiklerimizin tekrar kendi içindekileri include etmesi yani sonsuz döngüyü önlemiş oluyorz.
            serviceCollection.AddDbContext<ProgrammersBlogContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            //IDentity user ve role sınıfları istiyor. ve ıdentity db context istiyor biz zaten db conteximizi ıdentity olarak ayarlamıştık.
            //Kullanıcı adı ve parola ile ilgili ayarları buradan veriyoruz.
            serviceCollection.AddIdentity<User, Role>(options =>
                {
                    //User Password Options
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;

                    //User Username and Email Options

                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                }).AddEntityFrameworkStores<ProgrammersBlogContext>();
            serviceCollection.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(15);
            });
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            serviceCollection.AddScoped<ICommentService, CommentManager>();
            serviceCollection.AddSingleton<IMailService, MailManager>();
            return serviceCollection;

        }
    }
}
