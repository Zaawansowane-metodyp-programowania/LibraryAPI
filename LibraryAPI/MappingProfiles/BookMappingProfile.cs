﻿using LibraryAPI.Models;
using AutoMapper;
using LibraryAPI.Dtos;

namespace LibraryAPI
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookDto>();

            CreateMap<Book, AllBooksUserDto>();

            CreateMap<CreateBookDto, Book>();
        }
    }
}