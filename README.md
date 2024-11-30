# Kirby: Nightmare in Dream Land
11/29/2024 <br>
Sprint 5 <br>
by **Team Kirby Superstars** <br> 
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
between Quit and Continue buttons, to select a highlighted button, use 'enter'.

Quit exits the program and continue restarts the game in the level hub. Level Complete Screen happens when you 
enter the last door in Room 3 of each level.

**Debug Controls:**  

Press F1 to see debug text on screen
Press F2 to see sprite debug boxes
Press F3 to see level debug
Press F4 to see collision debug
Press F5 to see the screen zoomed out which allows you to see enemy respawn/despawn behavior
	Combine this with F3 to see where enemy spawnpoints

IMPORTANT: Level 3 in the level hub is a testing room for debugging purposes and not an actual playable level!

###############################################################################

Backlogged Tasks:

Visual implementations for **additional** particles and attack visual effects before demo day. These don't affect game play and were stretch goals under our sprite polish objective.

###############################################################################

Known bugs:

None!

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one
