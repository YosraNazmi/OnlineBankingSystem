using System;
using Microsoft.AspNetCore.Mvc;
using ABC_Bank.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ABC_Bank.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ABC_Bank.Controllers
{
	public class AccountController: Controller
	{

		private readonly ABCbankContext _context;

		public AccountController(ABCbankContext context)
        {
			_context = context;
		}

		public async Task<IActionResult> Account()
		{
			var aBCbankContext = _context.Accounts.Include(a => a.MyCustomer);
			return View(await aBCbankContext.ToListAsync());

		}

	}
}

