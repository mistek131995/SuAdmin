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
    private Modal? ModalRef;
    private AddHostModel addHostModel = new();

    private RenderFragment OpenModalButton;
    private RenderFragment CloseModalButton;

    protected override void OnInitialized()
    {
        CloseModalButton = builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "btn btn-secondary");
            builder.AddAttribute(2, "type", "button");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => ModalRef?.Hide()));
            builder.AddContent(4, "Закрыть");
            builder.CloseElement();
        };

        OpenModalButton = builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "btn btn-success");
            builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, () => ModalRef?.Show()));
            builder.AddContent(3, "Добавить хост");
            builder.CloseElement();
        };
    }
    public void AddHostAsync()
    {
        ModalRef?.Show();
    }
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