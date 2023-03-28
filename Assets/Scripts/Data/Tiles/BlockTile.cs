﻿using Game.Data.Items;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Data.Tiles
{
    [CreateAssetMenu(fileName = "Block", menuName = "Items/Block")]
    public class BlockTile : RuleTile<BlockTile.Neighbor>, IUsable
    {
        public float UseTime => .25f;
        public Sprite Icon => m_DefaultSprite;
        public float IconScale => iconScale;

        public Color color = Color.white;
        [SerializeField] private float iconScale = 1f;

        [Header("Data")]
        public int hardness = 50;

        [Header("Sounds")]
        public AudioClip hitSound;
        public AudioClip placeSound;

        public bool CanUseOnBlock(BlockTile block) => block != this;

        public override bool RuleMatch(int neighbor, TileBase tile) => neighbor switch
        {
            TilingRuleOutput.Neighbor.NotThis => tile == null,
            TilingRuleOutput.Neighbor.This => tile != null,
            _ => base.RuleMatch(neighbor, tile)
        };

        public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
        {
            tileData.color = this.color;
            base.GetTileData(location, tilemap, ref tileData);
        }

        public class Neighbor : RuleTile.TilingRuleOutput.Neighbor
        {
            public const int Null = 3;
            public const int NotNull = 4;
        }
    }
}
