public class GetActorsRequest
{
    public string ActorName { get; set; }
    public int? MinRank { get; set; }
    public int? MaxRank { get; set; }
    public string Provider { get; set; }
    public int Skip { get; set; } = 0; // with default value
    public int Take { get; set; } = 20; // with default value
}
