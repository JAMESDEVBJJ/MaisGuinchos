using MaisGuinchos.Dtos;
using MaisGuinchos.Dtos.Route;
using MaisGuinchos.Dtos.Tow.Travel;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;

namespace MaisGuinchos.Services
{
    public class TravelService : ITravelService
    {
        private readonly ITowTravelRepo _towTravelRepo;
        private readonly IMapsService _mapsService;

        public TravelService(ITowTravelRepo towTravelRepo, IMapsService mapsService)
        {
            _towTravelRepo = towTravelRepo;
            _mapsService = mapsService;
        }

        public Task<TowTravel?> GetActiveByDriverId(Guid driverId)
        {
            return _towTravelRepo.GetLastActiveByDriverId(driverId);
        }

        public Task<TowTravel?> GetActiveByClientId(Guid clientId)
        {
            return _towTravelRepo.GetActiveByClientId(clientId);
        }

        public CoordinateDto ResolveTarget(TowTravel travel)
        {
            if (travel.Status == TowTravelStatus.GoingToClient)
            {
                return new CoordinateDto
                {
                    Lat = travel.TowRequest.PickupLat,
                    Lon = travel.TowRequest.PickupLon
                };
            }

            if (travel.Status == TowTravelStatus.InProgress)
            {
                return new CoordinateDto
                {
                    Lat = travel.TowRequest.DropoffLat,
                    Lon = travel.TowRequest.DropoffLon
                };
            }

            throw new Exception("Estágio da viagem inválido.");
        }

        public async Task<TowTravelResponseDTO?> GetPendingTowTravel(Guid userId)
        {
            var towTravel = await _towTravelRepo.GetPendingByUserId(userId);

            if (towTravel == null)
                return null;

            return await ToDto(towTravel);
        }

        public async Task<TowTravelResponseDTO?> ToDto(TowTravel entity)
        {

            var lastDriverLoc = await _mapsService.GetLastLocationAsync(entity.DriverId);

            if (lastDriverLoc == null)
            {
                return null;
            }

            return new TowTravelResponseDTO
            {
                Id = entity.Id,

                DriverId = entity.DriverId,
                DriverName = entity.Driver.Name,

                ClientId = entity.TowRequest.ClientId,
                ClientName = entity.TowRequest.Client.Name,

                TowRequestId = entity.TowRequestId,

                FinalPrice = entity.FinalPrice,

                DistanceToPickupKm = entity.DistanceToPickupKm,
                TimeToPickupMin = entity.DurationMinToPickup,

                DistanceToDestinationKm = entity.DistanceToDestinationKm,
                TimeToDestinationMin = entity.DurationMinToDestination,

                Status = entity.Status,

                StartedAt = entity.StartedAt,
                EndedAt = entity.EndedAt,
                CanceledAt = entity.CanceledAt,
                CancellationReason = entity.CancellationReason,

                Origin = new LocationDTO
                {
                    Latitude = lastDriverLoc.Latitude,
                    Longitude = lastDriverLoc.Longitude,
                    Address = lastDriverLoc.DisplayName
                },

                Destination = new LocationDTO
                {
                    Latitude = entity.TowRequest.DropoffLat,
                    Longitude = entity.TowRequest.DropoffLon,
                    Address = null
                }
            };
        }
    }
}
