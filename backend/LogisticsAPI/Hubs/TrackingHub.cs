using System.Security.Claims;
using LogisticsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LogisticsAPI.Hubs;


[Authorize]
public class TrackingHub : Hub
{
    private readonly IShipmentRepository _shipmentRepo;

    public TrackingHub(IShipmentRepository shipmentRepo)
    {
        _shipmentRepo = shipmentRepo;
    }

    public async Task SubscribeToShipment(int shipmentId)
    {
        if (!CanAccessShipment(shipmentId)) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(shipmentId));
    }

    public async Task UnsubscribeFromShipment(int shipmentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(shipmentId));
    }

    public static string GroupName(int shipmentId) => $"shipment-{shipmentId}";

    private bool CanAccessShipment(int shipmentId)
    {
        var shipment = _shipmentRepo.GetById(shipmentId);
        if (shipment == null) return false;

        if (Context.User?.IsInRole("Admin") == true) return true;

        var rawId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? Context.User?.FindFirstValue("sub")
                 ?? Context.User?.FindFirstValue("nameid");

        return int.TryParse(rawId, out var userId) && shipment.UserId == userId;
    }
}
