using System.Collections.Generic;
using Blackjack;
using Blackjack.Models;
using Blackjack.Util;
using Moq;
using Xunit;

namespace Test;

public class SplitAndDoubleDownTests
{
    [Fact]
    public void PlayerCanSplitAndDoubleDownAndWinBoth()
    {
        int numPlayers = 1;

        // Setup Player 1 to split and double down and win on both hands
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Spades)); // Dealer hit
        cards.Push(new Card(CardValue.Nine, Suit.Spades)); // Player 1 split hand double down hit
        cards.Push(new Card(CardValue.Nine, Suit.Clubs)); // Player 1 hand double down hit
        cards.Push(new Card(CardValue.Three, Suit.Diamonds)); // Player 1 split hand first hit
        cards.Push(new Card(CardValue.Three, Suit.Hearts)); // Player 1 hand first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Eight, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();

        int betAmount = 250;
        int inputSequence = 1;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return betAmount.ToString();
                }
                else
                {
                    return "Y"; // Player 1 chooses to Split and Double down on both hands
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(2000, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanSplitAndDoubleDownOnSplitHandAndWinSplitHandAndLoseHand()
    {
        int numPlayers = 1;

        // Setup Player 1 to split
        // and double down on split hand
        // and not double down on hand
        // and win on split hand
        // and lose on hand
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Spades)); // Dealer hit
        cards.Push(new Card(CardValue.Nine, Suit.Spades)); // Player 1 split hand double down hit
        cards.Push(new Card(CardValue.Three, Suit.Diamonds)); // Player 1 split hand first hit
        cards.Push(new Card(CardValue.Nine, Suit.Hearts)); // Player 1 hand first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Seven, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Seven, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();

        int betAmount = 250;
        int inputSequence = 1;
        bool hasAgreedToSplit = false;
        bool hasAgreedToDoubleDown = false;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return betAmount.ToString();
                }
                else if (!hasAgreedToSplit)
                {
                    hasAgreedToSplit = true;
                    return "Y"; // Player 1 choose to split
                }
                else if (!hasAgreedToDoubleDown)
                {
                    hasAgreedToDoubleDown = true;
                    return "Y"; // Player 1 choose to double down on split hand
                }
                else
                {
                    return "S"; // Player 1 chooses to stand on hand
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(1250, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanSplitAndDoubleDownOnSplitHandAndWinSplitHandAndWinHand()
    {
        int numPlayers = 1;

        // Setup Player 1 to split
        // and double down on split hand
        // and not double down on hand
        // and win on split hand
        // and win on hand
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Spades)); // Dealer hit
        cards.Push(new Card(CardValue.Three, Suit.Spades)); // Player 1 hand hit
        cards.Push(new Card(CardValue.Nine, Suit.Spades)); // Player 1 split hand double down hit
        cards.Push(new Card(CardValue.Three, Suit.Diamonds)); // Player 1 split hand first hit
        cards.Push(new Card(CardValue.Nine, Suit.Hearts)); // Player 1 hand first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Seven, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Seven, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();

        int betAmount = 250;
        int inputSequence = 1;
        bool hasAgreedToSplit = false;
        bool hasAgreedToDoubleDownOnSplit = false;
        bool hasAgreedToNotDoubleDownOnHand = false;
        bool hasAgreedToHit = false;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return betAmount.ToString();
                }
                else if (!hasAgreedToSplit)
                {
                    hasAgreedToSplit = true;
                    return "Y"; // Player 1 choose to split
                }
                else if (!hasAgreedToDoubleDownOnSplit)
                {
                    hasAgreedToDoubleDownOnSplit = true;
                    return "Y"; // Player 1 choose to double down on split hand
                }
                else if (!hasAgreedToNotDoubleDownOnHand)
                {
                    hasAgreedToNotDoubleDownOnHand = true;
                    return "N"; // Player 1 choose to not double down on hand
                }
                else if (!hasAgreedToHit)
                {
                    hasAgreedToHit = true;
                    return "H"; // Player 1 choose to hit on hand
                }
                else
                {
                    return "S"; // Player 1 chooses to stand on hand
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(1750, game.Players[0].Money);
    }
}