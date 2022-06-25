using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CommnetController : Controller
    {
        private AppDbContext _context { get; }
        private IWebHostEnvironment _env { get; }
        public CommnetController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;    
        }
        public IActionResult Index()
        {
            List<ClientComment> comments = _context.Comments.ToList();
            return View(comments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClientComment comment)
        {
            if (!ModelState.IsValid) return View(comment);
            if(comment == null) return View(comment);
            string fileName = Guid.NewGuid().ToString() + comment.Photo.FileName;
            using(FileStream fs = new FileStream(Path.Combine(_env.WebRootPath,"assets/images", fileName), FileMode.Create))
            {
                comment.Photo.CopyTo(fs);
            }
            comment.PhotoUrl = fileName;
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            ClientComment comment = _context.Comments.Find(id);
            if (comment == null) return View();
           

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ClientComment comment)
        {
            if (comment == null) return View(comment);
            ClientComment oldComment = _context.Comments.Find(comment.Id);
            if(oldComment == null) return View(comment);
            if (comment.PhotoUrl != null)
            {
                if(System.IO.File.Exists(Path.Combine(_env.WebRootPath,"assets/images",oldComment.PhotoUrl)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets/images",oldComment.PhotoUrl));
                }
                string fileName = Guid.NewGuid().ToString() + comment.Photo.FileName;
                using (FileStream fs = new FileStream(Path.Combine(_env.WebRootPath, "assets/images", fileName), FileMode.Create))
                {
                    comment.Photo.CopyTo(fs);
                }
                comment.PhotoUrl = fileName;
            }
            oldComment.FullName = comment.FullName;
            oldComment.Role = comment.Role;
            oldComment.Comment = comment.Comment;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            ClientComment comment = _context.Comments.Find(id);
            if(comment == null) return View();
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets/images", comment.PhotoUrl)))
            {
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets/images", comment.PhotoUrl));
            }
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
