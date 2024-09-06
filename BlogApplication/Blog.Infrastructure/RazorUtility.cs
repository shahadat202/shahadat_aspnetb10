using Blog.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
    public class RazorUtility
    {
        public static IList<SelectListItem> ConvertCategories(IList<Category> categories)
        {
            var items = (from c in categories
                         select new SelectListItem(c.Name, c.Id.ToString()))
                          .ToList();

            items.Insert(0, new SelectListItem("Select a Category", string.Empty));

            return items;
        }
    }
}
