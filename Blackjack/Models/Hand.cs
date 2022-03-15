using Blackjack.Util;

namespace Blackjack.Models;

public class Hand
{
    private string PlayerName;
    private readonly InputConsole InputConsole;
    public List<Card> Cards;
    public int BetAmount;
    public bool IsSplitHand;
    public bool IsDoubledDown;

    public Hand(string playerName, bool isSplitHand, InputConsole inputConsole)
    {
        PlayerName = playerName;
        InputConsole = inputConsole;
        IsSplitHand = isSplitHand;
        Cards = new List<Card>();
        BetAmount = 0;
        IsDoubledDown = false;
    }
    
    public Move TakeTurn()
    {
        Console.Write($"{this.PlayerName}, It's your turn. Your {this.HandName}'s current score is {this.Count()}. Enter H to hit or enter S to stand: ");
        string? playerInput = InputConsole.ReadLine();
        while (
            playerInput == null ||
            (playerInput.ToUpper() != "S" && playerInput.ToUpper() != "H"))
        {
            Console.Write($"Invalid Value. Enter either S or H: ");
            playerInput = InputConsole.ReadLine();
        }

        Move choice = playerInput == "H" ? Move.Hit : Move.Stand;

        Console.WriteLine($"{this.PlayerName} choose to {choice}");

        return choice;
    }
    
    public void AddCard(Card card, bool faceDown = false)
    {
        Cards.Add(card);
        
        Console.WriteLine(faceDown
            ? $"{this.PlayerName} was dealt a card face down"
            : $"{this.PlayerName}'s {this.HandName} was dealt {card.Value} of {card.Suit}. Bringing their score to {this.Count()}");
        Thread.Sleep(500);
    }
    
    public int Count()
    {
        int result = 0;
        bool hasAce = false;
        foreach (Card card in Cards)
        {
            if (card.Value == CardValue.Ace)
            {
                hasAce = true;
            }
            else if (card.IsFaceCard)
            {
                result += 10;
            }
            else
            {
                result += (int) card.Value;
            }
        }

        if (hasAce)
        {
            if (result + 11 <= 21)
            {
                return result + 11;    
            }
            else
            {
                return result + 1;
            }
        }
        else
        {
            return result;
        }
    }

    public bool IsBusted => this.Count() > 21;
    
    public string HandName => this.IsSplitHand ? "Split Hand" : "Hand";

    public bool IsSplittable
    {
        get
        {
            if (this.IsSplitHand || this.Cards.Count != 2)
            {
                throw new ApplicationException("Checking for splits at the wrong time");
            }
            else
            {
                return this.Cards[0].Value == this.Cards[1].Value;
            }   
        }
    }

    public bool IsDoubleDownAble => 9 <= this.Count() && this.Count() <= 11;

    public bool HasCards => this.Cards.Count > 0;
}