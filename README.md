# PlaneGame

This is a clone of a hobbyist game I loved years ago. I think I found that on the CD that accompanied an issue of the german gaming magazine *GameStar* and it might be named *Pizza Dude*. (No, its not the game one finds when searching for that name nowadays...)

To find out what I have planned take a look at the [game design doc](https://github.com/tanuva/planegame/blob/master/GDD.md). Attribution for external resources can be found there, too.

Download a current (4 Nov 2015) standalone build for [OS X](https://nightsoul.org/files/planegame.app.zip), [Linux](https://nightsoul.org/files/planegame.tar.gz) and [Windows](https://nightsoul.org/files/planegame.exe.zip) and try it!

## Controls
There is no ingame help yet. You may configure keys through the Unity player configuration at startup (scroll down a little, lotsa XBox controller axes). The most important (and only) keys are these:

- W, S, A, D: roll, pitch (ailerons and elevators)
- Q, E: yaw (rudder) *You'll want to use this together with ailerons for turns.*
- R, F: increase/decrease throttle
- B: wheel brakes
- C: switch cockpit and chase cameras

# Setup
To actually run the game, you'll need an additional big blob of binary data that can be [downloaded separately](https://nightsoul.org/files/planegame_data.zip). This is done so that the binary data doesn't clutter the repository too much.

I use the command `zip planegame_data.tar.gz -@ < data-bundle-list.txt` to build that data package. The list was generated using `find Assets/data` and `find Assets/Plugins` and removing `.DS_Store` files manually afterwards. Yes, this sucks, but I'd have to learn more find foo to fix that. The pain's just not big enough yet. :)

# Screenshots
Everyone loves pictures!
![In flight towards Broken Tower Airport](https://github.com/tanuva/planegame/blob/master/Screenshots/2015-10 inflight-brokentower.png)
![Approaching Boxcar Intl.](https://github.com/tanuva/planegame/blob/master/Screenshots/2015-10 approach-boxcar-intl.png)
![Ready for take off!](https://github.com/tanuva/planegame/blob/master/Screenshots/2015-10 rfto-boxcar-intl.png)

