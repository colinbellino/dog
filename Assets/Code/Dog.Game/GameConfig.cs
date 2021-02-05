using UnityEngine;

namespace Dog.Game
{
	[CreateAssetMenu(menuName = "Dog/Game Config")]
	public class GameConfig : ScriptableObject
	{
		public LayerMask InteractionMask;
		public LayerMask GroundCheckMask;

		public ParticleSystem PetParticles;

		public CharacterComponent PlayerPrefab;
		public CharacterComponent DoggoPrefab;
	}
}
