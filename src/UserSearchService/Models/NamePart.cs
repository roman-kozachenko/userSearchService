// <copyright file="NamePart.cs" company="eVote">
//   Copyright © eVote
// </copyright>
namespace UserSearchService.Models
{
    public class NamePart
    {
        public string Text { get; set; }
        public uint MatchedSymbolsCount { get; set; }
    }
}