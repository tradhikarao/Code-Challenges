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
        public IEnumerable<OrderCombined> Get(DateTime fromDate, DateTime toDate, string desc = "")
        {
            return _dbContext.OrderCombined.ToList();
        }

        // GET: api/OrderCombined/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OrderCombined
        [HttpPost]
        public IActionResult Post([FromBody] OrderCombinedParamModel model)
        {
 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var combinedCount = _dbContext.OrderCombined.Count();


            //Use IQeryable collection type for faster performance
            var filteredOrderDetails = from ordDetail in _dbContext.OrderDetail.AsQueryable()
                                       where (ordDetail.OrderDesc.ToLower().Contains(model.descParam.ToLower()) || model.descParam == null) &&
                                       (ordDetail.OrderAmount >= model.fromAmountVal || model.fromAmountVal == null) &&
                                       (ordDetail.OrderAmount <= model.toAmountVal || model.toAmountVal == null)
                                        select new OrderDetail
                                        {
                                            OrderId = ordDetail.OrderId,
                                            OrderAmount = ordDetail.OrderAmount,
                                            OrderDesc = ordDetail.OrderDesc
                                        };

            //Use IQeryable collection type for faster performance
            var filteredOrderDates = from ordDate in _dbContext.OrderDate.AsQueryable()
                                     where ((ordDate.OrderedDate >= model.fromDateVal || model.fromDateParam == null) &&
                                     (ordDate.OrderedDate <= model.toDateVal || model.toDateParam == null))
                                     select new OrderDate()
                                     {
                                         OrderID = ordDate.OrderID,
                                         OrderedDate = ordDate.OrderedDate
                                     };
                                     

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
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
