﻿namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public string TitleSlug { get; set; }
        public string TagSlug { get; set; }
        public string AuthorSlug { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public bool PublishedOnly { get; set; } = true;
        public bool NotPublished { get; set; } = false;
        public string Tag { get; set; }
        public string KeyWord { get; set; }
    }
}
