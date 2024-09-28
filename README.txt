
Team Kirby Superstars - Sprint2 - 9/28/2024

Group Members:  Gwyneth Barnholtz, Mark DeLeo, Vivian Ferrigni, Payton Murphy,
                Carman Ngai, Lina Ordonez Aguiar

Project: Sprint2 - implementing Game Objects and Sprites of
         Kirby: Nightmare in Dreamland, Level 1 

###############################################################################

Program Controls:

Left and right arrows move Kirby.

Two consecutive left or right arrow presses (either hold then press or press
press) makes Kirby run.

'x' and down arrow allow him to jump and crouch respectively.

The 'a' and 'd' keys change Kirby's facing direction. We did not implement 'w'
and 's' keys bc Kirby never faces up or down.

The 'z' key:
    Kirby inhales when standing still
    Kirby holds his breath when moving left or right
    Kirby slides when crouching and z is pressed
    Kirby attacks (exhales) when floating

Number keys (1, 2, 3, 4) can be used to show Kirby use a different power-up modes
(Normal, Beam, Fire, Spark). ***

Use 'e' to cause Kirby to take damage and get "thrown" until other movement is
executed or he hits the edge of the screen.

Use keys 't' and 'y' to cycle between which block is currently being shown.

Use keys 'u' to hide item and 'i' to show item.

Use keys 'o' and 'p' to cycle between which enemy or npc is currently being shown.

Use 'q' to quit and 'r' to reset the program back to its initial state.

*** NOTE: Due to time constraints and the overwhelming amount
of animations for each powerup, animations for Beam, Fire, and Spark Kirby (other
than right/left facing Kirby) have been backlogged with permission from Professor
Kirby. To be done by Sprint3. 

###############################################################################

Backlogged Tasks:

Kirby movement animation sprites for power-up modes.
    note: if you press 2/3 and z the attacks for Beam and Fire kirby show up since
    these are projectiles (kiry itself appears as invalid since they were backlogged). 
    Spark kirby's attack is not a projectile so it is not 
    implemented in this sprint. Normal kirby can inhale, however, since object
    interaction is not being implemented his throwing up an enemy is not implemented.
    The star it self however, is done in this sprint.

Particle implementation. 

Item class, Kirby: Nightmare in Dreamland only has one item, maxim tomato, so we
chose to keep the drawing and loading code for this sprite just in our Game1.cs
file. This will move to a more appropriate place next Sprint. 

###############################################################################


Known bugs:

In our Sprint2, there are currently bugs regarding Kirby's multi-key movement
commands. Given a lot of the code around these commands depend on our future
Physics and keyboard implementation, we will leave this for next Sprint. 
    - For example the combo of crouch + z (slide) has a small frame interference
      we hope to work this out with the control refactor as well.

When letting go of float on the ground, 


###############################################################################

Other Design Tools:

We used the .NET code analyzer in the last week before Sprint2 deadline and corrected
over 500 warnings. Many of these warning pertained to Style; things like having extra
spaces between methods, lack of adherense to naming conventions, or redundant casts.

There were fewer warnings about Code Quality. These warnings included having non-
read-only public fields, if-statement can be simplified, including paraenthesis
for clarity.

We went through our files one by one and corrected each style/quality warning.
The only warning that we suppressed were making feilds read-only. This will be
refactored later to have only private fields. 
