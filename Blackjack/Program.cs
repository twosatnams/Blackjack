using Blackjack;
using Blackjack.Models;
using Blackjack.Util;

Console.WriteLine("Welcome to Blackjack");

int numPlayers;
Console.Write("Enter the number of players: ");
string? numPlayersInput = Console.ReadLine();
while (
    numPlayersInput == null || 
    !int.TryParse(numPlayersInput, out numPlayers) || 
    numPlayers < 1 || 
    numPlayers > 8)
{
    Console.Write("Invalid input. Please enter an integer value between 1 and 8: ");
    numPlayersInput = Console.ReadLine();
}

InputConsole inputConsole = new InputConsole();
Deck deck = new Deck();
Game game = new Game(numPlayers, deck, inputConsole);
game.Play();