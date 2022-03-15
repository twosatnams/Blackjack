using System.Collections.Generic;
using Blackjack;
using Blackjack.Models;
using Blackjack.Util;
using Moq;
using Xunit;

namespace Test;

public class DoubleDownTests
{
    [Fact]
    public void PlayerCanDoubleDownAndWin()
    {
        int numPlayers = 1;

        // Setup Player 1 to double down and win
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Nine, Suit.Diamonds)); // Player 1 card after doubling down
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Three, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        int betAmount = 500;
        
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
                    return "Y"; // Player 1 chooses to double down
                }
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(2 * betAmount * Constants.WinningMultiplier, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanDoubleDownAndLose()
    {
        int numPlayers = 1;

        // Setup Player 1 to double down and lose
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Four, Suit.Diamonds)); // Player 1 card after doubling down
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Three, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Eight, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        int betAmount = 500;
        
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
                    return "Y"; // Player 1 chooses to double down
                }
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(0, game.Players[0].Money);
    }
    
    [Fact]
    public void PlayerCanChooseNotToDoubleDownAndWin()
    {
        int numPlayers = 1;

        // Setup Player 1 to double down and win
        Stack<Card> cards = new Stack<Card>();
        cards.Push(new Card(CardValue.Two, Suit.Clubs)); // Dealer first hit
        cards.Push(new Card(CardValue.Ten, Suit.Clubs)); // Dealer second card
        cards.Push(new Card(CardValue.Jack, Suit.Spades)); // Player 1 second card
        cards.Push(new Card(CardValue.Five, Suit.Clubs)); // Dealer first card
        cards.Push(new Card(CardValue.Jack, Suit.Clubs)); // Player 1 first card

        Deck deck = new Deck(cards);
        Mock<InputConsole> mockInputConsole = new Mock<InputConsole>();
        
        int betAmount = 500;
        int inputSequence = 1;
        bool hasAgreedToNotDoubleDown = false;
        mockInputConsole
            .Setup(t => t.ReadLine())
            .Returns(() =>
            {
                if (inputSequence <= numPlayers)
                {
                    inputSequence += 1;
                    return betAmount.ToString();
                }
                else if (!hasAgreedToNotDoubleDown)
                {
                    hasAgreedToNotDoubleDown = true;
                    return "N"; // Player 1 chooses not to double down
                }
                else
                {
                    return "S";
                }
            });

        Game game = new Game(numPlayers, deck, mockInputConsole.Object, true);
        game.Play();
        
        Assert.Equal(1500, game.Players[0].Money);
    }
}