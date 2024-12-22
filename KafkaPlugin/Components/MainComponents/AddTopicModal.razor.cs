using Contracts.Components;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class AddTopicModal : ComponentBase
{
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

    private class AddTopicModel
    {
        public string Name { get; set; } = string.Empty;
        public int Partition { get; set; } = 0;
        public int ReplicationFactor { get; set; } = 0;
    }
}