using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;

namespace ProgrammersBlog.Mvc.Areas.Admin.ViewComponents
{
    //ViewComponent sınıfı Vİews>Shared>Components>AdminMenu>Default.cshtml sayfasını arıyor ve direk oraya bu modeli dönüyor. BU sayede direk o view içierisinde tekrar
    //controller isteği yapmadan direk kullanıcının rollerini kontrol ederek ona göre kategoride neleerin gösterileceğiniz belirleyebiliyoruz.
    [Authorize]
    public class AdminMenuViewComponent:ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public AdminMenuViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //Async fonksiyon olmadığı için result larını aldık. HttpContext.User ile şuanki giriş yapmış kullanıcıya ulaşılabiliyor.
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            if (user == null)
                return Content("Kullanıcı Bulunamadı");
            if (roles == null)
                return Content("Rol Bulunamadı");
            return View(new UserWithRolesViewModel
            {
                User=user,
                Roles=roles
            });

        }
    }
}
