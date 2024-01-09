using FluentValidation;

public class UpsertActorRequest
{
    public string Name { get; set; }
    public string Details { get; set; }
    public string Type { get; set; }
    public int Rank { get; set; }
    public string Source { get; set; }
}

public class InsertActorRequestValidator : AbstractValidator<UpsertActorRequest>
{
    public InsertActorRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(request => request.Type).NotEmpty().WithMessage("Type is required.");
        RuleFor(request => request.Rank)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rank must be a positive number.");
        RuleFor(request => request.Source).NotEmpty().WithMessage("Source is required.");
    }
}

public class UpdateActorRequestValidator : AbstractValidator<UpsertActorRequest>
{
    public UpdateActorRequestValidator()
    {
        RuleFor(request => request.Rank)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rank must be a positive number.");
    }
}
