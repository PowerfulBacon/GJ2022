# GJ2022
A game made around the theme of Duality.

The game idea is that you view down on a space station, can place rooms and build it however you like. You can play the game however you want and help who you want.
The idea is to be a mix between a zen-like game where you can't lose and can't win, but at the same time a game where you make tough decisions to keep the people you want to be alive alive.

While the core idea is around the duality between a zen like sandbox game and a stressful decision making game, other ways to fit the theme are planned to be added.
The game should have some kind of anomalous thingy that lets you get powers, but at the cost of something equally bad and good happening.

Will I be able to implement all of this in 10 days without using a game engine? Probably not, but hopefully there is a basic simulation by the end of it where you can watch your people and build a station.

## License

All assets including icons and sound are under a Creative Commons 3.0 BY-SA license unless otherwise indicated. https://creativecommons.org/licenses/by-sa/3.0/
The majority of game assets are taken from the Space Station 13 codebase 'Beestation', https://github.com/BeeStation/BeeStation-Hornet
This does not cover the code for the project.

## Update:

The game plan has changed to be a rimworld style game where you build a space station. Similar to before, except now rather than pre-determined room sizes the rooms will be designed by drag and drop.
This should be about the same difficulty to perform, although might require less abstraction down the line meaning more features will be required, however pathfinding should be easier and the game looks better and plays better.

![image](https://user-images.githubusercontent.com/26465327/150687457-2d106f7a-9949-4485-8a70-315d7c270c65.png)

## Code:

Subsybstem - Subsystems run processes on a seperate thread.
Managers - Managers store data. Used when seperate thread processing isn't needed for subsystems.
Utility - Stores no data, provides helper methods.
GlobalDataComponents - This should be managers (//TODO)

# Screenshots

![image](https://user-images.githubusercontent.com/26465327/150699998-b8a64d51-fbb2-422a-8849-2b1fd1a79870.png)

![image](https://user-images.githubusercontent.com/26465327/150700011-4fad9372-22ab-463f-bdd6-0a97cbe6ec23.png)

![image](https://user-images.githubusercontent.com/26465327/150700037-9a8343f0-7fa6-490f-847a-3ba4f01f1f14.png)

![image](https://user-images.githubusercontent.com/26465327/150989860-b57af124-09da-44e2-9b07-b696c05d7854.png)
(Running on the lab VMs)
