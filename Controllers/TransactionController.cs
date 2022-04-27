using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ABC_Bank.Data;
using ABC_Bank.Models;
using System.Dynamic;

namespace Controllers
{
    public class TransactionController : Controller
    {
        private readonly ABCbankContext _context;

        public TransactionController(ABCbankContext context)
        {
            _context = context;
        }

        // GET: Transaction
        public IActionResult Index(int? searchstring)
        {
            if (searchstring.HasValue)
            {
                return View(_context.Transactions.Where(x => x.AccountNumber.ToString().Contains(searchstring.ToString())).ToList());
            }

            return View(_context.Transactions.ToList());



        }

        // GET: Transaction
        public async Task<IActionResult> Transactions()
        {
            var acc = await _context.Accounts.ToListAsync();
            return View(await _context.Transactions.ToListAsync());
        }

        // GET: Transaction/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .FirstOrDefaultAsync(m => m.TransactionsId == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // GET: Transaction/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionsId,TransactionName,TransactionAmount,TransactionDate,DestinationAccount,AccountNumber")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactions);
        }

        // GET: Transaction/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions == null)
            {
                return NotFound();
            }
            return View(transactions);
        }

        // POST: Transaction/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionsId,TransactionName,TransactionAmount,TransactionDate,DestinationAccount,AccountNumber")] Transactions transactions)
        {
            if (id != transactions.TransactionsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionsExists(transactions.TransactionsId))
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
            return View(transactions);
        }

        // GET: Transaction/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .FirstOrDefaultAsync(m => m.TransactionsId == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // POST: Transaction/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactions = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transactions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionsExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionsId == id);
        }


        public IActionResult Deposit()
        {
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit([Bind("TransactionsId,TransactionName,TransactionAmount,TransactionDate,DestinationAccount,AccountNumber")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                transactions.TransactionName = "Deposit";
                transactions.TransactionDate = DateTime.UtcNow;
                transactions.DestinationAccount = transactions.AccountNumber;


                var account = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountNumber == transactions.AccountNumber);

                account.Balance += transactions.TransactionAmount;

                _context.Update(account);

                await _context.SaveChangesAsync();

                _context.Add(transactions);
                await _context.SaveChangesAsync();
                ViewData["Success"] = "Money has been Deposited fromyour account";
                return RedirectToAction(nameof(Deposit));
            }
            return View(transactions);
        }


        public IActionResult Withdrawal()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdrawal([Bind("TransactionsId,TransactionName,TransactionAmount,TransactionDate,DestinationAccount,AccountNumber")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                transactions.TransactionName = "Withdraw";
                transactions.TransactionDate = DateTime.UtcNow;
                transactions.DestinationAccount = transactions.AccountNumber;

                var account = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountNumber == transactions.AccountNumber);

                account.Balance -= transactions.TransactionAmount;

                _context.Update(account);

                await _context.SaveChangesAsync();

                _context.Add(transactions);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Withdrawal));
            }
            return View(transactions);
        }


        public IActionResult Statement()
        {
            return View();
        }

        public IActionResult Transfer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer([Bind("TransactionsId,TransactionName,TransactionAmount,TransactionDate,DestinationAccount,AccountNumber")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                transactions.TransactionName = "Transfer";
                transactions.TransactionDate = DateTime.UtcNow;
        


                var account1 = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountNumber == transactions.AccountNumber);

                var account2 = await _context.Accounts
               .FirstOrDefaultAsync(m => m.AccountNumber == transactions.DestinationAccount);

                account1.Balance -= transactions.TransactionAmount;
                account2.Balance += transactions.TransactionAmount;

                _context.Update(account1);
                _context.Update(account1);

                await _context.SaveChangesAsync();

                _context.Add(transactions);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Transfer));
            }
            return View(transactions);
        }


    }
}
