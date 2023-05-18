using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Hubs
{
    public static class UserHandler
    {
        public static HashSet<string> ConnectedUserIds = new HashSet<string>();
    }
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub
    {


        public override async Task OnConnectedAsync()
        {
            UserHandler.ConnectedUserIds.Add(Context.UserIdentifier);
            await Clients.All.SendAsync("statusUserConnected", Context.UserIdentifier);
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            UserHandler.ConnectedUserIds.Remove(Context.UserIdentifier);
            await Clients.All.SendAsync("statusUserDisconnected", Context.UserIdentifier);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("receive", Context.UserIdentifier, message);
        }
    }
}

