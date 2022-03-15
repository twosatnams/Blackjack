using Blackjack.Models;
using Blackjack.Util;

namespace Blackjack;

public class Game
{
    /// <summary>
    /// Flag to facilitate unit testing
    /// </summary>
    private bool StopAfterOneRound;
    
    private Deck Deck;
    public List<Player> Players;
    private Dealer Dealer;

    public Game(
        int numPlayers,
        Deck deck,
        InputConsole inputConsole,
        bool stopAfterOneRound = false)
    {
        Deck = deck;
        Dealer = new Dealer();
        StopAfterOneRound = stopAfterOneRound;
        Players = new List<Player>();
        for (int i = 1; i <= numPlayers; i++)
        {
            Players.Add(new Player($"Player {i}", inputConsole));
        }
    }

    public void Play()
    {
        while (!IsGameOver)
        {
            PromptPlayersToPlaceBets();
            DealInitialCards();
            CheckAndResolveNaturals();
            
            foreach (Player player in this.ActivePlayers)
            {
                player.CheckAndSplit();
                DealCardsForSplit(player);
                player.CheckAndDoubleDown(isSplitHand: false);
                player.CheckAndDoubleDown(isSplitHand: true);
                DealCardsForDoubleDown(player);
                ConcludePlayerTurnForHand(player, isSplitHand: false);
                ConcludePlayerTurnForHand(player, isSplitHand: true);
            }

            ConcludeRound();
            ResetAndBeginNewRound();

            if (StopAfterOneRound)
            {
                break;
            }
        }
        
        Console.WriteLine($"All players have lost their ${Constants.StartingMoney}. Game over ... \n");
    }

    private void PromptPlayersToPlaceBets()
    {
        Console.WriteLine("-----------------------------");
        foreach (Player player in this.NonEliminatedPlayers)
        {
            player.PlaceBet();
        }
    }

    private void CheckAndResolveNaturals()
    {
        Console.WriteLine("\nInitial set of cards have been dealt. Checking if any player has a natural hand ... ");
        Thread.Sleep(1000);

        bool anyNaturals = false;
        
        foreach (Player player in this.NonEliminatedPlayers)
        {
            if (player.HasNaturalHand())
            {
                Console.WriteLine($"{player.Name} has a natural hand. They get {Constants.WinningByNaturalMultiplier * player.Hand.BetAmount}");
                player.Money += (int)(Constants.WinningByNaturalMultiplier * player.Hand.BetAmount);
                anyNaturals = true;
            }
        }

        if (!anyNaturals)
        {
            Console.WriteLine("\nNo players had a natural hand. Moving on to player turns ... \n");
            Thread.Sleep(500);
        }
    }

    private void ConcludeRound()
    {
        Console.WriteLine("-----------------------------");
        Console.WriteLine("All players have finished their turns. Concluding this round ... ");
        Thread.Sleep(500);

        if (this.ActivePlayers.Count == 0)
        {
            Console.WriteLine("All players' hands had busted. Dealer wins");
            return;
        }

        Console.WriteLine($"Dealer's face-down card was {Dealer.Hand.Cards[1].ReadableValue}");

        while (Dealer.Hand.Count() < 17)
        {
            Console.WriteLine($"Dealer has a score: {Dealer.Hand.Count()}, which is less than 17. Dealer must hit");
            Dealer.Hand.AddCard(Deck.DealCard());
        }

        if (Dealer.Hand.IsBusted)
        {
            Console.WriteLine($"The dealer has busted with a score of {Dealer.Hand.Count()}. All active players get {Constants.WinningMultiplier}x");
            foreach (Player player in this.ActivePlayers)
            {
                int winningAmount = Constants.WinningMultiplier * player.Hand.BetAmount;
                player.Money += winningAmount;
                Console.WriteLine($"{player.Name} won ${winningAmount}");
            }

            return;
        }
        else
        {
            foreach (Player player in this.ActivePlayers)
            {
                this.ConcludePlayerWinnings(player, isSplitHand: false);
                if (player.SplitHand.Cards.Count > 0)
                {
                    this.ConcludePlayerWinnings(player, isSplitHand: true);
                }
            }
        }
    }

    private void ResetAndBeginNewRound()
    {
        Dealer.Hand.Cards = new List<Card>();

        foreach (Player player in this.NonEliminatedPlayers)
        {
            if (player.Money > 0)
            {
                player.Hand.BetAmount = 0;
                player.Hand.Cards = new List<Card>();
                player.SplitHand.Cards = new List<Card>();
                player.SplitHand.BetAmount = 0;
            }
            else
            {
                player.IsEliminated = true;
            }
        }
    }

    private void DealInitialCards()
    {
        Console.WriteLine("\nAll players have placed their bets. Dealing initial cards ... \n");
        Thread.Sleep(500);

        // Each player gets one card face up
        foreach (Player player in this.NonEliminatedPlayers)
        {
            player.Hand.AddCard(Deck.DealCard());
        }
        
        // Dealer takes one card face up
        Dealer.Hand.AddCard(Deck.DealCard());
        
        // Each player gets another card face up
        foreach (Player player in this.NonEliminatedPlayers)
        {
            player.Hand.AddCard(Deck.DealCard());
        }
        
        // Dealer takes another card but face down
        Dealer.Hand.AddCard(Deck.DealCard(), faceDown: true);
    }

    private void DealCardsForSplit(Player player)
    {
        if (player.SplitHand.HasCards)
        {
            player.Hand.AddCard(Deck.DealCard());
            player.SplitHand.AddCard(Deck.DealCard());
        }
    }

    private void DealCardsForDoubleDown(Player player)
    {
        if (player.SplitHand.HasCards && player.SplitHand.IsDoubledDown)
        {
            player.SplitHand.AddCard(Deck.DealCard());
        }
        
        if (player.Hand.IsDoubledDown)
        {
            player.Hand.AddCard(Deck.DealCard());
        }
    }

    private void ConcludePlayerTurnForHand(Player player, bool isSplitHand)
    {
        // If the player does not have a split hand and the call is for the split hand, exit immediately
        if (isSplitHand && !player.SplitHand.HasCards)
        {
            return;
        }
        
        Hand hand = isSplitHand ? player.SplitHand : player.Hand;

        if (hand.IsDoubledDown)
        {
            return;
        }

        Move playerMove = hand.TakeTurn();
        while (playerMove == Move.Hit)
        {
            hand.AddCard(Deck.DealCard());
            if (hand.IsBusted)
            {
                Console.WriteLine($"{player.Name}'s {hand.HandName} has busted! And eliminated from this round");
                break;
            }
            else
            {
                playerMove = hand.TakeTurn();
            }
        }
    }

    private void ConcludePlayerWinnings(Player player, bool isSplitHand)
    {
        // If the player does not have a split hand and the call is for the split hand, exit immediately
        if (isSplitHand && !player.SplitHand.HasCards)
        {
            return;
        }

        Hand hand = isSplitHand ? player.SplitHand : player.Hand;

        if (hand.Count() > Dealer.Hand.Count())
        {
            int winningAmount = Constants.WinningMultiplier * hand.BetAmount;
            player.Money += winningAmount;
            Console.WriteLine($"{player.Name}'s {hand.HandName} has a score of {hand.Count()} which is higher than the dealer's. They get ${winningAmount}");
        }
        else if (hand.Count() < Dealer.Hand.Count())
        {
            Console.WriteLine($"{player.Name}'s {hand.HandName} has a score of {hand.Count()} which is lower than the dealer's. They lose ${hand.BetAmount}");
        }
        else
        {
            player.Money += hand.BetAmount;
            Console.WriteLine($"{player.Name}'s {hand.HandName} has a score of {hand.Count()} which is equal to dealer's. They keep ${hand.BetAmount}");
        }
    }
    
    /// <summary>
    /// List of players that have not exhausted their money 
    /// </summary>
    private List<Player> NonEliminatedPlayers => this.Players.Where(p => !p.IsEliminated).ToList();
    
    /// <summary>
    /// List of players that are still in the round
    /// Excludes players that had a natural hand
    /// Excludes players that have busted one or both hands
    /// </summary>
    private List<Player> ActivePlayers => this.NonEliminatedPlayers
        .Where(player => !player.HasNaturalHand() && player.IsInRound)
        .ToList();

    public bool IsGameOver => Players.All(player => player.Money == 0);
}