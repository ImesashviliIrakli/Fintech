// See https://aka.ms/new-console-template for more information
using Shared.RabbitMq;

Console.WriteLine("Service A Listener");

string serviceBQueueName = "serviceAQueue";

using var rabbitMqConsumer = new RabbitMqConsumer(serviceBQueueName);
rabbitMqConsumer.StartConsuming();

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
