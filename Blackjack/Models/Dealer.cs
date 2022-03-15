using Blackjack.Util;

namespace Blackjack.Models;

public class Dealer : Player
{
    public Dealer() : base("Dealer", new InputConsole())
    {
        Money = Int32.MaxValue;
    }
}