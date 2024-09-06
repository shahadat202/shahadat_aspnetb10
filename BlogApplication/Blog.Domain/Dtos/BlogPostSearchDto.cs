﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Dtos
{
    public class BlogPostSearchDto
    {
        public string? Title { get; set; }
        public string? CreateDateFrom { get; set; }
        public string? CreateDateTo { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
