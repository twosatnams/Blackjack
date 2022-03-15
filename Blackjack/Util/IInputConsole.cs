namespace Blackjack.Util;

public interface IInputConsole
{
	void WriteLine(string s);
	string? ReadLine();
}