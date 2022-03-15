namespace Blackjack.Util;

public class InputConsole : IInputConsole
{
	public void WriteLine(string s)
	{
		Console.WriteLine(s);
	}
	public virtual string? ReadLine()
	{
		return Console.ReadLine();
	}
}