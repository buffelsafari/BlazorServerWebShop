namespace BlazorServerCrud1.Data.Messages
{
    public interface IMessageSubscriber
    {
        public void OnMessage(string msg);

    }
}
