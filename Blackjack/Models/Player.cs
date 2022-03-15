using Blackjack.Util;

namespace Blackjack.Models;

public class Player
{
    private readonly InputConsole InputConsole;
    public string Name;
    public int Money;
    public Hand Hand;
    public Hand SplitHand;
    public bool IsEliminated;

    public Player(string name, InputConsole inputConsole)
    {
        Name = name;
        Money = Constants.StartingMoney;
        Hand = new Hand(Name, false, inputConsole);
        SplitHand = new Hand(Name, true, inputConsole);
        IsEliminated = false;
        InputConsole = inputConsole;
    }

    public bool IsInRound => this.SplitHand.Cards.Count > 0 ? 
        !this.Hand.IsBusted || !this.SplitHand.IsBusted : 
        !this.Hand.IsBusted; 

    public void PlaceBet()
    {
        Console.Write($"{this.Name}, You have ${this.Money} left, how much do you want to bet? ");
        string? betAmountInput = InputConsole.ReadLine();
        while (
            betAmountInput == null ||
            !int.TryParse(betAmountInput, out Hand.BetAmount) ||
            Hand.BetAmount > this.Money ||
            Hand.BetAmount <= 0)
        {
            Console.Write($"Invalid Value. Enter an integer value less than {this.Money} and greater than 0: ");
            betAmountInput = InputConsole.ReadLine();
        }
        
        this.Money -= Hand.BetAmount;
    }

    public bool HasNaturalHand()
    {
        if (this.Hand.Cards.Count != 2)
        {
            return false;
        }
        else if (
            this.Hand.Cards[0].IsFaceCard && this.Hand.Cards[1].Value == CardValue.Ace ||
            this.Hand.Cards[1].IsFaceCard && this.Hand.Cards[0].Value == CardValue.Ace)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckAndSplit()
    {
        if (!this.Hand.IsSplittable || this.Money < this.Hand.BetAmount)
        {
            return;
        }
        
        Console.Write($"{this.Name}, you have a pair of {this.Hand.Cards[0].ReadableValue}, would you like to split? Y or N: ");
        string? splitChoiceInput = InputConsole.ReadLine();

        while (
            splitChoiceInput == null ||
            (splitChoiceInput.ToUpper() != "Y" && splitChoiceInput.ToUpper() != "N"))
        {
            Console.WriteLine("Invalid input. Enter 'Y' if you'd like to split, or 'N' if you do not");
            splitChoiceInput = InputConsole.ReadLine();
        }

        if (splitChoiceInput.ToUpper() == "Y")
        {
            Console.WriteLine($"{this.Name} has chosen to split. Creating a separate hand ... ");
            Card cardFromOriginalHand = this.Hand.Cards[1];
            this.SplitHand.Cards.Add(cardFromOriginalHand);
            this.Hand.Cards.Remove(cardFromOriginalHand);
            this.SplitHand.BetAmount = this.Hand.BetAmount;
            this.Money -= this.SplitHand.BetAmount;
        }
        else
        {
            Console.WriteLine($"{this.Name} has chosen not to split");
        }
    }

    public void CheckAndDoubleDown(bool isSplitHand)
    {
        // If the player does not have a split hand and the call is for the split hand, exit immediately
        if (isSplitHand && !this.SplitHand.HasCards)
        {
            return;
        }
        
        Hand hand = isSplitHand ? this.SplitHand : this.Hand;

        if (!hand.IsDoubleDownAble || this.Money < hand.BetAmount)
        {
            return;
        }
        
        Console.Write($"{this.Name}, your {hand.HandName} has a count of {hand.Count()}, would you like to double down? Y or N: ");
        string? doubleDownChoiceInput = InputConsole.ReadLine();

        while (
            doubleDownChoiceInput == null ||
            (doubleDownChoiceInput.ToUpper() != "Y" && doubleDownChoiceInput.ToUpper() != "N"))
        {
            Console.WriteLine("Invalid input. Enter 'Y' if you'd like to double down, or 'N' if you do not");
            doubleDownChoiceInput = InputConsole.ReadLine();
        }

        if (doubleDownChoiceInput.ToUpper() == "Y")
        {
            Console.WriteLine($"{this.Name} has chosen to double down on {hand.HandName}. It's worth ${2 * hand.BetAmount} now ... ");
            this.Money -= hand.BetAmount;
            hand.BetAmount += hand.BetAmount;
            hand.IsDoubledDown = true;
        }
        else
        {
            Console.WriteLine($"{this.Name} has chosen not to double down");
        }
    }
}