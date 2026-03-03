using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;

namespace Ordering.API.Persistence
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrdersRepository(IMongoClient mongoClient)
        {   
            _ordersCollection = mongoClient.GetDatabase("OrdersDb").GetCollection<Order>("Orders");
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var sortingByDate = Builders<Order>.Sort.Descending(d => d.CreatedOn);
            var pipeline = GetPipelineForOrder().Sort(sortingByDate);

            return await _ordersCollection.AggregateAsync(pipeline).Result.ToListAsync();
        }

        public async Task<Order> GetAsync(string id)
        {
            var pipeline = GetPipelineForOrder().Match(order => order.Id == id);

            return await _ordersCollection
                .AggregateAsync(pipeline)
                .Result
                .FirstOrDefaultAsync();
        }

        public async Task<Result<Order>> CreateAsync(Order order)
        {
            try
            {
                _ordersCollection.InsertOneAsync(order).Wait();
            }
            catch (Exception e)
            {
              return Result<Order>.Fail(e.Message);
            }

            var orderForReturn = await GetAsync(order.Id);

            if (orderForReturn == null)
            {
                return Result<Order>.Fail("Something went wrong, could not create order.");
            }

            return Result<Order>.Ok(orderForReturn);
        }

        private static PipelineDefinition<Order, Order> GetPipelineForOrder()
        {

            PipelineDefinition<Order, Order> pipeline = new[]
            {
               new BsonDocument("$lookup",
                   new BsonDocument
                   {
                       {"from", "Status"},
                       {"localField", "orderStatusId"},
                       {"foreignField", "_id"},
                       {"as", "status"}
                   }),
               new BsonDocument("$lookup",
                   new BsonDocument
                   {
                       {"from", "PaymentTypes"},
                       {"localField", "paymentTypeId"},
                       {"foreignField", "_id"},
                       {"as", "paymentType"}
                   }),
               new BsonDocument("$unwind",
                   new BsonDocument("path", "$status")),
               new BsonDocument("$unwind",
                   new BsonDocument("path", "$paymentType"))
           };

            return pipeline;
        }
    }
}

//var ordersResult = await _ordersCollection.Aggregate()
//    .Lookup(_statusCollection, order => order.StatusId, status => status.Id, (Order order) => order.Status)
//    .Lookup(_paymentTypeCollection, order => order.PaymentTypeId, paymentType => paymentType.Id, (Order order) =>        order.PaymentType)
//    .Unwind(order => order.Status)
//    .Unwind<BsonDocument, Order>(order => order["paymentType"])
//    .ToListAsync();