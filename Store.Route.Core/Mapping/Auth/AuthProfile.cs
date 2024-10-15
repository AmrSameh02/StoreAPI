using AutoMapper;
using Store.Route.Core.DTOs.Auth;
using Store.Route.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Mapping.Auth
{
    public class AuthProfile : Profile
    {

        public AuthProfile()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
        }

    }
}
