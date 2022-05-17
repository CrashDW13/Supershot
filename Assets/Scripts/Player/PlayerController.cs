using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

//This one does a lot. I'm going to break it down section by section. 
public class PlayerController : MonoBehaviour
{
    //These variables are the core of the player: the character controller, the animations, the input vector, and the vec3 that keeps track of player velocity. Definitely the most important part.

    private CharacterController controller;
    private Animator animator;
    private Vector2 inputVec;
    private Vector3 playerVelocity;
    [SerializeField] private float playerSpeed = 2.0f;

    //These variables keep track of jumping and gravity. 
    [SerializeField] private float jumpHeight = 40.0f;
    private bool isGrounded;
    private float gravity = -9.81f;
    private float jump;
    
    //The main camera.
    private Transform cam;

    //These three have to do with firing the gun and setting up all the right stuff for the bullets to go in the right direction, position, etc. 
    [SerializeField] private GameObject shootParticlePrefab;
    [SerializeField] private GameObject bullet;
    [SerializeField] Vector3 particleOffset;

    //These variables have to do with getting hit. You'll notice that there isn't a straight "health" variable; this game takes from some other
    //first person shooters where health is determined by how red the screen is. The bit that handles that is near the bottom. 

    private GameObject hitsScreen;
    private bool isInvincible = false;
    private float invincibleWindow = 0.5f;
    private int totalHitsTaken = 0; 

    private void Start()
    {
        //About what you'd expect. Getting the necessary components. 
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        hitsScreen = GameObject.Find("HitScreen");

        //Makes the cursor invisible, in true first person shooter fashion. 
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    void Update()
    {
        //Handles movement, immediately updates camera to follow.
        ProcessMove(inputVec);
        ProcessCam();

        //Checks for jumpability. That's definitely a real word. 
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Changes the height position of the player.
        playerVelocity.y += gravity * Time.fixedDeltaTime;

        //Kills the player if they fall off the edge.
        if (transform.position.y <= 0)
        {
            GetHurt(); 
        }

        

    }

    //Since this has to do with physics checks, I thought it'd be better to put in FixedUpdate().
    private void FixedUpdate()
    {
        isGrounded = controller.isGrounded;
    }

    //Updates the input vector. 
    public void OnMove(InputValue input)
    {
        inputVec = input.Get<Vector2>();

    }

    //Simple jump code. 
    public void OnJump()
    {
        if (isGrounded)
        {
            Debug.Log("GROUNDED");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            Debug.Log(playerVelocity.y);
        }
    }

    //Handles the firing of the gun. 
    public void OnFire()
    {
        //Step 1: Trigger fire animation.
        animator.SetTrigger("OnFire");

        //Step 2: Trigger muzzle flash. 
        GameObject bulletSpark = Instantiate(shootParticlePrefab, cam, false);
        bulletSpark.transform.rotation = cam.rotation;
        bulletSpark.transform.localPosition += particleOffset;

        //Step 3: Fire bullet.
        GameObject firedBullet = Instantiate(bullet, bulletSpark.transform.position, cam.rotation);
        firedBullet.transform.forward = transform.forward;
    }

    //Movement.
    private void ProcessMove(Vector2 input)
    {
        Vector3 move = Vector3.zero;
        move.x = inputVec.x;
        move.z = inputVec.y;

        controller.Move(((transform.forward * move.z) + (transform.right * move.x)) * playerSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    //Just keeps the transform rotated with the camera.
    private void ProcessCam()
    {
        transform.rotation = cam.rotation;
    }

    //Originally, I wasn't going to combine Rigidbodies and CharacterControllers because that's generally not a good thing, but 
    //the function that Character Controllers use to detect collisions *only works* when the player is moving. Which means that the
    //player can just... stand still to avoid damage. Since that's dumb, I decided to give the player a Rigidbody, but make it entirely
    //unnoticeable from a gameplay standpoint so it only exists to enter/exit triggers. 
    private void OnTriggerEnter(Collider other)
    {
        //If the player is on the receiving end of an axe...
        if (other.gameObject.CompareTag("Axe"))
        {
            //Increase total number of hits taken for end screen rank calculation.
            totalHitsTaken++; 

            //Get hurt, check for all the things that entails, then give the player some invincibility frames. 
            if (!isInvincible)
            {
                Debug.Log("SET INVINCIBLE");
                GetHurt();
                isInvincible = true;
            }

        }

        //If the player reaches the end, assign the damage number to the RankData class and go to the end screen victorious. 
        if (other.gameObject.CompareTag("EndGate"))
        {
            RankData.damage = totalHitsTaken;
            other.gameObject.BroadcastMessage("GoToEnd", "WIN");
        }
    }

    //Figures out what to do when the player gets hurt. 
    private void GetHurt()
    {
        //As stated before, that red overlay = health. The higher it gets, the closer the player is to death. 
        var color = hitsScreen.GetComponent<Image>().color;
        color.a += 0.2f;
        hitsScreen.GetComponent<Image>().color = color;

        //Begins a coroutine that causes the player to heal over time. 
        StartCoroutine(BeginHealing());

        //If the screen gets red enough, the player loses. 
        if (color.a > 0.9)
        {
            GameObject endGate = GameObject.Find("EndGate");
            endGate.gameObject.BroadcastMessage("GoToEnd", "LOSE"); 
        }
    }

    //Coroutine that slowly returns the player to their original health. 
    IEnumerator BeginHealing()
    {
        var color = hitsScreen.GetComponent<Image>().color; 

        //First, the coroutine waits until the player's invincibility is up. 
        yield return new WaitForSeconds(invincibleWindow);

        isInvincible = false; 

        //Then, it starts healing...
        for (float t = 0; t < 1; t += Time.deltaTime/100f)
        {
            //If the player has been fully healed, OR if they're invincible again, which means they've gotten hit again, the coroutine immediately stops.
            //This stops the coroutine from running several times, and prevents the player from healing while taking damage from multiple attacks. 
            if (color.a == 0 || isInvincible == true)
            {
                yield break;
            }

            //Otherwise, it starts actually healing. 
            else
            {
                color.a = Mathf.Lerp(color.a, 0f, t);
                hitsScreen.GetComponent<Image>().color = color;
                yield return null; 
            }
        }
    }
}
