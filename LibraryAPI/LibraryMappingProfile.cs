﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Models;
using AutoMapper;
using LibraryAPI.Dtos;

namespace LibraryAPI
{
    public class LibraryMappingProfile:Profile
    {
        public LibraryMappingProfile()
        {
            CreateMap<Books, BooksDto>();

            CreateMap<CreateBookDto, Books > ();
            



        }
    }
}
