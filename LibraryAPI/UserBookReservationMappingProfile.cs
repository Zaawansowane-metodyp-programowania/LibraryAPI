using AutoMapper;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI
{
    public class UserBookReservationMappingProfile : Profile
    {
        public UserBookReservationMappingProfile()
        {
            CreateMap<UserBookReservation, UserBookReservationDto>()
            .ForMember(m => m.BookName, c => c.MapFrom(s => s.Book.BookName));
        }
    }
}
