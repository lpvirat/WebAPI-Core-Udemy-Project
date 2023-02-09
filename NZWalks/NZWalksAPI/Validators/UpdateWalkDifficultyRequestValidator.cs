using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class UpdateWalkDifficultyRequestValidator:AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
