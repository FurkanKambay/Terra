using Tulip.Data;
using Tulip.Data.Gameplay;
using UnityEngine;

namespace Tulip.Character
{
    public class TangibleEntity : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody2D body;
        [SerializeField] Health health;

        private void OnEnable()
        {
            health.OnDie += HandleDied;
            health.OnRevive += HandleRevived;
        }

        private void OnDisable()
        {
            health.OnDie -= HandleDied;
            health.OnRevive -= HandleRevived;
        }

        private void HandleDied(DamageEventArgs death) => body.simulated = false;
        private void HandleRevived(IHealth source) => body.simulated = true;
    }
}