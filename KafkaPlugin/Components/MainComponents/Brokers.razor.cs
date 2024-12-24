using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KafkaPlugin.Database;
using KafkaPlugin.Interfaces.Providers;
using KafkaPlugin.Models.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace KafkaPlugin.Components.MainComponents;

public partial class Brokers : ComponentBase
{
    [Inject] private Context _context { get; set; }
    [Parameter] public EventCallback UpdateMainPageEvent { get; set; }
    [Parameter] public List<Broker>? brokers { get; set; }

    private RenderFragment<(string, int)> _deleteButton;

    protected override void OnInitialized()
    {
        _deleteButton = ((string ip, int port) args) => builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", "btn btn-secondary btn-sm");
            builder.AddAttribute(2, "type", "button");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, async () => await DeleteHostAsync(args)));
            builder.AddContent(4, (MarkupString)"<i class=\"bi bi-trash\"></i>");
            builder.CloseElement();
        };
    }

    private async Task DeleteHostAsync((string ip, int port) args)
    {
        var host = await _context.Hosts.FirstOrDefaultAsync(x => x.Ip == args.ip && x.Port == args.port);
        
        _context.Hosts.Remove(host);
        await _context.SaveChangesAsync();

        await UpdateMainPageEvent.InvokeAsync();
    }
}