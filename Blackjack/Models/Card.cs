namespace Blackjack.Models;

public class Card
{
    public Suit Suit;
    public CardValue Value;

    public Card(CardValue value, Suit suit)
    {
        Suit = suit;
        Value = value;
    }
    
    public bool IsFaceCard => this.Value is CardValue.Jack or CardValue.Queen or CardValue.King;

    public string ReadableValue => $"{this.Value} of {this.Suit}";
}