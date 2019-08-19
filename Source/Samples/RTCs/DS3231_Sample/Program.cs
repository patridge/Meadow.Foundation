using Meadow;
using System.Threading;

namespace DS3231_Sample
{
    class Program
    {
        static IApp app;

        static void Main(string[] args)
        {
            app = new DS3231App();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
