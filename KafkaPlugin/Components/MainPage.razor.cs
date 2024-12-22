using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components;

public partial class MainPage : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }
    
    private List<Broker>? _brokers;
    private List<Topic>? _topics;
    
    protected override async Task OnInitializedAsync()
    {
        _brokers = await _kafkaRepositoryProvider.BrokerRepository.GetAllAsync();
        _topics = await _kafkaRepositoryProvider.TopicRepository.GetAllAsync();
    }

    private async Task UpdateMainPageAsync()
    {
        _brokers = await _kafkaRepositoryProvider.BrokerRepository.GetAllAsync();
        _topics = await _kafkaRepositoryProvider.TopicRepository.GetAllAsync();
    }
}