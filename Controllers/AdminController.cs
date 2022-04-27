using System;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Bank.Controllers

{
	public class AdminController : Controller
	{
		public AdminController()
		{
		}


		public IActionResult Admin()
		{
			return View();
		}

       
    }
}

