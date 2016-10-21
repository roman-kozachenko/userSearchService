// <copyright file="TarantoolUserSearchService.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Models;
using Tarantool.Client;
using Tarantool.Client.Model;

namespace Services
{
    public class TarantoolUserSearchService : IUserSearchService
    {
        private readonly Box _box;

        public TarantoolUserSearchService(Box box)
        {
            this._box = box;
        }

        public async Task ReplaceUser(uint channelId, User user)
        {
            await _box.Call<Tuple<uint, uint, string>, Tuple<int>>(TarantoolFunctionNames.ReplaceUserFunctionName, Tuple.Create(channelId, user.Id, user.FullName));
        }

        public async Task RemoveUser(uint channelId, uint userId)
        {
            await _box.Call<Tuple<uint, uint>, Tuple<bool>>(TarantoolFunctionNames.RemoveUserFunctionName, Tuple.Create(channelId, userId));
        }

        public async Task<PaginationResult<MatchedUser>> SearchUser(uint channelId, string query, uint skip, uint take)
        {
            query = query ?? string.Empty;
          var matchingEntries = await _box.Call<Tuple<uint, string, uint, uint>,
                Tuple<uint, string, uint[]>>(
                TarantoolFunctionNames.SearchUsersFunctionName, Tuple.Create(channelId, query, skip, take));
            var users = matchingEntries.Data.Select(t => CreateMatchedUser(t, query)).ToList();
            return new PaginationResult<MatchedUser>()
            {
                Data = users,
                Length = take,
                Start = skip
            };
        }

        public void Dispose()
        {
            _box.Dispose();
        }

        private MatchedUser CreateMatchedUser(Tuple<uint, string, uint[]> match, string query)
        {
            var queryLength = (uint)query.Length;
            var userId = match.Item1;
            var fullName = match.Item2;
            var parts = fullName.Split(' ');

            var nameParts = new List<NamePart>();

            for (var i = 0; i < parts.Length; i++)
            {
                var matchFound = match.Item3.Any(m => m == i + 1u);
                var namePart = new NamePart()
                {
                    Text = parts[i],
                    MatchedSymbolsCount = matchFound ? queryLength : 0
                };

                nameParts.Add(namePart);
            }
            var matchedUser = new MatchedUser
            {
                UserId = userId,
                NameParts = nameParts
            };

            return matchedUser;
        }
    }
}