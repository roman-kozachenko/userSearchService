using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserSearchService.Models;

namespace UserSearchService.Services
{
    public class MockUserService : IUserSearchService
    {
        private readonly SampleUsers _sampleUsers = new SampleUsers();

        public async Task ReplaceUser(uint channelId, User user)
        {
            await Task.Yield();

            var existing = _sampleUsers.Data.First(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.FullName = user.FullName;
            }
            else
            {
                _sampleUsers.Data.Add(user);
            }
        }

        public async Task RemoveUser(uint channelId, uint userId)
        {
            await Task.Yield();

            var existing = _sampleUsers.Data.First(u => u.Id == userId);
            if (existing != null)
            {
                _sampleUsers.Data.Remove(existing);
            }
        }

        public async Task<PaginationResult<MatchedUser>> SearchUser(uint channelId, string query, uint skip, uint take)
        {
            await Task.Yield();

            var filteredUsers = FilterUsers(_sampleUsers.Data, query);
            var users = filteredUsers
                    .Skip((int)skip)
                    .Take((int)take);

            var matchedUsers = users.Select(u => CreateMatchedUser(query, u));

            var result = new PaginationResult<MatchedUser>()
            {
                Data = matchedUsers.ToList(),
                Length = take,
                Start = skip
            };

            return result;
        }

        private IEnumerable<User> FilterUsers(IEnumerable<User> users, string query)
        {
            return string.IsNullOrWhiteSpace(query) ?
                users :
                users.Where(u => u.FullName.Split(' ').Any(n => n.StartsWith(query, StringComparison.CurrentCultureIgnoreCase)));
        }

        private MatchedUser CreateMatchedUser(string query, User arg)
        {
            var parts = arg.FullName.Split(' ');
            var nameParts = parts.Select(part => new NamePart()
            {
                Text = part,
                MatchedSymbolsCount = !string.IsNullOrWhiteSpace(query) && part.StartsWith(query, StringComparison.CurrentCultureIgnoreCase) ? (uint)query.Length : 0
            }).ToList();

            var matchedUser = new MatchedUser
            {
                UserId = arg.Id,
                NameParts = nameParts
            };

            return matchedUser;
        }
    }
}
