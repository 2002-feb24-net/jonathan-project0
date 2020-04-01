using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using WheyMenDAL.Library.Model;
using WheyMenIOValidation.Library;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ValidationTesting
{
    public class BusinessTest
    {
        private readonly WheyMenContext _context;
        private readonly ITestOutputHelper output;

        public BusinessTest(ITestOutputHelper output)
        {
            this.output = output;
            _context = new WheyMenContext();
        }
        [Theory]
        [InlineData(9, 10)]
        [InlineData(2, 20)]
        public void TestReasonableQuantity(int value, int val2)
        {
            Assert.True(BusinessValidation.ValidateQuantity(value, val2));
        }
        [Theory]
        [InlineData(10000000,10)]
        [InlineData(0,20)]
        public void TestUnreasonableQuantity(int value,int val2)
        {
            Assert.False(BusinessValidation.ValidateQuantity(value,val2));
        }
        [Fact]
        public void TestRemovalOfEmptyOrders()
        {
            var order_1 = new Order
            {
                CustId = 1,
                LocId = 1,
                Timestamp = DateTime.Now,
            };
            var order_2 = new Order
            {
                CustId = 1,
                LocId = 2,
                Timestamp = DateTime.Now,
            };
            //start order, add no items
            _context.Order.Add(order_1);
            _context.SaveChanges();

            //start another order under same cid
            _context.Order.Add(order_2);
            _context.SaveChanges();


            //check first order was removed
            _context.Entry<Order>(order_1).Reload();
            Assert.True((_context.Order.Find(order_1.Id))==null);

            //delete added orders
            //_context.Remove(_context.Order.Single(oi => oi.Id == order_1.Id));
            _context.Remove(_context.Order.Single(oi => oi.Id == order_2.Id));
            _context.SaveChanges();
        }

        [Fact]
        public void TestTotalCalulation()
        {
            //create order
            //add order line with value x
            //add order line with value y
            var order_1 = new Order
            {
                CustId = 1,
                LocId = 1,
                Total = 0,
                Timestamp = DateTime.Now,
            };
            _context.Order.Add(order_1);
            _context.SaveChanges();
            var item = new OrderItem //value = 15.2*4 = 60.8
            {
                Qty = 4,
                Pid = 5,
                Oid = order_1.Id
            };

            var item_2 = new OrderItem //value 3*12 = 36
            {
                Qty = 3,
                Pid = 6,
                Oid = order_1.Id
            };
            //test total is 60.8 + +36 = 96.8
            _context.OrderItem.AddRange(item, item_2);
            _context.SaveChanges();
            var order = _context.Order.Find(order_1.Id);
            _context.Entry<Order>(order_1).Reload();
            output.WriteLine($"o1.id:{order_1.Id} total:"+order_1.Total.ToString());
            Assert.True(order.Total==(Decimal)81.6);

            //remove added order/order lines
            _context.Remove(item);
            _context.Remove(item_2);
            _context.Remove(order_1);
            _context.SaveChanges();
        }
    }
}
