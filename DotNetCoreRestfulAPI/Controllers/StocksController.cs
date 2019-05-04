using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreRestfulAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCoreRestfulAPI.Controllers
{
    [Route("stocks-api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {

        private readonly StockContext _context;

        public StocksController(StockContext context)
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
        /// Get all stocks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            return await _context.Stocks.ToListAsync();
        }

        /// <summary>
        /// Get a stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Stock>> GetStock(long id)
        {
            var Stock = await _context.Stocks.FindAsync(id);

            if (Stock == null)
            {
                return NotFound();
            }

            return Stock;
        }

        /// <summary>
        /// Get all quotes from a stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/quotes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Quote>>> GetStockQuotes(long id)
        {

            var Stock = await _context.Stocks.FindAsync(id);

            if (Stock == null)
            {
                return NotFound();
            }

            var Quotes = await _context.Quotes
                            .Where(quote => quote.Stock == Stock)
                            .OrderBy(quote => quote.Date)
                            .ToListAsync();

            return Quotes;
        }

        /// <summary>
        /// Create a new stock
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Stock>> PostStock(Stock stock)
        {
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, stock);
        }

        /// <summary>
        /// Update a stock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stock"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutStock(long id, Stock stock)
        {
            if (id != stock.Id)
            {
                return BadRequest();
            }

            _context.Entry(stock).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a stock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStock(long id)
        {
            var Stock = await _context.Stocks.FindAsync(id);

            if (Stock == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(Stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
