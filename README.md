# Game-Developer-Portfolio


- Table Of Contents
- Project 1
  - Short Videos + Explanation/Short Description
- Project 2
  - Short Videos + Explanation/Short Description
  
------------  
### Project 1 - Finite Labryinth (C#, Unity, Final Year Project)

A simple 2D Roguelike Space Shooter (prototype) where the player must clear 3 levels in total, and the goal for each level is to simply destroy the required number of enemy ships. Clearing each incrementing level rewards the player with powerups to tackle stronger enemies. The level environment, enemy ships, and the player are mostly randomised to provide the player a finite amount of new experiences.
  
Development Time: 1-2 Months 

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
<img align="center" width=33% height=33% src="https://user-images.githubusercontent.com/115569550/195622574-8187acd9-dede-41a8-a5b0-51ed9a3a5737.png"> <img align="center" width=33% height=33% src="https://user-images.githubusercontent.com/115569550/195626486-4b6adb39-a3ca-4ed0-b211-f489033e5ef4.png"> <img align="center" width=33% height=33% src="https://user-images.githubusercontent.com/115569550/195626499-0cf8918a-10a5-4679-be53-c9b2a2eb6f26.png">


At every run the player has randomised but balanced ship parts. A weapon, for instance, with lower power such as 100 will have the same total damage done in a given time (10 seconds) as a higher power such as 1500, this is due to the fire rate being slower at higher power and faster at lower power. These balancing concepts have also been applied to all the other ship parts - the higher the primary value is, the lower the secondary value.


------------
### Project 2 - Oswald: The Savior's Freedom (C#, Unity, Indiviudal Coursework Project)

A 2D RPG Platform game where the player must traverse through 3 levels with each level having their own conditions to progress through the next level. Progress through the levels as quick as possible and the game's story may differ.

Development Time: 1 Month

##### **Combat Gameplay (With Audio)** 
https://user-images.githubusercontent.com/115569550/195849173-953b1aeb-9d5f-49df-9bb2-01f17f4f2fde.mp4


The enemies engage with the player once they reach a certain distance and continuously attack until the player dies. The player can dodge attacks by dodging (player slide animation) at the right moment but they must also manage their energy well as you cannot dodge, attack, jump, etc. endlessly.


##### **Unlocking Bow Attack (With Audio)**
https://user-images.githubusercontent.com/115569550/195875471-fd1ee027-f6bb-4379-b413-3df3ce710062.mp4


The player, by default, does not have a Bow, but the player can get it by traversing through a difficult path. Once unlocked, the player can use it (beware of energy usage) to fire a magic arrow that deals massive damage to enemies. Also note that there are dialogues shown when gaining new powerups such as picking up the Bow or other items.


##### **Level Designs (With Audio)**
https://user-images.githubusercontent.com/115569550/195892588-3f8985ea-019b-4899-82fd-ee459490988a.mp4


The game has 3 levels with each level having better design. In particular, level 3 includes unique designs over the previous levels such as rolling boulders, elevators, and missing keys to unlock the door to the boss area. Also note in level 3 and in previous levels, there are 2D tile blocks that may seem to be missing, however they are intended to be missing to reduce taking up space in the total storage required for this application. 

##### **Boss Area, Boss Phases, and Boss Behaviours (With Audio)**
