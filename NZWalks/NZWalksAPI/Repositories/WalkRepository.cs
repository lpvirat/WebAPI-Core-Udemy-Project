using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class WalkRepository: IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext) 
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks.FindAsync(id);
            if (walk == null)
            {
                return null;
            }
            nZWalksDbContext.Walks.Remove(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;

        }

        public async Task<IEnumerable<Walk>> GetAllAysnc()
        {
            return await nZWalksDbContext.Walks.
                Include(x => x.Region).             //Fetching region and Walk Difficulty and adding in the walks output
                Include(x => x.WalkDifficulty).
                ToListAsync();
            
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            var walk = await nZWalksDbContext.Walks.
                Include(x => x.Region).             //Fetching region and Walk Difficulty and adding in the walks output
                Include(x => x.WalkDifficulty).
                FirstOrDefaultAsync(x => x.Id == id);

            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingwalk = await nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingwalk is null)
            {
                return null;
            }
            
            existingwalk.Length = walk.Length;
            existingwalk.Name = walk.Name;
            existingwalk.RegionId = walk.RegionId;
            existingwalk.WalkDifficultyId = walk.WalkDifficultyId;
            walk.Id = id;

            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

    }
}
