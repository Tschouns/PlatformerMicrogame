using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core;
using Assets.Scripts.Data.State;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A simple controller for enemies. Provides movement control over a patrol path.
    /// </summary>
    [RequireComponent(typeof(AnimationController), typeof(Collider2D))]
    public class EnemyController : MonoBehaviour, IHasState<EnemyState>
    {
        public PatrolPath path;
        public AudioClip ouch;

        internal PatrolPath.Mover mover;
        internal AnimationController control;
        internal Collider2D _collider;
        internal AudioSource _audio;
        SpriteRenderer spriteRenderer;

        public Bounds Bounds => _collider.bounds;
        public bool IsAlive { get; set; } = true;


        void Awake()
        {
            control = GetComponent<AnimationController>();
            _collider = GetComponent<Collider2D>();
            _audio = GetComponent<AudioSource>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                var ev = Schedule<PlayerEnemyCollision>();
                ev.player = player;
                ev.enemy = this;
            }
        }

        void Update()
        {
            if (this.IsAlive &&
                path != null)
            {
                if (mover == null) mover = path.CreateMover(control.maxSpeed * 0.5f);
                control.move.x = Mathf.Clamp(mover.Position.x - transform.position.x, -1, 1);
            }
        }

        #region StateHandling

        public EnemyState GetCurrentState()
        {
            return new EnemyState
            {
                Position = VectorData.FromUnityVector(this.transform.position),
                Velocity = VectorData.FromUnityVector(this.control.Body.velocity),
                Move = VectorData.FromUnityVector(this.control.move),
                IsAlive = this.IsAlive,
                ControlEnabled = this.control.enabled,
                ColliderEnabled = this._collider.enabled,
            };
        }

        public void ApplyState(EnemyState state)
        {
            Argument.AssertNotNull(state, nameof(state));

            this.transform.position = state.Position.ToUnityVector3();
            this.control.velocity = state.Velocity.ToUnityVector2();
            this.control.move = state.Move.ToUnityVector2();
            this.IsAlive = state.IsAlive;
            this.control.enabled = state.ControlEnabled;
            this._collider.enabled = state.ColliderEnabled;
        }

        #endregion
    }
}