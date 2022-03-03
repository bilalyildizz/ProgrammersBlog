using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProgrammersBlog.Data.Concrete.EntityFramework.Mappings;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Data.Concrete.EntityFramework.Contexts
{
    //microsoft ıdentity yapısını kullanmak için microsoft.aspnetcore.ıdentity.entityframeworkcore kurulmalı.
    //int parametresi sınıflarda kullanılan id'nin int olmasını sağlıyor.
    public class ProgrammersBlogContext:IdentityDbContext<User,Role,int,UserClaim,UserRole,UserLogin,RoleClaim,UserToken>
    {
       public  DbSet<Article> Articles { get; set; }
       public DbSet<Category> Categories { get; set; }
       public DbSet<Comment> Comments { get; set; }
       public DbSet<Role> Roles { get; set; }
       public DbSet<User> Users { get; set; }
       public DbSet<User> Logs { get; set; }

        //       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //       {//@ çift tırnak içerisindeki her şeyin string olarak algılanmasını sağlıyor.
        //           optionsBuilder.UseSqlServer(
        //               connectionString: @"Server=(localdb)\MSSQLLocalDB;Database=ProgrammersBlog;Trusted_Connection=True;
        //MultipleActiveResultSets=True;");
        //       }
        public ProgrammersBlogContext(DbContextOptions<ProgrammersBlogContext> options):base(options)
        {
            
        }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           modelBuilder.ApplyConfiguration(new ArticleMap());
           modelBuilder.ApplyConfiguration(new CategoryMap());
           modelBuilder.ApplyConfiguration(new CommentMap());
           modelBuilder.ApplyConfiguration(new RoleMap());
           modelBuilder.ApplyConfiguration(new UserMap());
           modelBuilder.ApplyConfiguration(new RoleClaimMap());
           modelBuilder.ApplyConfiguration(new UserClaimMap());
           modelBuilder.ApplyConfiguration(new UserLoginMap());
           modelBuilder.ApplyConfiguration(new UserRoleMap());
           modelBuilder.ApplyConfiguration(new UserTokenMap());
           modelBuilder.ApplyConfiguration(new LogMap());



        }
    }

}
