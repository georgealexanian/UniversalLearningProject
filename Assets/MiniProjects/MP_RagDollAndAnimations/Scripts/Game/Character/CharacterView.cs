namespace MiniProjects.MP_RagDollAndAnimations.Scripts.Game.Character
{
    using Model;
    using UnityEngine;

    public class CharacterView : MonoBehaviour
    {
        [field: SerializeField] public ZombieRagDollView ZombieRagDollView { get; private set; }
    }
}
