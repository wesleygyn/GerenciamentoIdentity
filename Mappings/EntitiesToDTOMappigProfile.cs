using AutoMapper;
using GerenciamentoIdentity.Models;
using GerenciamentoIdentity.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoIdentity.Mappings
{
    public class EntitiesToDTOMappigProfile : Profile
    {
        public EntitiesToDTOMappigProfile()
        {
            CreateMap<Usuario, UserManagerViewModel>();

        }
    }
}
