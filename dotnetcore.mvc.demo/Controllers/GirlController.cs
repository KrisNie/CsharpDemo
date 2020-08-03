using System.Text.Encodings.Web;
using dotnetcore.mvc.demo.DataAccess.Base;
using Microsoft.AspNetCore.Mvc;

namespace dotnetcore.mvc.demo.Controllers
{
    public class GirlController : Controller
    {
        private readonly LifeContext _context;

        public GirlController(LifeContext context)
        {
            _context = context;
        }

        // Every public method in a controller is callable as an HTTP endpoint
        // An HTTP endpoint is a targetable URL in the web application
        // such as https://localhost:5001/Girl, and combines the protocol used: HTTPS
        // you will see "This is my default action..." on page whose URL is "https://localhost:5001/Girl"

        // GET: /Girl/
        public IActionResult Index()
        {
            // todo: use mysql to show girl's information
            ViewData["GirlName"] = "hyomin";
            ViewData["GirlAge"] = 18;
            return View();
        }

        // GET: /Girl/Welcome/ 
        public string Welcome(string name, int age = 18)
        {
            // say hello to my girl 
            return HtmlEncoder.Default.Encode($"Hello my {age}-years-old girl {name}");
        }
    }
}