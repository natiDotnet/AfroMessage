using System;
using System.Linq;
using System.Collections.Generic;

namespace AfroMessage.Requests;

public record Recipient(string To, string Message);