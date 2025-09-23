# Map Editor
Map Editor

## Controls
`Left Mouse Button` – paint

`Shift` + `Right Mouse Button` – bucket fill

`Arrow Keys` – scroll the map

`W` / `S` – scroll the tile palette

`+` / `-` – zoom the map

`Q` – save the map

`E` – load the map

Palette on the right – clicking sets selected paint tile

## How to use
- Put tileset image in the root folder next to this script
- Set `TILESET_NAME` parameter with the name of the image
- Set `TILE_SIZE` parameter with the size of the single tile in pixels, e.g. `32`
- Set `MAP_WIDTH` and `MAP_HEIGHT` parameters to the size of the map in number of tiles, e.g. `200`, `200`
- Set `SCREEN_WIDTH` and `SCREEN_HEIGHT` parameters to setup editor application size, e.g. `1280`, `720`
- Set `SAVE_FILENAME` and `LOAD_FILENAME` - those files are also stored in the root folder next to this script
- Start the editor `python map_editor.py` (run it inside the script directory!)
- After finished and saved work, copy the content of the save file into the `Tiles` array in your map.json
