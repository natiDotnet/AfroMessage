using System;
using System.Linq;
using System.Collections.Generic;

namespace AfroMessage.Responses;

public class MessageResponse
{
    public string Status { get; set; }
    public Guid MessageId { get; set; }
    public string Message { get; set; }
    public string To { get; set; }
}
