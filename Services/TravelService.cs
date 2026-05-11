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

        public CoordinateDto? ResolveTarget(TowTravel travel)
        {
            return travel.Status switch
            {
                TowTravelStatus.GoingToClient => new CoordinateDto
                {
                    Lat = travel.TowRequest.PickupLat,
                    Lon = travel.TowRequest.PickupLon
                },

                TowTravelStatus.InProgress => new CoordinateDto
                {
                    Lat = travel.TowRequest.DropoffLat,
                    Lon = travel.TowRequest.DropoffLon
                },

                _ => null
            };

            throw new Exception("Estágio da viagem inválido para decidir o alvo da rota.");
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
            if (entity == null)
                return null;

            var lastDriverLoc = await _mapsService.GetLastLocationAsync(entity.DriverId);

            if (lastDriverLoc == null)
            {
                return null;
            }

            return new TowTravelResponseDTO
            {
                Id = entity.Id,

                DriverId = entity.DriverId,
                DriverName = entity.Driver.Name ?? string.Empty,
                DriverPhone = entity.Driver.NumeroTelefone ?? string.Empty,
                VehicleModelDriver = entity.Driver.Guincho!.Modelo ?? string.Empty,
                VehicleColorDriver = entity.Driver.Guincho!.Cor ?? string.Empty,
                PlacaDriver = entity.Driver.Guincho!.Placa ?? string.Empty,


                ClientId = entity.TowRequest.ClientId,
                ClientName = entity.TowRequest.Client.Name ?? string.Empty,
                ClientPhone = entity.TowRequest.Client.NumeroTelefone ?? string.Empty,
                VehicleModelClient = entity.TowRequest.VehicleType ?? string.Empty,
                Notes = entity.TowRequest.Notes ?? string.Empty,
                Questions = entity.TowRequest.VehicleIssue ?? string.Empty,

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
                CancellationReason = entity.CancellationReason ?? string.Empty,

                Origin = new LocationDTO
                {
                    Latitude = lastDriverLoc.Latitude,
                    Longitude = lastDriverLoc.Longitude,
                    Address = lastDriverLoc.DisplayName ?? string.Empty
                },

                Pickup = new LocationDTO
                {
                    Latitude = entity.TowRequest.PickupLat,
                    Longitude = entity.TowRequest.PickupLon,
                    Address = string.Empty
                },

                Destination = new LocationDTO
                {
                    Latitude = entity.TowRequest.DropoffLat,
                    Longitude = entity.TowRequest.DropoffLon,
                    Address = string.Empty
                }
            };
        }
    }
}
