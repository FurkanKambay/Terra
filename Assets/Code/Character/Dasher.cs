using SaintsField;
using Tulip.Data;
using UnityEngine;

namespace Tulip.Character
{
    public class Dasher : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Required] Rigidbody2D body;
        [SerializeField, Required] SaintsInterface<Component, IDasherBrain> brain;

        [Header("Config")]
        public float dashSpeed = 10f;
        public float dashCooldown = 0.5f;
        [SerializeField] ForceMode2D forceMode;

        private float timeSinceLastDash;

        private void Update()
        {
            timeSinceLastDash += Time.deltaTime;
            if (!brain.I.WantsToDash || timeSinceLastDash < dashCooldown) return;

            timeSinceLastDash = 0f;
            float direction = brain.I.HorizontalMovement;

            if (Mathf.Abs(direction) > 0.1f)
                body.AddForce(Vector2.right * (direction * dashSpeed), forceMode);
        }
    }
}