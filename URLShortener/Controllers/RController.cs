using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Models;

namespace URLShortener.Controllers
{
    public class RController : Controller
    {
        private ShortContext db;

        public RController(ShortContext context)
        {
            db = context;
        }

        public void L(string id)
        {
            var b = db.SUrl.FirstOrDefault(p => p.ShortURL == id);

            if (b != null)
            {
                Response.Redirect(b.LongURL);
            }
            else
            {
                Response.Redirect("/Home/FoundError");
            }
        }
    }
}