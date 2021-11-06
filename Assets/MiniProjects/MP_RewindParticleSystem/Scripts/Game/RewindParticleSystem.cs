namespace MiniProjects.MP_RewindParticleSystem.Scripts.Game
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(ParticleSystem))]
    [ExecuteInEditMode]
    public class RewindParticleSystem : MonoBehaviour
    {
        [Range(0,3f)] public float Timeline;
        [SerializeField] private ParticleSystem Ps;


        private void Awake()
        {
            Ps.randomSeed = 5;
        }

        void OnValidate()
        {
            Ps.Simulate(Timeline, true, true);
            Ps.Pause(true);
        }
    }
}
