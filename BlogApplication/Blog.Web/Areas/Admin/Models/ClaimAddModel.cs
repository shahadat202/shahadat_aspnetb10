﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Web.Areas.Admin.Models
{
    public class ClaimAddModel
    {
        public Guid UserId { get; set; }
        public SelectList? Users { get; set; }
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }
    }
}