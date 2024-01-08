using FluentValidation;

public class GetActorsRequest
{
    public string ActorName { get; set; }
    public int? MinRank { get; set; }
    public int? MaxRank { get; set; }
    public string Provider { get; set; }
    public int Skip { get; set; } = 0; // with default value
    public int Take { get; set; } = 20; // with default value
}

public class GetActorsRequestValidator : AbstractValidator<GetActorsRequest>
{
    public GetActorsRequestValidator()
    {
        RuleFor(request => request.MinRank)
            .GreaterThanOrEqualTo(1)
            .When(request => request.MinRank.HasValue)
            .WithMessage("Minimum rank must be a positive number.");

        RuleFor(request => request.MaxRank)
            .GreaterThanOrEqualTo(1)
            .When(request => request.MaxRank.HasValue)
            .WithMessage("Maximum rank must be a positive number.")
            .GreaterThanOrEqualTo(request => request.MinRank)
            .When(request => request.MaxRank.HasValue && request.MinRank.HasValue)
            .WithMessage("Maximum rank must be greater than or equal to the minimum rank.");

        RuleFor(request => request.Skip)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Skip must be a non-negative number.");

        RuleFor(request => request.Take)
            .InclusiveBetween(1, 100)
            .WithMessage("Take must be between 1 and 100.");
    }
}
