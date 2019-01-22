using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using FinalAssesment.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalAssesment.Controllers
{
    public class StudentController : Controller
    {
        private studentContext _ORM;
        IHostingEnvironment _ENV = null;
        public StudentController(studentContext ORM, IHostingEnvironment ENV)
        {
            _ORM = ORM;
            _ENV = ENV;
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(Student S, IFormFile ProfilePicture, IFormFile Cv)
        {
            string wwwRootPath = _ENV.WebRootPath;
            string FTPPathForPPs = wwwRootPath + "/WebData/PPs/";

            //if (!Directory.Exists(FTPPathForPPs))
            //{
            //    Directory.CreateDirectory(FTPPathForPPs);
            //}


            string UniqueName = Guid.NewGuid().ToString();
            string FileExtension = Path.GetExtension(ProfilePicture.FileName);

            FileStream FS = new FileStream(FTPPathForPPs + UniqueName + FileExtension, FileMode.Create);

            ProfilePicture.CopyTo(FS);






            FS.Close();

            S.ProfilePicture = "/WebData/PPs/" + UniqueName + FileExtension;

            string CVPath = "/WebData/CVs/" + Guid.NewGuid().ToString() + Path.GetExtension(Cv.FileName);
            FileStream CVS = new FileStream(wwwRootPath + CVPath, FileMode.Create);
            Cv.CopyTo(CVS);
            CVS.Close();
            S.Cv = CVPath;

            try
            {
                _ORM.Student.Add(S);
                _ORM.SaveChanges();

                string APIURL = "https://bulksms.vsms.net/eapi/submission/send_sms/2/2.0?username=john&password=abcd1234&message=Hi+Mom+%26+Dad&msisdn=44123123456,44123123457" + S.MobileNo + "&message=Welcome to our website.";

                using (var APIClient = new HttpClient())
                {
                    Task<HttpResponseMessage> RM = APIClient.GetAsync(APIURL);
                    Task<string> FinalRespone = RM.Result.Content.ReadAsStringAsync();
                }



                ViewBag.Message = "Registration Done Succefully!";
                ModelState.Clear();

                //Email object
                MailMessage oEmail = new MailMessage();
                oEmail.From = new MailAddress("dazzlinglove98@gmail.com");
                oEmail.To.Add(new MailAddress(S.Email));
                oEmail.Subject = "Welcome to ABC";
                oEmail.Body = "Dear " + S.FirstName + ",<br><br>" +
                    "Thanks for registering with ABC, We are glad to have you in our system." +
                    "<br><br>" +
                    "<b>Regards</b>,<br>ABC Team";
                oEmail.IsBodyHtml = true;
                SmtpClient oSMTP = new SmtpClient();
                oSMTP.Host = "smtp.gmail.com";
                oSMTP.Port = 587; //465 //25
                oSMTP.EnableSsl = true;
                oSMTP.Credentials = new System.Net.NetworkCredential("dazzlinglove98@gmail.com", "fatimashakeel");

            }
            catch (Exception ex)
            {
                
            }
           

            

            //
            return View();
        }

        /*public FileResult DownloadCV(string Path)
        {
            if (string.IsNullOrEmpty(Path))
            {
                ViewBag.Message = "Invalid Path";
                return null;
            }
            return File(Path, new MimeSharp.Mime().Lookup(Path), DateTime.Now.ToString("ddMMyyyyhhmmss") + System.IO.Path.GetExtension(Path));
        }*/


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