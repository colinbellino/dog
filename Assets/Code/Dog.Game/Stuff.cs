using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Dog.Game
{
	public static class Stuff
	{
		public static void Look(Character character, Camera camera, Vector2 lookInput, float sensitivity, float step)
		{
			var look = lookInput * (sensitivity * step);

			character.RotationX -= look.y;
			character.RotationX = Mathf.Clamp(character.RotationX, -90f, 90f);

			character.HeadTransform.localRotation = Quaternion.Euler(character.RotationX, 0f, 0f);
			character.BodyTransform.Rotate(Vector3.up, look.x);
		}

		public static void Move(Character character, Vector2 moveInput, float speed, float gravityModifier, float step)
        {
            if (character.IsGrounded)
            {
                character.Velocity = Vector3.zero;
            }

            var move = (character.BodyTransform.forward * moveInput.y + character.BodyTransform.right * moveInput.x);
            character.CharacterController.Move(move * (speed * step));

            character.Velocity += Physics.gravity * (gravityModifier * step * step);
            character.CharacterController.Move(character.Velocity);
        }

        public static void UpdateIsGrounded(Character character, LayerMask groundCheckMask)
        {
            character.IsGrounded = Physics.CheckSphere(
                character.GroundCheck.position,
                character.GroundCheckRadius,
        		groundCheckMask
            );
        }

        public static void Follow(Transform follower, Transform target)
        {
	        follower.position = target.position;
	        follower.rotation = target.rotation;
        }

        public static Character GetCharacterPointedAt(Camera camera, float maxDistance, LayerMask interactionMask)
        {
	        var didHit = Physics.Raycast(
		        camera.transform.position, camera.transform.forward,
		        out var hit, maxDistance, interactionMask
	        );
	        if (didHit)
	        {
		        return hit.collider.GetComponentInParent<Character>();
	        }

	        return null;
        }

        public static async UniTask AnimatePet(Transform transform)
        {
	        var originalRotation = transform.rotation;

			await DOTween.Sequence()
		        .Append(transform.DORotateQuaternion(originalRotation * Quaternion.Euler(new Vector3(-20f, 0f, 0f)), 0.2f))
		        .Append(transform.DORotateQuaternion(originalRotation, 0.1f))
		        .SetLoops(3)
	        ;
        }
	}
}
