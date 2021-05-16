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
            .ForMember(m => m.BookName, c => c.MapFrom(s => s.Book.BookName))
            .ForMember(m => m.AuthorName, c => c.MapFrom(s => s.Book.AuthorName))
            .ForMember(m => m.PublisherName, c => c.MapFrom(s => s.Book.PublisherName))
            .ForMember(m => m.PublishDate, c => c.MapFrom(s => s.Book.PublishDate))
            .ForMember(m => m.Category, c => c.MapFrom(s => s.Book.Category))
            .ForMember(m => m.Language, c => c.MapFrom(s => s.Book.Language))
            .ForMember(m => m.BookDescription, c => c.MapFrom(s => s.Book.BookDescription));

            CreateMap<UserBookReservation, AllUserForReservedBookDto>()
            .ForMember(m => m.Name, c => c.MapFrom(s => s.User.Name))
            .ForMember(m => m.Surname, c => c.MapFrom(s => s.User.Surname))
            .ForMember(m => m.BookName, c => c.MapFrom(s => s.Book.BookName));
        }
    }
}


