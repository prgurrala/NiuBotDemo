namespace MultiDialogsBot
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Dialogs;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;
    using MultiDialogsBot.FormFlow;
    using MultiDialogsBot.Models;

    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public virtual async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //Without using any dialogs, send a message back to user

                //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //Activity reply = activity.CreateReply($"From Bot : You said {activity.Text}");
                //await connector.Conversations.ReplyToActivityAsync(reply);

                //Call Greeting Dialog

                //await Conversation.SendAsync(activity, () => new GreetingDialog());

                //Call HoteBotDialog which is using Forms

                //await Conversation.SendAsync(activity, () => HotelBotDialog.dialog);

                //Call Luis Dialog

                //await Conversation.SendAsync(activity, MakeLuisDialog);

                //Call Enquiry Form Flow 

                await Conversation.SendAsync(activity, () => { return Chain.From(() => FormDialog.FromForm(Enquiry.BuildEnquiryForm)); });

                //await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                this.HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private IDialog<RoomReservation> MakeLuisDialog()
        {
            return Chain.From(() => new LUISDialog(RoomReservation.BuildForm));
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}