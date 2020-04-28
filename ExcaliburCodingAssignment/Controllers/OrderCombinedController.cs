using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ExcaliburCodingAssignment.Database;
using ExcaliburCodingAssignment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExcaliburCodingAssignment.Controllers
{
    [Route("api/OrderCombined")]
    [ApiController]
    public class OrderCombinedController : ControllerBase
    {

        private readonly ExcaliburDbContext _dbContext;

        public OrderCombinedController(ExcaliburDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/OrderCombined
        [HttpGet]
        public IEnumerable<OrderCombined> Get()
        {
            return _dbContext.OrderCombined.ToList();
        }

        // GET: api/OrderCombined/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<OrderCombined>>  Get(int id)
        {
            var orderCombined = await _dbContext.OrderCombined.FindAsync(id);

            if (orderCombined == null)
            {
                return NotFound();
            }

            return orderCombined;
        }

       

        // POST: api/OrderCombined/Search
        [HttpPost]
        [Route("Normalize")]
        public IActionResult Post([FromBody] OrderCombinedParamModel model)
        {
 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var combinedCount = _dbContext.OrderCombined.Count();


            //Use IQeryable collection type for faster performance
            /*var filteredOrderDetails = from ordDetail in _dbContext.OrderDetail.AsQueryable()
                                       where (ordDetail.OrderDesc.ToLower().Contains(model.descParam.ToLower()) || model.descParam == null) &&
                                       (ordDetail.OrderAmount >= model.fromAmountVal || model.fromAmountVal == null) &&
                                       (ordDetail.OrderAmount <= model.toAmountVal || model.toAmountVal == null)
                                        select new OrderDetail
                                        {
                                            OrderId = ordDetail.OrderId,
                                            OrderAmount = ordDetail.OrderAmount,
                                            OrderDesc = ordDetail.OrderDesc
                                        };
                                        */


                var filteredOrderDetails = _dbContext.OrderDetail
                                       .Where(ordDetail => ordDetail.OrderDesc.ToLower().Contains(model.descParam.ToLower()) || model.descParam == null)
                                       .Where(ordDetail => ordDetail.OrderAmount >= model.fromAmountVal || model.fromAmountVal == null)
                                       .Where(ordDetail => ordDetail.OrderAmount <= model.toAmountVal || model.toAmountVal == null)
                                       .ToList();


            //Use IQeryable collection type for faster performance
            /*var filteredOrderDates = from ordDate in _dbContext.OrderDate.AsQueryable()
                                     where ((ordDate.OrderedDate >= model.fromDateVal || model.fromDateParam == null) &&
                                     (ordDate.OrderedDate <= model.toDateVal || model.toDateParam == null))
                                     select new OrderDate()
                                     {
                                         OrderID = ordDate.OrderID,
                                         OrderedDate = ordDate.OrderedDate
                                     };
                                     */

            var filteredOrderDates = _dbContext.OrderDate
                                      .Where(ordDate => ordDate.OrderedDate >= model.fromDateVal || model.fromDateParam == null)
                                      .Where(ordDate => ordDate.OrderedDate <= model.toDateVal || model.toDateParam == null)
                                       .ToList();

            //Create the final collection by joining the prior two collections on the Order ID column
            IEnumerable < OrderCombined > result = from dt in filteredOrderDates
                                                   join detail in filteredOrderDetails
                                                   on dt.OrderID equals detail.OrderId
                                                   select new OrderCombined()
                                                   {
                                                       OrderedDate = dt.OrderedDate,
                                                       OrderAmount = detail.OrderAmount,
                                                       OrderDesc = detail.OrderDesc,
                                                       OrderId = dt.OrderID
                                                   };

            //Sort by Order date ASC first and then by order amount DESC
            result = result.OrderBy(p => p.OrderedDate).ThenByDescending(p => p.OrderAmount);

            //New collection to store the summed up amounts for each order Id
            var query = from orderCombinedRow in result
                        group orderCombinedRow by orderCombinedRow.OrderId into g
                        select new
                        {
                            Name = g.Key,
                            Sum = g.Sum(oc => oc.OrderAmount),
                        };

            //Truncate the data in the OrderCombined table only if there's data in there.
            if (combinedCount > 0)
            {
                IEnumerable<OrderCombined> toDelCombined = _dbContext.OrderCombined.Where(val => val.Id > 0);
                _dbContext.OrderCombined.RemoveRange(toDelCombined);
                _dbContext.SaveChanges();
            }

            //Insert into the OrderCombined table & persis to db
            foreach (var entity in result)
            {
                _dbContext.OrderCombined.Add(entity);
            }
            _dbContext.SaveChanges();

            return StatusCode(StatusCodes.Status200OK);
        }

        // PUT: api/OrderCombined/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OrderCombined orderCombined)
        {
            if (id != orderCombined.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(orderCombined).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderCombinedExists(id))
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


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderCombined>> Delete(int id)
        {
            var orderCombined = await _dbContext.OrderCombined.FindAsync(id);
            if (orderCombined == null)
            {
                return NotFound();
            }

            _dbContext.OrderCombined.Remove(orderCombined);
            await _dbContext.SaveChangesAsync();

            return orderCombined;
        }

        private bool OrderCombinedExists(int id)
        {
            return _dbContext.OrderCombined.Any(e => e.Id == id);
        }
    }
}
