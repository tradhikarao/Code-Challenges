using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExcaliburCodingAssignment.Database;
using ExcaliburCodingAssignment.Models;

namespace ExcaliburCodingAssignment.Controllers
{
    [Route("api/OrderDates")]
    [ApiController]
    public class OrderDatesController : ControllerBase
    {
        private readonly ExcaliburDbContext _context;

        public OrderDatesController(ExcaliburDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderDates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDate>>> GetOrderDate()
        {
            return await _context.OrderDate.ToListAsync();
        }

        // GET: api/OrderDates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDate>> GetOrderDate(int id)
        {
            var orderDate = await _context.OrderDate.FindAsync(id);

            if (orderDate == null)
            {
                return NotFound();
            }

            return orderDate;
        }

        // PUT: api/OrderDates/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDate(int id, OrderDate orderDate)
        {
            if (id != orderDate.OrderID)
            {
                return BadRequest();
            }

            _context.Entry(orderDate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderDates
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<OrderDate>> PostOrderDate(OrderDate orderDate)
        {
            _context.OrderDate.Add(orderDate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderDate", new { id = orderDate.OrderID }, orderDate);
        }

        // DELETE: api/OrderDates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDate>> DeleteOrderDate(int id)
        {
            var orderDate = await _context.OrderDate.FindAsync(id);
            if (orderDate == null)
            {
                return NotFound();
            }

            _context.OrderDate.Remove(orderDate);
            await _context.SaveChangesAsync();

            return orderDate;
        }

        private bool OrderDateExists(int id)
        {
            return _context.OrderDate.Any(e => e.OrderID == id);
        }
    }
}
