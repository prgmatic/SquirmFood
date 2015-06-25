using UnityEngine;

    class WormTile : GameTile
    {
        public float FrameRate = 15f;
        //public float MoveSpeed = .2f;

        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            _animator.speed = FrameRate / 15f;

        }
    }

