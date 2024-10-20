
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

Down arrow allows him to crouch.

Up arrow allows him to float.

The 'A' and 'D' keys change Kirby's facing direction.

    **NOTE** We plan on removing the hardcoded key mappings in the commands so that we can
    later swap them out and use WASD along with the arrows to move Kirby. This is reflected
    on the backlog.

The 'z' key:
    Kirby inhales when standing still
    Kirby slides when crouching and z is pressed
    Kirby attacks (exhales) when floating

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

Refactoring Commands: The current implementation of the commands includes hardcoded key mappings.
We are planning to refactor this by the next sprint so that the mappings can be swappable.

Kirby Reabsorption: kirby can't reabsorb a star once it is spewed out, just the way it was coded 
made it so only one "attack" would be active at a time and kirby couldnt start another attack
while another one is active. Refactoring is not an option at this point. This bocks him from
inhaling or letting go of exhale wile the star is active. 

###############################################################################


Known bugs:
 - if you are attacking and move the player, kirby doesnt move position, but the sprite changes, we are aware.
 - slide sometimes happens without changing the pose into sliding
 - sometimes movement just softlocks in position or onto float, it is unclear why 
 - pressing z  repeadetly and up while kirby is floating sometimes softlocks it into postion
 - restart is buggy on collision boxes
TODO: Add bugs


###############################################################################

Other Design Tools:

TODO: Add to or delete section

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one