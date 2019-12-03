using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortener.Models;  

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        URLShort url = new URLShort();

        private ShortContext db;

        public HomeController(ShortContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShortUrl(string id)
        {
            url.ShortURL = GetShortUrl(id); // Получение короткого кода для ссылки из переданной длинной ссылки

            var b = db.SUrl.FirstOrDefault(p => p.ShortURL == url.ShortURL); // Проверка наличия короткой ссылки в базе

            // Если в базе нет короткой ссылки - добавляем
            if (b == null)
            {
                url.LongURL = id;

                db.SUrl.Add(url);
                db.SaveChangesAsync();
            }
            else
            {
                url.LongURL = b.LongURL;
            }

            string resulturl = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/r/l/" + GetShortUrl(id);

            return new JsonResult(resulturl);
        }

        public IActionResult R()
        {
            return View();
        }

        public IActionResult FoundError()
        {
            return View();
        }

        private string GetShortUrl(string lUrl)
        {
            string sUrl = "";

            char[] a = lUrl.GetHashCode().ToString().ToCharArray();

            foreach (var rt in a)
            {
                sUrl += (char)(25 + Convert.ToInt32(rt));
            }

            return sUrl;
        }
    }
}