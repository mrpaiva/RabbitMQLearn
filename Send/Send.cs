using RabbitMQ.Client;
using System.Text;
using System.Web;

var encodedPassword = HttpUtility.UrlEncode("********");
var uri=new Uri($"amqp://mrpaiva:{encodedPassword}@localhost/");

var factory = new ConnectionFactory{
    Uri = uri,
    AutomaticRecoveryEnabled = true,
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

const string message = "Hello Man!";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);

Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();