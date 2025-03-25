

using ChatAppServer.Util;
using System.Text.Json.Serialization;

namespace ChatAppServer.Model
{
    public enum MessageEventTypeEnum
    {
        /** Indicates a new document was added to the set of documents matching the query. */
        ADDED = 0,
        /** Indicates a document within the query was modified. */
        MODIFIED = 1,

        /**
         * Indicates a document within the query was removed (either deleted or no longer matches the
         * query.
         */
        DELETE = 2,
        DELETE_FOR_BOTH = 3,
        NONE=-1

    }
    public class SendMessageArgs
    {
        /// <summary>
        /// Get message event type as String representation
        /// </summary>
        public string MessageEventTypeEnum { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
      
        public string MessageJson { get; set; }
        public string MessageId { get; set; }
        public string MessagePublicKey { get; set; }


        // Константы для MessageEventType
        // public const string ADDED = "ADDED";
        // public const string MODIFIED = "MODIFIED";
        // public const string DELETE = "DELETE";
        // public const string DELETE_FOR_BOTH = "DELETE_FOR_BOTH";

        public SendMessageArgs()
        {

        }
       

    }
}
