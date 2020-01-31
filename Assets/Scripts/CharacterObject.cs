using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{

    public Vector3 velocity;

    public float gravity = -0.01f;

    public Vector3 friction = new Vector3(0.85f, 0.99f, 0.85f);

    public CharacterController myController;

    public int currentState;
    public float currentStateTime;
    public float prevStateTime;


    // Start is called before the first frame update
    void Start()
    {
        myController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
   void FixedUpdate()
    {
        //uupdate input buffer

        //update input
        UpdateInput();


        //update state machine
        UpdateState();


        //update physics
        UpdatePhysics();

    }


    void UpdateState()
    {
        CharacterState myCurrentState = GameEngine.coreData.characterStates[currentState];

        UpdateStateEvents();

        prevStateTime = currentStateTime;
        currentStateTime++;

        
        if(currentStateTime >= myCurrentState.length)
        {
            if(myCurrentState.loop) { LoopState(); }
            else { EndState(); }  
        }
    }

    void LoopState()
    {
        currentStateTime = 0;
        //currentState = 0;
        prevStateTime = -1;
    }

    void EndState()
    {
        currentStateTime = 0;
        currentState = 0;
        prevStateTime = -1;
        StartState(currentState);
    }


    void UpdateStateEvents()
    {
        foreach(StateEvent _ev in GameEngine.coreData.characterStates[currentState].events)
        {

            if (currentStateTime >= _ev.start && currentStateTime <= _ev.end)
            {
                DoEventScript(_ev.script, _ev.variable);
            }
        }
    }

    void DoEventScript(int _index, float _var)
    {
        switch (_index)
        {
            case 0:
                VelocityY(_var);
                break;
            case 3:
                StickMove(_var);
                break;
        }
 
    }

    void StickMove(float _pow)
    {
        float _mov = 0;
        if (Input.GetAxisRaw("Horizontal") > deadzone)
        {
            _mov = 1;
        }

        if (Input.GetAxisRaw("Horizontal") < -deadzone)
        {
            _mov = -1;
        }
        
        velocity.x += _mov * moveSpeed * _pow;
    }

    void VelocityY(float _pow)
    {
        velocity.y = _pow;
    }

    public float deadzone = 0.2f;
    public float moveSpeed = 0.1f;
    public float jumpPow = 0.3f;

    void StartState(int _newState)
    {
        currentState = _newState;
        prevStateTime = -1;
        currentStateTime = 0;
    }



    void UpdateInput()
    {



        foreach(InputCommand c in GameEngine.coreData.commands)
        {

            if (c.inputString != "")
            {

                if (Input.GetButtonDown(c.inputString))
                {
                    StartState(c.state);
                    break;
                    
                }
            }
        }


    }


    void UpdatePhysics()
    {

        velocity.y += gravity;


        myController.Move(velocity);

        velocity.Scale(friction);

        //transform.position += velocity;
        //character



    }

}
