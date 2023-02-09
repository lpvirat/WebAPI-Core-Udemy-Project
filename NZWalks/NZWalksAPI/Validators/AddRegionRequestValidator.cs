using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class AddRegionRequestValidator:AbstractValidator<Models.DTO.AddRegionRequest>
    {
        //Fluent validation replacing the normal model validation
        public AddRegionRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Lat).InclusiveBetween(-90, 90);
            RuleFor(x => x.Long).InclusiveBetween(-180, 180);

        }
    }
}
