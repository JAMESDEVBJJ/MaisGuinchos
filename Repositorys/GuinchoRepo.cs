using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;

namespace MaisGuinchos.Repositorys
{
    public class GuinchoRepo : IGuinchoRepo
    {
        private readonly AppDbContext _dbContext;

        public GuinchoRepo(AppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public List<Guincho> GetGuinchos()
        {
            var guinchos = _dbContext.Guinchos.ToList();

            return guinchos; 
        }
    }
}
