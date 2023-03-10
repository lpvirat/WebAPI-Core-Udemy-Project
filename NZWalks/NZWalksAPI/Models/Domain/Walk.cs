namespace NZWalksAPI.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        // Navigation Properties and walk is having one-to-one relation with Region and WalkDifficulty
        //which means walk can be attached only to one region and walk can have only one particular Difficulty type
        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
