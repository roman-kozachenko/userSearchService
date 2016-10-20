// <copyright file="IUserSearchService.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System;
using System.Threading.Tasks;
using Services.Models;

namespace Services
{
    public interface IUserSearchService : IDisposable
    {
        Task ReplaceUser(uint channelId, User user);

        Task RemoveUser(uint channelId, uint userId);

        Task<PaginationResult<MatchedUser>> SearchUser(uint channelId, string query, uint skip, uint take);
    }
}