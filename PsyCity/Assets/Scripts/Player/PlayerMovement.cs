using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{

	public float moveSpeed;

	//animations
	private Animator anim;

	//rigidbody
	private Rigidbody2D RigidPlayer;

	//movement check
	private bool Moving;
	public Vector2 lastMove;

    PlayerStats playerStats;

	private static bool playerExists;

    public bool canMove;

    // Use this for initialization
    void Start () 
	{
        playerStats = GetComponent<PlayerStats>();

        anim = GetComponent<Animator> ();
        RigidPlayer = GetComponent<Rigidbody2D> ();
        
		if (!playerExists) 
		{
			playerExists = true;
			DontDestroyOnLoad (transform.gameObject);
		} 
		else 
		{
			Destroy(gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{

		Moving = false;

        if (!canMove)
        {
            RigidPlayer.velocity = Vector2.zero;
            return;
        }

		//movements

		//left right
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) 
		{
			RigidPlayer.velocity = new Vector2(Input.GetAxisRaw("Horizontal")* moveSpeed ,RigidPlayer.velocity.y);
			Moving = true;
			lastMove = new Vector2(Input.GetAxisRaw ("Horizontal"),0f);
		}

		//up down
		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f ) 
		{
			RigidPlayer.velocity = new Vector2(RigidPlayer.velocity.x,Input.GetAxisRaw("Vertical")* moveSpeed );
			Moving = true;
			lastMove = new Vector2(0f,Input.GetAxisRaw ("Vertical"));
		}

		//stp moving
		if (Input.GetAxisRaw ("Horizontal") < 0.5f && Input.GetAxisRaw ("Horizontal") > -0.5f) 
		{
			RigidPlayer.velocity = new Vector2(0f, RigidPlayer.velocity.y);
		}
		if (Input.GetAxisRaw ("Vertical") < 0.5f && Input.GetAxisRaw ("Vertical") > -0.5f) 
		{
			RigidPlayer.velocity = new Vector2(RigidPlayer.velocity.x ,0f);
		}

		//tell animator to do
		anim.SetFloat ("MoveX", Input.GetAxisRaw ("Horizontal"));
		anim.SetFloat ("MoveY", Input.GetAxisRaw ("Vertical"));
		anim.SetBool ("Moving", Moving);
		anim.SetFloat("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);

        

    }

    public void StartFight()
    {
        playerStats.GoBattle();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log("collision");
            StartFight();
        }

        if (collision.tag == "EndGame")
        {
            playerStats.LoadScene();
        }
    }

}
