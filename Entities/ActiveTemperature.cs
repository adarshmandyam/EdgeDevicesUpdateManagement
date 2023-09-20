namespace Entities
{
    public class ActiveTemperature
    {
        public class Ambient
        {
            public double Temperature { get; set; }

            public int Humidity { get; set; }
        }

        public class Machine
        {
            public double Temperature { get; set; }

            public double Pressure { get; set; }
        }

        public class MessageBody
        {
            public Machine? Machine { get; set; }

            public Ambient? Ambient { get; set; }

            public DateTime TimeCreated { get; set; }
        }
    }
}