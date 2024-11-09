
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

The 'A' and 'D' keys change Kirby's facing direction NOT movement.

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
Quit exits the program and continue restarts the game in room 1. Level Complete Screen happens when you 
enter the last door in Level 3.

Use 'q' to quit and 'r' to reset the current level back to its initial state (does not reset player health).

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

Use 'm' to mute sound.

###############################################################################

Backlogged Tasks:

Improve enemy spawning: make it so that enemies only load when nearby screen camera and will respawn when player goes 
backward through the level. We backlogged this task to complete in Sprint5 because it is of the features we wish to 
implement of having better enemy AI. 

###############################################################################


Known bugs:

Player Movement:
 - if you are attacking and move the player, it lets you do movement in place instead of staying on the attack animations for fire and spark kirbys
 - slide sometimes doesn't work when you change kirby's direction after sliding and Kirby stays in crouch but will be fine again once you exit crouch
 - movements sometimes have soft locks, mostly fixed but some movement has issues when multiple commands are being executed

Enemy:
 - PoppyBrosJr hopping sprites has a glitch
 - Flying enemy also struggles with wall collision because it is changing it's direction but not fixing the pathing
 - Looping enemy sounds don't stop when moving between levels
 - Hothead flamethrower needs to be separated out as a projectile attack (needs to be done to implement it as a collidable)

Mouse Controller and Game State:
- because using the mouse controller to go through rooms the game state is not changed, controls get stuck in the
  wrong keymap and the quit continue buttons are drawn on regular levels when you cycle to different levels after reaching the ending
  "Game Over"/"Level Complete" screens. We will fix next sprint
- if you transition between doors while moving, kirby keeps moving while transitioning
- when you lose your final health and die, it does not enter game end sequence and you instead lose access to Kirby and have full health again
  * we have the game over sequence coded in so we think it is an issue with state transition that we will fix next sprint

Particles:
- Need to stop run particles from displaying when double press happens when floating

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one