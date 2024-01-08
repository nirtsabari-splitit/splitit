// We shouldn't be using the same class for both the Domain Model and the Database Model,
// but for the sake of simplicity, we'll do it here.

public class ActorModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public string Type { get; set; }
    public int Rank { get; set; }
    public string Source { get; set; }
}
