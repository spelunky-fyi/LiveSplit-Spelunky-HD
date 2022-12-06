

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class CharSelectSegment : ISegment
  {
    private SpelunkyState? LastState;
    private int? LastCharSelectCountdown;

    public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
    {
      if (spelunky.CurrentLobbyType != LobbyType.Tutorial && spelunky.CurrentState != SpelunkyState.SplashScreen)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.ERROR,
          Message = "The game save must be reset to lock the lobby."
        };
      if (spelunky.CurrentState != SpelunkyState.CharacterSelect)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.INFO,
          Message = "Waiting for character selection screen."
        };
      return new SegmentStatus()
      {
        Type = SegmentStatusType.INFO,
        Message = "Waiting for character selection."
      };
    }

    public bool Cycle(SpelunkyHooks spelunky)
    {
      if (this.CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
        return false;
      SpelunkyState currentState = spelunky.CurrentState;
      int charSelectCountdown1 = spelunky.CharSelectCountdown;
      SpelunkyState? lastState = this.LastState;
      SpelunkyState spelunkyState = SpelunkyState.CharacterSelect;
      int num1;
      if ((lastState.GetValueOrDefault() == spelunkyState ? (lastState.HasValue ? 1 : 0) : 0) != 0)
      {
        int? charSelectCountdown2 = this.LastCharSelectCountdown;
        int num2 = 0;
        if ((charSelectCountdown2.GetValueOrDefault() == num2 ? (charSelectCountdown2.HasValue ? 1 : 0) : 0) != 0 && currentState == SpelunkyState.CharacterSelect)
        {
          num1 = charSelectCountdown1 != 0 ? 1 : 0;
          goto label_6;
        }
      }
      num1 = 0;
label_6:
      this.LastState = new SpelunkyState?(currentState);
      this.LastCharSelectCountdown = new int?(charSelectCountdown1);
      return num1 != 0;
    }
  }
}
