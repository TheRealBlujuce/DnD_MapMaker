DND Tool by Will Marda

// How to Use:

1. For tiles, and icons, you will need to make them or find them and import them into the tools under the correct folders:
   For Icons, they should go inside Resources/Sprites/Icons
   For Tiles, they should go inside Resources/Sprites/RawSprites

  When making new tilesets and adding tiles into tile palettes, you would save new tiles into Resources/Sprites/Tileset

  Icons are loaded automatically and assigned automatically via icon id's. Just make sure that the name and the icon (the actual sprite itself) is named the same. 
  Then, inside the Ghost Token prefab, you will add the token icon to the icon property in the ghost token placer script. This will then allow a token to be spawned with the correct icon.

  When making a new map, you need to add it to the Build Profiles -> Sceen List

// Keybinds

Left Shift + Left Click to pan
Middle mouse (or scrolling up and down on a trackpad) to zoom
Shift + L to start/stop leave particle effects from spawning
Shift + Left + Right arrow keys to change wind direction
Shift 1-5 to change world layers
1-2 number keys for changing from line tool to circle tool (and soom #3 for cone tool)
Shift + left click to multi-select tokens and then Shift + Right click and drag to move selected tokens

