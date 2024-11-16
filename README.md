# Kirby: Nightmare in Dream Land
by **Team Kirby Superstars**  
Gwyneth Barnholtz, Mark DeLeo, Vivian Ferrigni, Payton Murphy, Carman Ngai, Lina Ordonez Aguiar

## Contents
* [Controls](#controls)

<a id="controls"></a>
## Controls
### Keyboard
![image](.readme_content/keyboard%20guide.png) 
---
### Controller
![image](.readme_content/controller%20guide.png) 
---
**General control notes:**
- Double-tap **LEFT** and **RIGHT** to dash
- Hold **JUMP** longer to jump higher
- When you have an enemy in your mouth, press **ATTACK** again to spit it out, or **DOWN** to swallow it and take its power, if it has one
- Press **DOWN** to crouch, and **DOWN** + **ATTACK** to slide
- Press **UP** to float
- Press **UP** while in a door to enter it

**Gamepad-specific control notes:**  

Monogame is only directly compatible with XInput controllers, if you want to use DirectInput controllers you will need a way to emulate XInput with DirectInput. For example, to use Nintendo Switch controllers with this game, use [BetterJoy](https://www.betterjoy.org/) to emulate Nintendo Switch controllers as generic XInput. Controllers will be automatically mapped to a player slot, but to change this, use the left and right bumpers to decrement and increment the player slot that you control. There's nothing in the code stopping multiple controllers from controlling one player, so coordinate the controllers to be mapped to the players you want. If you press F1 to enter debug text mode, there will also be a graphic in the top-right corner to visualize left stick input. This can make it easier to tell who's controlling who, since the slots are color-coded to the Kirbys and will turn gray if no controller is mapped to that slot. 

<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>
<br/>

When on Game over and Level Complete Screens 'up' and 'down' arrow keys navigate
between Quit and Continue buttons, to select a highlighted button, use 'space'.

Quit exits the program and continue restarts the game in room 1. Level Complete Screen happens when you 
enter the last door in Level 3.


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
- reset does not reset player state (if kirby enters invulnerable and reset is hit he will still be in invulnerable)

Particles:
- Need to stop run particles from displaying when double press happens when floating

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one
