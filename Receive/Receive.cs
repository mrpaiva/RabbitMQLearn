﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Web;


var encodedPassword = HttpUtility.UrlEncode("!Angelica@401!");
var uri=new Uri($"amqp://mrpaiva:{encodedPassword}@rabbit.virtusit.co/");

var factory = new ConnectionFactory{
    Uri = uri,
    AutomaticRecoveryEnabled = true,
    NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine("[*] Waiting for messages.");

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"[x] Received {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync("hello", autoAck: true, consumer);

Console.WriteLine("Press [enter] to exit");
Console.ReadLine();