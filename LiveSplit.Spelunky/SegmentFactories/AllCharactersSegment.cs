

using System;

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class AllCharactersSegment : ISegment
  {
    private int TargetUnlockCount = 16;

    private SegmentStatus CheckStatus(CharactersState chars, int playCount)
    {
      if (chars.NumUnlockedCharacters > 16)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.ERROR,
          Message = string.Format("More than {0} characters have been unlocked.", (object) this.TargetUnlockCount)
        };
      return new SegmentStatus()
      {
        Type = SegmentStatusType.INFO,
        Message = string.Format("Waiting for characters ({0}/{1}). {2}", (object) chars.NumUnlockedCharacters, (object) CharactersState.NumCharacters, (object) this.NextCharacterSpawnChance(chars, playCount))
      };
    }

    public SegmentStatus CheckStatus(SpelunkyHooks spelunky) => this.CheckStatus(spelunky.CharactersState, spelunky.PlayCount);

    public bool Cycle(SpelunkyHooks spelunky)
    {
      CharactersState charactersState = spelunky.CharactersState;
      return this.CheckStatus(charactersState, spelunky.PlayCount).Type != SegmentStatusType.ERROR && charactersState.NumUnlockedCharacters >= this.TargetUnlockCount;
    }

    private string NextCharacterSpawnChance(CharactersState chars, int playCount)
    {
      switch (chars.NumUnlockedRandos)
      {
        case 0:
          return string.Format("Play count: {0} (Mines Spawn: {1}%)", (object) playCount, (object) (100f / (float) Math.Max(9, 51 - playCount)).ToString("0.00"));
        case 1:
          return string.Format("Play count: {0} (Jungle Spawn: {1}%)", (object) playCount, (object) (100f / (float) Math.Max(9, 101 - playCount)).ToString("0.00"));
        case 2:
          return string.Format("Play count: {0} (Ice Spawn: {1}%)", (object) playCount, (object) (100f / (float) Math.Max(9, 201 - playCount)).ToString("0.00"));
        case 3:
          return string.Format("Play count: {0} (Temple Spawn: {1}%)", (object) playCount, (object) (100f / (float) Math.Max(9, 301 - playCount)).ToString("0.00"));
        default:
          return "";
      }
    }
  }
}
