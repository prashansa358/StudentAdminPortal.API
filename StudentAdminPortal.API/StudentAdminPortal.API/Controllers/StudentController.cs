using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        public StudentController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentRepository.GetStudentsAsync();
            //with automapper
            // mapper.Map<List<Student>>(students);
            //without automapper
            //var domainModelStudents = new List<Student>();
            //foreach (var student in students)
            //{
            //    domainModelStudents.Add(new Student()
            //    {
            //        Id = student.Id,
            //        FirstName = student.FirstName,
            //        LastName = student.LastName,
            //        DateOfBirth = student.DateOfBirth,
            //        Email = student.Email,
            //        Mobile = student.Mobile,
            //        ProfileImageUrl = student.ProfileImageUrl,
            //        GenderId = student.GenderId,
            //        Address = new Address()
            //        { 
            //            Id = student.Address.Id,
            //            PhysicalAddress = student.Address.PhysicalAddress,
            //            PostalAddress = student.Address.PostalAddress,
            //        },
            //        Gender = new Gender()
            //        {
            //            Id = student.Gender.Id,
            //            Description = student.Gender.Description,
            //        }
            //    });
            //}
            //return Ok(domainModelStudents);

            return Ok(mapper.Map<List<Student>>(students));
        }

       [HttpGet]
       [Route("[controller]/{studentId:guid}")]
       public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            //Fetch Students Details

            var student = await studentRepository.GetStudentAsync(studentId);

            //Return Student
            if (student == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Student>(student));
        }
    }
}
