// See https://aka.ms/new-console-template for more information
using Shared.RabbitMq;

Console.WriteLine("Service B Listener");

string serviceBQueueName = "serviceBQueue";

using var rabbitMqConsumer = new RabbitMqConsumer(serviceBQueueName);
rabbitMqConsumer.StartConsuming();

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
