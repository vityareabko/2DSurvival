using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using UnityEngine;

public class PlayerMove
{
    private AnimationManager _playerAnimation;
    private Rigidbody2D _rb;
    
    private Character4D _character4D;
    
    private Vector2 _moveDirection;
    private float _moveSpeed = 2f;
    private float _yMove;
    private float _xMove;

    public PlayerMove(AnimationManager anim, Rigidbody2D rb, Character4D character4D, float moveSpeed)
    {
        _moveSpeed = moveSpeed;
        _rb = rb;
        _character4D = character4D;
        _playerAnimation = anim;
        _playerAnimation.SetState(CharacterState.Idle);
        TurnDown();
    }

    public void Tick()
    {
        _xMove = Input.GetAxisRaw("Horizontal");
        _yMove = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(_xMove, _yMove).normalized;
        
        InputAnimHandler();
        InputHandler();
    }

    public void FixedTick() => Move();
    
    private void Move()
    {
        
        if (_xMove == 0 && _yMove == 0)
            _rb.velocity = Vector2.zero;
        else
            _rb.velocity = new Vector2(_moveDirection.x * _moveSpeed, _moveDirection.y * _moveSpeed);
    }

    private void InputHandler()
    {
        if (_xMove < 0)
            _character4D.SetDirection(Vector2.left);
        else if (_xMove > 0)
            _character4D.SetDirection(Vector2.right);
        else if (_yMove > 0)
            _character4D.SetDirection(Vector2.up);
        else if (_yMove < 0)
            _character4D.SetDirection(Vector2.down);
    }
    
    private void InputAnimHandler()
    {
        if (Input.GetKeyDown(KeyCode.S))
            TurnDown();
        else if (Input.GetKeyDown(KeyCode.W))
            TurnUp();
        else if (Input.GetKeyDown(KeyCode.D))
            TurnRight();
        else if (Input.GetKeyDown(KeyCode.A))
            TurnLeft();

        if (_xMove == 0 && _yMove == 0)
            _playerAnimation.SetState(CharacterState.Idle);
        else
            _playerAnimation.SetState(CharacterState.Run);
    }


    private void TurnLeft() =>
        _character4D.SetDirection(Vector2.left);
    
    private void TurnRight() =>
        _character4D.SetDirection(Vector2.right);

    private void TurnUp() =>
        _character4D.SetDirection(Vector2.up);

    private void TurnDown() =>
        _character4D.SetDirection(Vector2.down);

    private void Show4Directions()=>
        _character4D.SetDirection(Vector2.zero);
    
}
