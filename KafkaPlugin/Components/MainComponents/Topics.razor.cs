using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace KafkaPlugin.Components.MainComponents;

public partial class Topics : ComponentBase
{
    [Parameter] public EventCallback UpdateMainPageEvent { get; set; }
    [Parameter] public List<Models.Repositories.Topic>? topics { get; set; }
}