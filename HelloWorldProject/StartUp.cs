namespace HelloWorldProject
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class StartUp
    {
        public Task Run(CancellationToken token = default)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    Console.WriteLine("Hello world");
                    await Task.Delay(1_000);
                }
            }, token);
        }
    }
}
