﻿// <copyright file="MatchedUser.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System.Collections.Generic;

namespace UserSearchService.Models
{
    public class MatchedUser
    {
        public List<NamePart> NameParts { get; set; }

        public uint UserId { get; set; }
    }
}