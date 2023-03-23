using System.Linq;
using Game.Data.Items;
using Game.Data.Tiles;
using UnityEngine;

namespace Game.Player
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance { get; private set; }

        public IUsable HotbarSelected { get; private set; }

        public Pickaxe ActivePickaxe => hotbar.First(usable => usable is Pickaxe) as Pickaxe;

        private readonly IUsable[] hotbar = new IUsable[9];

        private void OnHotbarSelected(int index) => HotbarSelected = hotbar[index];

        private void Start()
        {
            hotbar[0] = Resources.Load<Pickaxe>("Tools/Pickaxe");
            hotbar[1] = Resources.Load<BlockTile>("Tiles/Stone");
            hotbar[2] = Resources.Load<BlockTile>("Tiles/Dirt");
            hotbar[3] = Resources.Load<BlockTile>("Tiles/Sand");
            hotbar[4] = Resources.Load<BlockTile>("Tiles/Ice");

            HotbarSelected = hotbar[0];
            Input.Instance.HotbarSelected += OnHotbarSelected;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
    }
}
