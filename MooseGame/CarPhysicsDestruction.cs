using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarPhysicsDestruction : MonoBehaviour
{

    /// <summary>
    /// Reference to Rigidbody component attached to this car
    /// </summary>
    [Tooltip("A reference to the Rigidbody componenet attached to this car")]
    Rigidbody rb;

    /// <summary>
    /// Reference to Wheels attached to this car
    /// </summary>
    [Tooltip("A reference to the Wheels attached to this car")]
    Transform[] Wheels = new Transform[4]; // Set size to 4


    /// <summary>
    /// A reference to the game controller
    /// </summary>
    [Tooltip("A reference to the game controller")]
    [SerializeField]
    GameController gameController;


    /// <summary>
    /// A reference to if the prefab is in fact a truck
    /// </summary>
    [Tooltip("A reference to if the prefab is in fact a truck")]
    public bool isTruck;

    bool canAddPoints = true; // For some reason OnTriggerEnter would allow for two frames of input, rather than one, thanks Unity

    /// <summary>
    /// A reference to the explosion prefab, a game object.
    /// </summary>
    [Tooltip("A reference to the explosion prefab, a game object.")]
    public GameObject explosion;

    /// <summary>
    /// A reference to the audio source component.
    /// </summary>
    [Tooltip("A reference to the audio source component")]
    [SerializeField]
    private AudioSource aud;

    /// <summary>
    /// A reference to the explosion sound clip
    /// </summary>
    [Tooltip("A reference to the explosion sound clip")]
    public AudioClip explosionSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.priority = Random.Range(100,128);
       
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        for (int i = 0; i < Wheels.Length; i++)
        {
            Wheels[i] = gameObject.transform.GetChild(i);
            //Debug.Log(Wheels[i] + " Assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
// Tests our actual function in OnTriggerEnter
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i].GetComponent<Rigidbody>().useGravity = true;
                float Direction = Random.Range(0, 2);
                if (Direction == 1)
                {
                    Wheels[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(1000, 2000), Random.Range(1000, 3000), Random.Range(1000, 3000)));
                    Wheels[i].GetComponent<WheelSpinner>().DeleteByTime(10f);
                }
                else
                {
                    Wheels[i].GetComponent<Rigidbody>().AddForce(new Vector3(-Random.Range(1000, 3000), -Random.Range(1000, 3000), -Random.Range(1000, 3000)));
                    Wheels[i].GetComponent<WheelSpinner>().DeleteByTime(10f);
                    
                }
            }
            bool canAddPoints = true;
            if (canAddPoints)
            {
                gameController.AddPoints(100);
                canAddPoints = false;
            }
            rb.AddForce(new Vector3(0f, 500f, 0f));
            transform.DetachChildren();
            Destroy(gameObject, 3f);

            //rb.AddExplosionForce(1000f, transform.position,10f);
        }
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moose") && !isTruck)
        {

            //Call AddForce Coroutine in Wheels
            //Deparent the Wheels
            //AddForce to Car Body

            //Debug.Log("Falling Apart");
            for (int i = 0; i < Wheels.Length; i++)
            {
                Wheels[i].GetComponent<Rigidbody>().useGravity = true;
                float Direction = Random.Range(0, 2);
                if (Direction == 1) //Right
                {
                    Wheels[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(1000, 3000), Random.Range(1000, 3000), Random.Range(1000, 3000)));
                    Wheels[i].GetComponent<WheelSpinner>().DeleteByTime(5f);
                    Wheels[i].GetComponent<BoxCollider>().isTrigger = true;
                }
                else //Left
                {
                    Wheels[i].GetComponent<Rigidbody>().AddForce(new Vector3(-Random.Range(1000, 3000), -Random.Range(1000, 3000), -Random.Range(1000, 3000)));
                    Wheels[i].GetComponent<WheelSpinner>().DeleteByTime(5f);
                    Wheels[i].GetComponent<BoxCollider>().isTrigger = true;

                }
            }

            if (canAddPoints)
            {
                gameController.AddPoints(100);
                Instantiate(explosion, transform.position, Quaternion.identity);
                aud.PlayOneShot(explosionSoundClip, 0.3f);
                canAddPoints = false;
            }
            float CarBodyDirection = Random.Range(0, 2);
            if (CarBodyDirection == 1) //Right
            {
                rb.AddForce(new Vector3(Random.Range(1000, 3000), Random.Range(1000, 3000), Random.Range(1000, 3000)));
            }
            else //Left
            {
                rb.AddForce(new Vector3(-Random.Range(1000, 3000), Random.Range(1000, 3000), -Random.Range(1000, 3000)));

            }
            //transform.Rotate(new Vector3(1, 5, 3) * Time.deltaTime * 5f); // Rotate
            transform.DetachChildren();

            //Debug.Log("Boom!");
            Destroy(gameObject,3f);
        }

        if (other.CompareTag("Moose") && isTruck)
        {
            gameController.SwitchGameOver();
        }
    }
}
