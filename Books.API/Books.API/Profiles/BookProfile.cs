using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.API.Profiles
{
    public class BookProfile: Profile
    {
        public BookProfile()
        {
            CreateMap<Entities.Book, Models.Book>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(
                    src => $"{src.Author.FirstName}{src.Author.LastName}")); ;
        }
    }
}
