using System.Collections.Generic;

public class Response
{
    public string TraceId { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public List<Error> Errors { get; set; }
}
