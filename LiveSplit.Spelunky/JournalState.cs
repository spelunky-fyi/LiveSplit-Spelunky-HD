

using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.Spelunky
{
  public class JournalState
  {
    public static readonly int NumPlaceEntries = Enum.GetNames(typeof (PlaceEntry)).Length;
    public static readonly int NumMonsterEntries = Enum.GetNames(typeof (MonsterEntry)).Length;
    public static readonly int NumItemEntries = Enum.GetNames(typeof (ItemEntry)).Length;
    public static readonly int NumTrapEntries = Enum.GetNames(typeof (TrapEntry)).Length;
    public static readonly int NumEntries = JournalState.NumPlaceEntries + JournalState.NumMonsterEntries + JournalState.NumItemEntries + JournalState.NumTrapEntries;
    public bool[] PlaceEntries;
    public bool[] MonsterEntries;
    public bool[] ItemEntries;
    public bool[] TrapEntries;

    public JournalState(
      bool[] placeEntries,
      bool[] monsterEntries,
      bool[] itemEntries,
      bool[] trapEntries)
    {
      this.PlaceEntries = placeEntries;
      this.MonsterEntries = monsterEntries;
      this.ItemEntries = itemEntries;
      this.TrapEntries = trapEntries;
    }

    private static int CountUnlockedEntries(bool[] entries) => ((IEnumerable<bool>) entries).Where<bool>((Func<bool, bool>) (b => b)).Count<bool>();

    public int NumUnlockedPlaceEntries => JournalState.CountUnlockedEntries(this.PlaceEntries);

    public int NumUnlockedMonsterEntries => JournalState.CountUnlockedEntries(this.MonsterEntries);

    public int NumUnlockedItemEntries => JournalState.CountUnlockedEntries(this.ItemEntries);

    public int NumUnlockedTrapEntries => JournalState.CountUnlockedEntries(this.TrapEntries);

    public int NumUnlockedEntries => this.NumUnlockedPlaceEntries + this.NumUnlockedMonsterEntries + this.NumUnlockedItemEntries + this.NumUnlockedTrapEntries;

    public bool Equals(JournalState o) => new List<Tuple<bool[], bool[]>>()
    {
      new Tuple<bool[], bool[]>(this.PlaceEntries, o.PlaceEntries),
      new Tuple<bool[], bool[]>(this.MonsterEntries, o.MonsterEntries),
      new Tuple<bool[], bool[]>(this.ItemEntries, o.ItemEntries),
      new Tuple<bool[], bool[]>(this.TrapEntries, o.TrapEntries)
    }.Aggregate<Tuple<bool[], bool[]>, bool>(true, (Func<bool, Tuple<bool[], bool[]>, bool>) ((totalResult, entryPair) => totalResult && entryPair.Item1.Length == entryPair.Item2.Length && Enumerable.Range(0, entryPair.Item1.Length).Aggregate<int, bool>(true, (Func<bool, int, bool>) ((result, currentIndex) => result && entryPair.Item1[currentIndex] == entryPair.Item2[currentIndex]))));
  }
}
