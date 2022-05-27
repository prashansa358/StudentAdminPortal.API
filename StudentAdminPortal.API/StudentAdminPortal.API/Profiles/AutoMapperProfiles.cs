
using AutoMapper;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, DataModels.Student>().ReverseMap();
            CreateMap<Gender, DataModels.Gender>().ReverseMap();
            CreateMap<Address, DataModels.Address>().ReverseMap();
        }
    }
}
