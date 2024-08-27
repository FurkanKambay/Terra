using Tulip.Data.Items;
using UnityEngine;

namespace Tulip.Data
{
    public interface IWorld
    {
        public delegate void PlaceableEvent(TileModification modification);

        event IWorldProvider.ProvideWorldEvent OnRefresh;
        event PlaceableEvent OnPlaceTile;
        event PlaceableEvent OnHitTile;
        event PlaceableEvent OnDestroyTile;

        /// <summary>
        /// Damages a tile of the given type at the given cell coordinates.
        /// </summary>
        /// <returns>Whether the tile was destroyed.</returns>
        InventoryModification DamageTile(Vector2Int cell, TileType tileType, int damage);

        /// <summary>
        /// Tries to place a tile at the given cell coordinates.
        /// </summary>
        /// <returns>Whether the tile was placed successfully.</returns>
        InventoryModification PlaceTile(Vector2Int cell, PlaceableData placeableData);


        bool HasWall(Vector2Int cell);
        bool HasBlock(Vector2Int cell);
        bool HasCurtain(Vector2Int cell);

        PlaceableData GetWall(Vector2Int cell);
        PlaceableData GetBlock(Vector2Int cell);
        PlaceableData GetCurtain(Vector2Int cell);

        PlaceableData GetWallAtWorld(Vector3 worldPosition);
        PlaceableData GetBlockAtWorld(Vector3 worldPosition);
        PlaceableData GetCurtainAtWorld(Vector3 worldPosition);

        Vector3 CellCenter(Vector2Int cell);
        Vector2Int WorldToCell(Vector3 worldPosition);

        Bounds CellBoundsWorld(Vector2Int cell);
        bool DoesCellIntersect(Vector2Int cell, Bounds other);

        /// <param name="baseCell">The bottom-left cell, NOT center or pivot</param>
        /// <param name="entitySize"></param>
        bool CanAccommodate(Vector2Int baseCell, Vector2Int entitySize);
    }
}
