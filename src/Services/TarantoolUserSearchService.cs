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
            var matchingEntries = await _box.Call<Tuple<uint, string, uint, uint>,
                Tuple<uint, string, uint>>(
                TarantoolFunctionNames.SearchUsersFunctionName, Tuple.Create(channelId, query, skip, take));
            var users = matchingEntries.Data.GroupBy(t => t.Item1).Select(t => CreateMatchedUser(t, query)).ToList();
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

        private MatchedUser CreateMatchedUser(IGrouping<uint, Tuple<uint, string, uint>> matches, string query)
        {
            var queryLength = (uint)query.Length;
            var userId = matches.Key;
            var fullName = matches.First().Item2;
            var parts = fullName.Split(' ');

            var nameParts = new List<NamePart>();

            for (var i = 0; i < parts.Length; i++)
            {
                var matchedPart = matches.SingleOrDefault(m => m.Item3 == i + 1);
                var namePart = new NamePart()
                {
                    Text = parts[i],
                    MatchedSymbolsCount = matchedPart == null ? 0 : queryLength
                };

                nameParts.Add(namePart);
            }
            var matchedUser = new MatchedUser()
            {
                UserId = userId,
                NameParts = nameParts
            };

            return matchedUser;
        }
    }
}