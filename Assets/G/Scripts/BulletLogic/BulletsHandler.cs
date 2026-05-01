using System;
using UnityEngine;

namespace G.Scripts.BulletLogic
{
    public class BulletsHandler : MonoBehaviour
    {
        [SerializeField] private Bullet _classicBulletPrefab; // Теперь ссылка на префаб, а не настройки
    
        public Bullet ClassicBulletPrefab => _classicBulletPrefab;
    }
}