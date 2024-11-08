using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Camera MainCamera;

    private const float WALK_SPEED = 7.2f;
    private const float INVERT = -1f;
    private const float ROTATION_RATIO = 0.1f;
    private const float GRAVITY = -9.81f; // 重力の定数を追加
    private Vector3 _direction;
    private Vector3 _keyInput;
    private float _cameraAngle = 0f;
    private bool _isMoving = false;
    private Vector3 _velocity; // 速度ベクトルを追加
    private AudioSource audioSource; //オーディオソース
    public AudioClip footstep; //足音

    void Start()
    {
        // マウスカーソルを非表示にし、画面中央に固定する
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = footstep;
        audioSource.loop = true;
    }

    void Update()
    {

        // 重力を適用
        if (Controller.isGrounded)
        {
            _velocity.y = 0f; // 地面にいる場合、垂直速度をリセット
        }
        else
        {
            _velocity.y += GRAVITY * Time.deltaTime;
        }

        Controller.Move((_direction * WALK_SPEED + _velocity) * Time.deltaTime);

        // 足音の再生・停止制御
        if (_isMoving && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!_isMoving && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void OnLookEvent(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            _cameraAngle += delta.y * INVERT * ROTATION_RATIO;
            _cameraAngle = Mathf.Clamp(_cameraAngle, -89f, 89f);
            MainCamera.transform.localEulerAngles = new Vector3(_cameraAngle, 0f, 0f);

            this.transform.Rotate(new Vector3(0f, delta.x * ROTATION_RATIO, 0f), Space.Self);

            if (_isMoving)
            {
                _direction = this.transform.TransformVector(_keyInput);
            }
        }
    }

    public void OnMoveEvent(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _isMoving = true;
            Vector2 input = context.ReadValue<Vector2>();
            _keyInput = new Vector3(input.x, 0, input.y);
            _direction = this.transform.TransformVector(_keyInput);
        }
        else if (context.phase == InputActionPhase.Performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _keyInput = new Vector3(input.x, 0, input.y);
            _direction = this.transform.TransformVector(_keyInput);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isMoving = false;
            _direction = Vector2.zero;
        }
    }
}
