using AutoMapper;
using Lavadoras.API.Requests;
using Lavadoras.Domain.Entities;


namespace Lavadoras.API;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<CreateOperatorRequest, User>();
    }
}
