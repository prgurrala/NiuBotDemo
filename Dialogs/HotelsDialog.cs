namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class HotelsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Welcome to the Hotels finder!");

            var hotelsFormDialog = FormDialog.FromForm(this.BuildHotelsForm, FormOptions.PromptInStart);

            context.Call(hotelsFormDialog, this.ResumeAfterHotelsFormDialog);
        }

        private IForm<HotelsQuery> BuildHotelsForm()
        {
            OnCompletionAsyncDelegate<HotelsQuery> processHotelsSearch = async (context, state) =>
            {
                await context.PostAsync($"Ok. Searching for Hotels in {state.Destination} from {state.CheckIn.ToString("MM/dd")} to {state.CheckIn.AddDays(state.Nights).ToString("MM/dd")}...");
            };

            return new FormBuilder<HotelsQuery>()
                .Field(nameof(HotelsQuery.Destination))
                .Message("Looking for hotels in {Destination}...")
                .AddRemainingFields()
                .OnCompletion(processHotelsSearch)
                .Build();
        }

        private async Task ResumeAfterHotelsFormDialog(IDialogContext context, IAwaitable<HotelsQuery> result)
        {
            try
            {
                var searchQuery = await result;

                var hotels = await this.GetHotelsAsync(searchQuery);

                await context.PostAsync($"I found in total {hotels.Count()} hotels for your dates:");

                var resultMessage = context.MakeMessage();
                resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                resultMessage.Attachments = new List<Attachment>();

                foreach (var hotel in hotels)
                {
                    HeroCard heroCard = new HeroCard()
                    {
                        Title = hotel.Name,
                        Subtitle = $"{hotel.Rating} starts. {hotel.NumberOfReviews} reviews. From ${hotel.PriceStarting} per night.",
                        Images = new List<CardImage>()
                        {
                            new CardImage() { Url = hotel.Image }
                        },
                        Buttons = new List<CardAction>()
                        {
                            new CardAction()
                            {
                                Title = "More details",
                                Type = ActionTypes.OpenUrl,
                                Value = $"https://www.bing.com/search?q=hotels+in+" + HttpUtility.UrlEncode(hotel.Location)
                            }
                        }
                    };

                    resultMessage.Attachments.Add(heroCard.ToAttachment());
                }

                await context.PostAsync(resultMessage);
            }
            catch (FormCanceledException ex)
            {
                string reply;

                if (ex.InnerException == null)
                {
                    reply = "You have canceled the operation. Quitting from the HotelsDialog";
                }
                else
                {
                    reply = $"Oops! Something went wrong :( Technical Details: {ex.InnerException.Message}";
                }

                await context.PostAsync(reply);
            }
            finally
            {
                context.Done<object>(null);
            }
        }

        private async Task<IEnumerable<Hotel>> GetHotelsAsync(HotelsQuery searchQuery)
        {
            var hotels = new List<Hotel>();

            // Filling the hotels results manually just for demo purposes
            for (int i = 1; i <= 5; i++)
            {
                var random = new Random(i);
                Hotel hotel = new Hotel()
                {
                    Name = $"{searchQuery.Destination} Hotel {i}",
                    Location = searchQuery.Destination,
                    Rating = random.Next(1, 5),
                    NumberOfReviews = random.Next(0, 5000),
                    PriceStarting = random.Next(80, 450),
                    Image = $"https://placeholdit.imgix.net/~text?txtsize=35&txt=Hotel+{i}&w=500&h=260"
                };

                hotels.Add(hotel);
            }

            hotels.Sort((h1, h2) => h1.PriceStarting.CompareTo(h2.PriceStarting));

            return hotels;
        }
    }
}


///// <summary>
///// Simple Dialog, that invokes the QnAMaker if the incoming message is a question
///// </summary>
//[Serializable]
//public class FAQDialog : IDialog<object>
//{
//    public async Task StartAsync(IDialogContext context)
//    {
//        context.Wait(MessageReceivedAsync);
//    }
//    public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
//    {
//        var message = await argument;
//        await context.PostAsync(WebUtility.HtmlDecode(CallQnAMaker(message.Text)));
//        context.Wait(MessageReceivedAsync);
//    }
//    public string CallQnAMaker(string query)
//    {
//        string responseString = string.Empty;
//        var knowledgebaseId = < your KB ID>; // Use knowledge base id created.
//        var qnamakerSubscriptionKey = < your subscription key>; //Use subscription key assigned to you.
//                                                                //Build the URI
//        Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
//        var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");
//        //Add the question as part of the body
//        var postBody = $"{{\"question\": \"{query}\"}}";
//        //Send the POST request
//        using (WebClient client = new WebClient())
//        {
//            client.Encoding = System.Text.Encoding.UTF8;
//            //Add the subscription key header
//            client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
//            client.Headers.Add("Content-Type", "application/json");
//            responseString = client.UploadString(builder.Uri, postBody);
//        }
//        QnAMakerResult response;
//        try
//        {
//            response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
//        }
//        catch
//        {
//            throw new Exception("Unable to deserialize QnA Maker response string.");
//        }
//    }
//    //Callback, after the QnAMaker Dialog returns a result.
//    public async Task AfterQnA(IDialogContext context, IAwaitable<object> argument)
//    {
//        context.Wait(MessageReceivedAsync);
//    }
//}
//class QnAMakerResult
//{
//    /// <summary>
//    /// The top answer found in the QnA Service.
//    /// </summary>
//    [JsonProperty(PropertyName = "answer")]
//    public string Answer { get; set; }
//    /// <summary>
//    /// The score in range [0, 100] corresponding to the top answer found in the QnA    Service.
//    /// </summary>
//    [JsonProperty(PropertyName = "score")]
//    public double Score { get; set; }
//}