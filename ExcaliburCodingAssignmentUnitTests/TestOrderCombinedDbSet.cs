using System;
using System.Linq;
using ExcaliburCodingAssignment.Models;
using System.Collections.Generic;
using System.Text;

namespace ExcaliburCodingAssignmentUnitTests
{
    class TestOrderCombinedDbSet: TestDbSet<OrderCombined>
    {
        public override OrderCombined Find(params object[] keyValues)
        {
            return this.SingleOrDefault(orderCombined => orderCombined.Id == (int)keyValues.Single());
        }
    }
}
