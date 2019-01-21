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

        public string GetStudentsNames()
        {
            string Result = "";

            var r = Request;
            
            IList<Student> All = _ORM.Student.ToList<Student>();
            Result += "<h1 class='alert alert-success'>Total Students: " + All.Count + "</h1>";

            foreach (Student S in All)
            {
                Result += "<a href='/Student/StudentDetail?Id=" + S.Id + "'><p><span class='glyphicon glyphicon-user'></span> " + S.FirstName + "</p></a> <a href='/student/deletestudent1?id=" + S.Id + "'>Delete</a>";
            }

            return Result;
        }

        public string ShowAd()
        {
            string Ad = "";
            Ad = "<img class='img img-responsive' src='http://lorempixel.com/400/400/sports/Theta-Solutions/'/>";
            return Ad;
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

        public IActionResult DeleteStudent(Student student)
        {
            _ORM.Student.Remove(student);
            _ORM.SaveChanges();
            return RedirectToAction(nameof(StudentController.AllStudents));
        }

        public IActionResult DeleteStudent1(Student student)
        {
            _ORM.Student.Remove(student);
            _ORM.SaveChanges();
            return RedirectToAction(nameof(StudentController.AllStudents));
        }

        public string DeleteStudentByAjax(Student s)
        {
            string result;
            try
            {
                _ORM.Student.Remove(s);
                _ORM.SaveChanges();
                result = "Yes";
            }
            catch
            {
                result = "No";
            }
            return result;
        }

    }
}