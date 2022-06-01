using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public StudentController(IStudentRepository studentRepository, IMapper mapper,IImageRepository imageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
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
        [Route("[controller]/{studentId:guid}"),ActionName("GetStudentAsync")]
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


        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId))
            {
                //Update Details
                var updatedStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));
                if (updatedStudent != null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }
            }

            return NotFound();

        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
                var student = await this.studentRepository.DeleteStudent(studentId);
                return Ok(mapper.Map<Student>(student));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/add")]
        public async Task<IActionResult> AddStudentAsync([FromBody]AddStudentRequest request)
        {
          var student = await  studentRepository.AddStudent(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = student.Id },
   
            mapper.Map<Student>(student));

        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid studentId, IFormFile profileImage)
        {
            //check if student exists
            if (await studentRepository.Exists(studentId))
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
                //Upload the image to local storage
                var fileImagePath = await imageRepository.Upload(profileImage, fileName);

                //update the profile image path in the database

               if( await studentRepository.UpdateProfileImage(studentId, fileImagePath))
                {
                    return Ok(fileImagePath);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");

            }

            return NotFound();


        }



    }

}
