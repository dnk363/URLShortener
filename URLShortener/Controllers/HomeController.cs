using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;
using URLShortener.ViewModel;

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
            id.ToLower();
            Regex regex = new Regex(@"^http.*");
            MatchCollection matches = regex.Matches(id);
            ClaimsPrincipal currentUser = User;

            if (matches.Count == 0)
            {
                id = "http://" + id;//Добавление http к длинной ссылке, если его нет
            }

            url.ShortURL = GetShortUrl(id); // Получение короткого кода для ссылки из переданной длинной ссылки

            var b = db.ShortUrl.FirstOrDefault(p => p.ShortURL == url.ShortURL); // Проверка наличия короткой ссылки в базе

            // Если в базе нет короткой ссылки - добавляем
            if (b == null)
            {
                url.LongURL = id;

                if (currentUser.Identity.Name != null)
                {
                    url.UserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                }

                db.ShortUrl.Add(url);
                db.SaveChangesAsync();
            }
            else
            {
                url.LongURL = b.LongURL;
            }

            string resulturl = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/r/l/" + GetShortUrl(id);

            return new JsonResult(resulturl);
        }

        public IActionResult FoundError()
        {
            return View();
        }

        [Authorize]
        public IActionResult Data()
        {
            ClaimsPrincipal currentUser = User;

            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<URLShort> urlData = db.ShortUrl
                .Select(c => new URLShort { 
                                            ShortURL = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/r/l/" + c.ShortURL, 
                                            LongURL = c.LongURL, 
                                            UserId = c.UserId 
                                          })
                .Where(p => p.UserId == userId)
                .ToList();

            DataViewModel ivm = new DataViewModel { UrlData = urlData };


            return PartialView(ivm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
