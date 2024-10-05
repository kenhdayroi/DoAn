using LuxStay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LuxStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageTourController : Controller
    {
        private readonly string _connectionString;

        public ManageTourController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
