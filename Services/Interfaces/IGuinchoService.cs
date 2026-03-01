using MaisGuinchos.Dtos.Guincho;

namespace MaisGuinchos.Services.Interfaces
{
    public interface IGuinchoService
    {
        Task<Models.Guincho?> UpdateStatus(string userId, UpdateGuinchoStatusDTO statusDto);
    }
}
