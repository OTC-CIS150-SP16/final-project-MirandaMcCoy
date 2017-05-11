using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    // rb =               Rigidbody comp of this object
    // goStartPos =       Starting position of player (For reseting)
    // isJumpingYet =     Is the player currently jumping?
    // isGrounded =       Is the player on the ground?
    // jumpTimer =        Timer for fluid up/down motion (I'm sure there's a better way but it works and was quick)
    // failCount =        How bad does the player suck?   
    // speed =            How fast force change is
    // jumpHeight =       How high should it jump?
    private Rigidbody rb;
    private Vector3 goStartPos;
    private bool isJumpingYet = false;
    private bool isGrounded = true;
    private bool nextLevel = false;
    private int jumpTimer = 20;
    private int failCount = 0;
    private int level, levelMax, gemCount, gemsCollected;
    [SerializeField] private Material untouched, touched_1, touched_2;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private GameObject platforms;
    [SerializeField] private GameObject[] puzzles;

    void Awake () {
        rb = GetComponent<Rigidbody>();

        goStartPos = this.transform.position;

        level = 0;

        gemsCollected = 0;

        levelMax = puzzles.Length - 1;

        setPlatforms();
    }
	


	void Update () {
        CheckIsDead();

        Move();
    }



    void CheckIsDead()
    {
        Vector3 playPos = this.transform.position;
        
        if (playPos.y < -10 || nextLevel)
        {
            //Camera_Follow.resetCamPos();

            gemsCollected = 0;

            playPos = goStartPos;

            setPlatforms();

            this.transform.position = playPos;

            if (playPos.y < -10)
            {
                failCount++;
            }

            nextLevel = false;
        }
    }


    // Gets input from the player keys, then changes the velocity of the
      // rb to move the player
    void Move()
    {
        // HORIZONTAL MOVEMENT
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0F, 0.0F);

        rb.velocity = (movement * speed);



        // JUMP?
        // Gets up key
        bool moveV = Input.GetKeyDown("up");

        // Sets the movement amount to be used with force
        Vector3 moveUp = new Vector3(0.0F, jumpHeight, 0.0F);

        // Init jumping when up key is pressed and the user is still grounded
        if (moveV && isGrounded)
        {
            isJumpingYet = true;
        }
        // Cont to raise for 20 updates
        else if (isJumpingYet  && jumpTimer <= 20 && jumpTimer > 0)
        {
            rb.AddForce(moveUp * speed);

            jumpTimer--;
        }
        // Sink until grounded
        else if (!isGrounded)
        {
            rb.AddForce(-moveUp * speed);

            jumpTimer--;
        }
        // Reset after being grounded to be able to jump again
        else if (jumpTimer != 20 && isGrounded)
        {
            jumpTimer = 20;
            isJumpingYet = false;
        }
    }



    void setPlatforms()
    {
        Destroy(platforms);

        platforms = Instantiate(puzzles[level]) as GameObject;

        gemCount = Convert.ToInt32(platforms.gameObject.name.ToString().Substring(10, 1));
    }



    // When user collides with the ground, set isGrounded to true for jumping
    void OnCollisionEnter(Collision col)
    {
        Vector3 colPos = col.gameObject.transform.position;
        Vector3 playPos = this.transform.position;

        if ((col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Platform")) && colPos.y < playPos.y - .5F) // Colliding with the top of the ground
        {
            isGrounded = true;
        }


        // Change platform colors or delete
        if (col.gameObject.CompareTag("Platform"))
        {
            if (col.gameObject.transform.position.y < this.transform.position.y)
            {
                if (col.gameObject.GetComponent<Renderer>().material.ToString().Substring(7, 3) == untouched.ToString().Substring(7, 3))
                {
                    col.gameObject.GetComponent<Renderer>().material = touched_1;
                }
                else if (col.gameObject.GetComponent<Renderer>().material.ToString().Substring(7, 9) == touched_1.ToString().Substring(7, 9))
                {
                    col.gameObject.GetComponent<Renderer>().material = touched_2;
                }
                else if (col.gameObject.GetComponent<Renderer>().material.ToString().Substring(7, 9) == touched_2.ToString().Substring(7, 9))
                {
                    isGrounded = false;
                    col.gameObject.SetActive(false);
                }
            }
        }
    }

    // When user stops colliding with the ground, set isGrounded to false for jumping
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }



    // When the user collides with a gem, the gem disappears
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Gem"))
        {
            gemsCollected++;
            col.gameObject.SetActive(false);
            if (gemCount == gemsCollected)
            {
                gemsCollected = 0;
                if (level != levelMax)
                {
                    level++;
                }
                else
                {
                    level = 0;
                }

                nextLevel = true;
                setPlatforms();
            }
        }
    }
}
