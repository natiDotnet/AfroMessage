using System.Collections.Generic;
using System.Linq;

namespace AfroMessage.Results;

public sealed record ApiError : Error
{
    public ApiError(Error[] errors)
        : base(
            "General.API",
            errors[0].Description,
            ErrorType.Failure)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }
}
