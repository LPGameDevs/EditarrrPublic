# Level Editing.

The level editor is a simple tool that allows you to create levels for the game.

## How to use it?

There should be two distinct scenes in the project, one for editing levels and
one for playing the levels. Both scenes require the following scripts:

- `TilePainter.cs`

### Tile Painter

This script is where the magic happens. You can specify the different layers of
a tilemap for things like ground/damage/etc and specify the editor tiles that 
can be selected.

In the editor scene the painter will either load the save file for an existing 
level or a "default" template if no save file is found, and place all the tiles
on a tilemap before letting the player add/remove tiles as required.

In the game scene the painter will load the save file for the level and place
all the tiles/prefabs in the correct position before allowing the player to
be spawned and the game to start.

### Editor tiles

These are scriptable objects that are used to define the different things that 
can be placed in a level. Some are just 1x1 tiles that are used to draw ground,
walls, spikes, items, etc. Others are prefabs that will be spawned into the
level and have more complex logic.

All editor tiles can be found in `Assets/_Project/ScriptableObjects/EditorTiles`.

### Storage files

There are features for syncing levels with different servers, however the game 
stores all of its levels locally in json files. These files are named after the
level id contain all the required information (tile placement, status, creator,
etc). Sharing these files is how levels are shared between players.
