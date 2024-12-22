using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Database;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components;

public partial class MainPage : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _kafkaRepositoryProvider { get; set; }

    private List<Broker> brokers;
    
    protected override async Task OnInitializedAsync()
    {
        brokers = await _kafkaRepositoryProvider.BrokerRepository.GetAllAsync();
    }
}