using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = Task.Run(() => Main());
            t.Wait();
        }

        static async Task Main()
        {
            var noise = new PerlinNoise();

            int valuesPublished = 0;

            using (var connection = new HubConnection("http://localhost:50772/signalr"))
            {
                // Custom header for the password.
                connection.Headers.Add("PublisherKey", "xy123123");

                // Push "metrics" data to the hub.
                var proxy = connection.CreateHubProxy("sparklinesHub");

                await connection.Start();

                string noiseKey = "noise";

                for (double d = 0.0d; d < double.MaxValue; d += 0.10d)
                {
                    var noiseValue = Map(noise.Noise(d), -1, 1, 0, 100);

                    try
                    {
                        await proxy.Invoke("publishMetric", noiseKey, (int)noiseValue);
                        valuesPublished++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    await Task.Delay(200);

                    if (valuesPublished % 10 == 0)
                    {
                        Console.WriteLine("{0} values published.", valuesPublished);
                    }
                }
            }
        }

        public static double Map(double value, double sourceRangeMinimum, double sourceRangeMaximum, double targetRangeMinimum, double targetRangeMaximum)
        {
            if ((sourceRangeMaximum - sourceRangeMinimum) == 0)
            {
                return (targetRangeMinimum + targetRangeMaximum) / 2;
            }
            return targetRangeMinimum + (value - sourceRangeMinimum) * (targetRangeMaximum - targetRangeMinimum) / (sourceRangeMaximum - sourceRangeMinimum);
        }
    }
}
