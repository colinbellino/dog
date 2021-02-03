using UnityEngine;

namespace Dog.Game
{
    public class Player : MonoBehaviour
    {
        private CharacterController _characterController;
        private Camera _camera;

        [SerializeField] private Transform _groundCheck;
        [SerializeField] private float _groundCheckRadius = 0.5f;
        [SerializeField] private LayerMask _groundCheckMask;

        private float _xRotation;
        private Vector3 _velocity;
        private bool _isGrounded;

        private void Awake()
        {
            _characterController = GetComponentInChildren<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
        }

        public void Move(Vector2 moveInput, float speed, float gravityModifier, float step)
        {
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundCheckMask);

            if (_isGrounded)
            {
                _velocity = Vector3.zero;
            }

            var move = (transform.forward * moveInput.y + transform.right * moveInput.x);
            _characterController.Move(move * (speed * step));

            _velocity += Physics.gravity * (step * step * gravityModifier);
            _characterController.Move(_velocity);
        }

        public void Look(Vector2 lookInput, float sensitivity, float step)
        {
            var look = lookInput * (sensitivity * step);

            _xRotation -= look.y;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up, look.x);
        }
    }
}
