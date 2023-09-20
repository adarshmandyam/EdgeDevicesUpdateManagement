using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using Azure.Messaging.EventHubs;

namespace EdgeHubTriggerFunctions
{
    public class IoTHubToSignalR
    {
        private static HttpClient client = new HttpClient();

        public IoTHubToSignalR() 
        { 
            
        }

        [FunctionName("iothubtrigger")]
        public static async Task FilterMessageAndSendMessage(
            [IoTHubTrigger("messages/events", 
            Connection = "IoTHubConnectionString",
            ConsumerGroup = "iot-platform-func"
            )] EventData message,
            [SignalR(HubName = "broadcast")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.ToArray())}");
            
            var deviceData = JsonConvert.DeserializeObject<DeviceData>(Encoding.UTF8.GetString(message.Body.ToArray()));
            //deviceData.DeviceId = Convert.ToString(message.SystemProperties["iothub-connection-device-id"]);

            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "iotMessage",
                Arguments = new[] { JsonConvert.SerializeObject(deviceData) }
            })
            .ConfigureAwait(false);
        }        

        //private static void SaveToDatabase(MessageBody messageBody)
        //{
        //    try
        //    {
        //        using SqlConnection con = new SqlConnection(Environment.GetEnvironmentVariable("SQLConnectionString"));
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            con.Open();
        //            cmd.Connection = con;
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.CommandText = "usp_edge_devices_insert";
        //            cmd.Parameters.AddWithValue("@Temperature", messageBody.Machine.Temperature);
        //            cmd.Parameters.AddWithValue("@Pressure", messageBody.Machine.Pressure);
        //            cmd.Parameters.AddWithValue("@Humidity", messageBody.Ambient.Humidity);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }            
        //}
    }
}