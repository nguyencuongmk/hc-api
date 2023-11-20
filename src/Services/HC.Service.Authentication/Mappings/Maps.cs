using AutoMapper;
using HC.Service.Authentication.Models.DTOs;
using HC.Service.Authentication.Models.Requests;

namespace HC.Service.Authentication.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<RegisterRequest, RegisterDTO>();
        }
    }
}
