﻿using Glimesh.Base.Models.Users;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Glimesh.Base.Models.Clients.Chat
{
    /// <summary>
    /// Packet for chat messag received.
    /// </summary>
    public class ChatMessagePacketModel : ChatResponsePacketModel
    {
        /// <summary>
        /// The ID of the message
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The ID of the channel the message was sent in.
        /// </summary>
        public string ChannelID { get; set; }

        /// <summary>
        /// The user that sent the message.
        /// </summary>
        public UserModel User { get; set; }

        /// <summary>
        /// The plain-text message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The individual tokens of the message.
        /// </summary>
        public List<ChatMessageTokenModel> MessageTokens { get; set; } = new List<ChatMessageTokenModel>();

        /// <summary>
        /// The datetime the message was sent.
        /// </summary>
        public string SentAt { get; set; }

        /// <summary>
        /// Creates a new instance of the ChatMessagePacketModel class.
        /// </summary>
        /// <param name="serializedChatPacketArray">The serialized chat packet array</param>
        public ChatMessagePacketModel(string serializedChatPacketArray)
            : base(serializedChatPacketArray)
        {
            JObject chatMessage = (JObject)this.Payload.SelectToken("result.data.chatMessage");
            if (chatMessage != null)
            {
                this.ID = chatMessage.SelectToken("id")?.ToString();
                this.ChannelID = chatMessage.SelectToken("channel.id")?.ToString();

                JToken user = chatMessage.SelectToken("user");
                if (user != null)
                {
                    this.User = user.ToObject<UserModel>();
                }

                this.Message = chatMessage.SelectToken("message")?.ToString();

                JArray messageTokens = (JArray)chatMessage.SelectToken("tokens");
                if (messageTokens != null)
                {
                    foreach (JToken messageToken in messageTokens)
                    {
                        this.MessageTokens.Add(messageToken.ToObject<ChatMessageTokenModel>());
                    }
                }

                this.SentAt = chatMessage.SelectToken("insertedAt")?.ToString();
            }
        }
    }

    /// <summary>
    /// A token of a chat message.
    /// </summary>
    public class ChatMessageTokenModel
    {
        /// <summary>
        /// The type of token.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// The plain-text of the token.
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// The relative source path of the token.
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// The full url path of the token.
        /// </summary>
        public string url { get; set; }
    }
}
