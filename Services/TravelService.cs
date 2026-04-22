using MaisGuinchos.Dtos.Route;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;

namespace MaisGuinchos.Services
{
    public class TravelService : ITravelService
    {
        private readonly ITowTravelRepo _towTravelRepo;

       public TravelService(ITowTravelRepo towTravelRepo)
        {
            _towTravelRepo = towTravelRepo; 
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
    }
}
