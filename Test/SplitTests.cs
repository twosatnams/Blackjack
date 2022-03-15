using System;
using System.Collections.Generic;
using Blackjack;
using Blackjack.Models;
using Blackjack.Util;
using Moq;
using Xunit;

namespace Test;

public class SplitTests
{
    [Fact]
    public void PlayerCanSplitAndWinOnBoth()
    {
        int numPlayers = 1;

        // Setup Player 1 to split and win on both of the hands
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Queen, Suit.Hearts)); // Player 1 split hand first hit
        cards.Push(new Card(CardValue.King, Suit.Diamonds)); // Player 1 hand card
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Eight, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        
        int betAmount = 500;
        int inputSequence = 1;
        bool hasAgreedToSplit = false;
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
                    return "Y"; // Player 1 chooses to Split
                }
                else
                {
                    return "S";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(2 * betAmount * Constants.WinningMultiplier, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanSplitAndWinOnOne()
    {
        int numPlayers = 1;

        // Setup Player 1 to split and win on one of the hands
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Two, Suit.Hearts)); // Player 1 split hand first hit
        cards.Push(new Card(CardValue.King, Suit.Diamonds)); // Player 1 hand first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Eight, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();

        int betAmount = 500;
        int inputSequence = 1;
        bool hasAgreedToSplit = false;
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
                    return "Y"; // Player 1 chooses to Split
                }
                else
                {
                    return "S";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(betAmount * Constants.WinningMultiplier, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanSplitAndLoseBoth()
    {
        int numPlayers = 1;

        // Setup Player 1 to split and lose on one of the hands
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Two, Suit.Hearts)); // Player 1 split hand first hit
        cards.Push(new Card(CardValue.Three, Suit.Hearts)); // Player 1 hand first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Eight, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();

        int betAmount = 500;
        int inputSequence = 1;
        bool hasAgreedToSplit = false;
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
                    return "Y"; // Player 1 chooses to Split
                }
                else
                {
                    return "S";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(0, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanChooseNotToSplitAndWin()
    {
        int numPlayers = 1;

        // Setup Player 1 to not split and win
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Nine, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Nine, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();

        int betAmount = Constants.StartingMoney;
        int inputSequence = 1;
        bool hasAgreedToNotSplit = false;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return betAmount.ToString();
                }
                else if (!hasAgreedToNotSplit)
                {
                    hasAgreedToNotSplit = true;
                    return "N"; // Player 1 chooses not to Split
                }
                else
                {
                    return "S";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(Constants.StartingMoney * Constants.WinningMultiplier, game.Players[0].Money);
    }
}