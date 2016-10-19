// <copyright file="IUserSearchService.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using UserSearchService.Models;

namespace UserSearchService.Services
{
    public interface IUserSearchService
    {
        void AddUser(User user);

        bool RemoveUser(User user);

        void UpdateUser(User user);

        PaginationResult<User> SearchUser(string query, int skip, int take);
    }
}