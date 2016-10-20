using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Services;
using Services.Models;
using Tarantool.Client;
using Tarantool.Client.Model;
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

            using (var box = CreateConnectedBox().GetAwaiter().GetResult())
            {
                using (var userService = new TarantoolUserSearchService(box))
                {
                    InsertUsers(names, random, userService).GetAwaiter().GetResult();
                }
            }
        }

        private static async Task InsertUsers(Names names, Random random, IUserSearchService userService)
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

        private static async Task<Box> CreateConnectedBox()
        {
            var addresses = await Dns.GetHostAddressesAsync("localhost");
            var box = new Box(new ConnectionOptions
            {
                EndPoint = new IPEndPoint(addresses.First(x => x.AddressFamily == AddressFamily.InterNetwork), 3301),
                GuestMode = false,
                UserName = "admin",
                Password = "password"
            });

            await box.Connect();

            return box;
        }
    }
}
