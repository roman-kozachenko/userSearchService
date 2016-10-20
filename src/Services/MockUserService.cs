using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Services.Models;

namespace Services
{
    public class MockUserService : IUserSearchService
    {
        private readonly List<User> _users = new List<User>();
        private const string FileName = "data.xml";
        private readonly bool _saveData;

        public MockUserService(bool saveData = false)
        {
            _saveData = saveData;
            if (!File.Exists(FileName))
            {
                return;
            }

            using (var file = File.OpenRead(FileName))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(List<User>));
                    _users = (List<User>)serializer.Deserialize(file);
                }
                catch (Exception)
                {
                }
            }
        }

        public async Task ReplaceUser(uint channelId, User user)
        {
            await Task.Yield();

            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.FullName = user.FullName;
            }
            else
            {
                _users.Add(user);
            }
        }

        public async Task RemoveUser(uint channelId, uint userId)
        {
            await Task.Yield();

            var existing = _users.First(u => u.Id == userId);
            if (existing != null)
            {
                _users.Remove(existing);
            }
        }

        public async Task<PaginationResult<MatchedUser>> SearchUser(uint channelId, string query, uint skip, uint take)
        {
            await Task.Yield();

            var filteredUsers = FilterUsers(_users, query);
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

        public void Dispose()
        {
            if (_saveData)
            {
                using (var file = File.Create(FileName))
                {
                    var serializer = new XmlSerializer(typeof(List<User>));
                    serializer.Serialize(file, _users);
                }
            }
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
