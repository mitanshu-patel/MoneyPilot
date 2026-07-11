using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using MoneyPilot.Domain.Entities;
using MoneyPilot.Shared.Common;

namespace MoneyPilot.Application.Users.Add
{
    public class AddUserHandler(IMoneyPilotRepo moneyPilotRepo) : RequestHandlerBase<AddUserCommand, CustomResponse<AddUserResult>>
    {
        protected override async Task<CustomResponse<AddUserResult>> ExecuteCommandAsync(AddUserCommand command)
        {
            var userExist = await moneyPilotRepo.GetUsers().Where(v => v.Email.Equals(command.Email)).AnyAsync();
            if (userExist)
            {
                return CustomHttpResult.BadRequest<AddUserResult>("User with the same email already exists.");
            }

            var user = new User { Email = command.Email, Password = command.Password.ComputeSHA256Hash(), UserOId = Guid.NewGuid() };
            var userId = await moneyPilotRepo.AddNewUserAsync(user);
            return CustomHttpResult.Ok<AddUserResult>(new(userId));
        }

        protected override CustomResponse<AddUserResult> GetValidationFailedResult(ValidationResult validationResult)
        {
            return validationResult.GetValidationResult<AddUserResult>();
        }

        protected override IValidator<AddUserCommand> GetValidator()
        {
            var validator = new InlineValidator<AddUserCommand>();
            validator.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            validator.RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,12}$")
                .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character with atleast 8 to 12 characters.")
                .NotEmpty().WithMessage("Password is required.");
            return validator;
        }
    }
}
