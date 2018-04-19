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
        [Prompt("What is your Company Name?")]
        public string Company { get; set; }
        [Prompt("What is your job title there??")]
        public string JobTitle { get; set; }
        [Prompt("What is best number to contact you?")]
        public  string Phone { get; set; }
        [Prompt("Can we help you with anything else?")]
        public string HowCanIHelp { get; set; }
        [Prompt("Would you like us to add to our Email List? {||}")]
        public bool SignMeUpToTheEmailList { get; set; }
        [Prompt("Which Service are you interested in? {||}")]
        public Service ServiceRequired { get; set; }
        public enum Service { Consultancy, Support, ProjectDelivery, Other}

        public static IForm<Enquiry> BuildEnquiryForm()
        {
            return new FormBuilder<Enquiry>()
                .Field("SignMeUpToTheEmailList")
                .Field("ServiceRequired")
                .AddRemainingFields()
                .Build();
        }
    }
}