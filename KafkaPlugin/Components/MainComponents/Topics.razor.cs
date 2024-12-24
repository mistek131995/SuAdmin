using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Interfaces.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace KafkaPlugin.Components.MainComponents;

public partial class Topics : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }
    
    [Parameter] public EventCallback UpdateMainPageEvent { get; set; }
    [Parameter] public List<Models.Repositories.Topic>? topics { get; set; }

    private RenderFragment<string> _deleteTopicsButton;

    protected override void OnInitialized()
    {
        _deleteTopicsButton = (topicName) => builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "btn btn-secondary btn-sm");
            builder.AddAttribute(2, "type", "button");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, async () => await DeleteTopicAsync(topicName)));
            builder.AddContent(4, (MarkupString)"<i class=\"bi bi-trash\"></i>");
            builder.CloseElement();
        };
    }

    private async Task DeleteTopicAsync(string topicName)
    {
        await _kafkaRepositoryProvider.TopicRepository.DeleteAsync(topicName);
        
        await UpdateMainPageEvent.InvokeAsync();
    }
}