

using System;
using System.IO;
using System.Linq;

namespace LiveSplit.Spelunky
{
  public class SpelunkyHooks : IDisposable
  {
    public RawProcess Process { get; private set; }

    public SpelunkyHooks(RawProcess process) => this.Process = process;

    public string DeviatedGameSavePath { get; set; }

    public string GameDirectoryPath => Path.GetDirectoryName(this.Process.FilePath);

    public string GameSavePath => this.DeviatedGameSavePath ?? this.GameDirectoryPath + "\\Data\\spelunky_save.sav";

    private int Game => this.Process.ReadInt32(this.Process.BaseAddress + 1393772);

    private int Gfx => this.Process.ReadInt32(this.Game + 76);

    public int CharSelectCountdown => this.Process.ReadInt32(this.Gfx + 1190892);

    public SpelunkyState CurrentState => (SpelunkyState) this.Process.ReadInt32(this.Game + 88);

    public SpelunkyLevel CurrentLevel => (SpelunkyLevel) this.Process.ReadInt32(this.Game + 4457940);

    public TunnelManChapter TunnelManChapter => (TunnelManChapter) this.Process.ReadInt32(this.Game + 4479972);

    public LobbyType CurrentLobbyType => (LobbyType) this.Process.ReadInt32(this.Game + 4479968);

    public bool Invalidated => this.Process.HasExited;

    public int PlayCount => this.Process.ReadInt32(this.Game + 4479432);

    public int TunnelManRemaining => this.Process.ReadInt32(this.Game + 4479976);

    private int JournalUnlocksTable => this.Game + 4479980;

    private int JournalPlaceUnlocksTable => this.JournalUnlocksTable + 512;

    private int JournalMonsterUnlocksTable => this.JournalUnlocksTable + 768;

    private int JournalItemUnlocksTable => this.JournalUnlocksTable + 1024;

    private int JournalTrapUnlocksTable => this.JournalUnlocksTable + 1280;

    private int CharacterUnlocksTable => this.Game + 4482028;

    private bool[] ReadJournalEntries(int processOffset, int size)
    {
      bool[] flagArray = new bool[size];
      foreach (int index in Enumerable.Range(0, size))
      {
        int num = this.Process.ReadInt32(processOffset + index * 4);
        switch (num)
        {
          case 0:
            flagArray[index] = false;
            continue;
          case 1:
            flagArray[index] = true;
            continue;
          default:
            throw new Exception(string.Format("Unexpected journal entry value: {0}", (object) num));
        }
      }
      return flagArray;
    }

    public JournalState JournalState => new JournalState(this.ReadJournalEntries(this.JournalPlaceUnlocksTable, JournalState.NumPlaceEntries), this.ReadJournalEntries(this.JournalMonsterUnlocksTable, JournalState.NumMonsterEntries), this.ReadJournalEntries(this.JournalItemUnlocksTable, JournalState.NumItemEntries), this.ReadJournalEntries(this.JournalTrapUnlocksTable, JournalState.NumTrapEntries));

    private bool[] ReadCharacters(int processOffset, int size)
    {
      bool[] flagArray = new bool[size];
      foreach (int index in Enumerable.Range(0, size))
      {
        int num = this.Process.ReadInt32(processOffset + index * 4);
        switch (num)
        {
          case 0:
            flagArray[index] = false;
            continue;
          case 1:
            flagArray[index] = true;
            continue;
          default:
            throw new Exception(string.Format("Unexpected character unlock value: {0}", (object) num));
        }
      }
      return flagArray;
    }

    public CharactersState CharactersState => new CharactersState(this.ReadCharacters(this.CharacterUnlocksTable, CharactersState.NumCharacters));

    public void Dispose() => this.Process.Dispose();
  }
}
