using BlazorServerCrud1.Data.Messages;
using CosmosDBData;
using CosmosDBService.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Reflection;

namespace BlazorServerCrud1.Pages
{


    public partial class Index : ComponentBase, IDisposable
    {
        [Parameter]
        public string? message { get; set; }

        [Parameter]
        public List<string> Chat { get; set; } = new List<string>();

        public string? sendit { get; set; }

        private Guid subscriberId = Guid.NewGuid();

        protected override async Task OnInitializedAsync()
        {
            messageService.Subscribe(subscriberId, this);

            

            //messageService.OnMessage -= OnMessage;
            //messageService.OnMessage += OnMessage;

            await base.OnInitializedAsync();

        }

        protected override async Task OnParametersSetAsync()
        {
            

            await base.OnParametersSetAsync();
            
            

        }

        private void Submit()
        {
            messageService.Send(new Message 
            { 
                Id=Guid.NewGuid(),
                SenderId=subscriberId,
                Created=DateTime.Now,
                Title="TestMessage",
                Text=sendit
            
            });
        }

        


        public void Dispose()
        {
            messageService.UnSubscribe(subscriberId, this);
            //messageService.OnMessage -= OnMessage;                        
        }

        public void OnMessage(string msg) 
        {
            Debug.WriteLine("message" + msg);
            InvokeAsync(() =>
            {
                message = msg;
                Chat.Add(msg);
                StateHasChanged();
            });
        }

    }

    
}
