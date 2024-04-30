namespace MassTransitSaga.Settings
{
    public interface IDatabaseSettings
    {
        string Username { get; set; }
        string Password { get; set; }
        string Host { get; set; }
        string Database { get; set; }
        int? Port { get; set; }


    }
    public class DatabaseSettings : IDatabaseSettings
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Database { get; set; }
        public int? Port { get; set; }

    }


}
