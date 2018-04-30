namespace MultiDialogsBot
{
    using System;
    using Microsoft.Bot.Builder.FormFlow;

    [Serializable]
    public class HotelsQuery
    {
        public enum ServiceOptions
        {
            CreateRedirect, RedirectURL
        }

        [Prompt("Please enter your {&}")]
        public ServiceOptions? Service;


        [Prompt("Please enter your {&}?")]
        public string Name { get; set; }

        [Prompt("Please enter your {&}?")]
        public string AccountID { get; set; }


        [Prompt("Please enter your Source URL?")]
        public string SourceURL { get; set; }

        [Prompt("Please enter your Destination URL?")]
        public string DestinationURL { get; set; }
    }
}