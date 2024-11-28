using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWebSite.Controllers
{
    [Authorize]
    public class ListOfRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListOfRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ListOfRequests
/*        public async Task<IActionResult> Index()
        {
            return _context.ListOfRequests != null ?
                        View(await _context.ListOfRequests.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.ListOfRequests'  is null.");
        }*/

        public async Task<IActionResult> Index(int pageIndex = 1, int itemsPerPage = 10)
        {
            /*var request = await _context.ListOfRequests
                .Include(r => r.EmailMessage)
                .ToListAsync();

            return View(request);*/

            int totalItems = await _context.ListOfRequests.CountAsync();

            var paginatedData = await _context.ListOfRequests
            .Include(r => r.EmailMessage)  // Include related EmailMessage entity
            .Skip((pageIndex - 1) * itemsPerPage)  // Skip previous pages
            .Take(itemsPerPage)  // Take only the items for the current page
            .ToListAsync();  // Fetch the data asynchronously*/

            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            var viewModel = new PaginatedViewModel
            {
                Data = paginatedData,
                CurrentPage = pageIndex,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Test()
        {
            var request = await _context.ListOfRequests
                            .Include(r => r.EmailMessage)
                            .ToListAsync();

            return View(request);
        }

        // GET: ListOfRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ListOfRequests == null)
            {
                return NotFound();
            }

            var listOfRequests = await _context.ListOfRequests
                .FirstOrDefaultAsync(m => m.ListOfRequestsID == id);
            if (listOfRequests == null)
            {
                return NotFound();
            }

            return View(listOfRequests);
        }

        // GET: ListOfRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ListOfRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListOfRequestsID,HeadOfConversation,TailOfConversation")] ListOfRequests listOfRequests)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listOfRequests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(listOfRequests);
        }

        // GET: ListOfRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ListOfRequests == null)
            {
                return NotFound();
            }

            var listOfRequests = await _context.ListOfRequests.FindAsync(id);
            if (listOfRequests == null)
            {
                return NotFound();
            }
            return View(listOfRequests);
        }

        // POST: ListOfRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(.ListOfRequestsID,HeadOfConversation,TailOfConversation")] ListOfRequests listOfRequests)
        {
            if (id != listOfRequests.ListOfRequestsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listOfRequests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListOfRequestsExists(listOfRequests.ListOfRequestsID))
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
            return View(listOfRequests);
        }*/

        // GET: ListOfRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ListOfRequests == null)
            {
                return NotFound();
            }

            var listOfRequests = await _context.ListOfRequests
                .FirstOrDefaultAsync(m => m.ListOfRequestsID == id);
            if (listOfRequests == null)
            {
                return NotFound();
            }

            return View(listOfRequests);
        }

        // POST: ListOfRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ListOfRequests == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ListOfRequests'  is null.");
            }
            var listOfRequests = await _context.ListOfRequests.FindAsync(id);
            if (listOfRequests != null)
            {
                _context.ListOfRequests.Remove(listOfRequests);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListOfRequestsExists(int id)
        {
            return (_context.ListOfRequests?.Any(e => e.ListOfRequestsID == id)).GetValueOrDefault();
        }
    }
}
