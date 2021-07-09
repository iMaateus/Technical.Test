using FluentValidation;
using System.Net;

namespace Technical.Test.Business.Models.Validations
{
    public class ServerValidation : AbstractValidator<Server>
    {
        public ServerValidation()
        {
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("The {PropertyName} field is required")
                .Length(2, 100)
                .WithMessage("The {PropertyName} field must be between {MinLength} and {MaxLength} characters");

            RuleFor(r => r.Port).InclusiveBetween(0, 65535)
                    .WithMessage("The {PropertyName} field must be between {From} and {To}");

            RuleFor(r => IPValidate(r.IP)).Equal(true)
                    .WithMessage("The IP Address field is invalid");
        }

        public bool IPValidate(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }
    }
}
