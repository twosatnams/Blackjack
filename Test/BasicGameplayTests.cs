using System;
using System.Collections.Generic;
using Blackjack;
using Blackjack.Models;
using Blackjack.Util;
using Xunit;
using Moq;

namespace Test;

public class BasicGameplayTests
{
    [Fact]
    public void GameEndsWhenAllPlayersRunOutOfMoney()
    {
        int numPlayers = 2;

        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Queen, Suit.Clubs)); // Player 2 first hit
        cards.Push(new Card(CardValue.King, Suit.Spades)); // Player 1 first hit
        cards.Push(new Card(CardValue.Three, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Jack, Suit.Clubs)); // Player 2 second card
        cards.Push(new Card(CardValue.Queen, Suit.Diamonds)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Six, Suit.Clubs)); // Player 2 first card
        cards.Push(new Card(CardValue.Seven, Suit.Clubs)); // Player 1 first card
        
        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        
        // Setup both player to bet 1000
        int inputSequence = 1;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return Constants.StartingMoney.ToString();
                }
                else
                {
                    return "H";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object);
        game.Play();
        
        Assert.True(game.IsGameOver);
        Assert.All(game.Players, player => Assert.True(player.IsEliminated));
    }
    
    [Fact]
    public void PlayersInRoundGet2xWhenDealerBusts()
    {
        int numPlayers = 2;

        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.King, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Seven, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Four, Suit.Clubs)); // Player 2 second card
        cards.Push(new Card(CardValue.Three, Suit.Clubs)); // Player 1 second card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Player 2 first card
        cards.Push(new Card(CardValue.Queen, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        
        // Setup both player to bet 1000
        int inputSequence = 1;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return Constants.StartingMoney.ToString();
                }
                else
                {
                    return "S";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.All(game.Players, player => Assert.True(
            player.Money == Constants.StartingMoney * Constants.WinningMultiplier));
    }
    
    [Fact]
    public void WinnerGet2xWhenDealerHasLowerScore()
    {
        int numPlayers = 2;

        // Setup Player 1 to win, and player 2 to lose
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.King, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Three, Suit.Spades)); // Player 2 second card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 second card
        cards.Push(new Card(CardValue.Seven, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Jack, Suit.Clubs)); // Player 2 first card
        cards.Push(new Card(CardValue.Queen, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        
        // Setup both player to bet 1000
        int inputSequence = 1;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return Constants.StartingMoney.ToString();
                }
                else
                {
                    return "S";
                }
                
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(Constants.StartingMoney * Constants.WinningMultiplier, game.Players[0].Money);
        Assert.Equal(0, game.Players[1].Money);
    }
}