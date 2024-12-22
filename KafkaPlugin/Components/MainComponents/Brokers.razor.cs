using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class Brokers : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }
    
    private List<Broker>? _brokers;
    
    protected override async Task OnInitializedAsync()
    {
        _brokers = await _kafkaRepositoryProvider.BrokerRepository.GetAllAsync();
    }
}