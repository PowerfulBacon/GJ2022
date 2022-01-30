# GJ2022

This project is a highly efficient 2D game engine, running in C# using openGL for graphics. The rendering engine is a heavilly modified variant of a 3D block based rendering engine I made for another project that never got finished, with optimisations for 2D graphics, large amounts of code cleanup and some pretty big refactorings making it almost completely seperate from the original renderer.
The game is inspired by games such as Rimworld and Prison Architect where you command pawns to construct and perform actions for you, while you exist in a god-like observer state, as well as taking inspiration from Space Station 13, a simulation/sandbox/social deduction game where you play as a crewmember aboard a technologically advanced but cursed space station.
The game and gameplay itself isn't that impressive, however the framework for the engine and possible expansions that can be made in the future is where this project makes up for that, with expandable frameworks for:
 - New rendering systems and shaders
 - New easilly addible entities
 - New AI behaviours and pawn actions
 - New subsystems that can perform processing

While it would still need a lot of work to become a fun and playable game, this project is successful as a prototype / proof of concept.

The global game jam lasted from 20/01/2022 to 30/01/2022 and in this time approximately ~12000 lines of code have been added to the repo and ~3000 removed, probably going to take a break for a while to get less burnt out.

Unfortunately I spent a lot of time making the engine robust, so didn't get to implement as many game related features as I hoped, however in the future it would be nice to add pawn health systems, atmospherics, environmental dangers, hostile NPCs, improved textures for pawns etc.

## Initial Game Plan

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
![image](https://user-images.githubusercontent.com/26465327/151701715-516d425d-f59d-49a2-b50a-c8c2a40ef45d.png)

![image](https://user-images.githubusercontent.com/26465327/151701696-71b36c87-d0fb-4bd1-ba8b-9de9a3de0168.png)

![image](https://user-images.githubusercontent.com/26465327/151696771-f4655219-0f6a-4b92-8677-767716b5b850.png)

![image](https://user-images.githubusercontent.com/26465327/151695011-6920e1b4-ea3a-47a0-95a8-898adf6c96e8.png)

![image](https://user-images.githubusercontent.com/26465327/151676063-88193305-e95f-4929-be42-14213ef7437d.png)

![image](https://user-images.githubusercontent.com/26465327/151675978-3c7d3439-bb42-4433-b7ea-cff446f360a7.png)

![image](https://user-images.githubusercontent.com/26465327/151675950-59bea494-db30-4dfe-9ab6-21c084e22e89.png)

![image](https://user-images.githubusercontent.com/26465327/151660809-42c4ef5e-74f9-451c-80ab-d45a63042c6b.png)

![image](https://user-images.githubusercontent.com/26465327/151601612-20988587-1d69-44ee-a557-d7ab98be9829.png)

![image](https://user-images.githubusercontent.com/26465327/151583013-fe18a87a-3d6e-481f-82c9-44461e6304a9.png)

![image](https://user-images.githubusercontent.com/26465327/150699998-b8a64d51-fbb2-422a-8849-2b1fd1a79870.png)

![image](https://user-images.githubusercontent.com/26465327/150700011-4fad9372-22ab-463f-bdd6-0a97cbe6ec23.png)

![image](https://user-images.githubusercontent.com/26465327/150700037-9a8343f0-7fa6-490f-847a-3ba4f01f1f14.png)

![image](https://user-images.githubusercontent.com/26465327/150989860-b57af124-09da-44e2-9b07-b696c05d7854.png)
(Running on the lab VMs)
