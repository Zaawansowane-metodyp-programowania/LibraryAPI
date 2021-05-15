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
            CreateMap<UserBookReservation, AllBooksReservedByUserDto>()
            .ForMember(m => m.BookName, c => c.MapFrom(s => s.Book.BookName));

            CreateMap<UserBookReservation, AllUserForReservedBookDto>()
            .ForMember(m => m.Name, c => c.MapFrom(s => s.User.Name))
            .ForMember(m => m.Surname, c => c.MapFrom(s => s.User.Surname))
            .ForMember(m => m.BookName, c => c.MapFrom(s => s.Book.BookName));
        }
    }
}


//