

using System;
using System.Collections.Generic;

namespace LiveSplit.Spelunky
{
  public class EnabledPatchContainer
  {
    private List<IPatch> Patches;

    public EnabledPatchContainer() => this.Patches = new List<IPatch>();

    public bool TryAddAndEnable(EnabledPatchContainer.PatchInstantiator inst)
    {
      try
      {
        IPatch patch = inst();
        patch.Apply();
        this.Patches.Add(patch);
        return true;
      }
      catch (PatchInitializationFailedException ex)
      {
        Console.WriteLine("Warning: Failed to initialize patch: " + ex.Message);
        return false;
      }
    }

    public void RevertAll()
    {
      foreach (IPatch patch in this.Patches)
        patch.Revert();
    }

    public delegate IPatch PatchInstantiator();
  }
}
