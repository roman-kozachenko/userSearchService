using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Services;
using Services.Models;
using UsersGenerator;

namespace ConsoleApplication
{
    public class Program
    {
        private const int MaxNamesCount = 5;
        private const int UsersCount = 2 * 1000;
        public static void Main(string[] args)
        {
            var names = new Names();
            var random = new Random(DateTime.UtcNow.Millisecond);
            using (var userService = new MockUserService(true))
            {
                InsertUsers(names, random, userService).GetAwaiter().GetResult();
            }
        }

        private static async Task InsertUsers(Names names, Random random, MockUserService userService)
        {
            for (var i = 0u; i < UsersCount; i++)
            {
                var user = GenerateUser(i, names.List, random);
                await userService.ReplaceUser(0, user);
            }
        }

        private static User GenerateUser(uint id, string[] possibleNames, Random random)
        {
            var user = new User
            {
                Id = id
            };

            var names = new string[random.Next(MaxNamesCount) + 1];
            for (var i = 0; i < names.Length; i++)
            {
                names[i] = possibleNames[random.Next(possibleNames.Length)];
            }

            user.FullName = string.Join(" ", names);

            return user;
        }
    }
}
