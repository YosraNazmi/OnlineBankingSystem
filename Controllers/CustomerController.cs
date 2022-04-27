using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ABC_Bank.Data;
using ABC_Bank.Models;
using ABC_Bank.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Controllers
{
    public class CustomerController : Controller
    {
        private readonly ABCbankContext _context;

        public CustomerController(ABCbankContext context)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index(string searchstring)
        {
            var customer = from m in _context.Customers select m;
            if (!String.IsNullOrEmpty(searchstring))
            {
                customer = customer.Where(s => s.FirstName!.Contains(searchstring));
            }
            return View(await customer.ToListAsync());
        }

        // GET: Customer/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Role,Title,FirstName,SecondName,Email,DOB,Address,City,State,PostCode,Tell,AccountType,Password")] Customers customers)
        {
            if (ModelState.IsValid)
            {
                customers.Password = HashClass.HashGenerator(customers.Password);
                _context.Add(customers);
                await _context.SaveChangesAsync();

                var account = new Accounts();
                account.MyCustomerId = customers.Id;
                account.AccountNumber = GenerateRandomNum();
                account.Balance = 0;

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(customers);
        }

        public static int GenerateRandomNum()
        {
            // Number of digits for random number to generate
            int randomDigits = 4;

            int _max = (int)Math.Pow(10, randomDigits);
            Random _rdm = new Random();
            int _out = _rdm.Next(0, _max);

            while (randomDigits != _out.ToString().ToArray().Distinct().Count())
            {
                _out = _rdm.Next(0, _max);
            }
            return _out;
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Customers customer)
        {
            if (ModelState.IsValid)
            {
                //var user = await _context.Customers.SingleOrDefaultAsync(m => m.Email == customer.Email && m.Password == customer.Password);
                var user = await _context.Customers.Where(x => x.Email.ToLower().Equals(customer.Email.ToLower())).FirstOrDefaultAsync();
                var account = await _context.Accounts
                .FirstOrDefaultAsync(m => m.MyCustomerId == user.Id);

                if (user == null) {

                    ViewBag.authError = "no user found";
                    return RedirectToAction("Login", "Customer");

                }
                else
                {
                    bool isValid = HashClass.VerifyPassword(user.Password, customer.Password);
                    //bool isValid = true;
                    if (!isValid)
                    {
                        ViewBag.authError = "user not found";
                        return RedirectToAction("Login", "Customer");

                    }
                    else
                    {
                        if (user.Role.Equals("Customer"))
                        {
                            HttpContext.Session.SetString("FullName", user.FirstName + " " + user.SecondName);
                            HttpContext.Session.SetInt32("AccountID", account.AccountNumber);

                            HttpContext.Session.SetInt32("balance", (int)account.Balance);
                            return RedirectToAction("Account", "Account");

                        }
                        else if (user.Role.Equals("Admin"))
                        {
                            HttpContext.Session.SetString("FullName", user.FirstName + " " + user.SecondName);
                            return RedirectToAction("Admin", "Admin");
                        }
                        else
                        {
                            ViewBag.authError = "no user found";
                            return RedirectToAction("Login", "Customer");
                        }

                    }
                   

                }
                
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Customer/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }
            return View(customers);
        }

        // POST: Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Role,Title,FirstName,SecondName,Email,DOB,Address,City,State,PostCode,Tell,AccountType,Password")] Customers customers)
        {
            if (id != customers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customers);
        }

        // GET: Customer/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customers = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Customer/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customers = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
