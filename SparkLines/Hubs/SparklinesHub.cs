using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SparkLines.Hubs
{
    public class SparklinesHub : Hub
    {
        static ConcurrentDictionary<string, List<int>> Values = new ConcurrentDictionary<string, List<int>>();

        static int maximumValues = 49;

        public void PublishMetric(string name, int value)
        {
            if (IsAuthorised())
            {
                // Update the dictionary with new data.
                Values.AddOrUpdate(name, new List<int>() { value }, (k, v) =>
                    {
                        int skip = v.Count > maximumValues ? v.Count - maximumValues : 0;
                        return v.Skip(skip).Concat(new int[] { value }).ToList();
                    });

                var valuesToSend = new List<int>() { };
                Values.TryGetValue(name, out valuesToSend);

                // Publish the metrics data to connected clients.
                Clients.All.metricsUpdated(name, valuesToSend);
            }
        }

        private bool IsAuthorised()
        {
            return base.Context.Headers["PublisherKey"] == "xy123123";
        }
    }
}