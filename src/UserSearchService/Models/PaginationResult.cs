// <copyright file="PaginationResult.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System.Collections.Generic;

namespace UserSearchService.Models
{
    public class PaginationResult<T>
    {
        public List<T> Data { get; set; }
        public uint Start { get; set; }
        public uint Length { get; set; }
    }
}