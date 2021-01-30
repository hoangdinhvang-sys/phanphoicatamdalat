using AutoMapper;
using DotNetS.Models;
using DotNetS.Models.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetS.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, RegisterVMModel>();
        }
    }
}