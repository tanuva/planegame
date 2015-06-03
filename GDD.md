# PlaneGame Design Doc

# Environment
## Scenarios
- Exactly one
- Simple hand-crafted island or heavily smoothed out Australia. Try the latter.
	- Not Australia. Too much detail, too much work to make it nice. Its easier to create a very simplistic (terraced height levels) island from scratch.

	Advantage: interesting flights over terrain of varying height. Stylized terrain allows for more *creative* decoration without breaking immersion.
- TODO At least land/sea textures (do a little more painting)
- TODO Dome enforcing level borders (magic grid shader?)
- 3 airports
	- TODO prefab: runway, tower, glowing? downward arrow

### Notes
- To export xcf -> raw: Invert image y axis, select "raw" file type and "planar" format, save as *.raw

# Gameplay
- 1 playable plane
- TODO random deliveries
- Controller input (only if it doesn't block keyboard input)
	- Input in Unity is a mess, but now it works somewhat (on Macs)
	- Keyboard input is possible concurrently
- TODO cockpit view? Only if bearable without geometry
- TODO plane breaking due to overspeed or unexpected landings
	- (At least particle smoke)
	- Well. Allowing for absurd maneuvers is quite cool. Test a little.

# Maybes
- Fuel system (speed dependent consumption)
	- gas station
	- air refueling!
	- money for deliveries
	- varying delivery package weight?
- A contrasting heavy metal aircraft
- Ability to get out of the plane and walk on foot (Asset Store: Character Pack)

# Art

# UI/Controls

# Attributions
- Radar Tower: CC-BY, made by SFC Paul Ray Smith Simulation & Training Technology Center
- Cessna Model:
