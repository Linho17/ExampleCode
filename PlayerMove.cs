using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatePlayer
{
    Prepare,
    Run,
    Swim,
    Jump,
    Down,
    DubbleJump,
    SwimOut,
    Die
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speedRun;
    [SerializeField] private float _forceJump;
    [SerializeField] private Vector2 _directionJump;

    [SerializeField] WaterLevels waterLevels;
    private float[] levelPosition;
    private int currentLevel = -1;

    private Swipe _swipe;


    [SerializeField] private Transform checkPoint;
   
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Vector2 checkSize;

    [SerializeField] private SoundEffect soundEffect;
    


    
    public StatePlayer state;
    public StatePlayer stateBeforeDie;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;



    [SerializeField] ParticleSystem particleBubble;
    [SerializeField] PlayerStats playerStats;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _swipe = GetComponent<Swipe>();
        levelPosition = waterLevels.GetLevels();
        particleBubble.Stop();
    }


    private void Start()
    {
        state = StatePlayer.Prepare;
        _animator.CrossFade("prepare_1", 0f);
    }


    public void Go()
    {
        state = StatePlayer.Run;
        _animator.CrossFade("walk_1", 0f);
    }


    public void Die()
    {
        stateBeforeDie = state;
        state = StatePlayer.Die;
        
    }




    

    public void Revive()
    {
        state = stateBeforeDie;
        if (state != StatePlayer.Swim)
        {
            state = StatePlayer.Run;
        }
       

        playerStats.SetStart(SettingsKey.GetDistance(), SettingsKey.GetFish(),false);

        
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (state == StatePlayer.Prepare)
            return;


        if (state!=StatePlayer.Die)
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && state != StatePlayer.Swim)
            {
                soundEffect.GroundedAudio();
                if (state != StatePlayer.Run && state != StatePlayer.Down)
                {
                    _animator.CrossFade("walk_1", 0f);
                    state = StatePlayer.Run;
                }
            }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == StatePlayer.Prepare)
            return;


        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            soundEffect.SwimInAudio();

            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector2.zero;
           
            if(state == StatePlayer.DubbleJump || state == StatePlayer.Run || state == StatePlayer.SwimOut)
                _animator.CrossFade("dive_up_1", 0f);
            else
                _animator.CrossFade("dive_down_1", 0f);

            state = StatePlayer.Swim;
            particleBubble.Play();
            currentLevel =0;
            
            
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state == StatePlayer.Prepare)
            return;


        if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            soundEffect.SwimOutAudio();
            _rigidbody2D.gravityScale = 10;
            _animator.CrossFade("grounded_1", 0f);
            state = StatePlayer.SwimOut;
            particleBubble.Stop();

            
        }
    }

   

    private void Update()
    {
        

            StatusSwipe statusSwipe = _swipe.TouchReader();


            switch (state)
            {
                case StatePlayer.Die:
                    return;

                case StatePlayer.Prepare:
                    return;
                    


                case StatePlayer.Run:
                    if (Input.GetKeyDown(KeyCode.UpArrow) || statusSwipe == StatusSwipe.SliceUP)
                    {


                        state = StatePlayer.Jump;
                        _animator.CrossFade("jump_1", 0f);
                        soundEffect.JumpAudio();


                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || statusSwipe == StatusSwipe.HoldDown)
                    {

                         soundEffect.SliceAudio();
                         state = StatePlayer.Down;
                        _animator.CrossFade("dawn_1", 0f);

                    }
                    break;

                case StatePlayer.Swim:
                    if (Input.GetKeyDown(KeyCode.UpArrow) || statusSwipe == StatusSwipe.SliceUP)
                    {
                        SwipUp();
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || statusSwipe == StatusSwipe.SliceDown)
                    {
                        SwimDown();

                    }

                    ChangeLevel(ref currentLevel);
                    break;


                case StatePlayer.Jump:
                    if (Input.GetKeyDown(KeyCode.UpArrow) || statusSwipe == StatusSwipe.SliceUP)
                    {
                    soundEffect.FlapAudio();
                        state = StatePlayer.DubbleJump;
                        _animator.CrossFade("fly_1", 0f);
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || statusSwipe == StatusSwipe.SliceDown)
                    {
                        //Быстрое преземление

                    }
                    break;

                case StatePlayer.Down:


                    if (!(Input.GetKey(KeyCode.DownArrow) || statusSwipe == StatusSwipe.HoldDown))
                    {
                        soundEffect.StopSlice();
                        _animator.CrossFade("walk_1", 0f);
                        state = StatePlayer.Run;
                    }
                    break;


                case StatePlayer.DubbleJump:
                   
                    break;
            }

            transform.Translate(Vector3.right * _speedRun * Time.deltaTime, Space.World);
        
    }

    private void SwimDown()
    {
        currentLevel++;
        if (currentLevel < levelPosition.Length)
            _animator.CrossFade("swim_down", 0f);
    }

    private void SwipUp()
    {
        currentLevel--;
        if (currentLevel >= 0)
            _animator.CrossFade("swim_up", 0f);
    }

    private void SetJump(Vector2 direction, float force)
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }




    private void ChangeLevel(ref int level)
    {
        if (level < -1)
        {
            level = -1;
            return;
        }

        if (level == -1)
        {
            if (Physics2D.OverlapBox(checkPoint.position, checkSize, 0,groundLayer))
            {
                level = 0;
                return;
            }
            
            SetJump(Vector2.up, _forceJump*1.5f);
           
            return;
        }



        if (level >= levelPosition.Length)
        {
            level--;
            return;
        }

        

        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, levelPosition[level]), 0.1f);
        
    }

    private void Push()
    {
        SetJump(Vector2.right, _forceJump/2);
    }

    private void Jump()
    {
       SetJump(_directionJump, _forceJump);
    }

    private void Fly()
    {
        
        SetJump(_directionJump + Vector2.right, _forceJump);
    }



    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(checkPoint.position,checkSize);
    }

}
