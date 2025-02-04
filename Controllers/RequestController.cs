﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Humanizer;
using System.Net.Mail;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Tsp;
using Microsoft.AspNetCore.Authorization;

namespace HelpDeskWebSite.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Request
        public async Task<IActionResult> Index()
        {
              return _context.EmailMessages != null ? 
                          View(await _context.EmailMessages.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.EmailMessages'  is null.");
        }

        // GET: Request/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var requests = await _context.ListOfRequests
                .FirstOrDefaultAsync(m => m.ListOfRequestsID == id);

            var conversation = new List<ListOfRequests>();
            conversation.Add(requests);

            int? headRequest = requests.HeadOfConversation;

            var usersConversation = new List<EmailMessage>();

            if (requests == null)
            {
                return NotFound();//In development
            }
            else
            {
                while (headRequest != null)
                {
                    var current = await _context.EmailMessages.SingleOrDefaultAsync(i => i.Id == headRequest);

                    headRequest = current.InReply;

                    usersConversation.Add(current);

                    if (current == null || current.InReply == null)
                    {
                        break;
                    }
                }
            }

            var paginatedData = await _context.ListOfRequests
                .Include(r => r.EmailMessage)
                .OrderByDescending(r => r.ListOfRequestsID)
                .Take(10)
                .ToListAsync();

            var viewModel = new ListOfRequestsAndRequest
            {
                Data = paginatedData,   //for the left list
                emailMessages = usersConversation,   //for displaying conversation
                listOfRequests = conversation
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RequestStatusUpdateTOCloseAsync(UpdateResolutionANDCloseRequest updateRequestStatus)
        {
            if (ModelState.IsValid)
            {
                var currentRequest = await _context.ListOfRequests
                            .FirstOrDefaultAsync(m => m.ListOfRequestsID == updateRequestStatus.RequestID);

                currentRequest.Resolution = updateRequestStatus.UpdatedResolution;
                currentRequest.Status = "Closed";

                _context.ListOfRequests.Update(currentRequest);
                await _context.SaveChangesAsync();

            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(updateRequestStatus.UpdatedResolution);
                    Console.WriteLine(updateRequestStatus.RequestID);
                    Console.WriteLine("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
                    Console.WriteLine(error.ErrorMessage); // Log errors for troubleshooting
                }
            }

            return RedirectToAction("Details", "Request", new { id = updateRequestStatus.RequestID });
        }

        [HttpPost]
        public async Task<ActionResult> RequestStatusUpdateToOpen(int requestID)
        {
            var currentRequest = await _context.ListOfRequests
                            .FirstOrDefaultAsync(m => m.ListOfRequestsID == requestID);

            currentRequest.Status = "Open";

            _context.ListOfRequests.Update(currentRequest);
            await _context.SaveChangesAsync();


            return RedirectToAction("Details", "Request", new { id = requestID });
        }

        [HttpPost]
        public async Task<ActionResult> RequestStatusUpdateToOnhold(int requestID)
        {
            var currentRequest = await _context.ListOfRequests
                            .FirstOrDefaultAsync(m => m.ListOfRequestsID == requestID);

            currentRequest.Status = "Onhold";

            _context.ListOfRequests.Update(currentRequest);
            await _context.SaveChangesAsync();


            return RedirectToAction("Details", "Request", new { id = requestID });
        }


        /*   for testing 
         *   [HttpPost]
                public IActionResult RequestStatusUpdateTOClose(string Email)
                {
                    Console.WriteLine($" Email: {Email}");
                    Console.WriteLine(Email);
                    Console.WriteLine("wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");


                    return RedirectToAction("Details", "Request", new { id = 34 });
                }*/


        public async Task<IActionResult> DetailsOrigin(int? id)
        {
            var requests = await _context.ListOfRequests
                .FirstOrDefaultAsync(m => m.ListOfRequestsID == id);

            int? headRequest = requests.HeadOfConversation;

            var usersConversation = new List<EmailMessage>();

            if (requests == null)
            {
                return NotFound();//In development
            }
            else
            {
                while (headRequest != null)
                {
                    var current = await _context.EmailMessages.SingleOrDefaultAsync(i => i.Id == headRequest);

                    headRequest = current.InReply;

                    usersConversation.Add(current);

                    if (current == null || current.InReply == null)
                    {
                        break;
                    }
                }
            }

            return View(usersConversation);
        }



        // GET: Request/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Request/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Recipient,Sender,From,Subject,BodyPlain,StrippedText,StrippedSignature,BodyHtml,StrippedHtml,AttachmentCount,Timestamp,Token,Signature,MessageHeaders,ContentIdMap")] EmailMessage emailMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emailMessage);
        }

        // GET: Request/Edit/5
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

        // POST: Request/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Recipient,Sender,From,Subject,BodyPlain,StrippedText,StrippedSignature,BodyHtml,StrippedHtml,AttachmentCount,Timestamp,Token,Signature,MessageHeaders,ContentIdMap")] EmailMessage emailMessage)
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

        // GET: Request/Delete/5
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

        // POST: Request/Delete/5
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
