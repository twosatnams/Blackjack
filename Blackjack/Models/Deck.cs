namespace Blackjack.Models;

public class Deck
{
    private Stack<Card> Cards = new Stack<Card>();

    public Deck()
    {
        List<Card> cards = new List<Card>();
        List<Suit> allSuits = new List<Suit>()
        {
            Suit.Clubs, Suit.Diamonds, Suit.Hearts, Suit.Spades
        };
        
        foreach (Suit suit in allSuits)
        {
            cards.Add(new Card(CardValue.Ace, suit));
            cards.Add(new Card(CardValue.Two, suit));
            cards.Add(new Card(CardValue.Three, suit));
            cards.Add(new Card(CardValue.Four, suit));
            cards.Add(new Card(CardValue.Five, suit));
            cards.Add(new Card(CardValue.Six, suit));
            cards.Add(new Card(CardValue.Seven, suit));
            cards.Add(new Card(CardValue.Eight, suit));
            cards.Add(new Card(CardValue.Nine, suit));
            cards.Add(new Card(CardValue.Ten, suit));
            cards.Add(new Card(CardValue.Jack, suit));
            cards.Add(new Card(CardValue.Queen, suit));
            cards.Add(new Card(CardValue.King, suit));
        }

        Random randomizer = new Random();
        cards = cards.OrderBy(x => randomizer.Next()).ToList();
        foreach (Card card in cards)
        {
            Cards.Push(card);
        }
        
        Console.WriteLine("Deck of cards is ready ... ");
    }

    /// <summary>
    /// Constructor for unit tests where we want to control the order of cards dealt
    /// </summary>
    /// <param name="cards">Already initialized deck of cards</param>
    public Deck(Stack<Card> cards)
    {
        Cards = cards;
    }

    public Card DealCard()
    {
        return Cards.Pop();
    }
}