using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSAEDU.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RSAEDU.ViewModel;

namespace RSAEDU.Controllers
{
    [Authorize]
    public class CreateUserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
          public CreateUserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

       public CreateUserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        //
        // GET: /CreateUser/
        public ActionResult Index(string UserType,string UserId)
        {
            List<UserSubject> list = new List<UserSubject>();

            list = (from us in db.UserSubjects.ToList()
                    join user in db.Users.ToList() on us.UserId equals user.Id
                    join fac in db.FacultyInfoes.ToList() on us.FacultyId equals fac.Id
                    join clas in db.ClassInfoes.ToList() on us.ClassId equals clas.Id
                    join sub in db.SubjectInfoes.ToList() on us.SubjectId equals sub.Id
                    select new UserSubject
                    {                        
                        UserName = user.UserName,
                        UserId = user.Id,
                        SubjectName = sub.SubjectName,
                        ClassName = clas.ClassName,
                        ManualCode = us.ManualCode,
                        FacultyName = fac.FacultyName                        

                    }).ToList();

            if (User.Identity.Name == "Admin" || User.Identity.Name == "System")
            {

            }
            else
            {
                list = list.Where(t => t.UserName == User.Identity.Name).ToList();
            }
                //(from e in db.UserSubjects)

            return View(list);
        }





        public ActionResult CreateTeacher() 
        {

            if (Convert.ToString(TempData["Success"]) != null && !string.IsNullOrEmpty(Convert.ToString(TempData["Success"])))
            {
                 ViewBag.message = Convert.ToString(TempData["Success"]) + " Want to add more."; 
            }else
             ViewBag.message ="Create a new Teacher User";
       
            
            ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
            ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");       
            return View("CreateTeacher");
        
        }

        [HttpPost]
        public ActionResult CreateTeacher(UserModel model) 
        { 
            try{
            if (ModelState.IsValid)
                {
                   
                        model.UserType = "T"; //teacher
            
                       // var HasUser = db.Users.Where(t => t.UserName == model.UserName).SingleOrDefault();


                           
                            var user = new ApplicationUser() { UserName = model.UserName, UserType = model.UserType };
                            var result = UserManager.Create(user, model.Password);
                            if (result.Succeeded)
                            {
                                //find user Id

                                var UserId = db.Users.Where(t => t.UserName == model.UserName).SingleOrDefault();

                                foreach (var Item in model.Subjects)
                                {

                                    if (Item.IsCheck == "on")
                                    {
                                        UserSubject sub = new UserSubject();

                                        sub.EntryBy = User.Identity.Name;
                                        sub.EntryDate = DateTime.Now;
                                        sub.ClassId = model.ClassId;
                                        sub.FacultyId = model.FacultyId;
                                        sub.ManualCode = Item.ManualCode;
                                        sub.SubjectId = Item.Id;
                                        sub.UserId = UserId.Id;

                                        db.UserSubjects.Add(sub);
                                    }
                                }

                                db.SaveChanges();

                                TempData["Success"] = "User Create Success with Subject";
                                return RedirectToAction("CreateTeacher");
                            }
                            else
                            {
                                AddErrors(result);
                            }   
                     }

            ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
            ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");
            return View(model);
        
            }
            catch (Exception ex) {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
       
        }


        //Add Subject To Teacher
        [HttpPost]
        public ActionResult AddSubject(UserModelAddSubject model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                 

                    var HasUser = db.Users.Where(t => t.Id == model.UserName).SingleOrDefault();


                    if (HasUser != null)
                    {
                        UserSubject userModel = db.UserSubjects.Where(t => t.SubjectId == model.SubjectId && t.ClassId == model.ClassId && t.FacultyId == model.FacultyId && t.UserId == model.UserName).SingleOrDefault();

                        if (userModel != null)
                        {
                            TempData["Success"] = "Subject allready assigned !";  
                        }
                        else
                        {
                            UserSubject sub = new UserSubject();

                            sub.EntryBy = User.Identity.Name;
                            sub.EntryDate = DateTime.Now;
                            sub.ClassId = model.ClassId;
                            sub.FacultyId = model.FacultyId;
                            sub.ManualCode = db.SubjectInfoes.Find(model.SubjectId).ManualCode;
                            sub.SubjectId = model.SubjectId;
                            sub.UserId = HasUser.Id;

                            db.UserSubjects.Add(sub);
                            db.SaveChanges();
                            TempData["Success"] = "Subject Is successfully assigned !";
                            return RedirectToAction("Index", "CreateUser", new { UserType = "T" });
                 
                        }
                    }
                    else {
                        TempData["Success"] = "User is not found!";  
                    }

                                          
                   
                }

                ViewBag.TeacherList = new SelectList(db.Users.ToList(), "Id", "UserName");
                ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
                ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");
                return View(model);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
       
           
        }

        public ActionResult AddSubject() 
        {
            ViewBag.Class_Id = new SelectList(db.ClassInfoes.ToList(), "Id", "ClassName");
            ViewBag.Faculty_Id = new SelectList(db.FacultyInfoes.ToList(), "Id", "FacultyName");

            ViewBag.TeacherList = new SelectList(db.Users.Where(t=>t.UserType =="T").ToList(), "Id", "UserName");
            return View();
        }

        public ActionResult CreateUser() 
        {

            ViewBag.UserType = new SelectList(UserType.getUserType().ToList(),"Id","Name");
            return View("CreateUser");
        }


        [HttpPost]
        public ActionResult CreateUser(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var allStudent = db.StudentInfoes.ToList();
                    foreach (var item in allStudent)
                    {

                        string Roll =item.RollNo.ToString();
                        model.UserName = Convert.ToString(item.RollNo);

                        if (Roll.ToString().Length < 6) {
                            int count = 6 - item.RollNo.ToString().Length;

                            for (int i = 0; i < count; i++)
                                Roll += "0";
                        }
                        model.Password = Convert.ToString(Roll);
                        model.UserType = "S";

                        var user = new ApplicationUser() { UserName = model.UserName, UserType = model.UserType };
                        var result = UserManager.Create(user, model.Password);
                        if (result.Succeeded)
                        {
                           // return RedirectToAction("Index", "CreateUser");
                        }
                        else
                        {
                            AddErrors(result);
                        }
                    }
                }

                ViewBag.UserType = new SelectList(UserType.getUserType(), "Id", "Name");
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex) {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
	}

    public class UserType {

        public string Id { get; set; }
        public string Name { get; set; }
        public static List<UserType> getUserType() {

            List<UserType> type = new List<UserType>();
            type.Add(new UserType { Id="S",Name="Student" });
            type.Add(new UserType { Id = "T", Name = "Teacher" });
            type.Add(new UserType { Id = "Sff", Name = "Stuff" });

            return type;
        }
    }

}