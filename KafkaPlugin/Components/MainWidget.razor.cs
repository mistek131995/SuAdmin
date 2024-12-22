using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components;

public partial class MainWidget : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }
    
    private List<Broker> _brokers;
    
    
    protected override async Task OnInitializedAsync()
    {
        _brokers = await _kafkaRepositoryProvider.BrokerRepository.GetAllAsync();
    }
}