using MaisGuinchos.Models;

namespace MaisGuinchos.Repositorys.Interfaces
{
    public interface IGuinchoRepo
    {
        List<Guincho> GetGuinchos();

        Task<Guincho?> GetGuinchoByUserId(Guid userId);

        Task<Guincho> UpdateGuincho(Guincho guincho); 
    }
}
