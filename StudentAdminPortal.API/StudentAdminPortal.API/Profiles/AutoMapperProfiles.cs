using AutoMapper;
using DataModels = StudentAdminPortal.API.DataModels;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Profiles.AfterMaps;

namespace StudentAdminPortal.API.Profiles
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, DataModels.Student>().ReverseMap();
            CreateMap<Gender, DataModels.Gender>().ReverseMap();
            CreateMap<Address, DataModels.Address>().ReverseMap();
            CreateMap<UpdateStudentRequest, DataModels.Student>().AfterMap<UpdateStudentRequestAfterMap>();
            CreateMap<AddStudentRequest, DataModels.Student>().AfterMap<AddStudentRequestAfterMap>();
        }
    }
}
