using System;
using System.Collections.Generic;
using System.Linq;

namespace AfroMessage.Responses;

public class MessageResponse
{
    public string? Status { get; set; }
    public Guid? MessageId { get; set; }
    public string? Message { get; set; }
    public string? To { get; set; }
}
