using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await regionRepository.GetAllAsync(); //Getting regions from Domain Model

            //converting the Domain Model regions to DTO's and returning them 
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region => 
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,

            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if(region is null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validating the client request before sending the data
            //if(!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //Commented the above model validation for replacing with Fluent validation

            //DTO request is converted to domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat  = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //Passing the domain to Repository
            await regionRepository.AddAsync(region);

            //Converting the domain back to DTO response
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id}, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get the region from DB
            var region = await regionRepository.DeleteAsync(id);

            //If null Not Found
            if(region is null)
            {
                return NotFound();
            }

            //convert response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            //Validating the Client request before sending the data
            //if(!ValidateUpdateRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //Commented the above model validation for replacing with Fluent validation

            //Convert DTO to domain model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            region = await regionRepository.UpdateAsync(id, region);

            //If null then Not Found
            if(region is null)
            {
                return NotFound();
            }

            //Convert domain back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            return Ok(regionDTO);

        }

        #region Private Validation methods for AddRegion

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest is null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"{nameof(addRegionRequest)} cannot be null or empty or whitespace");
                return false;
            }
            if(string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} cannot be null or empty or whitespace");
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} cannot be null or empty or whitespace");
            }
            if (addRegionRequest.Area<=0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} should be greater than zero");
            }
            if (Double.IsNaN(addRegionRequest.Lat))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)} must be any real number");
            }
            if (Double.IsNaN(addRegionRequest.Long))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} must be any real number");
            }
            if (addRegionRequest.Population<0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} should be greater than zero");
            }

            if(ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest is null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $"{nameof(updateRegionRequest)} cannot be null or empty or whitespace");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} cannot be null or empty or whitespace");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} cannot be null or empty or whitespace");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} should be greater than zero");
            }
            if (Double.IsNaN(updateRegionRequest.Lat))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat), $"{nameof(updateRegionRequest.Lat)} must be any real number");
            }
            if (Double.IsNaN(updateRegionRequest.Long))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long), $"{nameof(updateRegionRequest.Long)} must be any real number");
            }
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} should be greater than zero");
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
