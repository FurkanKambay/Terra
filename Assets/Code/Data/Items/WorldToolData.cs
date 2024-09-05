using UnityEngine;

namespace Tulip.Data.Items
{
    /// <summary>
    /// A world tool such as a pickaxe.
    /// </summary>
    [CreateAssetMenu(menuName = "Items/World Tool", order = 4)]
    public class WorldToolData : BaseWorldToolData
    {
        public int Power => power;
        public TileType TileType => tileType;

        [Header("World Tool Data")]
        [SerializeField, Min(0)] protected int power = 50;
        [SerializeField] protected TileType tileType = TileType.Block;

        public override ToolUsability GetUsability(IWorld world, Vector2Int cell) =>
            !world.IsCellEntityFree(cell) ? ToolUsability.NotNow
            : world.HasBlock(cell) ? ToolUsability.Available
            : ToolUsability.NoEffect;

        public override InventoryModification UseOn(IWorld world, Vector2Int cell) =>
            world.DamageTile(cell, tileType, power);
    }
}
