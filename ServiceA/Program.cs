// See https://aka.ms/new-console-template for more information
using Shared.RabbitMq;

Console.WriteLine("Service B Listener");

string rabbitMqConnectionString = "amqp://guest:guest@localhost:5672/";
string serviceBQueueName = "serviceAQueue";

using var rabbitMqConsumer = new RabbitMqConsumer(rabbitMqConnectionString, serviceBQueueName);
rabbitMqConsumer.StartConsuming();

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();
