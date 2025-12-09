import pygame
import json
from collections import deque

# --- Settings ---
TILE_SIZE = 32
MAP_WIDTH, MAP_HEIGHT = 50, 50
SCREEN_WIDTH, SCREEN_HEIGHT = 1280, 720
PALETTE_WIDTH = 100
PALETTE_TILE_SIZE = 32
SCROLL_SPEED = 20
PALETTE_SCROLL_SPEED = 1
TILESET_NAME = "MapTileset.png"
SAVE_FILENAME = "SmallStandard.json"
LOAD_FILENAME = "SmallStandard.json"

# Zoom
zoom_level = 1.0
ZOOM_STEP = 0.25
MIN_ZOOM, MAX_ZOOM = 0.25, 3.0

# --- Init ---
pygame.init()
screen = pygame.display.set_mode((SCREEN_WIDTH + PALETTE_WIDTH, SCREEN_HEIGHT))
pygame.display.set_caption("Tilemap Editor")

# --- Map ---
tiles = [[0 for _ in range(MAP_HEIGHT)] for _ in range(MAP_WIDTH)]
drawing = False
selected_tile = 0
palette_scroll = 0
offset_x, offset_y = 0, 0

# --- Loading tileset image ---
tileset_img = pygame.image.load(TILESET_NAME).convert_alpha()
tileset_width, tileset_height = tileset_img.get_size()
tiles_x = tileset_width // TILE_SIZE
tiles_y = tileset_height // TILE_SIZE

TILES_IMG = []
for y in range(tiles_y):
    for x in range(tiles_x):
        rect = pygame.Rect(x*TILE_SIZE, y*TILE_SIZE, TILE_SIZE, TILE_SIZE)
        TILES_IMG.append(tileset_img.subsurface(rect).copy())

PALETTE_VISIBLE = SCREEN_HEIGHT // PALETTE_TILE_SIZE

clock = pygame.time.Clock()

# --- Functions ---
def draw_map():
    tile_size_zoom = int(TILE_SIZE * zoom_level)
    start_x = offset_x // tile_size_zoom
    start_y = offset_y // tile_size_zoom
    end_x = min(start_x + SCREEN_WIDTH // tile_size_zoom + 1, MAP_WIDTH)
    end_y = min(start_y + SCREEN_HEIGHT // tile_size_zoom + 2, MAP_HEIGHT)
    for x in range(start_x, end_x):
        for y in range(start_y, end_y):
            img = pygame.transform.scale(TILES_IMG[tiles[x][y]], (tile_size_zoom, tile_size_zoom))
            screen.blit(img, (x*tile_size_zoom - offset_x, y*tile_size_zoom - offset_y))
            pygame.draw.rect(screen, (50,50,50), (x*tile_size_zoom - offset_x, y*tile_size_zoom - offset_y, tile_size_zoom, tile_size_zoom), 1)

def draw_palette():
    start = palette_scroll
    end = min(start + PALETTE_VISIBLE, len(TILES_IMG))
    for i in range(start, end):
        img = pygame.transform.scale(TILES_IMG[i], (PALETTE_TILE_SIZE, PALETTE_TILE_SIZE))
        rect = pygame.Rect(SCREEN_WIDTH + 50, (i-start)*PALETTE_TILE_SIZE + 10, PALETTE_TILE_SIZE, PALETTE_TILE_SIZE)
        screen.blit(img, rect.topleft)
        if i == selected_tile:
            pygame.draw.rect(screen, (255,255,0), rect, 3)

def bucket_fill(start_x, start_y, target_tile, replacement_tile):
    if target_tile == replacement_tile:
        return
    queue = deque()
    queue.append((start_x, start_y))
    while queue:
        x, y = queue.popleft()
        if 0 <= x < MAP_WIDTH and 0 <= y < MAP_HEIGHT:
            if tiles[x][y] == target_tile:
                tiles[x][y] = replacement_tile
                for nx, ny in [(x+1,y),(x-1,y),(x,y+1),(x,y-1)]:
                    if 0 <= nx < MAP_WIDTH and 0 <= ny < MAP_HEIGHT:
                        queue.append((nx, ny))

# --- Main loop ---
running = True
while running:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

        elif event.type == pygame.MOUSEBUTTONDOWN:
            mx, my = event.pos
            tile_size_zoom = int(TILE_SIZE * zoom_level)
            if mx < SCREEN_WIDTH:
                tx = (mx + offset_x) // tile_size_zoom
                ty = (my + offset_y) // tile_size_zoom
                if 0 <= tx < MAP_WIDTH and 0 <= ty < MAP_HEIGHT:
                    if event.button == 1: # LMB
                        tiles[tx][ty] = selected_tile
                        drawing = True
                    elif event.button == 3: # RMB
                        mods = pygame.key.get_mods()
                        if mods & pygame.KMOD_SHIFT:
                            bucket_fill(tx, ty, tiles[tx][ty], selected_tile)
            else:
                index = (my - 10)//PALETTE_TILE_SIZE + palette_scroll
                if 0 <= index < len(TILES_IMG):
                    selected_tile = index

        elif event.type == pygame.MOUSEBUTTONUP:
            if event.button == 1:
                drawing = False

        elif event.type == pygame.MOUSEMOTION:
            if drawing:
                mx, my = event.pos
                tile_size_zoom = int(TILE_SIZE * zoom_level)
                if mx < SCREEN_WIDTH:
                    tx = (mx + offset_x) // tile_size_zoom
                    ty = (my + offset_y) // tile_size_zoom
                    if 0 <= tx < MAP_WIDTH and 0 <= ty < MAP_HEIGHT:
                        tiles[tx][ty] = selected_tile
                                       
        elif event.type == pygame.KEYDOWN:
            tile_size_zoom = int(TILE_SIZE * zoom_level)

            center_map_x = (offset_x + SCREEN_WIDTH // 2) / tile_size_zoom
            center_map_y = (offset_y + SCREEN_HEIGHT // 2) / tile_size_zoom

            if event.key in (pygame.K_EQUALS, pygame.K_KP_PLUS):  # zoom in
                new_zoom = min(MAX_ZOOM, zoom_level + ZOOM_STEP)
                if new_zoom != zoom_level:
                    zoom_level = new_zoom
            elif event.key in (pygame.K_MINUS, pygame.K_KP_MINUS):  # zoom out
                new_zoom = max(MIN_ZOOM, zoom_level - ZOOM_STEP)
                if new_zoom != zoom_level:
                    zoom_level = new_zoom

            # after zooming, recalculate the offset to keep the center fixed
            tile_size_zoom = int(TILE_SIZE * zoom_level)
            offset_x = int(center_map_x * tile_size_zoom - SCREEN_WIDTH // 2)
            offset_y = int(center_map_y * tile_size_zoom - SCREEN_HEIGHT // 2)

            # prevent going outside the map boundaries
            offset_x = max(0, min(offset_x, MAP_WIDTH * tile_size_zoom - SCREEN_WIDTH))
            offset_y = max(0, min(offset_y, MAP_HEIGHT * tile_size_zoom - SCREEN_HEIGHT))

    # --- Pressed keys ---
    keys = pygame.key.get_pressed()
    tile_size_zoom = int(TILE_SIZE * zoom_level)

    # Map scroll
    if keys[pygame.K_UP]:
        offset_y = max(0, offset_y - SCROLL_SPEED)
    if keys[pygame.K_DOWN]:
        offset_y = min(MAP_HEIGHT*tile_size_zoom - SCREEN_HEIGHT, offset_y + SCROLL_SPEED)
    if keys[pygame.K_LEFT]:
        offset_x = max(0, offset_x - SCROLL_SPEED)
    if keys[pygame.K_RIGHT]:
        offset_x = min(MAP_WIDTH*tile_size_zoom - SCREEN_WIDTH, offset_x + SCROLL_SPEED)

    # Palette scroll
    if keys[pygame.K_w]:
        palette_scroll = max(0, palette_scroll - PALETTE_SCROLL_SPEED)
    if keys[pygame.K_s]:
        palette_scroll = min(len(TILES_IMG) - PALETTE_VISIBLE, palette_scroll + PALETTE_SCROLL_SPEED)

    # Save/Load map
    if keys[pygame.K_q]:
        with open(SAVE_FILENAME, "w") as f:
            json.dump([[tiles[x][y] for x in range(MAP_WIDTH)] for y in range(MAP_HEIGHT)], f)
    if keys[pygame.K_e]:
        with open(LOAD_FILENAME, "r") as f:
            loaded = json.load(f)
            for y in range(MAP_HEIGHT):
                for x in range(MAP_WIDTH):
                    tiles[x][y] = loaded[y][x]

    screen.fill((0,0,0))
    draw_map()
    draw_palette()
    pygame.display.flip()
    clock.tick(60)

pygame.quit()
