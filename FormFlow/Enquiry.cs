using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiDialogsBot.FormFlow
{
    [Serializable]
    public class Enquiry
    {
        [Prompt("What is Your Name?")]
        public string Name { get; set; }
        [Prompt("What is your Account ID?")]
        public string AccountID { get; set; }
        [Prompt("What is Source URL?")]
        public string SourceURL { get; set; }
        [Prompt("What is Destination URL?")]
        public string DestinationURL { get; set; }
        //[Prompt("Can we help you with anything else?")]
        //public string HowCanIHelp { get; set; }
        //[Prompt("Would you like us to add to our Email List? {||}")]
        //public bool SignMeUpToTheEmailList { get; set; }
        [Prompt("Which Service are you interested in? {||}")]
        public Service ServiceRequired { get; set; }
        public enum Service { RedirectURL, Redirect, Firewall, Other}

        public static IForm<Enquiry> BuildEnquiryForm()
        {
            OnCompletionAsyncDelegate<Enquiry> processHotelsSearch = async (context, state) =>
            {
                await context.PostAsync($"Thank you {state.Name}. Account ID : {state.AccountID} Your request will be processed soon. A redirect will be added from  {state.SourceURL} to {state.DestinationURL}. Enter any key to restart bot conversation...");
            };

            return new FormBuilder<Enquiry>()
                .Message("Please enter details for DoIT to process your request....")
                .Field("Name")
                .Field("AccountID")
                .Field("ServiceRequired")
                .AddRemainingFields()
                .OnCompletion(processHotelsSearch)
                .Build();
        }
    }
}