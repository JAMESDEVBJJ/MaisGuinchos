using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Hubs;
using MaisGuinchos.Migrations;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TowRequest = MaisGuinchos.Models.TowRequest;

namespace MaisGuinchos.Services
{
    public class TowRequestService : ITowRequestService
    {
        private readonly ITowRequestRepo _towRequestRepo;
        private readonly IHubContext<TowHub> _hubContext;

        public TowRequestService(ITowRequestRepo towRequestRepo, IHubContext<TowHub> hubContext) {
            _towRequestRepo = towRequestRepo;
            _hubContext = hubContext;
        }

        public async Task<Guid> CreateAsync(Guid clientId, CreateTowRequestDto dto)
        {
            var request = new TowRequest
            {
                Id = Guid.NewGuid(),
                ClientId = clientId,
                DriverId = dto.DriverId,
                PickupLat = dto.PickupLat,
                PickupLon = dto.PickupLon,
                DropoffLat = dto.DropoffLat,
                DropoffLon = dto.DropoffLon,
                TotalDistanceKm = dto.TotalDistanceKm,
                DurationMinutes = dto.DurationMinutes,
                SuggestedPrice = dto.SuggestedPrice,
                VehicleType = dto.VehicleType,
                VehicleIssue = dto.VehicleIssue,
                Notes = dto.Notes,
                Status = TowRequestStatus.WaitingDriverResponse
            };

            await _towRequestRepo.AddAsync(request);
            await _towRequestRepo.SaveChangesAsync();

            await _hubContext.Clients.Group(dto.DriverId.ToString()).SendAsync("ReceiveTowRequest", new
            {
                RequestId = request.Id,
                ClientId = clientId,
                PickupLat = request.PickupLat,
                PickupLon = request.PickupLon,
                DropoffLat = request.DropoffLat,
                DropoffLon = request.DropoffLon,
                TotalDistanceKm = request.TotalDistanceKm,
                DurationMinutes = request.DurationMinutes,
                SuggestedPrice = request.SuggestedPrice,
                VehicleType = request.VehicleType,
                VehicleIssue = request.VehicleIssue,
                Notes = request.Notes
            });

            return request.Id;
        }

        public async Task<TowRequest> GetTowRequestById(Guid towRequestId)
        {
            var request = await _towRequestRepo.GetByIdAsync(towRequestId);
            return request ?? throw new Exception("Tow request not found");
        }
    }
}
