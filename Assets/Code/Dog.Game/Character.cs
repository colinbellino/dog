using UnityEngine;

namespace Dog.Game
{
	[SelectionBase]
	public class Character : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] public Transform HeadTransform;
		[SerializeField] public Transform GroundCheck;
		[SerializeField] public float GroundCheckRadius = 0.4f;

		public string Name => name;
		public Transform BodyTransform => transform;
		public CharacterController CharacterController => _characterController;
		public float RotationX { get; set; }
		public Vector3 Velocity { get; set; }
		public bool IsGrounded { get; set; }
	}
}
