// <copyright file="UsersController.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using Microsoft.AspNetCore.Mvc;
using UserSearchService.Services;

namespace UserSearchService.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserSearchService _userSearchService;

        public UsersController(IUserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult GetData(int pageIndex, int pageSize, string query = null)
        {
            System.Threading.Thread.Sleep(1000);
            var results = _userSearchService.SearchUser(query, pageIndex*pageSize, pageSize);
            return Json(results);
        }

    }
}