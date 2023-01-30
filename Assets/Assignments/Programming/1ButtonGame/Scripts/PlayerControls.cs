using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Blading_Player
{

    public class PlayerControls : SerializedMonoBehaviour
    {
        [HideInInspector]
        public Rigidbody2D rb;
        private Animator myAnimator;
        public GameObject WinMenu;
        public GameObject PauseButton;

        [Header("Player Variables")]
        [Space]
        [ReadOnly, LabelText("Current State")] public States currentState;

        public StateMachine stateMachinePlayer;


        [SerializeField, ShowInInspector, ReadOnly] private float moveSpeed, topSpeed, acelleration, decelleration, jumpHeight;
        [HorizontalGroup("Group Booleans", 0.33f, LabelWidth = 50f)]
        [ReadOnly] public bool grounded, canFall, canGrind, grinding;
        [ReadOnly, LabelText("State Collider")] public GameObject currentStateCollider;
        [HorizontalGroup("Space Bar", 1f, LabelWidth = 200f)]
        [SerializeField, ReadOnly, LabelText("Is Space Bar Pressed Down?")] private bool lastSpaceBarInput;

        public float MoveSpeed => moveSpeed;

        #region Variables not to be seen or changed.
        
        // This will be the folder with all the box colliders in it
        private GameObject currentStateColliderFolder;
        public List<GameObject> listOfStateColliders;


        // Meant to be here for how long it takes to do a trick but will be taken out.
        private int ActiveCollider;
        private string activeStateAnimation;

        #endregion

        private void Awake()
        {
            ActiveCollider = 0;
            moveSpeed = 1;
            jumpHeight = 5;
            grinding = false;
            canGrind = false;
            canFall = false;
            myAnimator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {
            stateMachinePlayer = GetComponentInChildren<StateMachine>();
        }

        // Update is called once per frame
        void Update()
        {
            lastSpaceBarInput = Input.GetKeyDown("space");

            if (moveSpeed < topSpeed + 0.001f)
            {
                moveSpeed += acelleration * Time.deltaTime;
            }
            else if (moveSpeed > topSpeed + 0.001f)
            {
                moveSpeed -= decelleration * Time.deltaTime;
            }
            else if (moveSpeed >= topSpeed - 0.001f && moveSpeed < topSpeed + 0.001f)
            {
                moveSpeed = topSpeed;
            }
        }

        // At the Start of a game we will put the user into 1 state.
        public void startGame(States _startingState)
        {
            currentState = _startingState;
            activeStateAnimation = stateMachinePlayer.PlayerStateDictionary[_startingState].stateAnimation.name;
            myAnimator.Play(activeStateAnimation);
            listOfStateColliders = stateMachinePlayer.PlayerStateDictionary[_startingState].listOfStateColliders;
            currentStateCollider = listOfStateColliders[ActiveCollider];
            currentStateCollider.gameObject.SetActive(true);
            topSpeed = stateMachinePlayer.PlayerStateDictionary[_startingState].topSpeed;
            acelleration = stateMachinePlayer.PlayerStateDictionary[_startingState].acelleration;
            decelleration = stateMachinePlayer.PlayerStateDictionary[_startingState].decelleration;
            canFall = stateMachinePlayer.PlayerStateDictionary[_startingState].canFall;
            canGrind = stateMachinePlayer.PlayerStateDictionary[_startingState].canGrind;
        }

        // This method will handle changing the state of the player.
        public void entredNewState(States _newCurrentState)
        {
            // Uno, if the player's new state has a differnet animation it will play the new animation, if not it will not change the animaton.
            if (stateMachinePlayer.PlayerStateDictionary[currentState].stateAnimation != 
                stateMachinePlayer.PlayerStateDictionary[_newCurrentState].stateAnimation)
            {
                // This will play the current state animation IF it is different to the last one.
                myAnimator.Play(stateMachinePlayer.PlayerStateDictionary[_newCurrentState].stateAnimation.name);

                // If the animation is different.
                // We will now cycle through the box colliders and turn off all the old ones.
                foreach (GameObject collider in listOfStateColliders)
                {
                    collider.gameObject.SetActive(false);
                }
                ActiveCollider = 0;
                // Turn on all the new collider.
                listOfStateColliders = stateMachinePlayer.PlayerStateDictionary[_newCurrentState].listOfStateColliders;
                currentStateCollider = listOfStateColliders[ActiveCollider];
                currentStateCollider.gameObject.SetActive(true);
            }

            // If we need to compare anything else between the old and new states we will do it above.
            currentState = _newCurrentState;

            // Now we will change all the important Variables.
            // We could easily just reference them like:
            // "stateMachinePlayer.PlayerStateDictionary[currentState].topSpeed"
            // but this wont show up in inspector as "Read only" and thus we are changing this to say:
            // "topSpeed"
            topSpeed = stateMachinePlayer.PlayerStateDictionary[currentState].topSpeed;
            acelleration = stateMachinePlayer.PlayerStateDictionary[currentState].acelleration;
            decelleration = stateMachinePlayer.PlayerStateDictionary[currentState].decelleration;
            canFall = stateMachinePlayer.PlayerStateDictionary[currentState].canFall;
            canGrind = stateMachinePlayer.PlayerStateDictionary[currentState].canGrind;
            jumpHeight = stateMachinePlayer.jumpHeight;
        }
        void FixedUpdate()
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                grounded = true;
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
            else if (collision.gameObject.tag == "Rail" && canGrind == true)
            {
                entredNewState(States.Grind);
                grounded = true;
                Vector2 vel = rb.velocity;                              // <<<<<<<<<<<--------------<<<<<<<<<<<<<<<<<<<<<<<------------ MOVED FROM STATE MACHINE.GRINDING.
                rb.velocity = new Vector2(vel.x, 0);
            }
            else if (collision.gameObject.tag == "Finish" && currentState != States.Fall)
            {
                WinMenu.SetActive(true);
                PauseButton.SetActive(false);
                Time.timeScale = 0;
            }
        }

        // These are all the scripts referenced in the animator
        #region Animator Scripts.
        public void ExitJump() => stateMachinePlayer.ExitJump();
        public void FallOver() => StartCoroutine(stateMachinePlayer.FallOver());

        // On Jump Increase height.
        public void Jump()
        {
            // Makes the player jump.
            rb.velocity += Vector2.up * jumpHeight;
            grounded = false;
        }

        public void FinishAirTrick()
        {
            entredNewState(States.Air_CantGrind);
        }

        // Turns off the last collider and gets the next.
        public void nextCollider()
        {
            currentStateCollider.gameObject.SetActive(false);
            // Turn on all the new collider.
            ActiveCollider = Mathf.Clamp(ActiveCollider, 0, listOfStateColliders.Count-1);
            currentStateCollider = listOfStateColliders[ActiveCollider];
            currentStateCollider.gameObject.SetActive(true);
            ActiveCollider++;
        }

        public void LoopAnimationColliders()
        {
            foreach (GameObject collider in listOfStateColliders)
            {
                collider.gameObject.SetActive(false);
            }
            ActiveCollider = 0;
            // Turn on all the new collider.
            currentStateCollider = listOfStateColliders[ActiveCollider];
            currentStateCollider.gameObject.SetActive(true);
        }
        #endregion

    }
}
