using MaisGuinchos.Dtos.Guincho;
using MaisGuinchos.Hubs;
using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MaisGuinchos.Services
{
    public class GuinchoService : IGuinchoService
    {
        private readonly IGuinchoRepo _guinchoRepo;
        private readonly IUserService _userService;
        private readonly IHubContext<TowHub> _hubContext;
        public GuinchoService(IGuinchoRepo guinchoRepo, IUserService userService, IHubContext<TowHub> hubContext)
        {
            _guinchoRepo = guinchoRepo;
            _userService = userService;
            _hubContext = hubContext;
        }

        public async Task<Models.Guincho?> UpdateStatus(string userId, UpdateGuinchoStatusDTO statusDto)
        {

            if (!Guid.TryParse(userId, out var userGuid))
            {
                throw new ArgumentException("Invalid user ID format.", nameof(userId));
            }

            var guincho = await _guinchoRepo.GetGuinchoByUserId(userGuid);

            if (guincho == null)
            {
                return null;
            }

            guincho.Disponivel = statusDto.status;

            await _guinchoRepo.UpdateGuincho(guincho);

            await _hubContext.Clients.Group("Clientes")
                .SendAsync("GuinchoStatusUpdated", new
                {
                    MotoristaId = guincho.UserId,
                    Disponivel = guincho.Disponivel
                });

            return guincho;
        }
    }
}
