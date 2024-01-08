using System.Collections.Generic;

public class ActorsResponseEntry
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class ActorsResponse : Response
{
    public List<ActorsResponseEntry> Actors { get; set; }
}
