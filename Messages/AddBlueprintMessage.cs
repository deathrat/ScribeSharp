using System;
using ScribeSharp.Model;

namespace ScribeSharp.Messages
{
    public class AddBlueprintMessage
    {
        public BlueprintItem Item { get; set; } 

        public Action<bool, string> Callback { get; set; }

        public AddBlueprintMessage(Action<bool, string> callback)
        {
            Callback = callback;
        }
    }
}