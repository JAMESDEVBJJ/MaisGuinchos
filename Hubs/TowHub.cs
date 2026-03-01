using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MaisGuinchos.Hubs
{
    [Authorize]
    public class TowHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var role = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                if (role == "Motorista")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                }

                if (role == "Cliente")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "Clientes");
                }
            }
    
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}

