using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BPITest.DB;
using BPITest.Models;
using Microsoft.AspNetCore.Mvc;

namespace BPITest.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyDbContext _db;
        public UsersController(MyDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string searchString)
        {
            var users = from s in _db.AllUsers
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.ToString().Contains(searchString));
            }

            return View(users.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.AllUsers.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.AllUsers.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User obj)
        {
            if (ModelState.IsValid)
            {
                _db.AllUsers.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.AllUsers.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.AllUsers.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
