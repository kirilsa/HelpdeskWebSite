using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskWebSite.Controllers
{
    public class TicketTrackingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketTrackingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TicketTracking
        public async Task<IActionResult> Index()
        {
            return _context.Conversations != null ?
                        View(await _context.Conversations.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Conversations'  is null.");
        }

        // GET: TicketTracking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Conversations == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(m => m.ConversationId == id);
            if (conversation == null)
            {
                return NotFound();
            }

            var combinedModel = Tuple.Create(conversation, _context.EmailMessages);

            return View(combinedModel);
        }

        // GET: TicketTracking/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TicketTracking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConversationId,UsersConversation")] Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(conversation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conversation);
        }

        // GET: TicketTracking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Conversations == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation == null)
            {
                return NotFound();
            }
            return View(conversation);
        }

        // POST: TicketTracking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConversationId,UsersConversation")] Conversation conversation)
        {
            if (id != conversation.ConversationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conversation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConversationExists(conversation.ConversationId))
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
            return View(conversation);
        }

        // GET: TicketTracking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Conversations == null)
            {
                return NotFound();
            }

            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(m => m.ConversationId == id);
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // POST: TicketTracking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Conversations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Conversations'  is null.");
            }
            var conversation = await _context.Conversations.FindAsync(id);
            if (conversation != null)
            {
                _context.Conversations.Remove(conversation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConversationExists(int id)
        {
            return (_context.Conversations?.Any(e => e.ConversationId == id)).GetValueOrDefault();
        }
    }
}
