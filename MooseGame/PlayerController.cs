using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
 * The purpose of this script is to:
 *  A: Control lateral movement of the player
 *  B: Generate a "lane" system in which the player moves between predetermined positions
 *  C: Replicate behaviors of PC Control for Mobile
 */

/* 
 * When this script is applied to a game object, RequireComponent will automatically generate the specified component during play. 
 * If a designer tries to remove the specified component, they will be prohibited so long as they do not remove this script.
 * To disable this, comment out the unwanted RequireComponent statement holding specified type of component.
*/
[RequireComponent(typeof(CharacterController))] 
public class PlayerController : MonoBehaviour
{
    [Tooltip("Constant Float Value. Distance between lanes, used to shift between them.")]
    private const float LANE_DISTANCE = 8.0f;

    //Movement
    /// <summary>
    /// A reference to the CharacterController component 
    /// </summary>
    private CharacterController controller;
    /// <summary>
    /// Float Value. Amount of force we would like applied to our Character.
    /// </summary>
    [Tooltip("Float Value. Amount of force we would like applied to our Character.")]
    [SerializeField]
    private float jumpForce = 4.0f;
    /// <summary>
    /// A reference to the Rigidbody component 
    /// </summary>
    [Tooltip("Float Value. Downward force we would like to apply to our Character as they move about the game world.")]
    private float gravity = 12.0f;
    /// <summary>
    /// A reference to the Rigidbody component 
    /// </summary>
    [Tooltip("Float Value. Velcoity to be applied along the Y-Axis, when called for.")]
    private float verticalVelocity;
    [Tooltip("Float Value. Units to be applied")]
    [SerializeField] // Note though while we can view in inspector, still private, so we can change for its first use, but defaults thereafter to assigned value in code, merely here for debugging
    private float speed = 7.0f;
    [Tooltip("Int Value, index value to set states for each specified lane. 0 = left, 1 = middle, 2 = right.")]
    [SerializeField]
    private int desiredLane = 1;


    [Header("Matts's newly added Variables")]
    public bool MattIsCool = true;
    /// <summary>
    /// Control Flag for when we call a console print statement
    /// </summary>
    [Tooltip("Control Flag for when we call a console.")]
    [SerializeField]
    bool consoleTextDisplayed;
    /// <summary>
    /// Size of lane array which use as min and max value
    /// </summary>
    [Tooltip("Size of lane array which use as min and max value.")]
    [SerializeField]
    int laneSize;
    /// <summary>
    /// Desired way in which we swing left to right
    /// </summary>
    [Tooltip("Desired way in which we swing left to right.")]
    [SerializeField]
    int desiredIncriment;

    public Animator MoseAnimator;


    private void Start()
    {
        MoseAnimator = gameObject.transform.GetChild(0).GetComponent<Animator>();

        desiredIncriment = desiredLane;
        consoleTextDisplayed = false; // We haven't displayed console text, so false to start
        laneSize = 2;//GameObject.Find("GameControllerOBJ").GetComponent<GameController>().lanes.Length; // Access Game Controller to Find Length of Lanes array
        controller = GetComponent<CharacterController>();
    }


    private void Update()
    {
        
        //float move = 1f;
        
        
        //Check if we are running either in the Unity editor or in a   
        //standalone build. 
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        //readInputPC();
        ClampPlayerToLane();
        if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            //MoseAnimator.Play("moose rig2");

            //transform.rotation = new Quaternion(0, -90, 0, 0);

            MoseAnimator.ResetTrigger("MDTrigger");

            MoseAnimator.SetTrigger("MDTrigger");


            
            //MoveLane(false);
            //print("move left");
            desiredIncriment--;
            //PrintIntValue(desiredLane);
        }
        if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            MoseAnimator.ResetTrigger("MDTrigger");


            MoseAnimator.SetTrigger("MDTrigger");

            //transform.rotation = new Quaternion(0, 90, 0, 0);

            //MoveLane(true);
            //print("move right");
            desiredIncriment++;
            //PrintIntValue(desiredLane);
        }
        //calculate where we should be

        Vector3 targetPosition = transform.position.z * Vector3.forward;
   
            targetPosition += new Vector3(desiredIncriment,1.38f,0) * LANE_DISTANCE;

        //calculate move delta

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * speed;
        moveVector.y = -0f;
        moveVector.z = 0f;

        //move player

        controller.Move(moveVector * Time.deltaTime);


        /*
        //gather the inputs on which lane we should be

        if (Input.GetKeyDown(KeyCode.A)) //move left
        {
            //MoveLane(false);
            //print("move left");
        }
        if (Input.GetKeyDown(KeyCode.D)) //move right
        {
            //MoveLane(true);
            //print("move right");
        }
        //calculate where we should be

        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
            //print(desiredLane);
            PrintIntValue(desiredLane);
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
            //print(desiredLane);
            PrintIntValue(desiredLane);
        }


        //calculate move delta

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * speed;
        moveVector.y = -0f;
        moveVector.z = 0f;

        //move player

        controller.Move(moveVector * Time.deltaTime);
        */


        //Check if we are running on a mobile device, code unreachable unless we change build target in build settings
#elif UNITY_IOS || UNITY_ANDROID



        //calculate where we should be

        Vector3 targetPosition = transform.position.z * Vector3.forward;

        targetPosition += new Vector3(desiredIncriment, 0, 0) * LANE_DISTANCE;

        //calculate move delta

        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).x * speed;
        moveVector.y = -0f;
        moveVector.z = 0f;

        //move player

        controller.Move(moveVector * Time.deltaTime);


        ClampPlayerToLane();
        if (Input.touchCount > 0) // Check if we touched screen
        {
            // Store first instance of touch in a variable
            Touch touch = Input.touches[0];


            if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
             
            // Store position of screen touch as Position of Raycast
            Vector3 pixelPos = touch.position;
            // Convert Position of Raycast into screen position
            Vector3 worldPos = Camera.main.ScreenToViewportPoint(pixelPos); 

            if (worldPos.x > 0.5f) //move right if touch was on right side of screen
            {

                MoseAnimator.ResetTrigger("MDTrigger");

                MoseAnimator.SetTrigger("MDTrigger");

                desiredIncriment++;
                PrintIntValue(desiredLane);
            }
            else //move left if touch was not on right side of screen, therefore left
            {
                
                MoseAnimator.ResetTrigger("MDTrigger");

                MoseAnimator.SetTrigger("MDTrigger");
                desiredIncriment--;
                PrintIntValue(desiredLane);
            }
            
        }
       
        }
#endif

    }


    private void MoveLane(bool goingRight)
    {
        desiredLane += (goingRight) ? 1 : -1;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2); //lowest value is 0, highest is 2, eddit this to change how far the player can move.
    }

    /// <summary>
    /// Clamp Player to maximum value of movement along x axis. Player cannot move more right than max of number of lanes, cannot move more left than negative inverse of number of lanes
    /// </summary>
    void ClampPlayerToLane()
    {
        //Debug.Log(desiredIncriment);
        if (desiredIncriment > laneSize)
        {
            //Debug.Log("Lane size maxed");
            desiredIncriment = laneSize; // Set to Maximum of laneSize
        }
        else if (desiredIncriment < -laneSize)
        {
            //Debug.Log("Lane size Mined");
            desiredIncriment = -laneSize; // Set to minimum of laneSize
        }
    }




    /// <summary>
    /// Call SetLane depending on Input.
    /// </summary>
    void readInputPC()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetLane(-1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SetLane(1);
        }
    }

    /// <summary>
    /// Set Lane to which the player is to be moved.
    /// </summary>
    void SetLane(int desiredIncriment)
    {
        desiredLane = desiredLane + desiredIncriment;
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        targetPosition += new Vector3(desiredIncriment, 0, 0) * LANE_DISTANCE;


        /*
        switch (desiredLane)
        {
            case 0:
            break;
            default:
                if (desiredLane > laneSize)
                {
                    desiredLane = laneSize; // Set to Maximum of laneSize
                }
                else if (desiredLane < laneSize-laneSize)
                {
                    desiredLane = laneSize - laneSize; // Set to minimum of laneSize
                }
            break;
        }
        */
    }



    /// <summary>
    /// Print our integer, once, even in update. Can use with any value.
    /// </summary>
    /// <param name="value">The integer value we would like to print.</param>
    void PrintIntValue(int value)
    {
        if (!consoleTextDisplayed)
        {
            //Debug.Log(value);
            SwitchConsoleTextDisplayed(); // Sets ConsoleTextDisplayed
        }
        //SwitchConsoleTextDisplayed(); // Set False so we can call it again
    }
    /// <summary>
    /// Switch our ConsoleTextDisplayed Bool between True and False
    /// </summary>
    void SwitchConsoleTextDisplayed()
    {
        consoleTextDisplayed = !consoleTextDisplayed;
    }

   
}
