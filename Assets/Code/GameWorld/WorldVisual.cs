using System;
using System.Collections.Generic;
using System.Linq;
using Furkan.Common;
using Tulip.Data;
using Tulip.Data.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tulip.GameWorld
{
    public class WorldVisual : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] World world;
        [SerializeField] Tilemap wallTilemap;
        [SerializeField] Tilemap blockTilemap;
        [SerializeField] Tilemap curtainTilemap;

        private void Start() => World_Refresh(world.WorldData);

        private void OnEnable()
        {
            if (!world)
                return;

            world.OnRefresh += World_Refresh;
            world.OnPlaceTile += World_PlaceTile;
            world.OnDestroyTile += World_DestroyTile;
        }

        private void OnDisable()
        {
            if (!world)
                return;

            world.OnRefresh -= World_Refresh;
            world.OnPlaceTile -= World_PlaceTile;
            world.OnDestroyTile -= World_DestroyTile;
        }

        public Vector2Int WorldToCell(Vector3 worldPosition) =>
            (Vector2Int)blockTilemap.WorldToCell(worldPosition);

        public Vector3 GetCellCenterWorld(Vector2Int cell) =>
            blockTilemap.GetCellCenterWorld((Vector3Int)cell);

        public Bounds CellBoundsWorld(Vector2Int cell) =>
            new(GetCellCenterWorld(cell), blockTilemap.GetBoundsLocal((Vector3Int)cell).size);

        private void World_Refresh(WorldData worldData)
        {
            if (worldData == null)
                return;

            resetTilemap(wallTilemap);
            resetTilemap(blockTilemap);
            resetTilemap(curtainTilemap);

            // TODO: Improve performance here
            wallTilemap.SetTiles(worldData.Walls.Select(selector).ToArray(), ignoreLockFlags: true);
            blockTilemap.SetTiles(worldData.Blocks.Select(selector).ToArray(), ignoreLockFlags: true);
            curtainTilemap.SetTiles(worldData.Curtains.Select(selector).ToArray(), ignoreLockFlags: true);
            return;

            void resetTilemap(Tilemap tilemap)
            {
                tilemap.ClearAllTiles();
                tilemap.size = worldData.Dimensions.WithZ(1);
                tilemap.transform.position = new Vector3(-worldData.Dimensions.x / 2f, -worldData.Dimensions.y, 0);
            }

            TileChangeData selector(KeyValuePair<Vector2Int, PlaceableData> kvp)
            {
                (Vector2Int cell, PlaceableData placeableData) = kvp;

                return new TileChangeData(
                    (Vector3Int)cell,
                    (bool)placeableData ? placeableData.RuleTileData : null,
                    (bool)placeableData ? placeableData.Color : Color.white,
                    Matrix4x4.identity
                );
            }
        }

        private void World_PlaceTile(TileModification modification)
        {
            var cell = (Vector3Int)modification.Cell;
            PlaceableData placeableData = modification.PlaceableData;

            Tilemap tilemap = GetTilemap(placeableData.TileType);
            tilemap.SetTile(cell, placeableData.RuleTileData);

            if (!placeableData)
            {
                tilemap.SetTile(cell, null);
                return;
            }

            tilemap.SetTile(cell, placeableData.RuleTileData);
            tilemap.SetColor(cell, placeableData.Color);
        }

        private void World_DestroyTile(TileModification modification)
        {
            Tilemap tilemap = GetTilemap(modification.PlaceableData.TileType);
            tilemap.SetTile((Vector3Int)modification.Cell, null);
        }

        private Tilemap GetTilemap(TileType tileType) => tileType switch
        {
            TileType.Wall => wallTilemap,
            TileType.Block => blockTilemap,
            TileType.Curtain => curtainTilemap,
            _ => throw new ArgumentOutOfRangeException(nameof(tileType))
        };
    }
}
