using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Exceptions;
using MaisGuinchos.Hubs;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MaisGuinchos.Services
{
    public class TowRequestService : ITowRequestService
    {
        private readonly ITowRequestRepo _towRequestRepo;
        private readonly IMapsService _locationService;
        private readonly ITowTravelRepo _towTravelRepo;
        private readonly IHubContext<TowHub> _hubContext;
        private readonly IUserService _userService;

        public TowRequestService(ITowRequestRepo towRequestRepo,
            IHubContext<TowHub> hubContext, IUserService userService,
            ITowTravelRepo towTravelRepo,
            IMapsService locationService)
        {
            _towRequestRepo = towRequestRepo;
            _hubContext = hubContext;
            _userService = userService;
            _towTravelRepo = towTravelRepo;
            _locationService = locationService;
        }

        public async Task<Guid> CreateAsync(Guid clientId, CreateTowRequestDto dto)
        {
            var exists = await _towRequestRepo.HasActiveRequestAsync(clientId, dto.DriverId);

            if (exists)
                throw new Exception("Já existe uma solicitação ativa para este motorista.");

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

            var user = await _userService.GetUserById(clientId);

            var clientName = user?.Name ?? "Unknown Client";

            await _hubContext.Clients.Group(dto.DriverId.ToString()).SendAsync("ReceiveTowRequest", new GetTowsPendingsDTO
            {
                Id = request.Id,
                ClientId = clientId,
                ClientName = clientName,
                PickupLat = request.PickupLat,
                PickupLon = request.PickupLon,
                DropoffLat = request.DropoffLat,
                DropoffLon = request.DropoffLon,
                TotalDistanceKm = request.TotalDistanceKm,
                DurationMinutes = request.DurationMinutes,
                SuggestedPrice = request.SuggestedPrice,
                VehicleType = request.VehicleType,
                VehicleIssue = request.VehicleIssue,
                Notes = request.Notes,
                CreatedAt = request.CreatedAt,
            });

            return request.Id;
        }

        public async Task<TowRequest> GetTowRequestById(Guid towRequestId)
        {
            var request = await _towRequestRepo.GetByIdAsync(towRequestId);
            return request ?? throw new Exception("Tow request not found");
        }

        public async Task<List<GetTowsPendingsDTO>> GetTowsPendings(Guid driverId)
        {
            var tows = await _towRequestRepo.GetPendingsAsync(driverId);

            if (tows == null || !tows.Any())
                return new List<GetTowsPendingsDTO>();

            var result = tows.Select(t => new GetTowsPendingsDTO
            {
                Id = t.Id,
                ClientName = t.Client.Name,
                ClientId = t.ClientId,
                PickupLat = t.PickupLat,
                PickupLon = t.PickupLon,
                DropoffLat = t.DropoffLat,
                DropoffLon = t.DropoffLon,
                TotalDistanceKm = t.TotalDistanceKm,
                DurationMinutes = t.DurationMinutes,
                SuggestedPrice = t.SuggestedPrice,
                VehicleType = t.VehicleType,
                VehicleIssue = t.VehicleIssue,
                Notes = t.Notes,
                Status = t.Status,
                CreatedAt = t.CreatedAt
            }).ToList();

            return result;
        }

        public async Task<PutTowCounterOfferDTO> UpdateTowRequestCounterOffer(Guid id, TowRequestCounterOfferDto counterOffer)
        {
            var towRequest = await _towRequestRepo.GetByIdAsync(id);

            if (towRequest == null)
            {
                throw new Exception("TowRequest não encontrada");
            }

            if (towRequest.Status != TowRequestStatus.WaitingDriverResponse)
            {
                throw new Exception("Contra oferta só pode ser feita em solicitações pendentes");
            }

            towRequest.CounterOfferPrice = counterOffer.NewPrice;
            towRequest.CounterOfferPercent = counterOffer.Percent;
            towRequest.CounterOfferReason = counterOffer.Reason;

            //towRequest.CounterOfferDriverId = driverId; tow global prevista para o futuro, caso seja necessário identificar qual motorista fez a contra oferta

            towRequest.CounterOfferAt = DateTime.UtcNow;
            towRequest.UpdatedAt = DateTime.UtcNow;

            towRequest.Status = TowRequestStatus.CounterOfferSent;

            await _towRequestRepo.UpdateAsync(towRequest);

            await _hubContext.Clients.User(towRequest.ClientId.ToString()).SendAsync("ReceiveCounterOffer", new PutTowCounterOfferDTO
            {
                Id = towRequest.Id,
                ClientId = towRequest.ClientId,
                ClientName = towRequest.Client.Name,
                DriverId = towRequest.DriverId,
                DriverName = towRequest.Driver.Name,
                PickupLat = towRequest.PickupLat,
                PickupLon = towRequest.PickupLon,
                DropoffLat = towRequest.DropoffLat,
                DropoffLon = towRequest.DropoffLon,
                TotalDistanceKm = towRequest.TotalDistanceKm,
                DurationMinutes = towRequest.DurationMinutes,
                SuggestedPrice = towRequest.SuggestedPrice,
                FinalPrice = towRequest.FinalPrice,
                CounterOfferPrice = towRequest.CounterOfferPrice,
                CounterOfferPercent = towRequest.CounterOfferPercent,
                CounterOfferReason = towRequest.CounterOfferReason,
                CounterOfferAt = towRequest.CounterOfferAt,
                Status = (int)towRequest.Status,
                CreatedAt = towRequest.CreatedAt
            });

            return new PutTowCounterOfferDTO
            {
                Id = towRequest.Id,

                ClientId = towRequest.ClientId,
                ClientName = towRequest.Client.Name,

                DriverId = towRequest.DriverId,
                DriverName = towRequest.Driver.Name,

                PickupLat = towRequest.PickupLat,
                PickupLon = towRequest.PickupLon,

                DropoffLat = towRequest.DropoffLat,
                DropoffLon = towRequest.DropoffLon,

                TotalDistanceKm = towRequest.TotalDistanceKm,
                DurationMinutes = towRequest.DurationMinutes,

                SuggestedPrice = towRequest.SuggestedPrice,
                FinalPrice = towRequest.FinalPrice,

                CounterOfferPrice = towRequest.CounterOfferPrice,
                CounterOfferPercent = towRequest.CounterOfferPercent,
                CounterOfferReason = towRequest.CounterOfferReason,
                CounterOfferAt = towRequest.CounterOfferAt,

                Status = (int)towRequest.Status,

                CreatedAt = towRequest.CreatedAt
            };
        }

        public async Task<PutTowCancelCounterOfferDTO> RejectCounterOffer(Guid idTowRequest)
        {
            var towRequest = await _towRequestRepo.GetByIdAsync(idTowRequest);

            if (towRequest == null)
            {
                throw new Exception("TowRequest não encontrada");
            }

            if (towRequest.Status != TowRequestStatus.CounterOfferSent)
            {
                throw new Exception("Contra oferta só pode ser rejeitada se tiver sido enviada");
            }

            towRequest.UpdatedAt = DateTime.UtcNow;
            towRequest.Status = TowRequestStatus.CounterOfferRejected;

            await _towRequestRepo.UpdateAsync(towRequest);

            await _hubContext.Clients.User(towRequest.DriverId.ToString()).SendAsync("CounterOfferRejected", new PutTowCancelCounterOfferDTO
            {
                Id = towRequest.Id,
                Status = (int)towRequest.Status
            });

            return new PutTowCancelCounterOfferDTO
            {
                Id = towRequest.Id,
                Status = (int)towRequest.Status
            };
        }

        public async Task<AcceptTowRequestResponseDTO> AcceptTowRequest(Guid idTowRequest)
        {
            var towRequest = await _towRequestRepo.GetByIdAsync(idTowRequest);

            if (towRequest == null)
            {
                throw new NotFoundException("TowRequest não encontrada");
            }

            if (towRequest.Status != TowRequestStatus.WaitingDriverResponse && towRequest.Status != TowRequestStatus.CounterOfferRejected)
            {
                throw new BadRequestException("Somente solicitações pendentes podem ser aceita pelo motorista.");
            }

            towRequest.UpdatedAt = DateTime.UtcNow;

            towRequest.Status = TowRequestStatus.Accepted;

            var towTravelId = Guid.NewGuid();

            var towTravel = new Models.TowTravel
            {
                Id = towTravelId,
                TowRequestId = towRequest.Id,
                DriverId = towRequest.DriverId,
                FinalPrice = towRequest.FinalPrice ?? towRequest.SuggestedPrice,
                EstimatedArrivalTime = towRequest.DurationMinutes,
                Status = TowTravelStatus.GoingToClient,
                CreatedAt = DateTime.UtcNow
            };

            var driverLocation = await _locationService.GetLastLocationAsync(towRequest.DriverId);

            if (driverLocation == null)
            {
                throw new BadRequestException("Localização do motorista não encontrada");
            }

            await _towTravelRepo.AddAsync(towTravel);

            await _towRequestRepo.UpdateAsync(towRequest);

            var response = new AcceptTowRequestResponseDTO
            {
                TowRequestId = towRequest.Id,
                TowTravelId = towTravelId,
                TowRequestStatus = towRequest.Status,
                DriverLat = driverLocation.Latitude,
                DriverLon = driverLocation.Longitude,
                PickupLat = towRequest.PickupLat,
                PickupLon = towRequest.PickupLon,
                DestinationLat = towRequest.DropoffLat,
                DestinationLon = towRequest.DropoffLon,
                TowDriverId = towRequest.DriverId,
                FinalPrice = towTravel.FinalPrice,
                EstimatedArrivalTime = towTravel.EstimatedArrivalTime,
                DistanceKm = towRequest.TotalDistanceKm
            };

            await _hubContext.Clients.User(towRequest.ClientId.ToString()).SendAsync("TowRequestAccepted", response);

            return response;
        }

        public async Task<AcceptTowRequestResponseDTO> AcceptCounterOffer(Guid idTowRequest)
        {
            var towRequest = _towRequestRepo.GetByIdAsync(idTowRequest).Result; //filtra só ultima

            if (towRequest == null)
            {
                throw new NotFoundException("TowRequest não encontrada");
            }

            if (towRequest.Status != TowRequestStatus.CounterOfferSent)
            {
                throw new BadRequestException("Somente contra ofertas enviadas podem ser aceitas pelo cliente.");
            }

            towRequest.UpdatedAt = DateTime.UtcNow;
            towRequest.Status = TowRequestStatus.Accepted;

            var driverLocation = await _locationService.GetLastLocationAsync(towRequest.DriverId);


            if (driverLocation == null)
            {
                throw new BadRequestException("Localização do motorista não encontrada");
            }

            await _towRequestRepo.UpdateAsync(towRequest);

            var towTravelId = Guid.NewGuid();

            await _towTravelRepo.AddAsync(new TowTravel {
                Id = towTravelId,
                TowRequestId = towRequest.Id,
                DriverId = towRequest.DriverId,
                FinalPrice = towRequest.CounterOfferPrice ?? towRequest.SuggestedPrice,
                EstimatedArrivalTime = towRequest.DurationMinutes,
                Status = TowTravelStatus.GoingToClient,
                CreatedAt = DateTime.UtcNow
            });

            var response = new AcceptTowRequestResponseDTO
            {
                TowRequestId = towRequest.Id,
                TowTravelId = towTravelId,
                TowRequestStatus = towRequest.Status,
                DriverLat = driverLocation.Latitude,
                DriverLon = driverLocation.Longitude,
                PickupLat = towRequest.PickupLat,
                PickupLon = towRequest.PickupLon,
                DestinationLat = towRequest.DropoffLat,
                DestinationLon = towRequest.DropoffLon
            };

            await _hubContext.Clients.User(towRequest.DriverId.ToString()).SendAsync("CounterOfferAccepted", response);

            return response;
        }
    }
}
