//Goddammit96
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    /*
	 * This script goes in the player (which has to have tag Player).
	 * Players needs another collider but with the IsTrigger activated.
	 * This controls basic behaviour of the player
	 **/

    private Animator animator; // References the animator
    public GameObject movementJoystick; // Get the game object which controls the player movement.
    private Rigidbody rigidBody; // Player's rigidbody.
    private VirtualJoystick mover; // To get the input vector.
    public GameObject spellCanvasPrefab;
    private GameObject spellCanvas;

    public int movementSpeed; // Movement speed.
    private int originalSpeed;
    public int dashForce; // Throwback speed.
    public int force; // Used to calculate the strength of the character, linked to how much "damage"
    // it does on collision.
    public bool isDashing; // Says if the character is in a dash attack
    private bool isRunning;

    public float stunTime; // Time the player will be stunned.
    public bool isStunned = false; // Says whether the player is stunned.

    // Unity coroutines
    private IEnumerator speedDown;
    private IEnumerator stunEnu;
    private IEnumerator dashEnu;

    private GlobalVariables globals; // An empty object with all the global variables in it.

    private int myPlayer; // An integer indicating which player the script is referring to.

    private bool allowMove;
    private TimeController endTime;

    void Start()
    {
        isRunning = false;

        animator = GetComponent<Animator>();
        isStunned = false;

        rigidBody = this.GetComponent<Rigidbody>();
        mover = movementJoystick.GetComponent<VirtualJoystick>();

        globals = GameObject.Find("GlobalVariables").GetComponent<GlobalVariables>();
        myPlayer = globals.currentSpawnedPlayer;

        gameObject.tag = globals.tags[myPlayer]; // Set a tag to the player depending on the order it spawned
        globals.currentSpawnedPlayer++;
        // Store the speed of the player (to use when reducing speed or so)
        originalSpeed = movementSpeed;

        isDashing = false;

        allowMove = false;

        if (isLocalPlayer)
        {
            spellCanvas = Instantiate(spellCanvasPrefab) as GameObject;
            spellCanvas.transform.SetParent(transform);
        }

        GameObject count = GameObject.Find("Time");
        endTime = count.GetComponent<TimeController>();
    }

    void Update()
    {
        allowMove = endTime.allowPlayerMovement;
        if (isLocalPlayer)
        {
            if (isDashing)
            {
                Dash();
                Invoke("DashReset", 0.05f);
            }
            // Controls the behaviour of the player depending on if it's stunned.
            if (!isStunned)
            {
                // Moves the player
                Move();
                // Controls the stamina
                if (GetComponent<Stamina>().GetCurrentStamina() <= 0)
                    isStunned = true;
            }
            else
            {
                // Stun the player
                isStunned = true;
                stunEnu = DoStun();
                // Player loses the ball if on possession.
                if (tag == GetComponent<BallHandler>().hasTheBall && GetComponent<BallHandler>().hasTheBall != null)
                {
                    GetComponent<BallHandler>().CmdUngrabBall();
                }
                // Animation
                animator.SetBool("isRunning", false);
                animator.Play("Stun");
                // Start the stun coroutine
                StartCoroutine(stunEnu);
            }
        }
    }

    private void DashReset()
    {
        isDashing = false;
    }

    // This function is called to set the fireball spell animation
    public void PlaySpellAnimation()
    {
        animator.Play("Spell");
    }

    //This function is called when a player is stunned
    private IEnumerator DoStun()
    {
        while (true)
        {
            yield return new WaitForSeconds(stunTime);
            // Stun time is over.
            isStunned = false;
            GetComponent<Stamina>().StopStun();
        }
    }

    private void Dash()
    {
        GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity +
                                            GetComponent<Rigidbody>().transform.forward * dashForce);
    }

    // Called to move the player depending on the input vector.
    private void Move()
    {
        if (allowMove)
        {
            mover.inputVector = GameObject.Find("MovementJoystick").GetComponent<VirtualJoystick>().inputVector;
            transform.Translate(mover.inputVector * movementSpeed * Time.deltaTime, Space.World);
            
            if (mover.inputVector != Vector3.zero)
            {
                if (!isRunning)
                    
                    animator.Play("Running");
                isRunning = true;
                transform.rotation = Quaternion.LookRotation(mover.inputVector);
                animator.SetBool("isRunning", isRunning);
            }
            else
            {
                isRunning = false;
                // Stop the running animation
                animator.Play("Idle");
                animator.SetBool("isRunning", isRunning);
            }
        }
    }

    private IEnumerator ReduceSpeed(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            movementSpeed = originalSpeed;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // When hit by another player that is dashing and this player is not
        if (col.collider.gameObject.tag.Contains("Player") &&
            col.collider.gameObject.GetComponent<PlayerController>().isDashing)
        {
            Debug.Log("in");
            // Player loses the ball if on possession.
            if (tag == this.GetComponent<BallHandler>().hasTheBall &&
                this.GetComponent<BallHandler>().hasTheBall != null)
            {
                this.GetComponent<BallHandler>().CmdUngrabBall();
                col.collider.GetComponent<BallHandler>().CmdGrabBall(col.collider.GetComponent<GameObject>());
            }

            // Get the direction of the other player 
            Vector3 otherPos = col.collider.transform.position; // Collider's position
            Vector3 myPos = transform.position; // My position
            var heading = otherPos - myPos;
            var distance = heading.magnitude;
            var direction = heading / distance; // This is now the normalized direction.
            // Move the player
            Vector3 movement = new Vector3(-direction.x, 0.0f, -direction.y);
            rigidBody.AddForce(movement * col.collider.GetComponent<PlayerController>().dashForce, ForceMode.Impulse);
            // Reduce Stamina on collision
            GetComponent<Stamina>().TakeDamage(col.collider.GetComponent<PlayerController>().force);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Ice":
                if (isLocalPlayer && other.GetComponent<IceLifetime>().owner != gameObject.tag)
                {
                    speedDown = ReduceSpeed(5.0f);
                    movementSpeed = movementSpeed / 2;
                    animator.speed = 0.5f;
                    StartCoroutine(speedDown);
                }
                break;
        }
    }

    // Called every time player is hit by a particle system.
    void OnParticleCollision(GameObject other)
    {
        switch (other.tag)
        {
            case "FireBall":
                GetComponent<Stamina>().TakeDamage(GetComponent<FireballPlayer>().damage);
                break;
        }
    }
}