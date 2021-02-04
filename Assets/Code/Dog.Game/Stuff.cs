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

            character.Velocity += Physics.gravity * (step * step * gravityModifier);
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

        public static Character GetCharacterPointedAt(Camera camera, LayerMask interactionMask)
        {
	        var didHit = Physics.Raycast(
		        camera.transform.position, camera.transform.forward,
		        out var hit, maxDistance: 100f, interactionMask
	        );
	        if (didHit)
	        {
		        return hit.collider.GetComponentInParent<Character>();
	        }

	        return null;
        }

        public static async UniTask AnimatePet(Transform transform)
        {
	        var originalPosition = transform.localPosition;

			await DOTween.Sequence()
		        .Append(transform.DOLocalMove(originalPosition + new Vector3(0f, 0.5f, 0f), 0.2f))
		        .Append(transform.DOLocalMove(originalPosition, 0.1f))
		        .SetLoops(3)
	        ;
        }
	}
}
