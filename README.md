# Entity-Continuity
A Simulation of Life

Expanding on the work that I have been doing with path finding algorithms to make my implementation of a life simulation.
> Inspired of Conway's Game of Life.

## Rules
*	Entity cells navigate around a generated map, attempting to acquire randomly dispersed food cells.
*	If entities obtain food cells, they will increase their level by 1.
*	If they reach level 10, they will split into two level 0 cells of the same house.
*	This process will then continue, slowly increasing the population of the house.
*	Higher levelled cells can hunt lower levelled cells of a different house that have a level higher than 0. If it captures the target cell, it will consume its levels and terminate it.
*	Lower levelled cells will avoid higher levelled cells of a different house.
*	As entities move around the map, they will increase their hunger level. The larger the cell's level, the faster this will happen. If a cell reaches 20, it will consume 1 of its levels until there are none left causing it to die.

## Demo Video
[![Project Demo Video](http://img.youtube.com/vi/dFQGAOWGyvA/0.jpg)](http://www.youtube.com/watch?v=dFQGAOWGyvA "Entity Continuity Demo Video")

## Screenshots
<img src="https://raw.githubusercontent.com/jackkimmins/Entity-Continuity/master/imgs/1.png">
