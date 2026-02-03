using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using MaisGuinchos.Services.Interfaces;

namespace MaisGuinchos.Services
{
    public class GuinchoService : IGuinchoService
    {
        private readonly IGuinchoRepo _guinchoRepo;
        private readonly IUserService _userService;
        public GuinchoService(IGuinchoRepo guinchoRepo, IUserService userService) {
            _guinchoRepo = guinchoRepo;
            _userService = userService;
        }

    }
}
