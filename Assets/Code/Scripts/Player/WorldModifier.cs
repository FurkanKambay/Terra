using System;
using Game.Data;
using Game.Data.Items;
using Game.Gameplay;
using Game.Input;
using UnityEngine;

namespace Game.Player
{
    public class WorldModifier : MonoBehaviour
    {
        [SerializeField] Vector2 hotspotOffset;
        public float range = 5f;
        public bool smartCursor;

        public Vector3Int MouseCell { get; private set; }

        public Vector3Int? FocusedCell
        {
            get => focusedCell;
            private set
            {
                if (focusedCell == value) return;
                focusedCell = value;
                OnChangeCellFocus?.Invoke(focusedCell);
            }
        }

        public event Action<Vector3Int?> OnChangeCellFocus;

        private Inventory inventory;
        private ItemWielder itemWielder;
        private BoxCollider2D playerCollider;
        private Camera mainCamera;

        private Vector3Int? focusedCell;
        private Vector2 rangePath;
        private Vector3 hitPoint;

        private void Awake()
        {
            InputHelper.Actions.Player.ToggleSmartCursor.performed += _ => smartCursor = !smartCursor;

            inventory = GetComponent<Inventory>();
            itemWielder = GetComponent<ItemWielder>();
            playerCollider = GetComponent<BoxCollider2D>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (itemWielder.HotbarItem is not Tool) return;
            AssignCells();
        }

        private void HandleItemSwing(Usable item, ItemSwingDirection _)
        {
            if (item is not Tool tool) return;

            if (!FocusedCell.HasValue) return;
            if (World.Instance.CellIntersects(FocusedCell.Value, playerCollider.bounds)) return;

            WorldTile tile = World.Instance.GetTile(FocusedCell.Value)?.WorldTile;
            if (!tool.IsUsableOnTile(tile)) return;

            inventory.ApplyModification(item.Type switch
            {
                ItemType.Block => World.Instance.PlaceTile(FocusedCell.Value, item as WorldTile),
                ItemType.Wall => InventoryModification.Empty,
                ItemType.Pickaxe => World.Instance.DamageTile(FocusedCell.Value, ((Pickaxe)item).Power),
                _ => InventoryModification.Empty
            });
        }

        private void AssignCells()
        {
            Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(InputHelper.Instance.MouseScreenPoint);
            MouseCell = World.Instance.WorldToCell(mouseWorld);

            Vector2 hotspot = (Vector2)transform.position + hotspotOffset;
            rangePath = Vector2.ClampMagnitude(mouseWorld - hotspot, range);

            if (!smartCursor || inventory.HotbarSelected?.Item is not Pickaxe)
            {
                float distance = Vector3.Distance(hotspot, mouseWorld);
                FocusedCell = distance <= range ? MouseCell : null;
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(
                hotspot, rangePath, range,
                LayerMask.GetMask("World"));

            hitPoint = hit.point - (hit.normal * 0.1f);
            FocusedCell = hit.collider ? World.Instance.WorldToCell(hitPoint) : null;
        }

        private void OnDrawGizmosSelected()
        {
            if (!smartCursor) return;

            Vector2 hotspot = (Vector2)transform.position + hotspotOffset;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(hotspot, hotspot + rangePath);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(hitPoint, .1f);
        }

        private void OnEnable() => itemWielder.OnSwing += HandleItemSwing;
        private void OnDisable() => itemWielder.OnSwing -= HandleItemSwing;
    }
}
