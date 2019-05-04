using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreRestfulAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreRestfulAPI.Controllers
{
    [Route("stocks-api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {

        private readonly StockContext _context;

        public QuotesController(StockContext context)
        {
            _context = context;

            // inserting a initial stock and quote
            if (_context.Stocks.Count() == 0)
            {
                Stock MSFT = new Stock { Symbol = "MSFT", Company = "Microsoft Corporation" };
                Quote Quote1 = new Quote { Price = 129.77, Stock = MSFT, Date = new DateTime(2019, 04, 29) };
                Quote Quote2 = new Quote { Price = 130.60, Stock = MSFT, Date = new DateTime(2019, 04, 30) };
                Quote Quote3 = new Quote { Price = 127.88, Stock = MSFT, Date = new DateTime(2019, 05, 01) };
                _context.Stocks.Add(MSFT);
                _context.Quotes.Add(Quote1);
                _context.Quotes.Add(Quote2);
                _context.Quotes.Add(Quote3);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Get a quote
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Quote>> GetQuote(long id)
        {
            var Quote = await _context.Quotes.FindAsync(id);

            if (Quote == null)
            {
                return NotFound();
            }

            return Quote;
        }

        /// <summary>
        /// Create a new quote
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {

            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetQuote), new { id = quote.Id }, quote);
        }

        /// <summary>
        /// Update a quote
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quote"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutQuote(long id, Quote quote)
        {
            if (id != quote.Id)
            {
                return BadRequest();
            }

            _context.Entry(quote).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a quote
        /// </summary>
        /// <param name="id"></param>
        /// <returns>pra que serve?</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteQuote(long id)
        {
            var Quote = await _context.Quotes.FindAsync(id);

            if (Quote == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(Quote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
