using System;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private AnimationManager _animation;
        [SerializeField] private Character4D _character4D;

        private PlayerMove _playerMove;

        private void Awake()
        {
            _playerMove = new PlayerMove(_animation, _rb, _character4D);
        }

        private void Update()
        {
            _playerMove.Tick();
        }

        private void FixedUpdate()
        {
            _playerMove.FixedTick();
        }
    }
}