using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsAPI.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId.HasValue)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(userId.Value));
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (userId.HasValue)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(userId.Value));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public static string GroupName(int userId) => $"user-{userId}";

    private int? GetUserId()
    {
        var rawId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? Context.User?.FindFirstValue("sub")
                 ?? Context.User?.FindFirstValue("nameid");

        return int.TryParse(rawId, out var id) ? id : null;
    }
}
