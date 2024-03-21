using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperheroQuoter.Server.Models;

namespace SuperheroQuoter.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly QuoteContext _context;
        public record QuoteInput(string Quote, string Name);

        public QuotesController(QuoteContext context)
        {
            _context = context;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var quotes = await _context.Quotes.Include(q => q.Author).ToListAsync();
            var quotesWithAuthorName = quotes.Select(q => new Quote
            {
                Id = q.Id,
                Text = q.Text,
                AuthorId = q.AuthorId,
                Author = new Author
                {
                    Id = q.Author.Id,
                    Name = q.Author.Name
                }
            }).ToList();

            return quotesWithAuthorName;
        }

        // GET: api/Quotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _context.Quotes.Include(q => q.Author).FirstOrDefaultAsync(q => q.Id == id);

            if (quote == null)
            {
                return NotFound();
            }

            var quoteWithAuthorName = new Quote
            {
                Id = quote.Id,
                Text = quote.Text,
                AuthorId = quote.AuthorId,
                Author = new Author
                {
                    Id = quote.Author.Id,
                    Name = quote.Author.Name
                }
            };

            return quoteWithAuthorName;
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(QuoteInput quoteInput)
        {
            if (!QuoteWithAuthorExists(quoteInput))
            {
                var quotes = new Quote();
                quotes.Text = quoteInput.Quote;

                var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == quoteInput.Name);

                if (author != null)
                {
                    quotes.Author = author;
                }
                else
                {
                    var newAuthor = new Author();
                    newAuthor.Name = quoteInput.Name;
                    _context.Authors.Add(newAuthor);
                    await _context.SaveChangesAsync();

                    quotes.Author = newAuthor;
                }

                _context.Quotes.Add(quotes);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetQuotes", new { id = quotes.Id }, quotes);
            }

            return UnprocessableEntity("Quote already exists");
        }

        // POST: api/Quotes/Multiple
        [HttpPost("Multiple")]
        public async Task<ActionResult<IEnumerable<Quote>>> PostQuotes(List<QuoteInput> quotes)
        {
            var newQuotes = new List<Quote>();

            foreach (var quoteInput in quotes)
            {
                if (!QuoteWithAuthorExists(quoteInput))
                {

                    var quote = new Quote();
                    quote.Text = quoteInput.Quote;

                    var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == quoteInput.Name);

                    if (author != null)
                    {
                        quote.Author = author;
                    }
                    else
                    {
                        var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Name == quoteInput.Name);

                        if (existingAuthor != null)
                        {
                            quote.Author = existingAuthor;
                        }
                        else
                        {
                            var newAuthor = new Author();
                            newAuthor.Name = quoteInput.Name;
                            _context.Authors.Add(newAuthor);
                            await _context.SaveChangesAsync();

                            quote.Author = newAuthor;
                        }
                    }

                _context.Quotes.Add(quote);
                await _context.SaveChangesAsync();

                newQuotes.Add(quote);
                }
            }

            return newQuotes;
        }


        // DELETE: api/Quotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quotes = await _context.Quotes.FindAsync(id);
            if (quotes == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(quotes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuoteWithAuthorExists(QuoteInput quoteInput)
        {
            var author = _context.Authors.FirstOrDefault(a => a.Name == quoteInput.Name);

            if (author != null)
            {
                return _context.Quotes.Any(q => q.Text == quoteInput.Quote && q.AuthorId == author.Id);
            }

            return false;
        }
    }
}
