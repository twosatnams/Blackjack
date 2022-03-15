# Blackjack
Blackjack implemented in C#

## Introduction
- Read: https://bicyclecards.com/how-to-play/blackjack/
- Watch: https://www.youtube.com/playlist?list=PLLALQuK1NDrj7DaGymT8e8PmguI64cu-P

## Architecture
1. `Game` is the orchestrator, and maintains access to the `Deck` and a list of `Player`s and their respective hands.
2. `Deck` is a container of all cards, and responsible for dealing cards.
3. `Player` is responsible for interacting with the user tactical decisions like splitting, doubling-down etc, and manipulating the `Hand` accordingly.
4. `Hand` is a container of cards that a player holds, and responsible for interacting with the user for turns (hit or stand).

## How to Run (Assuming you're running MacOS)
1. Install .Net 6 SDK: https://dotnet.microsoft.com/en-us/download
2. Open a terminal, and execute `dotnet`. If you see `command not found` then troubleshoot here: https://dotnet.microsoft.com/en-us/learn/dotnet/hello-world-tutorial/install
3. Clone this repository: git clone https://github.com/twosatnams/Blackjack
4. `cd` into the repository directory and then into the `Blackjack` directory.
6. Run `dotnet run`.
7. To run unit tests, run `dotnet test`.

## ToD
- [x] Basic rules
- [x] Player wins 1.5x if they get a natural hand aka blackjack
- [x] Allow players to split
- [x] Allow players to double down
- [x] Allow players to split and double down
- [x] Unit test coverage
- [ ] Insurance
