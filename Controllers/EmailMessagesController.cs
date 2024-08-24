using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;

namespace HelpDeskWebSite.Controllers
{
    public class EmailMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmailMessages
        public async Task<IActionResult> Index()
        {
              return _context.EmailMessages != null ? 
                          View(await _context.EmailMessages.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EmailMessages'  is null.");
        }

        // GET: EmailMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmailMessages == null)
            {
                return NotFound();
            }

            var emailMessage = await _context.EmailMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailMessage == null)
            {
                return NotFound();
            }

            var conversationId = _context.Conversations.FirstOrDefault(c => c.ConversationId == id).UsersConversation;
            List<int> idsToDisplay = conversationId.Split(',')
                .Select(int.Parse)
                .ToList();

            var filtredData = _context.EmailMessages.Where(d => idsToDisplay.Contains(d.Id)).ToList();

            return View(filtredData);
        }

        // GET: EmailMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EmailMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Recipient,Sender,From,Subject,BodyPlain,StrippedText,StrippedSignature,BodyHtml,StrippedHtml,AttachmentCount,Timestamp,Token,Signature,MessageHeaders,ContentIdMap,Date")] EmailMessage emailMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emailMessage);
        }

        // GET: EmailMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EmailMessages == null)
            {
                return NotFound();
            }

            var emailMessage = await _context.EmailMessages.FindAsync(id);
            if (emailMessage == null)
            {
                return NotFound();
            }
            return View(emailMessage);
        }

        // POST: EmailMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Recipient,Sender,From,Subject,BodyPlain,StrippedText,StrippedSignature,BodyHtml,StrippedHtml,AttachmentCount,Timestamp,Token,Signature,MessageHeaders,ContentIdMap,Date")] EmailMessage emailMessage)
        {
            if (id != emailMessage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailMessageExists(emailMessage.Id))
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
            return View(emailMessage);
        }

        // GET: EmailMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmailMessages == null)
            {
                return NotFound();
            }

            var emailMessage = await _context.EmailMessages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailMessage == null)
            {
                return NotFound();
            }

            return View(emailMessage);
        }

        // POST: EmailMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmailMessages == null)
            {
                return Problem("Entity set 'ApplicationDbContext.EmailMessages'  is null.");
            }
            var emailMessage = await _context.EmailMessages.FindAsync(id);
            if (emailMessage != null)
            {
                _context.EmailMessages.Remove(emailMessage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailMessageExists(int id)
        {
          return (_context.EmailMessages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
