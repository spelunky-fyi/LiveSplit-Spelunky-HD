

using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.Spelunky
{
  public class SaveChangePatch : IPatch
  {
    private static string SpelunkySave = "spelunky_save";
    private static string SplitterSave = "splitter_save";
    private static readonly byte?[] SearchSignature = ((IEnumerable<char>) SaveChangePatch.SpelunkySave.ToCharArray()).Select<char, byte?>((Func<char, byte?>) (c => new byte?((byte) c))).ToArray<byte?>();
    private static readonly byte[] RevertValue = ((IEnumerable<char>) SaveChangePatch.SpelunkySave.ToCharArray()).Select<char, byte>((Func<char, byte>) (c => (byte) c)).ToArray<byte>();
    private static readonly byte[] OverwriteValue = ((IEnumerable<char>) SaveChangePatch.SplitterSave.ToCharArray()).Select<char, byte>((Func<char, byte>) (c => (byte) c)).ToArray<byte>();
    private SpelunkyHooks Spelunky;
    private List<int> PatchAddresses;

    public SaveChangePatch(SpelunkyHooks spelunky)
    {
      this.PatchAddresses = new List<int>();
      this.Spelunky = spelunky;
      this.FindPatchAddresses();
    }

    private void FindPatchAddresses()
    {
      int? nullable = new int?(-1);
      while ((nullable = this.Spelunky.Process.FindBytes(SaveChangePatch.SearchSignature, nullable.Value + 1)).HasValue)
        this.PatchAddresses.Add(nullable.Value);
      if (this.PatchAddresses.Count == 0)
        throw new PatchInitializationFailedException("Failed to find any patch offsets for save change patch");
    }

    public void Apply()
    {
      foreach (int patchAddress in this.PatchAddresses)
      {
        this.Spelunky.Process.SetMemoryWritable(patchAddress, SaveChangePatch.SplitterSave.Length);
        this.Spelunky.Process.WriteBytes(patchAddress, SaveChangePatch.OverwriteValue);
      }
      this.Spelunky.DeviatedGameSavePath = this.Spelunky.GameDirectoryPath + "\\Data\\" + SaveChangePatch.SplitterSave + ".sav";
    }

    public void Revert()
    {
      foreach (int patchAddress in this.PatchAddresses)
        this.Spelunky.Process.WriteBytes(patchAddress, SaveChangePatch.RevertValue);
      this.Spelunky.DeviatedGameSavePath = (string) null;
    }
  }
}
