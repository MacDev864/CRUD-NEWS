using CRUD_NEWS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_NEWS.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsDbContext _dbContext;
        public NewsController(NewsDbContext db)
        {
            _dbContext = db;

        }

        public IActionResult Index()
        {
            ViewData["Title"] = "หน้าแรก";
            ViewData["project_name"] = "CRUD News";
            ViewData["asp_action"] = "Index";
            ViewData["asp_action_create"] = "Create";
            ViewData["asp_action_update"] = "update";
            ViewData["asp_Controller"] = "News";
            ViewData["page_name"] = "หน้าแรก";
            ViewData["asp_action_view"] = "Detail";


            return View();

        }

        public async Task<IActionResult> Detail(string? id)
        {
            ViewData["Title"] = "หน้าแรก";
            ViewData["project_name"] = "CRUD News";
            ViewData["asp_action"] = "Index";
            ViewData["asp_action_create"] = "Create";
            ViewData["asp_action_view"] = "Detail";
            ViewData["asp_action_update"] = "update";
            ViewData["asp_Controller"] = "News";
            ViewData["page_name"] = "รายละเอียดข่าวสาร";
            ViewData["id"] = id;
            if (id == "")
            {
                return NotFound();

            }
            var news = await _dbContext.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);

        }

    }
}
