using FluentValidation;

namespace Technical.Test.Business.Models.Validations
{
    public class VideoValidation : AbstractValidator<Video>
    {
        public VideoValidation()
        {
            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 100)
                .WithMessage("The {PropertyName} field must be between {MinLength} and {MaxLength} characters");
        }
    }
}
