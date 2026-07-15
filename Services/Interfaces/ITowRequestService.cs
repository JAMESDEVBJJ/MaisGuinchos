using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface ITowRequestService
    {
        Task<GetTowsPendingsDTO> CreateAsync(Guid clientId, CreateTowRequestDto dto);

        Task<TowRequest> GetTowRequestById(Guid towRequestId);

        Task<List<GetTowsRequestsByUserIdDTO>> GetTowsRequestsByUserId(Guid userId);


        Task<List<GetTowsPendingsDTO>> GetTowsPendings(Guid driverId);

        Task<List<GetTowsPendingsDTO>> GetTowsPendingsForClient(Guid clientId);

        Task<PutTowCounterOfferDTO> UpdateTowRequestCounterOffer(Guid id, TowRequestCounterOfferDto counterOffer);

        Task<PutTowCancelCounterOfferDTO> RejectCounterOffer(Guid idTowRequest);

        Task<AcceptTowRequestResponseDTO> AcceptTowRequest(Guid idTowRequest);

        Task<AcceptTowRequestResponseDTO> AcceptCounterOffer(Guid idTowRequest);
    }
}
