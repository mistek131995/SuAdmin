using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class Brokers : ComponentBase
{
    [Parameter] public EventCallback UpdateMainPageEvent { get; set; }
    [Parameter] public List<Broker>? brokers { get; set; }
}