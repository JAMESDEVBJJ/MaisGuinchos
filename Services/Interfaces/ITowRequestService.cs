using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface ITowRequestService
    {
        Task<Guid> CreateAsync(Guid clientId, CreateTowRequestDto dto);

        Task<TowRequest> GetTowRequestById(Guid towRequestId);

        Task<List<GetTowsPendingsDTO>> GetTowsPendings(Guid driverId);

        Task<PutTowCounterOfferDTO> UpdateTowRequestCounterOffer(Guid id, TowRequestCounterOfferDto counterOffer);

        Task<PutTowCancelCounterOfferDTO> RejectCounterOffer(Guid idTowRequest);

        Task<AcceptTowRequestResponseDTO> AcceptTowRequest(Guid idTowRequest);

        Task<AcceptTowRequestResponseDTO> AcceptCounterOffer(Guid idTowRequest);
    }
}
