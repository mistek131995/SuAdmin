﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaPlugin.Models.Tables;
using Microsoft.AspNetCore.Components;
using PluginContracts.Database;

namespace KafkaPlugin.Components;

public partial class MainWidget : ComponentBase
{
    [Inject] private IAdminClient _adminClient { get; set; }
    [Inject] private Context _context { get; set; }
    
    private List<BrokerMetadata> _brokers;
    
    
    protected override async Task OnInitializedAsync()
    {
        _brokers = await Task.Run(() => _adminClient.GetMetadata(TimeSpan.FromSeconds(2)).Brokers);
    }
}