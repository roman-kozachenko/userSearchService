// <copyright file="UsersController.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserSearchService.Services;

namespace UserSearchService.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserSearchService _userSearchService;
        private const uint ChannelId = 0;
        public UsersController(IUserSearchService userSearchService)
        {
            _userSearchService = userSearchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetData(uint pageIndex, uint pageSize, string query = null)
        {
            var results = await _userSearchService.SearchUser(ChannelId, query, pageIndex * pageSize, pageSize);
            return Json(results.Data);
        }

    }
}