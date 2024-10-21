
Team Kirby Superstars
Sprint2: 9/30/2024 - 10/18/2024

Group Members:  Gwyneth Barnholtz, Mark DeLeo, Vivian Ferrigni, Payton Murphy,
                Carman Ngai, Lina Ordonez Aguiar

Project: Sprint3 - Level Loading/Segmentation and Collision Handling
         Kirby: Nightmare in Dreamland, Level 1 

###############################################################################

Program Controls:

Right-clicking progresses to the next room.

Left-clicking regresses to the previous room.

Left and right arrow keys move Kirby.

Two consecutive left or right arrow presses (either hold then press or press press) 
makes Kirby run.

'X' allows him to jump. Hold for long jump and press for short.

Down arrow allows him to crouch. Down and z alows him to slide.

Up arrow allows him to float. It will also be used for him to travel through doors to move between levels.

The 'A' and 'D' keys change Kirby's facing direction.

    **NOTE** We plan on removing the hardcoded key mappings in the commands so that we can
    later swap them out and use WASD along with the arrows to move Kirby. This is reflected
    on the backlog.

The 'z' key:
    Kirby inhales when standing still and can swallow a nearby enemy
    Kirby slides when crouching and z is pressed
    Kirby attacks 
        Exhales when floating
        Uses beam when in beam
        Uses fire when in fire
        Uses spark when in spark
        Unleashes star attack if Kirby has inhaled an enemy

Number keys (1, 2, 3, 4) can be used to show Kirby use a different power-up modes
(Normal, Beam, Fire, Spark). ***

Use 'q' to quit and 'r' to reset the program back to its initial state.

Use 'f1' to toggle debug mode for graphics.

Use 'f2' to toggle debug mode for sprites.

Use 'f3' to toggle debug mode for level.

Use 'f4' to toggle culling.

Use 'f5' to toggle debug mode for collision.

Use 'F' to toggle fullscreen.

Use '+' to increase the window size.

Use '-' to decrease the window size.

Use ']' to increase the frame rate.

Use '[' to decrease the frame rate. 

###############################################################################

Backlogged Tasks:

Swimming animation: IMPORTANT - We got permission from one of the graders to leave this on the backlog
for the forseeable future without losing points. We are unsure if we are going to implement swimming as
a part of the game design at this point since it is not required for the project and we would need to 
implement water collision behavior. We are currently treaing water tiles as air. We will decide if we will
implement swimming at a later sprint. For now, we are leaving it on the backlog.

Particle implementation: IMPORTANT - We got permission from one of the graders to leave this 
on the backlog for the forseeable future without losing points; it is not a requirement for 
the class and should be implemented near the end of the project if we have time.There is a 
considerable amount of particle animations that supplement the animations for the entities 
and projectiles. The particles are purely visual, with no collision or physics. It is part of 
the animation, however, due to Kirby having overwhelmingly more animations than Zelda and Mario, 
we needed to prioritize getting in the entitiy and projectile animations that were required for 
this sprint. 

Refactoring Commands: We completed our initial goal for refactoring from last sprint. However, the current implementation of the commands includes
hardcoded key mappings. We are planning to refactor this again by the next sprint so that the mappings can be swappable.

Kirby Reabsorption: kirby can't reabsorb a star once it is spewed out, just the way it was coded 
made it so only one "attack" would be active at a time and kirby couldnt start another attack
while another one is active. Refactoring is not an option at this point. This blocks him from
inhaling or letting go of exhale while the star is active. 

Smooth out some collision responses and physics: we plan on making collision smoother including less jittery/like
teleportation and adjusting falling/falling speeds in certain scenarios since we focused our efforts this sprint on 
getting things like slope collision to work. We also plan on adjusting his jump range to be more accurate to the game.

A couple projectile attacks: Kirby's star attack after releasing a swallowed enemy currently does not despawn but this will be added by next sprint.
We are also working on adding two more projectile attacks that are missing from enemy spark and flamethrower attacks. Due to the sheer number of 
Kirby attacks, enemy attacks, and other collision interactions we backlogged these two and will complete them by next sprint.

Implementing life loss/health system: we will be using the fall off screen after falling beneath the waterfall to trigger one of the health mechanics
and reset his position at the start of the level including resetting the level itself.

###############################################################################


Known bugs:

Player Movement: note: most major bugs can be exited out of by pressing jump aka 'x'
 - if you are attacking and move the player, kirby doesnt move position, but the sprite changes, we are aware
 - slide sometimes happens without changing the pose into sliding
 - sometimes movement just softlocks in position or onto float, it is unclear but we are working on finding the source and fixing it
 - pressing z repeatedly and up while kirby is floating sometimes softlocks it into postion
 - Kirby can jump off screen if you jump at the edge of the level, this will be patched soon
    He can also fall off screen past waterfall and cannot be recovered because of our buggy restart so the program must be rerun

 Level Loading:
 - restart is currently completely broken after implementing a level loader and an object manager in this game
    we are pretty sure we are repeating some calls on object lists due to incorrect synchronization that is causing these issues

Collision:
 - there are physics issues sometimes that are caused by multiple responses going off due to multiple intersection tiles 
   please press jump at this time and it should exit out of most of these "stuck" states
    these include: 
        being stuck on a slope and/or walking across air (this combination is often triggered)
        entering into falling animation while on slope

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one