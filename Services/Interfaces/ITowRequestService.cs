using MaisGuinchos.Dtos.Tow;
using MaisGuinchos.Models;

namespace MaisGuinchos.Services.Interfaces
{
    public interface ITowRequestService
    {
        Task<Guid> CreateAsync(Guid clientId, CreateTowRequestDto dto);

        Task<TowRequest> GetTowRequestById(Guid towRequestId);
    }
}
