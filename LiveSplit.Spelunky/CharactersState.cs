

using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.Spelunky
{
  public class CharactersState
  {
    public static readonly int NumCharacters = Enum.GetNames(typeof (SpelunkyCharacter)).Length;
    public bool[] UnlockedChars;

    public CharactersState(bool[] unlockedChars) => this.UnlockedChars = unlockedChars;

    private static int CountUnlockedCharacters(bool[] entries) => ((IEnumerable<bool>) entries).Where<bool>((Func<bool, bool>) (b => b)).Count<bool>();

    public int NumUnlockedCharacters => CharactersState.CountUnlockedCharacters(this.UnlockedChars);

    private static int CountUnlockedRandos(bool[] entries)
    {
      int num = 0;
      if (entries[2])
        ++num;
      if (entries[4])
        ++num;
      if (entries[6])
        ++num;
      if (entries[7])
        ++num;
      return num;
    }

    public int NumUnlockedRandos => CharactersState.CountUnlockedRandos(this.UnlockedChars);

    public bool Equals(CharactersState o) => new List<Tuple<bool[], bool[]>>()
    {
      new Tuple<bool[], bool[]>(this.UnlockedChars, o.UnlockedChars)
    }.Aggregate<Tuple<bool[], bool[]>, bool>(true, (Func<bool, Tuple<bool[], bool[]>, bool>) ((totalResult, entryPair) => totalResult && entryPair.Item1.Length == entryPair.Item2.Length && Enumerable.Range(0, entryPair.Item1.Length).Aggregate<int, bool>(true, (Func<bool, int, bool>) ((result, currentIndex) => result && entryPair.Item1[currentIndex] == entryPair.Item2[currentIndex]))));
  }
}
