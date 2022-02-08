using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]

    public class HomeController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;

        public HomeController(UserManager<User> userManager, IArticleService articleService, ICommentService commentService, ICategoryService categoryService)
        {
            _userManager = userManager;
            _articleService = articleService;
            _commentService = commentService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByNonDeleted();
            var articlesCountResult = await _articleService.CountByNonDeleted();
            var commentsCountResult = await _commentService.CountByNonDeleted();
            var userCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAllAsync();

            if (categoriesCountResult.ResultStatus == ResultStatus.Success && articlesResult.ResultStatus == ResultStatus.Success && commentsCountResult.ResultStatus == ResultStatus.Success && userCount > -1 && articlesResult.ResultStatus == ResultStatus.Success)
            {
                return View(new DashboardViewModel
                {
                    Articles = articlesResult.Data,
                    ArticlesCount = articlesCountResult.Data,
                    CategoriesCount = categoriesCountResult.Data,
                    CommentsCount = commentsCountResult.Data,
                    UsersCount=userCount
                });
            }

            return NotFound();


        }
    }
}
