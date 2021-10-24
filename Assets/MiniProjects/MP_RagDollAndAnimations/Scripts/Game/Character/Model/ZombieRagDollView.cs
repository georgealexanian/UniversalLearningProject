namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character.Model
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ZombieRagDollView : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> ragDollRigidBodies;
        [SerializeField] private List<Collider> ragDollColliders;
    }
}
