using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Domain;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger _logger;

        public OrdersController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Add(Order order)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "orderQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message = JsonSerializer.Serialize(order);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "orderQueue",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" [x] Sent {0}", message);

                }

                return Accepted(order);
            }
            catch (Exception e)
            {
                _logger.LogError("Algo deu errado ao tentar criar um pedido",e);
                return new StatusCodeResult(500);
            }
        }
    }
}
