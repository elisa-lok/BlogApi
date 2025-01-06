using FluentValidation;

namespace BlogApi.Models.Validators;

public class UserValidator : AbstractValidator<User>
{
  public UserValidator(){
    RuleFor(u => u.Name)
        .NotEmpty()
        .WithMessage("Name is required")
        .Length(3,50)
        .WithMessage("The length of Name must be between 3 and 50.");
    
    RuleFor(u => u.Email)
        .NotEmpty()
        .WithMessage("Email is required")
        .EmailAddress()
        .WithMessage("Invalid email address.");

    RuleFor(u => u.Password)
        .NotEmpty()
        .WithMessage("Password is required")
        .Length(6, 20)
        .WithMessage("The length of Password must be between 6 and 20.");

    RuleFor(u => u.Country)
        .NotEmpty()
        .WithMessage("Country is required")
        .Length(2, 50)
        .WithMessage("The length of Country must be between 2 and 50.");

    // RuleFor(u => u.BirthDate)
    //     .NotEmpty()
    //     .WithMessage("BirthDate is required")
    //     .Must(BeAValidDate)
    //     .WithMessage("Invalid BirthDate.");

    RuleFor(u => u.PhoneNumber)
        .NotEmpty()
        .WithMessage("PhoneNumber is required");

    // RuleFor(u => u.Gender)
    //     .NotEmpty()
    //     .WithMessage("Gender is required")
    //     .IsInEnum()
    //     .WithMessage("Invalid Gender.");

    RuleFor(u => u)
        .Custom((user, context) =>
        {
            if (user.Country.ToLower() == "new zealand" && !user.PhoneNumber.StartsWith("64"))
            {
                context.AddFailure("The phone number must start with 64 for New Zealand users.");
            }
        });
  } 
}

