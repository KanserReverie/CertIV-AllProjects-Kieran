using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Blading_Player
{
    public class StateMachine : SerializedMonoBehaviour
    {
        public Animator myAnimator;
        public PlayerControls myPlayerControls;

        // Dictionary to hold all the States in.
        [HideInInspector]
        public Dictionary<States, PlayerStates> PlayerStateDictionary = new Dictionary<States, PlayerStates>();

        public int startSpeed;
        #region State Settings.
        [TabGroup("States", "StateSettings")]
        [TabGroup("States/StateSettings/Tabs", "Ground")]
        [TabGroup("States/StateSettings/Tabs/Ground/Tabs", "Space Up")] public PlayerStates PSGrounded_SpaceUp;
        [TabGroup("States/StateSettings/Tabs/Ground/Tabs", "Space Down")] public PlayerStates PSGrounded_SpaceDown;

        [TabGroup("States/StateSettings/Tabs", "Jump")]

        [TabGroup("States/StateSettings/Tabs/Jump/Tab", "Jump Settings"), LabelText("Jump Height")] public float jumpHeight;
        [TabGroup("States/StateSettings/Tabs/Jump/Tab", "Jump")] public PlayerStates PSJump;

        [TabGroup("States/StateSettings/Tabs", "Air")]

        [TabGroup("States/StateSettings/Tabs/Air/Tabs", "Air Idle")]

        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Air Idle/Tabs", "Can Grind")] public PlayerStates PSAir_CanGrind;
        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Air Idle/Tabs", "Can't Grind")] public PlayerStates PSAir_CantGrind;

        [TabGroup("States/StateSettings/Tabs/Air/Tabs", "Air Trick Settings")]

        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Air Trick Settings/Tabs", "Trick Timers"), LabelText("Trick 1 Timer"), Range(0.01f, 2f)] public float timerPrepareTrick1;
        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Air Trick Settings/Tabs", "Trick Timers"), LabelText("Trick 2 Timer"), Range(0.01f, 2f)] public float timerPrepareTrick2;
        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Air Trick Settings/Tabs", "Trick Timers"), LabelText("Trick 2 Timer Prep State"), Range(0.01f, 2f)] public float timerPrepareStateTrick2;

        [TabGroup("States/StateSettings/Tabs/Air/Tabs", "Prepareing Air Tricks")]

        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Prepareing Air Tricks/Tabs", "Trick 1")] public PlayerStates PSPrepareAirTrick1;
        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Prepareing Air Tricks/Tabs", "Trick 2")] public PlayerStates PSPrepareAirTrick2;

        [TabGroup("States/StateSettings/Tabs/Air/Tabs", "Doing Air Tricks")]

        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Doing Air Tricks/Tabs", "Trick 1")] public PlayerStates PSAirTrick1;
        [TabGroup("States/StateSettings/Tabs/Air/Tabs/Doing Air Tricks/Tabs", "Trick 2")] public PlayerStates PSAirTrick2;

        [TabGroup("States/StateSettings/Tabs", "Grind")] public PlayerStates PSGrind;

        [TabGroup("States/StateSettings/Tabs", "Fall")]

        [TabGroup("States/StateSettings/Tabs/Fall/Tabs", "Fall Settings"), LabelText("GameOver Screen")] public GameObject GameOverScreen;
        [TabGroup("States/StateSettings/Tabs/Fall/Tabs", "Fall Settings"), LabelText("PauseBtn (DisableOnDeath)")] public GameObject PauseButton;
        [TabGroup("States/StateSettings/Tabs/Fall/Tabs", "Fall")] public PlayerStates PSFall;
        #endregion

        #region Variables not to be seen or changed.
        private int trickLoadCounter;
        private float currentTimerPrepareTrick1;
        private float currentTimerPrepareTrick2;
        private float currentTimerPrepareStateTrick2;
        private bool KeyComeUp;
        #endregion

        void Awake()
        {
            trickLoadCounter = 0;
            KeyComeUp = false;
            currentTimerPrepareTrick1 = timerPrepareTrick1;
            currentTimerPrepareTrick2 = timerPrepareTrick2;
            currentTimerPrepareStateTrick2 = timerPrepareStateTrick2;

            //Can use this code to reset dictionary.
            //PlayerStateDictionary.Clear(); <<<----------------------------<<<-------------- OLD SCRIPT NOW DISABLED

            PlayerStateDictionary.Add(States.Grounded_SpaceUp, PSGrounded_SpaceUp);
            PlayerStateDictionary.Add(States.Grounded_SpaceDown, PSGrounded_SpaceDown);
            PlayerStateDictionary.Add(States.Air_CanGrind, PSAir_CanGrind);
            PlayerStateDictionary.Add(States.Air_CantGrind, PSAir_CantGrind);
            PlayerStateDictionary.Add(States.Jump, PSJump);
            PlayerStateDictionary.Add(States.PrepareAirTrick1, PSPrepareAirTrick1);
            PlayerStateDictionary.Add(States.PrepareAirTrick2, PSPrepareAirTrick2);
            PlayerStateDictionary.Add(States.AirTrick1, PSAirTrick1);
            PlayerStateDictionary.Add(States.AirTrick2, PSAirTrick2);
            PlayerStateDictionary.Add(States.Grind, PSGrind);
            PlayerStateDictionary.Add(States.Fall, PSFall);
        }

        void Start()
        {
            myPlayerControls.rb.velocity = new Vector2(startSpeed, 0);
            myPlayerControls.startGame(States.Air_CantGrind);
            NextState();
            StartCoroutine(CheckToFall());
        }

        //Plz dont judge me.
        void Update()
        {
            // The "current" variables are the current ones AT THIS second.
            // They will be checked in the state machine to check which state to go into.
            currentTimerPrepareTrick1 = timerPrepareTrick1;
            currentTimerPrepareTrick2 = timerPrepareTrick2;
            currentTimerPrepareStateTrick2 = timerPrepareStateTrick2;

            // This basically is how long after the first press down before the second one needs to be pressed to move into
            // PrepareAirTrick1 or it will just stay in this state and reset timer.
            if (currentTimerPrepareTrick1 < timerPrepareTrick1)
            {
                currentTimerPrepareTrick1 += Time.deltaTime;
            }
            else
            {
                currentTimerPrepareTrick1 = timerPrepareTrick1;
            }

            // This basically is how long after the first second press down before the third one needs to be pressed to move into
            // PrepareAirTrick2 or it will just stay in this state and reset timer.
            if (currentTimerPrepareTrick2 < timerPrepareTrick2)
            {
                currentTimerPrepareTrick2 += Time.deltaTime;
            }
            else
            {
                currentTimerPrepareTrick2 = timerPrepareTrick2;
            }
            
            // This basically is how long after the third press will the player do the second trick.
            // PrepareAirTrick2 or it will just stay in this state and reset timer.
            if (currentTimerPrepareStateTrick2 < timerPrepareStateTrick2)
            {
                currentTimerPrepareStateTrick2 += Time.deltaTime;
            }
            else
            {
                currentTimerPrepareStateTrick2 = timerPrepareStateTrick2;
            }

            // Basically only resets if ALL timers expire.
            // Trust me logically this works.
            if (currentTimerPrepareStateTrick2 >= timerPrepareStateTrick2 && currentTimerPrepareTrick2 >= timerPrepareTrick2 && currentTimerPrepareTrick1 >= timerPrepareTrick1)
            {
                trickLoadCounter = 0;
                KeyComeUp = false;
            }
        }

        // These will be referenced in the animation itself.
        #region Animator Scripts.

        // When the jump animation ends.
        public void ExitJump()
        {
            switch (trickLoadCounter)
            {
                case 0:
                    if (Input.GetKey(KeyCode.Space) == false)
                    {
                        myPlayerControls.entredNewState(States.Air_CantGrind);
                    }
                    else
                    {
                        myPlayerControls.entredNewState(States.Air_CanGrind);
                        trickLoadCounter++;
                        currentTimerPrepareTrick1 = 0f;
                    }
                    break;
                case 1:
                    if (Input.GetKey(KeyCode.Space) == false)
                    {
                        myPlayerControls.entredNewState(States.Air_CantGrind);
                    }
                    else
                    {
                        myPlayerControls.entredNewState(States.PrepareAirTrick1);
                        trickLoadCounter++;
                        currentTimerPrepareTrick2 = 0f;
                    }
                    break;
                case 2:
                    if (Input.GetKey(KeyCode.Space) == false)
                    {
                        myPlayerControls.entredNewState(States.PrepareAirTrick1);
                    }
                    else
                    {
                        myPlayerControls.entredNewState(States.PrepareAirTrick2);
                        trickLoadCounter++;
                        currentTimerPrepareStateTrick2 = 0f;
                    }
                    break;
                default:
                    currentTimerPrepareTrick2 = 0;
                    break;
            }
        }

        public IEnumerator FallOver()
        {

            myPlayerControls.entredNewState(States.Fall);

            yield return new WaitForSeconds(1);

            // Enable Game Over Screen.
            GameOverScreen.SetActive(true);

            // Pause Pause Button to disable.
            PauseButton.SetActive(false);
        }

        #endregion


        #region State Machine.
        // It will constantly check if the Player stops moving.
        // This will also allow me force the player "rb" velocity to move ....
        // .... I think....
        IEnumerator CheckToFall()
        {
            // Enter State
            Debug.Log("Start CheckToFall Coroutine");

            myPlayerControls.rb.velocity = new Vector2(startSpeed, 0);

            // Start State Machine.
            while (myPlayerControls.rb.velocity.x > 0)
            {
                yield return 0; // Wait until next frame.
            }
            myPlayerControls.entredNewState(States.Fall);
            StartCoroutine(FallOver());

            NextState();
            Debug.Log("Exit CheckToFall Coroutine");
        }

        // Grounded Space SpaceUp.
        IEnumerator SMGrounded_SpaceUp()
        {
            // Enter State.
            Debug.Log("Entered Grounded_SpaceUp State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.Grounded_SpaceUp)
            {
                // This is my update loop for this state.

                // If we press down Space.
                // Move to Ground - Space Down
                if (myPlayerControls.grounded && Input.GetKey(KeyCode.Space))
                {
                    myPlayerControls.entredNewState(States.Grounded_SpaceDown);
                }

                // If we go into the air (e.g. Fall off cliff) AND still not pressing down Space.
                // Move to Air - Can't Grind
                else if (!myPlayerControls.grounded) // We go into the air (e.g. Fall off cliff).
                {
                    myPlayerControls.entredNewState(States.Air_CantGrind);
                }

                yield return 0; // Wait until next frame.
            }
            // Start next state.
            NextState();
            Debug.Log("Exit Grounded_SpaceUp State");
        }

        // Grounded Space SpaceDown.
        IEnumerator Grounded_SpaceDown()
        {
            // Enter State.
            Debug.Log("Entered Grounded_SpaceDown State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.Grounded_SpaceDown)
            {
                // This is my update loop for this state.

                // If we let go of Space.
                // Move to Jump
                if (Input.GetKey(KeyCode.Space) == false)
                {
                    myPlayerControls.entredNewState(States.Jump);
                }
                // If we go into the air (e.g. Fall off cliff)
                // Move to Air - CantGrind
                else if (!myPlayerControls.grounded) // We go into the air (e.g. Fall off cliff).
                {
                    myPlayerControls.entredNewState(States.Air_CantGrind);
                }
                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit Grounded_SpaceDown State");
        }

        // FFS Kieran why did you make this soooo complicated before.
        // Jumping state
        // It should be noted we only leave this state hitting the ground OR finishing the animation.
        IEnumerator Jump()
        {
            // Enter State.
            Debug.Log("Entered Jump State");

            // Start State Machine
            while (myPlayerControls.currentState == States.Jump)
            {
                // This is my update loop for this state.

                // If we are in the air AND press down Space while still 'Jumping'.
                // Add to our Trick Counter.
                if (trickLoadCounter <= 0 && Input.GetKeyDown(KeyCode.Space))
                {
                    switch (trickLoadCounter)
                    {
                        case 1:
                            currentTimerPrepareTrick1 = 0;
                            break;
                        case 2:
                            currentTimerPrepareTrick2 = 0;
                            break;
                        default:
                            currentTimerPrepareTrick2 = 0;
                            break;
                    }
                    trickLoadCounter++;
                }
                yield return 0; // Wait until next frame. 
            }
            NextState();
            Debug.Log("Exit Jump_SpaceUp State");
        }

        IEnumerator Air_CantGrind()
        {
            // Enter State.
            Debug.Log("Entered Air_CantGrind State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.Air_CantGrind)
            {
                // This is my update loop for this state.

                // If they hit the ground.
                // Move to Ground - SpaceUp
                if (myPlayerControls.grounded)
                {
                    myPlayerControls.entredNewState(States.Grounded_SpaceUp);
                }

                // If in the air AND Space goes down and first time.
                // Move to Air - Space Down
                else if (Input.GetKey(KeyCode.Space) && trickLoadCounter < 1)
                {
                    myPlayerControls.entredNewState(States.Air_CanGrind);
                    trickLoadCounter++;
                    currentTimerPrepareTrick1 = 0f;
                }

                // If in the air AND space goes down second + time.
                // Move to PrepareAirTrick1
                else if (Input.GetKey(KeyCode.Space) && trickLoadCounter == 1 && currentTimerPrepareTrick1 < timerPrepareTrick1)
                {
                    myPlayerControls.entredNewState(States.PrepareAirTrick1);
                    trickLoadCounter++;
                    currentTimerPrepareTrick2 = 0f;
                }
                // If in the air AND space goes down third + time.
                // Move to PrepareAirTrick2
                else if (Input.GetKey(KeyCode.Space) && trickLoadCounter >= 2 && currentTimerPrepareTrick2 < timerPrepareTrick2)
                {
                    myPlayerControls.entredNewState(States.PrepareAirTrick2);
                    trickLoadCounter++;
                    currentTimerPrepareStateTrick2 = 0f;
                }

                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit Air_CantGrind State");
        }

        IEnumerator Air_CanGrind()
        {
            // Enter State.
            Debug.Log("Entered Air_CanGrind State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.Air_CanGrind)
            {
                // This is my update loop for this state.

                // If they hit the ground.
                // Move to Ground - SpaceUp
                if (myPlayerControls.grounded)
                {
                    myPlayerControls.entredNewState(States.Grounded_SpaceUp);
                }

                // If Player is Grinding.
                // Move to Grind
                else if (myPlayerControls.grinding)
                {
                    myPlayerControls.entredNewState(States.Grind);
                }

                // If in the air AND Space goes up.
                // Move to Air - CantGrind
                else if (Input.GetKey(KeyCode.Space) == false)
                {
                    myPlayerControls.entredNewState(States.Air_CantGrind);
                }
                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit Air_CanGrind State");
        }

        IEnumerator PrepareAirTrick1()
        {
            // Enter State.
            Debug.Log("Entered PrepareAirTrick1");

            // Start State Machine.
            while (myPlayerControls.currentState == States.PrepareAirTrick1)
            {
                // If hits the ground
                // Move to Ground - Space Up
                if (myPlayerControls.grounded)
                {
                    myPlayerControls.entredNewState(States.Grounded_SpaceUp);
                }

                // If in the air AND Timer Runs Out.
                // Move to Air Trick 1 Load - Space Up
                else if (currentTimerPrepareTrick2 >= timerPrepareTrick2)
                {
                    myPlayerControls.entredNewState(States.AirTrick1);
                }

                // Move to Air Trick 1 Load - Space Up
                else if (Input.GetKey(KeyCode.Space) == false && !KeyComeUp)
                {
                    KeyComeUp = true;
                }
                else if(Input.GetKeyUp(KeyCode.Space) && KeyComeUp)
                {
                    myPlayerControls.entredNewState(States.PrepareAirTrick2);
                    trickLoadCounter++;
                    currentTimerPrepareStateTrick2 = 0f;
                }

                yield return 0; // Wait until next frame.
            }
            KeyComeUp = true;
            NextState();
            Debug.Log("Exit PrepareAirTrick1 State");
        }

        IEnumerator PrepareAirTrick2()
        {
            // Enter State.
            Debug.Log("Entered PrepareAirTrick2");

            // Start State Machine.
            while (myPlayerControls.currentState == States.PrepareAirTrick2)
            {
                // If hits the ground
                // Move to Ground - Space Up
                if (myPlayerControls.grounded)
                {
                    myPlayerControls.entredNewState(States.Grounded_SpaceUp);
                }
                else if (currentTimerPrepareStateTrick2 >= timerPrepareStateTrick2)
                {
                    myPlayerControls.entredNewState(States.AirTrick2);
                }
                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit PrepareAirTrick2 State");
        }

        IEnumerator AirTrick1()
        {
            // Enter State.
            Debug.Log("Entered AirTrick1 State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.AirTrick1)
            {
                // If we touch the ground WHILE still doing the trick we will fall over.
                // Move to Fall
                if (myPlayerControls.grounded)
                {
                    StartCoroutine(FallOver());
                }
                // It should be noted at the end of the trick the player will go into the normal air state.
                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit AirTrick1 State");
        }

        // -----------------------------------------------
        // --------------------- !!! ---------------------
        // -------------- FOR FUTURE KIERAN --------------
        // --------------------- !!! ---------------------
        // -----------------------------------------------
        // You might now notice that this state looks 
        // very similar to "AirTrick1". While you would be
        // right in assuming they are nearly identical, it
        // should be noted that having them in 2 SEPARATE
        // STATES means that you can give them different 
        // points or in this case accelleration (check the 
        // inspector).
        //      Even though I haven't implemented a points
        // based system yet this would be easier by just
        // checking the state the player was in and if the
        // trick gave points and if adding inital points
        // as a variable in each trick state (or maybe all
        // states but keeping the "normal" states as 0 or 
        // reseting the combo or stack.
        // -------------- 1/06/2021 10:28pm --------------
        // -----------------------------------------------

        IEnumerator AirTrick2()
        {
            // Enter State.
            Debug.Log("Entered AirTrick2 State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.AirTrick2)
            {
                // If we touch the ground WHILE still doing the trick we will fall over.
                // Move to Fall
                if (myPlayerControls.grounded)
                {
                    StartCoroutine(FallOver());
                }
                // It should be noted at the end of the trick the player will go into the normal air state.

                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit AirTrick2 State");
        }

        // Yea this is the Grinding State.
        // This will probs be done by if the player can grind and "Enters" a rail collider
        // --------------------------<<<<<<<<<<<<<<<<----------------------------<<<<<<<<<<<<<<<<<------------------- YOU MIGHT NEED TO ADD GROUNDED TO ALL THE RELEVENT STATES
        // Or atleast to when they "collide with rail"
        IEnumerator Grind()
        {
            // Enter State.
            Debug.Log("Entered Grind State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.Grind)
            {
                // This is my update loop for this state.

                // If the player lets go of the space key.
                // Move to Jump
                if (Input.GetKey(KeyCode.Space) == false)
                {
                    myPlayerControls.entredNewState(States.Jump);
                }
                yield return 0; // Wait until next frame.
            }
            NextState();
            Debug.Log("Exit Grind State");
        }

        IEnumerator Fall()
        {
            // Enter State.
            Debug.Log("Entered Fall State");

            // Start State Machine.
            while (myPlayerControls.currentState == States.Fall)
            {
                // Will wait for 10 seconds.
                // This is mainly in place incase I want to run something later on.
                yield return new WaitForSeconds(10);
            }
            NextState();
            Debug.Log("Exit AirTrick2 State");
        }
        
        private void NextState()
        {
            switch (myPlayerControls.currentState)
            {
                case States.Grounded_SpaceUp:
                    StartCoroutine(SMGrounded_SpaceUp());
                    break;
                case States.Grounded_SpaceDown:
                    StartCoroutine(Grounded_SpaceDown());
                    break;
                case States.Jump:
                    StartCoroutine(Jump());
                    break;
                case States.Air_CantGrind:
                    StartCoroutine(Air_CantGrind());
                    break;
                case States.Air_CanGrind:
                    StartCoroutine(Air_CanGrind());
                    break;
                case States.PrepareAirTrick1:
                    StartCoroutine(PrepareAirTrick1());
                    break;
                case States.PrepareAirTrick2:
                    StartCoroutine(PrepareAirTrick2());
                    break;
                case States.AirTrick1:
                    StartCoroutine(AirTrick1());
                    break;
                case States.AirTrick2:
                    StartCoroutine(AirTrick2());
                    break;
                case States.Grind:
                    StartCoroutine(Grind());
                    break;
                case States.Fall:
                    StartCoroutine(Fall());
                    break;
                default:
                    break;
            }
        }
        #endregion
    }

    public enum States
    {
        Grounded_SpaceUp,
        Grounded_SpaceDown,
        Test_State,
        Air_CanGrind,
        Air_CantGrind,
        Jump,
        PrepareAirTrick1,
        PrepareAirTrick2,
        AirTrick1,
        AirTrick2,
        Grind,
        Fall
    };
    public enum SpaceBarStates
    {
        Up, Down, Either
    };

    // Class for all the player states

    [HideReferenceObjectPicker]
    public class PlayerStates
    {
        public States stateName;
        [SerializeField]
        private Animator playerAnimator;
        public AnimationClip stateAnimation;
        public GameObject allPlayerStateCollidersFolder;
        public List<GameObject> listOfStateColliders;
        public float topSpeed;
        [HorizontalGroup("Group Accelleration and Deceleration", 0.5f, LabelWidth = 100f)]
        public float acelleration, decelleration;

        [HorizontalGroup("Group Booleans", 0.5f, LabelWidth = 65f)]
        public bool canGrind, canFall;

        [EnumToggleButtons]
        [LabelText("Space Bar")]
        public SpaceBarStates SpaceBarState;
    }
}