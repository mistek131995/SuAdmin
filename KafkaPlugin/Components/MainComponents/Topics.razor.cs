using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Interfaces.Providers;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class Topics : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }
    
    private List<Models.Repositories.Topic>? _topics;

    protected override async Task OnInitializedAsync()
    {
        _topics = await _kafkaRepositoryProvider.TopicRepository.GetAllAsync();
    }
}