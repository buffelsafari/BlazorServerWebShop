namespace BlazorServerCrud1.Data.Messages
{
    public interface IMessageService
    {
        //EventHandler<String>? OnMessage { get; set; }

        bool Subscribe(Guid id, IMessageSubscriber subscriber);
        
        bool UnSubscribe(Guid id, IMessageSubscriber subscriber);

        public void Send(string message);


    }
}
