using FastFoodWeb.Data;
using FastFoodWeb.Models;
using FastFoodWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FastFoodWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepo, ApplicationDbContext context)
        {
            _logger = logger;
            _productRepo = productRepo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Trang chủ";
            ViewBag.Featured = await _productRepo.GetFeaturedAsync();
            ViewBag.Categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}