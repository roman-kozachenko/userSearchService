﻿using System.Collections.Generic;
using System.Linq;
using UserSearchService.Models;

namespace UserSearchService.Services
{
    public class MockUserService : IUserSearchService
    {
        private List<User> Users => new List<User>
        {
            new User() {Id = 0, FullName = "Roman0 0Kozachenko"},
            new User() {Id = 1, FullName = "Roman1 1Kozachenko"},
            new User() {Id = 2, FullName = "Roman2 2Kozachenko"},
            new User() {Id = 3, FullName = "Roman3 3Kozachenko"},
            new User() {Id = 4, FullName = "Roman4 4Kozachenko"},
            new User() {Id = 5, FullName = "Roman5 5Kozachenko"},
            new User() {Id = 6, FullName = "Roman6 6Kozachenko"},
            new User() {Id = 7, FullName = "Roman7 7Kozachenko"},
            new User() {Id = 8, FullName = "Roman8 8Kozachenko"},
            new User() {Id = 9, FullName = "Roman9 9Kozachenko"},
            new User() {Id = 10, FullName = "Roman10 10Kozachenko"},
            new User() {Id = 11, FullName = "Roman11 11Kozachenko"},
            new User() {Id = 12, FullName = "Roman12 12Kozachenko"},
            new User() {Id = 13, FullName = "Roman13 13Kozachenko"},
            new User() {Id = 14, FullName = "Roman14 14Kozachenko"},
            new User() {Id = 15, FullName = "Roman15 15Kozachenko"},
            new User() {Id = 16, FullName = "Roman16 16Kozachenko"},
            new User() {Id = 17, FullName = "Roman17 17Kozachenko"},
            new User() {Id = 18, FullName = "Roman18 18Kozachenko"},
            new User() {Id = 19, FullName = "Roman19 19Kozachenko"},
            new User() {Id = 20, FullName = "Roman20 20Kozachenko"},
            new User() {Id = 21, FullName = "Roman21 21Kozachenko"},
            new User() {Id = 22, FullName = "Roman22 22Kozachenko"},
            new User() {Id = 23, FullName = "Roman23 23Kozachenko"},
            new User() {Id = 24, FullName = "Roman24 24Kozachenko"},
            new User() {Id = 25, FullName = "Roman25 25Kozachenko"},
            new User() {Id = 26, FullName = "Roman26 26Kozachenko"},
            new User() {Id = 27, FullName = "Roman27 27Kozachenko"},
            new User() {Id = 28, FullName = "Roman28 28Kozachenko"},
            new User() {Id = 29, FullName = "Roman29 29Kozachenko"},
            new User() {Id = 30, FullName = "Roman30 30Kozachenko"},
            new User() {Id = 31, FullName = "Roman31 31Kozachenko"},
            new User() {Id = 32, FullName = "Roman32 32Kozachenko"},
            new User() {Id = 33, FullName = "Roman33 33Kozachenko"},
            new User() {Id = 34, FullName = "Roman34 34Kozachenko"},
            new User() {Id = 35, FullName = "Roman35 35Kozachenko"},
            new User() {Id = 36, FullName = "Roman36 36Kozachenko"},
            new User() {Id = 37, FullName = "Roman37 37Kozachenko"},
            new User() {Id = 38, FullName = "Roman38 38Kozachenko"},
            new User() {Id = 39, FullName = "Roman39 39Kozachenko"},
            new User() {Id = 40, FullName = "Roman40 40Kozachenko"},
            new User() {Id = 41, FullName = "Roman41 41Kozachenko"},
            new User() {Id = 42, FullName = "Roman42 42Kozachenko"},
            new User() {Id = 43, FullName = "Roman43 43Kozachenko"},
            new User() {Id = 44, FullName = "Roman44 44Kozachenko"},
            new User() {Id = 45, FullName = "Roman45 45Kozachenko"},
            new User() {Id = 46, FullName = "Roman46 46Kozachenko"},
            new User() {Id = 47, FullName = "Roman47 47Kozachenko"},
            new User() {Id = 48, FullName = "Roman48 48Kozachenko"},
            new User() {Id = 49, FullName = "Roman49 49Kozachenko"},
            new User() {Id = 50, FullName = "Roman50 50Kozachenko"},
            new User() {Id = 51, FullName = "Roman51 51Kozachenko"},
            new User() {Id = 52, FullName = "Roman52 52Kozachenko"},
            new User() {Id = 53, FullName = "Roman53 53Kozachenko"},
            new User() {Id = 54, FullName = "Roman54 54Kozachenko"},
            new User() {Id = 55, FullName = "Roman55 55Kozachenko"},
            new User() {Id = 56, FullName = "Roman56 56Kozachenko"},
            new User() {Id = 57, FullName = "Roman57 57Kozachenko"},
            new User() {Id = 58, FullName = "Roman58 58Kozachenko"},
            new User() {Id = 59, FullName = "Roman59 59Kozachenko"},
            new User() {Id = 100, FullName = "Roman100 100Kozachenko"},
            new User() {Id = 101, FullName = "Roman101 101Kozachenko"},
            new User() {Id = 102, FullName = "Roman102 102Kozachenko"},
            new User() {Id = 103, FullName = "Roman103 103Kozachenko"},
            new User() {Id = 104, FullName = "Roman104 104Kozachenko"},
            new User() {Id = 105, FullName = "Roman105 105Kozachenko"},
            new User() {Id = 106, FullName = "Roman106 106Kozachenko"},
            new User() {Id = 107, FullName = "Roman107 107Kozachenko"},
            new User() {Id = 108, FullName = "Roman108 108Kozachenko"},
            new User() {Id = 109, FullName = "Roman109 109Kozachenko"},
            new User() {Id = 110, FullName = "Roman110 110Kozachenko"},
            new User() {Id = 111, FullName = "Roman111 111Kozachenko"},
            new User() {Id = 112, FullName = "Roman112 112Kozachenko"},
            new User() {Id = 113, FullName = "Roman113 113Kozachenko"},
            new User() {Id = 114, FullName = "Roman114 114Kozachenko"},
            new User() {Id = 115, FullName = "Roman115 115Kozachenko"},
            new User() {Id = 116, FullName = "Roman116 116Kozachenko"},
            new User() {Id = 117, FullName = "Roman117 117Kozachenko"},
            new User() {Id = 118, FullName = "Roman118 118Kozachenko"},
            new User() {Id = 119, FullName = "Roman119 119Kozachenko"},
            new User() {Id = 120, FullName = "Roman120 120Kozachenko"},
            new User() {Id = 121, FullName = "Roman121 121Kozachenko"},
            new User() {Id = 122, FullName = "Roman122 122Kozachenko"},
            new User() {Id = 123, FullName = "Roman123 123Kozachenko"},
            new User() {Id = 124, FullName = "Roman124 124Kozachenko"},
            new User() {Id = 125, FullName = "Roman125 125Kozachenko"},
            new User() {Id = 126, FullName = "Roman126 126Kozachenko"},
            new User() {Id = 127, FullName = "Roman127 127Kozachenko"},
            new User() {Id = 128, FullName = "Roman128 128Kozachenko"},
            new User() {Id = 129, FullName = "Roman129 129Kozachenko"},
            new User() {Id = 130, FullName = "Roman130 130Kozachenko"},
            new User() {Id = 131, FullName = "Roman131 131Kozachenko"},
            new User() {Id = 132, FullName = "Roman132 132Kozachenko"},
            new User() {Id = 133, FullName = "Roman133 133Kozachenko"},
            new User() {Id = 134, FullName = "Roman134 134Kozachenko"},
            new User() {Id = 135, FullName = "Roman135 135Kozachenko"},
            new User() {Id = 136, FullName = "Roman136 136Kozachenko"},
            new User() {Id = 137, FullName = "Roman137 137Kozachenko"},
            new User() {Id = 138, FullName = "Roman138 138Kozachenko"},
            new User() {Id = 139, FullName = "Roman139 139Kozachenko"},
            new User() {Id = 140, FullName = "Roman140 140Kozachenko"},
            new User() {Id = 141, FullName = "Roman141 141Kozachenko"},
            new User() {Id = 142, FullName = "Roman142 142Kozachenko"},
            new User() {Id = 143, FullName = "Roman143 143Kozachenko"},
            new User() {Id = 144, FullName = "Roman144 144Kozachenko"},
            new User() {Id = 145, FullName = "Roman145 145Kozachenko"},
            new User() {Id = 146, FullName = "Roman146 146Kozachenko"},
            new User() {Id = 147, FullName = "Roman147 147Kozachenko"},
            new User() {Id = 148, FullName = "Roman148 148Kozachenko"},
            new User() {Id = 149, FullName = "Roman149 149Kozachenko"},
            new User() {Id = 150, FullName = "Roman150 150Kozachenko"},
            new User() {Id = 151, FullName = "Roman151 151Kozachenko"},
            new User() {Id = 152, FullName = "Roman152 152Kozachenko"},
            new User() {Id = 153, FullName = "Roman153 153Kozachenko"},
            new User() {Id = 154, FullName = "Roman154 154Kozachenko"},
            new User() {Id = 155, FullName = "Roman155 155Kozachenko"},
            new User() {Id = 156, FullName = "Roman156 156Kozachenko"},
            new User() {Id = 157, FullName = "Roman157 157Kozachenko"},
            new User() {Id = 158, FullName = "Roman158 158Kozachenko"},
            new User() {Id = 159, FullName = "Roman159 159Kozachenko"},
        };

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public bool RemoveUser(User user)
        {
            return Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            var existing = Users.Single(u => u.Id == user.Id);

            existing.FullName = user.FullName;
        }

        public PaginationResult<User> SearchUser(string query, int skip, int take)
        {
            var filteredUsers = FilterUsers(query, Users);
            var data = filteredUsers.Skip(skip).Take(take);
            var result = new PaginationResult<User>()
            {
                Data = data.ToList(),
                Length = data.Count(),
                Start = skip
            };

            return result;
        }


        private static IEnumerable<User> FilterUsers(string query, IEnumerable<User> users)
        {
            return string.IsNullOrWhiteSpace(query) ? users : users.Where(u => u.FullName.Split(' ').Any(n => n.StartsWith(query)));
        }
    }
}
