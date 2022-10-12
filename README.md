# Game-Developer-Portfolio


- Table Of Contents
- Project 1
  - Short Videos + Explanation/Short Description
- Project 2
  - Short Videos + Explanation/Short Description
  
------------  
### Project 1 - Finite Labryinth
  
  A simple 2D Roguelike Space Shooter (prototype) where the player must clear 3 levels in total, and the goal for each level is to simply destroy the required number of enemy ships. Clearing each incrementing level rewards the player with powerups to tackle stronger enemies. The level environment, enemy ships, and the player are mostly randomised to provide the player a finite amount of new experiences.

##### **Combat Gameplay**
https://user-images.githubusercontent.com/115569550/195172027-6882dbeb-2435-43ff-aab4-3719c1019698.mp4


The combat gameplay, shooting enemy ships but the player has to be careful when taking on many ships as they must maintain their resources (health, energy, armour) well. To summarise the player's resources, the health is tied to the armour in which you only take health damage when the armour is depleted, and the armour is the only one with damage reduction. Lastly, the player's energy is required when moving faster (from walking to running) and for firing and dodging projectiles.
 
##### **Randomly Generated Level Environments**
https://user-images.githubusercontent.com/115569550/195172093-65d5dcfb-75af-494a-8d7e-82c4dc55852f.mp4


Every level is always different. Each level will always have a random number of rooms, made of asteroids, between 1 and 7, and are randomly placed in the space, each rooms are connected by corridors which are done by using an optimization technique, Random Restart Hill Climb, to find the shortest Hamiltonian Path (each room is traversed only once and has a path). Then enemies are randomly distributed inside and outside of the rooms.

##### **Augment System**
https://user-images.githubusercontent.com/115569550/195343464-8d0037a0-0d9f-44e2-a4ad-8c9a6fc7acf2.mp4


At the end of each level (except the last level), players can improve their ship parts (Weapon, Armour, Booster, Frame, Core) by augmenting them. Each augments have tiers where the higher the tier the stronger the improvement on that ship part, but in exchange, it requires a higher cost.

##### **Randomised And Balanced Ship Parts**



 
