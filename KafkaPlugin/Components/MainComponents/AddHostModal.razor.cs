using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Contracts.Components;
using KafkaPlugin.Database;
using KafkaPlugin.Database.Database;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KafkaPlugin.Components.MainComponents;

public partial class AddHostModal : ComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; }
    
    private Modal? ModalRef;
    private AddHostModel addHostModel = new();
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var dotNetRef = DotNetObjectReference.Create(this);
            
            await JSRuntime.InvokeVoidAsync("addHostBtnHandler", "addHost", dotNetRef);
            await JSRuntime.InvokeVoidAsync("closeModal", "closeModal", dotNetRef);
        }
    }
    
    [JSInvokable]
    public void AddHostAsync()
    {
        Console.WriteLine("Метод OnButtonClicked вызван.");
        ModalRef?.Show();
    }

    [JSInvokable]
    public void CloseModal() => ModalRef?.Hide();
    
    private Task HandleClose()
    {
        Console.WriteLine("Модальное окно закрыто.");
        return Task.CompletedTask;
    }

    private async Task OnSubmitHandlerAsync()
    {
        await using var context = new Context();

        context.Add(new Host()
        {
            Ip = addHostModel.Ip,
            Port = addHostModel.Port,
        });
        
        await context.SaveChangesAsync();
        
        Console.WriteLine(addHostModel.Ip + ":" + addHostModel.Port);
        
        CloseModal();
    }
    
    private class AddHostModel
    {
        [Required(ErrorMessage = "Заполните IP адрес")]
        public string Ip { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Заполните порт")]
        public int Port { get; set; } = 0;
    }
}