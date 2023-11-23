


namespace CommandService.API.AsyncDataServices
{
    public class SampleBackgroundService : IHostedService, IDisposable
    {
        private Timer _timer;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            // This method is called when the application starts
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            // This method will be called periodically by the timer
            Console.WriteLine("Background task is running...");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // This method is called when the application is shutting down
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
        public void Dispose()
        {
            // Dispose of resources if needed
            _timer?.Dispose();
        }
    }
}
