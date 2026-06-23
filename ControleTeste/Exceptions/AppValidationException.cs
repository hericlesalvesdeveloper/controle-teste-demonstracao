using System;

namespace ControleTeste.Exceptions;

public class AppValidationException : AppException
{
    public AppValidationException(string message) : base(message) { }

    public override int StatusCode => 400;
}
