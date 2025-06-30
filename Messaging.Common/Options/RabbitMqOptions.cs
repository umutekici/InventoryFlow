namespace Messaging.Common.Options
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string VirtualHost { get; set; } = null!;
    }
}
