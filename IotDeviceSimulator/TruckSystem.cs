using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureIoT.TempDevice
{
    public class TruckSystem
    {
        //Connection string for device to cloud messaging
        private static readonly string connectionString_IoTHub = "HostName=AdarshEdgeIoTHub.azure-devices.net;DeviceId=truckDevice;SharedAccessKey=tF/GoKij/k/obMNn36xZVsbzBxFA4g6vqOFWDLIakdk=";

        //Device Client
        static DeviceClient? truckDeviceClient;

        //Random Generator
        static Random random = new Random();

        //truck sensor details
        static string deviceID = Guid.NewGuid().ToString();
        const double truckTemperature_min = 20;
        const double truckTemperature_max = 40;
        static double truckTemperature = 20;
        const double truckLatitude_min = 80;
        const double truckLatitude_max = 120;
        static double truckLatitude = 80;
        const double truckLongitude_min = 80;
        const double truckLongitude_max = 120;
        static double truckLongitude = 80;
        static DateTime eventEnqueueUtcTime = DateTime.UtcNow;

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.WriteLine("Press CTRL+C to stop the simulation");
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Stopping the Application....");
                cts.Cancel();
                e.Cancel = true;
            };

            truckDeviceClient = DeviceClient.CreateFromConnectionString(connectionString_IoTHub);

            SendMessagesToIoTHub(truckDeviceClient, cts.Token);

            Console.ReadLine();

        }

        private static async void SendMessagesToIoTHub(DeviceClient? truckDeviceClient, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                truckLatitude = GenerateSensorReading(truckLatitude, truckLatitude_min, truckLatitude_max);
                truckLongitude = GenerateSensorReading(truckLongitude, truckLongitude_min, truckLongitude_max);
                truckTemperature = GenerateSensorReading(truckTemperature, truckTemperature_min, truckTemperature_max);

                var json = CreateJSON(deviceID, truckTemperature, truckLatitude, truckLongitude, eventEnqueueUtcTime);
                var message = CreateMessage(json);
                await truckDeviceClient.SendEventAsync(message, token);
                Console.WriteLine($"Sending message at {DateTime.Now} and Message : {json}");
                await Task.Delay(5000);
            }
        }

        private static double GenerateSensorReading(double currentValue, double min, double max)
        {
            double percentage = 5; // 5%

            // generate a new value based on the previous supplied value
            // The new value will be calculated to be within the threshold specified by the "percentage" variable from the original number.
            // The value will also always be within the the specified "min" and "max" values.
            double value = currentValue * (1 + ((percentage / 100) * (2 * random.NextDouble() - 1)));

            value = Math.Max(value, min);
            value = Math.Min(value, max);

            return value;
        }

        private static string CreateJSON(string deviceID, double tempdata, double lattitudedata, double longitude, DateTime eventEnqueueUtcTime)
        {
            var data = new
            {
                deviceId = deviceID,
                temp = tempdata,
                lat = lattitudedata,
                lngt = longitude,
                eventEnqueuedUtcTime = eventEnqueueUtcTime
            };
            return JsonConvert.SerializeObject(data);
        }

        private static Message CreateMessage(string jsonObject)
        {
            var message = new Message(Encoding.ASCII.GetBytes(jsonObject));

            // MESSAGE CONTENT TYPE
            message.ContentType = "application/json";
            message.ContentEncoding = "UTF-8";

            return message;
        }
    }
}
