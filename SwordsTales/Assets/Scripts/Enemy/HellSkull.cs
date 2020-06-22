using UnityEngine;

namespace Enemy
{
    public class HellSkull : Enemy
    {
        public override void ReceiveDamage(int damage)
        {
            CurrentHealth -=damage;
            if(CurrentHealth <= 0)
            {
                Die();
            }
        }
    }
}
