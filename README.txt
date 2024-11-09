
Team Kirby Superstars
Sprint4: 10/21/2024 - 11/09/2024

Group Members:  Gwyneth Barnholtz, Mark DeLeo, Vivian Ferrigni, Payton Murphy,
                Carman Ngai, Lina Ordonez Aguiar

Project: Sprint4 - Finished framework and first level

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

8, 9, and 0 keys can be used to show the different powerup cards (beam, spark, fire) on the HUD.
Note that this will later be changed to show the cards when Kirby gets a powerup in Sprint5.

When on Game over and Level Complete Screens 'up' and 'down' arrow keys navigate
between Quit and Continue buttons, to select a highlighted button, use 'space'.
Quit exits the program and continue restarts the game in room 1. 

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



###############################################################################


Known bugs:

Player Movement: note: most major bugs can be exited out of by pressing jump aka 'x'
 - if you are attacking and move the player, kirby doesnt move position, but the sprite changes, we are aware
    - this bug will now sometimes cause it to clip into the floor and go the end of room
 - sometimes kirby clips into floor and speed runs the level, we are aware
 - slide sometimes happens without changing the pose into sliding
 - sometimes movement just softlocks in position or onto float, it is unclear but we are working on finding the source and fixing it
 - pressing z repeatedly and up while kirby is floating sometimes softlocks it into postion

 Level Loading:
 - restart is currently completely broken after implementing a level loader and an object manager in this game
    we are pretty sure we are repeating some calls on object lists due to incorrect synchronization that is causing these issues

Collision:
 - there are physics issues sometimes that are caused by multiple responses going off due to multiple intersection tiles 
   please press jump at this time and it should exit out of most of these "stuck" states
    these include: 
        being stuck on a slope and/or walking across air (this combination is often triggered)
        entering into falling animation while on slope
 - some of the enemies are having bugs with colliding into tiles because of the multiple response issue

Mouse Controller and Game State:
- because using the mouse controller to go through rooms the game state is not changed and so controls get stuck in the
wrong keymap and the quit continue buttons are draw on regular levels. Will fix next sprint. 

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one