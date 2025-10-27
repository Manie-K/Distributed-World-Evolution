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

## Adding a new map to the client
The structure of a json map is as follows:

`Name` - name of the map

`TexturePath` - path to the tileset texture, with the root being the `Content` folder

`TileSize` - size in pixels of a single tile from the tileset texture

`TilesetTextureWidth` - total width in pixels of the tileset texture

`TilesetTextureHeight` - total height in pixels of the tileset texture

`MapWidth` - desired total map width, in number of tiles

`MapHeight` - desired total map height, in number of tiles

`TilesetData` - data about each tile in the tileset texture; contains `id` of a tile (starting from 0, at the top-left of the texture), `Type` from the TileProperty.cs enum (you can add new ones if needed), and `Walkable` which is set to true if entities can walk on the given tile

`Tiles` - 2D array of tile ids representing playable map

When the map json is ready:
- Place it inside `SharedLibrary/WorldMap/Maps`, set the file parameter `CopyToOutputDirectory` to `PreserveNewest`
- Add a file link reference inside `Client/Content/Maps` to your json file, set the file parameter `CopyToOutputDirectory` to `PreserveNewest`
- Add the tileset texture to `Client/Content/Maps/Tiles` and include the new image using `Content.mgcb`
- In the client, add a new option in the `InitializeRows` function in `SwitchPageMapSelection.cs`
- In the shared library, add a new case in the `GetMapFileName` function in `Tilemap.cs`, the string must match the json filename
