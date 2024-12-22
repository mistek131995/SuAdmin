using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Contracts.Components;
using KafkaPlugin.Database;
using KafkaPlugin.Database.Database;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class AddHostModal : ComponentBase
{
    [Parameter] public EventCallback OnHostAdded { get; set; }
    
    private Modal? _modalRef;
    private AddHostModel _addHostModel = new();

    private RenderFragment? _openModalButton;
    private RenderFragment? _closeModalButton;

    protected override void OnInitialized()
    {
        _closeModalButton = builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "btn btn-secondary btn-sm");
            builder.AddAttribute(2, "type", "button");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => _modalRef?.Hide()));
            builder.AddContent(4, "Закрыть");
            builder.CloseElement();
        };

        _openModalButton = builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "btn btn-success btn-sm");
            builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, () => _modalRef?.Show()));
            builder.AddContent(3, "Добавить хост");
            builder.CloseElement();
        };
    }
    
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
            Ip = _addHostModel.Ip,
            Port = _addHostModel.Port,
        });
        
        await context.SaveChangesAsync();
        
        _modalRef?.Hide();
    }
    
    private class AddHostModel
    {
        [Required(ErrorMessage = "Заполните IP адрес")]
        public string Ip { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Заполните порт")]
        public int Port { get; set; } = 0;
    }
}