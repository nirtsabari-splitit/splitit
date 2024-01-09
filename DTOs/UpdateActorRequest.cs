using FluentValidation;

public class UpdateActorRequest
{
    public string Name { get; set; }
    public string Details { get; set; }
    public string Type { get; set; }
    public int Rank { get; set; }
    public string Source { get; set; }
}

public class UpdateActorRequestValidator : AbstractValidator<UpdateActorRequest>
{
    public UpdateActorRequestValidator()
    {
        RuleFor(request => request.Rank)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rank must be a positive number.");
    }
}
