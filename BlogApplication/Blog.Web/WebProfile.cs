using AutoMapper;
using Blog.Domain.Entities;
using Blog.Web.Areas.Admin.Models;

namespace Blog.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<BlogPostCreateModel, BlogPost>().ReverseMap();
            CreateMap<BlogPostUpdateModel, BlogPost>()
                .ForMember(x => x.PostDate, opt => opt.Ignore()).ReverseMap();
        }
    }
}
