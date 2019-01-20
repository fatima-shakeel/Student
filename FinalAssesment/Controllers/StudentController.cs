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
            return View();
        }
    }
}