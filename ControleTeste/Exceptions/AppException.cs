using System;

namespace ControleTeste.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }

    public virtual int StatusCode => 500;
}
