using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcDemo.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace MvcDemo.Controllers.Girl
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
        public async Task<IActionResult> Index()
        {
            // todo: use mysql to show girl's information
            return View(await _context.Girls.ToListAsync());
        }

        // GET: /Girl/Welcome/ 
        public string Welcome(string name, int age = 18)
        {
            // say hello to my girl 
            return HtmlEncoder.Default.Encode($"Hello my {age}-years-old girl {name}");
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var girl = await _context.Girls
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (girl == null)
            {
                return NotFound();
            }

            return View(girl);
        }
    }
}