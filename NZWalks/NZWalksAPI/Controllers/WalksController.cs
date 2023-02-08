using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository,IMapper mapper,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            //Validating the Client request before sending the data
            if (!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }

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

            //Validating the Client request before sending the data
            if (!await ValidateUpdateWalkAsync(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }
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
        #region Private Methods for Walks

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest is null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"{nameof(addWalkRequest)} cannot be null or empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} cannot be null or empty");
            }
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} must be greater than 0");
            }

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is not valid or existed");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is not valid or existed");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest is null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"{nameof(updateWalkRequest)} cannot be null or empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} cannot be null or empty");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} must be greater than 0");
            }

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is not valid or existed");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is not valid or existed");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion


    }
}
