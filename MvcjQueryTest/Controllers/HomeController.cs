using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using MvcjQueryTest.Models;
using Newtonsoft.Json;

namespace MvcjQueryTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if(Session["User"] != null)
            {
                return View();
            }

            else
            {
                return RedirectToAction("Login");
            }
           
            
        }

        public JsonResult GetJobDetails()
        {
            using (jQueryAjaxEntities db = new jQueryAjaxEntities())
            {
                List<jobdetailsDTO> job = db.jobdetails.Select(x => new jobdetailsDTO
                {
                    JobID = x.JobID,
                    Task_Name = x.Task_Name,
                    Description = x.Description,
                    Date_Started = x.Date_Started,
                    Date_Finished = x.Date_Finished,
                    Status = x.Status,
                }).ToList();


                return Json(job, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult AddJob()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddJob(jobdetail job)
        {
            using(jQueryAjaxEntities db = new jQueryAjaxEntities())
            {
                db.jobdetails.Add(job);
                db.SaveChanges();
                return Json("Success");
            }

        }

        public ActionResult Edit(int? id)
        {
            jobdetail job = new jobdetail();
            using (jQueryAjaxEntities db = new jQueryAjaxEntities())
            {
                job = db.jobdetails.Where(x => x.JobID == id).FirstOrDefault<jobdetail>();
            }
            return View(job);
            
        }

        [HttpPost]
        public JsonResult Edit(jobdetail job)
        {
            using(jQueryAjaxEntities db = new jQueryAjaxEntities())
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
            }    
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            using(jQueryAjaxEntities db = new jQueryAjaxEntities())
            {
                jobdetail job = db.jobdetails.Where(x=> x.JobID == id).FirstOrDefault<jobdetail>();
                db.jobdetails.Remove(job);
                db.SaveChanges();
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checker(User u)
        {
            using(userEntities db = new userEntities())
            {

                var userDetails = db.Users.Where(x => x.UserName == u.UserName && x.Password == u.Password).FirstOrDefault();
                if(userDetails ==null)
                {
                    u.LoginError = "Wrong username or password";
                    return View("Login", u);
                }
                else
                {
                    Session["User"] = "LogIn";
                    return RedirectToAction("Index");
                }
                
            }   
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}