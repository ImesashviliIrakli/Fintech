using Shared.RabbitMq;

Console.WriteLine("Service A Listener");

string serviceBQueueName = "serviceAQueue";

using var rabbitMqConsumer = new RabbitMqConsumer(serviceBQueueName);
rabbitMqConsumer.StartConsuming();

Console.WriteLine("Press Ctrl+C to exit.");

var cancellationTokenSource = new CancellationTokenSource();
AppDomain.CurrentDomain.ProcessExit += (s, e) => cancellationTokenSource.Cancel();
Console.CancelKeyPress += (s, e) => {
    e.Cancel = true;
    cancellationTokenSource.Cancel();
};

Task.Run(() =>
{
    while (!cancellationTokenSource.Token.IsCancellationRequested)
    {
        Thread.Sleep(1000);
    }
}).Wait();