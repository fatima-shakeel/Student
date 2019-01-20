using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalAssesment.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssesment.Controllers
{
    public class StudentController : Controller
    {
        private studentContext _ORM;
        public StudentController(studentContext ORM)
        {
            _ORM = ORM;
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student S)
        {
            _ORM.Student.Add(S);
            _ORM.SaveChanges();
            return View();
        }


        public IActionResult AllStudents()
        {
            IList<Student> AllStudents = _ORM.Student.ToList<Student>();
            return View(AllStudents);
        }

        public IActionResult StudentDetail(int Id)
        {
            return View(_ORM.Student.Where(m => m.Id.Equals(Id)).FirstOrDefault());
            
        }

        [HttpGet]
        public IActionResult EditStudent(int Id)
        {
            Student student = _ORM.Student.Where(m => m.Id.Equals(Id)).FirstOrDefault();
            return View(student);
        }

        [HttpPost]
        public IActionResult EditStudent(Student student)
        {
            try
            {
                _ORM.Student.Update(student);
                _ORM.SaveChanges();
                ViewBag.MessageSucess = "Record Updated Succefully";
            }
            catch
            {
                ViewBag.MessageFailure = "Error! Could not Update Record";
            }
            return View(student);
        }

    }
}