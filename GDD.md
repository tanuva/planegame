# PlaneGame Design Doc
The basic concept is kept very simple so that I at least have a chance to finish this in a finite amount of time. Ideas that won't appear anytime soon are kept in the *Optional* section (obviously). This document also serves as simple task management platform (see TODOs). Of course I could've used Github issues for that, but I'd have to keep this document up to date anyway.

# Environment
## Scenarios
- Exactly one
- Simple hand-crafted island or heavily smoothed out Australia. Try the latter.
	- Not Australia. Too much detail, too much work to make it nice. Its easier to create a very simplistic (terraced height levels) island from scratch.

	Advantage: interesting flights over terrain of varying height. Stylized terrain allows for more *creative* decoration without breaking immersion.
- TODO At least land/sea textures (do a little more painting)
	- Land is mostly painted, sea still a little grassy
- TODO Dome enforcing level borders (magic grid shader?)
- 3 airports, needing prefabs
	- tower: simple radio tower with blinkenlight on top
	- TODO airport building
	- TODO glowing(?) downward arrow
		- Just use the radio tower's blinkenlight instead of a separate arrow. Still needs a *delivery drop-off area* marked somehow.
	- TODO runway
		- Just paint it on the ground, that island doesn't have any paved roads anyway

### Notes
- To export a raw file for use as heightmap from Gimp: Invert image y axis, select ``raw`` file type and ``planar`` format, save as *.raw

# Gameplay
- 1 playable plane
- TODO random deliveries
	- Pick up at one airport, drop off at another
	- Rewards? Money for fuel? Some kind of turbo? - Turbo would be possible to implement in this version. Fuel is *optional*.
- TODO cockpit view? Only if bearable without extra geometry
- TODO plane breaking due to overspeed or unexpected landings
	- (At least particle smoke)
	- Allowing for absurd maneuvers is quite cool. Test a little.

# Optional
- Fuel system (speed dependent consumption)
	- gas station
	- air refueling!
		- Needs way too much airspace. Anti-feature.
	- money for deliveries
	- varying delivery package weight?
- A contrasting heavy metal aircraft
- A scenario with several smaller islands, maybe like a reef
- Ability to get out of the plane and walk on foot (Asset Store: Character Pack)

# Art
All used artwork is either available for free on the Unity Asset Store or I make it by myself. (Limited skills included.) This limits choice quite a bit, but getting gameplay to work is time consuming enough by itself.

If I had someone doing artwork, I'd like to see this in a steampunk scenario. Maybe even a steam machine powered plane (!) with flapping wings, might look a little like a dragon. (I don't care about coal and water supplies there. Maybe people have found a way to burn coal very efficiently.) The airport buildings would be made of wood, copper and iron. (TODO What about availability of bricks?). The rocky basin/valley that exists on the current island might evolve into a coal mine. Water is available aplenty through a desalination plant.

# UI/Controls
- Minimal UI. Delivery destinations and availability signaled by world entities.
	- No typical plane instruments except speedometer
	- TODO color coding for speedometer near stall speed
- Controller input (only if it doesn't block keyboard input)
	- Input in Unity is a mess, but now it works somewhat (on OSX, no idea about Linux/Windows)
	- Keyboard input is possible concurrently
- Keys
	- Analog: Roll/Pitch/Yaw
	- Digital: Thrust (+/- keys), reset plane
		- reset plane: rotates the plane upright and sets it 5 meters or so up from current location

# Attributions
- Cessna model by 3dregenerator. Free for personal and educational use.
- Cash register sound by kiddpark on [freesound.org](http://freesound.org/people/kiddpark/sounds/201159/)
- Uses [Unity-XboxCtrlrInput](https://github.com/JISyed/Unity-XboxCtrlrInput) by JISyed (public domain)
