

namespace LiveSplit.Spelunky
{
  public interface IPatch
  {
    void Apply();

    void Revert();
  }
}
