using System.Collections.Concurrent;
using System.Diagnostics;

namespace BlazorServerCrud1.Data.Messages
{
    public class MessageService:IMessageService
    {
        //public EventHandler<string>? OnMessage { get; set; }

        private ConcurrentDictionary<Guid, IMessageSubscriber> subscribers=new ConcurrentDictionary<Guid, IMessageSubscriber>();
        public MessageService()
        {
            //OnMessage?.Invoke(this, "hello world");
            //var l=OnMessage.GetInvocationList();
        }

        public bool Subscribe(Guid id,IMessageSubscriber subscriber)
        {
            Debug.WriteLine("message subscriber added");
            return subscribers.TryAdd(id, subscriber);
        }

        public bool UnSubscribe(Guid id, IMessageSubscriber subscriber)
        {
            
            if (subscribers.TryRemove(id, out IMessageSubscriber? sub))
            {
                Debug.WriteLine("subscriber removed");
                return true;
            }
            return false;
        }
                
        public void Send(string message)
        {
            foreach (KeyValuePair<Guid, IMessageSubscriber> sub in subscribers)
            {
                Debug.WriteLine(sub.Key);
                sub.Value.OnMessage(message);
            }

            //OnMessage?.Invoke(this, message);
        }

        
    }
        
}
