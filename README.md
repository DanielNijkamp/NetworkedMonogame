# Monogame networking
A networked ECS PoC using [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs) for leaderboard generation

## Info:
this project is for my 2nd internship where i learned a lot about .NET App/Web development. 
Although i learned a lot about front-end and back-end i found the back-end to be more interesting and relevant to game development
As someone who wants to specialize in game engine development and system architecture
i found CQRS very interesting and wanted to test if this pattern could be realistically implemented and used in a Networked Game Architecture 

The idea of the project is to have a normal client-server architecture for our game. and be able to move around with the player. 
this will be done via commands, separate from our game we will have a system to count up how many times a player has moved and create a read-only leaderboard where a client can query it.

This project is only for showcasing the end-result and highlighting the architecture, having so many new ideas like ECS, Networking, CQRS to implement 
each with its own thought process and complexity's its only natural that this project is not perfect. 
some of the code is somewhat poorly written and hard-coded due to time constraints.

The strict-component iteration is something i though of myself and might be a stupid idea to implement in a real game engine but who knows.
its something i would not implement in a production game engine without further experimentation, research and profiling.

***

## Features:

* **Bi-directional command communication for client & server:** Both the client and server can send the same kind of Command([DTO](https://en.wikipedia.org/wiki/Data_transfer_object)) with each its own implementation of how to respond to it
* **Strict Component iteration:** The EntityStorage object (which stores our entire game state) has 2 different data structures: 

    * Key-value-pair row-based structure storing our Game as ` EntityID : Components` which allows systems to process on a per-entity basis which is the standard
    * Column-based type-instance structure that stores component instances per type, for example: `TransformComponent (type) : TranformComponent[] (Array of instances)`.
this allows system to strictly iterate **on components only** not requiring entities to be involved (has been changed to use string as key for serialization). Also since serialization can destroy object references the componentMap is able to be rebuild using the dictionary
* **Re-usability, customizability and testability of components:** Due to the nature of ECS our component and systems are separate and isolated, meaning we can easily created isolated tests for each of them. they can also be added to any entity so they are highly reusable. 

* **Client-Server replication:** since we are storing the entire game in a database-like structure all 
we have to do to update a client or the server is to replicate our data from one to the other. this is also why our Bi-directional commands work



***
## Technologies & design/architectural patterns:
The client is developed in [Monogame](https://monogame.net/) 
using a custom [ECS](https://en.wikipedia.org/wiki/Entity_component_system) implementation based on Martin's ECS with custom features.

the game server is a setup using [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-8.0#what-is-signalr) using RPC.
for our leaderboard we just have a basic ASP.NET API

we also use [MessagePack](https://msgpack.org/index.html) for speedy binary message format instead of JSON

[Command pattern](https://refactoring.guru/design-patterns/command) is used for inner-application communication but also communication between client and server. every action concerning a entity is done via a command.
commands are processed to their respective handlers using [MediatR](https://github.com/jbogard/MediatR)

[Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) is also used. This allows us to register our systems as singletons and automatically allow our systems to have their dependencies with only minor initial setup.

For our server leaderboard communication we use [RabbitMQ]() for background messaging. 

