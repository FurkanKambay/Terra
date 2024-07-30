using Tulip.Data.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Tulip.Data.Tiles
{
    [CreateAssetMenu]
    public sealed class CustomRuleTile : RuleTile<CustomRuleTile.Neighbor>
    {
        public WorldTile WorldTile { get; internal set; }

        public override bool RuleMatch(int neighbor, TileBase tile) => neighbor switch
        {
            Neighbor.Null => tile == null,
            Neighbor.NotNull => tile != null,
            _ => base.RuleMatch(neighbor, tile)
        };

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(location, tilemap, ref tileData);
            tileData.color = WorldTile.Color;

            if (WorldTile.Ore)
                tileData.gameObject = WorldTile.Ore.Prefab;
        }

        // ReSharper disable once ClassNeverInstantiated.Global
        public class Neighbor : RuleTile.TilingRuleOutput.Neighbor
        {
            public const int Null = 3;
            public const int NotNull = 4;
        }
    }
}
