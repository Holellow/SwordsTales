using UnityEngine;

namespace Enemy
{
    public class Enemy : Unit
    {
        [SerializeField] private float _maxhealth;
        [SerializeField] private float _currentHealth;

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = value;
        }
        private float Maxhealth
        {
            get => _maxhealth;
            set => _maxhealth = value;
        }
    
        void Start()
        {
            CurrentHealth = Maxhealth;
        }

        protected override void Die()
        {
            Debug.Log("Died");
            Destroy(gameObject);
        }

        public override void ReceiveDamage(int damage)
        {
            CurrentHealth -=damage;
            if(CurrentHealth <= 0)
            {
                Die();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
