using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Migrations;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using TowRequest = MaisGuinchos.Models.TowRequest;

namespace MaisGuinchos.Services
{
    public class TowRequestService : ITowRequestService
    {
        private readonly ITowRequestRepo _towRequestRepo;   
        public TowRequestService(ITowRequestRepo towRequestRepo) {
            _towRequestRepo = towRequestRepo;
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

            return request.Id;
        }

        public async Task<TowRequest> GetTowRequestById(Guid towRequestId)
        {
            var request = await _towRequestRepo.GetByIdAsync(towRequestId);
            return request ?? throw new Exception("Tow request not found");
        }
    }
}
