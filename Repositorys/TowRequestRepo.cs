using MaisGuinchos.Models;
using MaisGuinchos.Repositorys.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaisGuinchos.Repositorys
{
    public class TowRequestRepo : ITowRequestRepo
    {
        private readonly AppDbContext _appDbContext;
        public TowRequestRepo(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(TowRequest request)
        {
            await _appDbContext.TowRequests.AddAsync(request);
        }

        public async Task<TowRequest?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.TowRequests
                .Include(x => x.Client)
                .Include(x => x.Driver).FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
