# PlaneGame

This is a clone of a hobbyist game I loved years ago. I think I found that on the CD that accompanied an issue of the german gaming magazine *GameStar* and it might be named *Pizza Dude*. (No, its not the game one finds when searching for that name nowadays...)

To find out what I have planned take a look at the [game design doc](https://github.com/tanuva/planegame/blob/master/GDD.md). Attribution for external resources can be found there, too.

# Setup
To actually run the game, you'll need an additional big blob of binary data that can be [downloaded separately](https://nightsoul.org/files/planegame_data.zip). This is done so that the binary data doesn't clutter the repository too much.

I use the command `tar czf planegame_data.tar.gz --files-from=data-bundle-list.txt` to build that data package. The list was generated using `find Assets/data` and `find Assets/Plugins` and removing `.DS_Store` files manually afterwards. Yes, this sucks, but I'd have to learn more find foo to fix that. The pain's just not big enough yet. :)
