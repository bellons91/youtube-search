namespace YouTubeSearch
{
    class Configurations
    {
        public string ApiKey { get; private set; }
        public string ChannelId { get; private set; }

        private Configurations() { }

        public static Configurations GetConfig()
        {
            return new Configurations
            {
                ApiKey =   "<my-api-key>",
                ChannelId =  "<my-channel-id>"
            };
        }

    }
}
