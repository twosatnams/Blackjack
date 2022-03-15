using System.Collections.Generic;
using Blackjack;
using Blackjack.Models;
using Blackjack.Util;
using Moq;
using Xunit;

namespace Test;

public class NaturalsTest
{
    [Fact]
    public void PlayerWithNaturalHandGetsOneAndHalfTimes()
    {
        int numPlayers = 1;

        // Setup Player 1 to have a natural hand
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.King, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Ace, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        int betAmount = 1000;
        
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(betAmount.ToString);

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(1500, game.Players[0].Money);
    }
}