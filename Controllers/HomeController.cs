using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BPITest.DB;
using BPITest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace BPITest.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _db;
        public HomeController(MyDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string idString, string departmentString)
        {
            string connString = "Data Source=ap-ntc2136-sql;Initial Catalog=BPIDBTest;User ID=h4409;Password=510754;TrustServerCertificate=true";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand("SearchData", connection);
                command.CommandType = CommandType.StoredProcedure;

                if (!string.IsNullOrEmpty(idString))
                {
                    command.Parameters.AddWithValue("@ID", idString);
                }

                if (!String.IsNullOrEmpty(departmentString))
                {
                    command.Parameters.AddWithValue("@Department", departmentString);
                }

                connection.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                List<User> users = new List<User>();
                while (reader.Read())
                {
                    User user = new User
                    {
                        ID = reader["ID"].ToString(),
                        UserName = reader["UserName"].ToString(),
                        Pwd = reader["Pwd"].ToString(),
                        Department = reader["Department"].ToString()
                    };
                    users.Add(user);
                }
                ViewData["idString"] = idString;
                ViewData["departmentString"] = departmentString;
                return View(users);
            }
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
                // ID ซ้ำ?
                if (_db.AllUsers.Any(u => u.ID == obj.ID))
                {
                    ModelState.AddModelError("ID", "ID is already taken!!!!!");
                    return View(obj);
                }

                _db.AllUsers.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }


        public IActionResult Edit(string id)
        {
            if (id == null)
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

        public IActionResult Delete(string id)
        {
            if (id == null)
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

        public IActionResult ExportToExcel()
        {
            var users = _db.AllUsers.ToList();
            byte[] excelData;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");

                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "UserName";
                worksheet.Cells[1, 3].Value = "Password";
                worksheet.Cells[1, 4].Value = "Department";


                for (int i = 0; i < users.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = users[i].ID;
                    worksheet.Cells[i + 2, 2].Value = users[i].UserName;
                    worksheet.Cells[i + 2, 3].Value = users[i].Pwd;
                    worksheet.Cells[i + 2, 4].Value = users[i].Department;
                }

                excelData = package.GetAsByteArray();
            }

            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
        }

    }
}
