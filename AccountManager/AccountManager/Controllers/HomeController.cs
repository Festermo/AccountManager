﻿using Microsoft.AspNetCore.Mvc;

namespace AccountManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Account", "Account");
        }
    }
}