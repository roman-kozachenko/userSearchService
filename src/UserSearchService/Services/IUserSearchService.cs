// <copyright file="IUserSearchService.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System.Threading.Tasks;
using UserSearchService.Models;

namespace UserSearchService.Services
{
    public interface IUserSearchService
    {
        Task ReplaceUser(uint channelId, User user);

        Task RemoveUser(uint channelId, uint userId);

        Task<PaginationResult<MatchedUser>> SearchUser(uint channelId, string query, uint skip, uint take);
    }
}