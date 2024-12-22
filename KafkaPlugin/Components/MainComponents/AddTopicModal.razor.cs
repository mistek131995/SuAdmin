using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Contracts.Components;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class AddTopicModal : ComponentBase
{
    [Parameter] public EventCallback OnTopicAdded { get; set; }
    
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }
    
    private Modal? _modalRef;
    private AddTopicModel _model = new();
    
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

    public async Task OnSubmitAsync()
    {
        await _kafkaRepositoryProvider.TopicRepository.CreateAsync(new Topic()
        {
            Name = _model.Name,
            ReplicationFactor = _model.ReplicationFactor,
            PartitionCount = _model.PartitionCount,
        });
        
        _modalRef?.Hide();
        await OnTopicAdded.InvokeAsync();
    } 

    private class AddTopicModel
    {
        [Required(ErrorMessage = "Заполните имя топика")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Заполните кол-во партиций"), Range(1, int.MaxValue, ErrorMessage = "Диапазон значений фактора репликации от 1 до 2147483647")]
        public int PartitionCount { get; set; } = 1;
        
        [Required(ErrorMessage = "Заполните фактор репликации"), Range(1, short.MaxValue, ErrorMessage = "Диапазон значений фактора репликации от 1 до 32767")]
        public short ReplicationFactor { get; set; } = 1;
    }
}