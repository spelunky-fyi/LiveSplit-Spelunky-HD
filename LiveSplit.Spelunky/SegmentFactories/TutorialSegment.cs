

namespace LiveSplit.Spelunky.SegmentFactories
{
  public class TutorialSegment : ISegment
  {
    private LobbyType? LastLobbyType;

    public SegmentStatus CheckStatus(SpelunkyHooks spelunky)
    {
      LobbyType? lastLobbyType = this.LastLobbyType;
      LobbyType lobbyType = LobbyType.DoorOpenNoGem;
      if ((lastLobbyType.GetValueOrDefault() == lobbyType ? (lastLobbyType.HasValue ? 1 : 0) : 0) != 0 && spelunky.CurrentLobbyType == LobbyType.DoorOpenNoGem)
        return new SegmentStatus()
        {
          Type = SegmentStatusType.ERROR,
          Message = "Lobby must be locked during the tutorial segment."
        };
      return new SegmentStatus()
      {
        Type = SegmentStatusType.INFO,
        Message = "Waiting for tutorial completion."
      };
    }

    public bool Cycle(SpelunkyHooks spelunky)
    {
      if (this.CheckStatus(spelunky).Type == SegmentStatusType.ERROR)
        return false;
      LobbyType currentLobbyType = spelunky.CurrentLobbyType;
      LobbyType? lastLobbyType = this.LastLobbyType;
      LobbyType lobbyType = LobbyType.Breaking;
      int num = (lastLobbyType.GetValueOrDefault() == lobbyType ? (lastLobbyType.HasValue ? 1 : 0) : 0) == 0 ? 0 : (currentLobbyType == LobbyType.DoorOpenNoGem ? 1 : 0);
      this.LastLobbyType = new LobbyType?(currentLobbyType);
      return num != 0;
    }
  }
}
