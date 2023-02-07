using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAysnc(); //Getting Walks from domain model

            //Converting the Domain walks to DTO as response using Auto-Mapper
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkasync")]
        public async Task<IActionResult> GetWalkasync(Guid id)
        {
            var walks = await walkRepository.GetAsync(id);
            if (walks is null)
            {
                return NotFound();
            }

            //Converting the Domain walks to DTO as response using Auto-Mapper
            var walksDTO = mapper.Map<Models.DTO.Walk>(walks);
            return Ok(walksDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Converting DTO to domain
            var walkDomain = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            //Sending the domain model to Repository
            var walk = await walkRepository.AddAsync(walkDomain);

            //Converting the Domain back to DTO as response
            var WalksDTO = new Models.DTO.Walk()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId

            };
            return CreatedAtAction(nameof(GetWalkasync), new { id = WalksDTO.Id }, WalksDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to domain model
            var walkDomain = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //Pass Domain model to Repository
            var walk = await walkRepository.UpdateAsync(id, walkDomain);

            //Convert back the domain to DTO as a response
            if (walk == null)
            {
                return NotFound("Walk not Found");
            }
            var walksDTO = new Models.DTO.Walk
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId

            };
            return Ok(walksDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound("Walk is not found");
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);

        }

    }
}
