using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaisGuinchos.Repositorys
{
    public class GuinchoRepo : IGuinchoRepo
    {
        private readonly AppDbContext _dbContext;

        public GuinchoRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Guincho> GetGuinchos()
        {
            var guinchos = _dbContext.Guinchos.ToList();

            return guinchos;
        }

        public async Task<Guincho?> GetGuinchoByUserId(Guid userId)
        {
            var guincho = await _dbContext.Guinchos.FirstOrDefaultAsync(g => g.UserId == userId);

            return guincho;
        }

        public async Task<Guincho> UpdateGuincho(Guincho guincho)
        {
            _dbContext.Guinchos.Update(guincho);

            await _dbContext.SaveChangesAsync();

            return guincho;
        }
    }
}
