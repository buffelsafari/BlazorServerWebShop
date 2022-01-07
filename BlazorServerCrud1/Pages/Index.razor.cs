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
            messageService.Send(sendit);
        }

        //private void Enter(KeyboardEventArgs args)
        //{
        //    if (args.Code == "Enter" || args.Code == "NumpadEnter")
        //    {
        //        messageService.Send(sendit);
        //    }
        //}

        //private void OnMessage(object? sender, string msg)
        //{ 
        //    Debug.WriteLine("message"+msg);
        //    InvokeAsync(() => 
        //    {
        //        message=msg;
        //        StateHasChanged();
        //    });
            
            
        //}


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
                StateHasChanged();
            });
        }

    }

    
}
