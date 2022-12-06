

using System;

namespace LiveSplit.Spelunky
{
  public class PatchInitializationFailedException : Exception
  {
    public PatchInitializationFailedException()
      : base("Patch initialization failed")
    {
    }

    public PatchInitializationFailedException(string message)
      : base(message)
    {
    }

    public PatchInitializationFailedException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
