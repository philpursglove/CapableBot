using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CapableBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                if (activity.Text.ToLower().Contains("markdown"))
                {
                    Activity markdownReply = activity.CreateReply();
                    activity.Text = "#H1 I Am A Header";
                    await connector.Conversations.ReplyToActivityAsync(markdownReply);
                    activity.Text = "I am **bold**. I am *italic*.";

                    await connector.Conversations.ReplyToActivityAsync(markdownReply);
                }

                if (activity.Text.ToLower().Contains("url"))
                {
                    Activity urlReply = activity.CreateReply();
                    urlReply.Text = "[Azure](http://azure.com)";

                    await connector.Conversations.ReplyToActivityAsync(urlReply);
                }

                if (activity.Text.ToLower().Contains("image"))
                {
                    Activity imageReply = activity.CreateReply();
                    imageReply.Text = "![Robot](" + Url.Content() + ")";

                    await connector.Conversations.ReplyToActivityAsync(imageReply);
                }

                if (activity.Text.ToLower().Contains("carousel"))
                {
                    Activity carouselReply = activity.CreateReply();
                    carouselReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                    await connector.Conversations.ReplyToActivityAsync(carouselReply);
                }

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
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