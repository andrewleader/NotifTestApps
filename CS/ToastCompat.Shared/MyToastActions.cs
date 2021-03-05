using System;
using System.Collections.Generic;
using System.Text;

namespace ToastCompat
{
    public enum MyToastActions
    {
        /// <summary>
        /// View the conversation
        /// </summary>
        ViewConversation,

        /// <summary>
        /// Inline reply to a message
        /// </summary>
        Reply,

        /// <summary>
        /// Like a message
        /// </summary>
        Like,

        /// <summary>
        /// View the image included in the message
        /// </summary>
        ViewImage
    }
}
