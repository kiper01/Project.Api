﻿using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using Project.Core.Entities;
using Project.Core.Models.Dto;
using Project.Core.Models.Dto.Programs;
using Project.Core.Models.Dto.User;
using System.Linq;

namespace Project.Api
{
    internal class MapProfile : Profile
    {

        public MapProfile(IConfiguration configuration)
        {
            ConfigureUser();
            ConfigurePrograms();
            //ConfigureItem();
        }
        private void ConfigureUser()
        {
            CreateMap<User, DtoUser>();
            CreateMap<DtoUser, User>();

            CreateMap<User, DtoEditUser>();
            CreateMap<DtoEditUser, User>();

            CreateMap<User, DtoLogUser>();
            CreateMap<DtoLogUser, User>();

            CreateMap<User, DtoClaimUser>();
            CreateMap<DtoClaimUser, User>();
        }
        private void ConfigurePrograms()
        {
            CreateMap<Programs, DtoGetProgramResponse>();
            CreateMap<DtoGetProgramResponse, Programs>();
        }

        /* private void ConfigureItem()
         {
             CreateMap<Item, DtoItem>();
             CreateMap<DtoItem, Item>();
         } */

    }

}