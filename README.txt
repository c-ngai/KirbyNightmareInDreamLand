
Team Kirby Superstars
Sprint2: 9/9/2024 - 9/28/2024

Group Members:  Gwyneth Barnholtz, Mark DeLeo, Vivian Ferrigni, Payton Murphy,
                Carman Ngai, Lina Ordonez Aguiar

Project: Sprint2 - implementing Game Objects and Sprites of
         Kirby: Nightmare in Dreamland, Level 1 

###############################################################################

Program Controls:

Left and right arrows move Kirby.

Two consecutive left or right arrow presses (either hold then press or press
press) makes Kirby run.

'x' allows him to jump. Hold for long jump and press for short.

Down arrow allows him to crouch.

The 'a' and 'd' keys change Kirby's facing direction. We did not implement 'w'
and 's' keys bc Kirby never faces up or down.

The 'z' key:
    Kirby inhales when standing still
    Kirby slides when crouching and z is pressed
    Kirby attacks (exhales) when floating

Number keys (1, 2, 3, 4) can be used to show Kirby use a different power-up modes
(Normal, Beam, Fire, Spark). ***

Use 'e' to cause Kirby to take damage and get the invulnerable animation.

Use keys 't' and 'y' to cycle forward and backward between which block is currently being shown.

Use keys 'u' to hide item and 'i' to show item.

Use keys 'o' and 'p' to cycle between which enemy or npc is currently being shown.

Use 'q' to quit and 'r' to reset the program back to its initial state.

*** NOTE: Backlogged: Due to time constraints and the overwhelming amount
of animations for each powerup, animations for Beam, Fire, and Spark Kirby (other
than right/left facing Kirby and using attack ('z' during beam and fire Kirbys) have been 
backlogged with permission from Professor Kirby. Note the "Invalid Sprite" icon shows up
because there is an animation of these sprites that has not been implemented for reasons
mentioned before. To be done by Sprint3. 

###############################################################################

Backlogged Tasks:

Kirby movement animation sprites for power-up modes and swimming animation: As mentioned
above, there are significantly more animations in Kirby than there are in the other two
games (at least hundreds of more frames), so getting all of the animations for every single 
Kirby copy ability was unrealistic in the timeframe of this Sprint. After talking to Professor
Kirby, we had permission to only implement as many animations as the other games would have,
however we ended up doing much more than that. Some of the copy ability animations and the
swimming animations needed to be backlogged for this reason.
    note: if you press 2/3 and z the attacks for Beam and Fire kirby show up since
    these are projectiles (kirby itself appears as invalid since they were backlogged). 
    Spark kirby's attack is not a projectile so it is not 
    implemented in this sprint. Normal kirby can inhale, however, since object
    interaction is not being implemented his throwing up an enemy is not implemented.
    The star itself however, is done in this sprint.

Particle implementation: There is a considerable amount of particle animations that 
supplement the animations for the entities and projectiles. The particles are purely
visual, with no collision or physocs. It is part of the animation, however, due to 
Kirby having overwhelmingly more animations than Zelda and Mario, we needed to 
prioritize getting in the entitiy and projectile animations that were required for 
this sprint.

Refactoring current enemy implementation into a enemy state pattern to remove switch switches.
We were having trouble implementing an enemy state pattern so we plan on worrying about this
after Sprint2 since the current implementation does work fine.

Refactoring keyboard controller and commands. the controller and commands were implemented in 
a unconventional way and Professor Kirby is concerned of the consequences of having "Undo"
in commands but it is currently working correctly. To avoid future issues, this will be 
refactored after Sprint2. 

###############################################################################


Known bugs:

In our Sprint2, there are currently bugs regarding Kirby's multi-key/combo movement
commands. Given a lot of the code around these commands depend on our future
Physics and keyboard implementation, we will leave this for next Sprint. 
    - For example the combo of crouch + z (slide) has a small frame interference
      and does not stop after a designated time when pressed down. 
    - When letting go of float on the ground, Kirby does not attack/exhale.
    - Pressing x while floating does not make Kirby float up.


###############################################################################

Other Design Tools:

We used the .NET code analyzer in the last week before Sprint2 deadline and corrected
over 500 warnings. Many of these warning pertained to Style; things like having extra
spaces between methods, lack of adherense to naming conventions, or redundant casts.

There were fewer warnings about Code Quality. These warnings included having non-
read-only public fields, if-statement can be simplified, including paraenthesis
for clarity.

We went through our files one by one and corrected each style/quality warning.
The only warning that we suppressed were making fields read-only. This will be
refactored later to have only private fields. 

###############################################################################

Documentation/Planning Tools:

Google Drive of Meeting Notes and other Documents: https://drive.google.com/drive/folders/1nM9rI0OnBROJPyXMQbhXYzAOB0yF1QNA?usp=sharing 
Current Trello of Task Management: https://trello.com/invite/b/66f251d9de3625e8267d2b35/ATTI4fe46a09f5ff46b9f0fbfb8de88d8bff86EA3A3F/kirby-nightmare-in-dreamland-pls-use-this-one
Old Trello of Task Management: https://trello.com/b/J3QRsOik/do-not-use-outdated-kirby-nightmare-in-dreamland
    Note: This was used before we realized we were on a free trial and had to migrate to the 
    free version. However, the time documentation of edits are still there. And because the free trial ended
    we can no longer add more pepole into it to view it. 