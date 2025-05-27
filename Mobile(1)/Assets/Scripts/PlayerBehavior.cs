using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehavior : MonoBehaviour
{

    /// <summary>
    /// A reference to the RigidBody component TEST 1
    /// </summary>
    private Rigidbody rb;

    // SET SPEED TO THIS WHEN TESTING NORMAL GAME 
    [Tooltip("How fast the ball moves left/right manually")]
    public float dodgeSpeed = 5;

    // SET SPEED TO THIS WHEN TESTING NEW ADDITION
    //public float dodgeSpeed = 7.5f;

    [Tooltip("How fast the ball moves forward automatically")]
    [Range(0, 10)]
    public float rollSpeed = 5;

    public enum MobileHorizMovement
    {
        Accelerometer, ScreenTouch
    }

    public MobileHorizMovement horizMovement = MobileHorizMovement.Accelerometer;

    [Header("Swipe Properties")]
    [Tooltip("How far will the player move upon swiping")]
    public float swipeMove = 2f;

    [Tooltip("How far must the player swipe before we will execute the action (in inches)")]
    public float minSwipeDistance = 0.25f;


    [Header("Scaling Properties")]
    [Tooltip("Min size for player")]
    public float minScale = 0.5f;

    [Tooltip("Max size for player")]
    public float maxScale = 3.0f;

    private float currentScale = 1f;



    private float minSwipeDistancePixels;

    private Vector2 touchStart;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        minSwipeDistancePixels = minSwipeDistance * Screen.dpi;
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

        if (Input.GetMouseButton(0))
        {
            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                TouchObjects(screenPos, touch);
            }
            else
            {
                TouchObjects(screenPos);
            }
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            TouchObjects(touch.position);
            SwipeTeleport(touch);
            ScalePlayer();
        }
#elif UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            TouchObjects(touch.position);
            SwipeTeleport(touch);
            ScalePlayer();
        }
        
#endif
    }

    /// <summary>
    /// prime place to put physics calc happening over a period of time
    /// </summary>
    void FixedUpdate()
    {
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR

        if (Input.GetMouseButton(0))
        {
            var screenPos = Input.mousePosition;
            horizontalSpeed = CalculateMovement(screenPos);
        }

        if (horizMovement == MobileHorizMovement.Accelerometer)
        {
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }

        if (Input.touchCount > 0)
        {

            if (horizMovement == MobileHorizMovement.ScreenTouch)
            {
                Touch touch = Input.touches[0];

                SwipeTeleport(touch);
                ScalePlayer();
            }

            /*
            Touch touch = Input.touches[0];

            SwipeTeleport(touch);
            ScalePlayer();
            */
        }
#elif UNITY_IOS || UNITY_ANDROID

        if (horizMovement == MobileHorizMovement.Accelerometer)
        {
            horizontalSpeed = Input.acceleration.x * dodgeSpeed;
        }
        
        if (Input.touchCount > 0)
        {

            if (horizMovement == MobileHorizMovement.ScreenTouch)
            {
                Touch touch = Input.touches[0];

                SwipeTeleport(touch);
                ScalePlayer();
            }
            
            //var firstTouch = Input.touches[0];
            //var screenPos = firstTouch.position;
            //horizontalSpeed = CalculateMovement(screenPos);
        }
        
#endif

        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }

    private float CalculateMovement(Vector3 screenPos)
    {
        var cam = Camera.main;

        var viewPos = cam.ScreenToViewportPoint(screenPos);

        float xMove = 0;

        if (viewPos.x < 0.5f)
        {
            xMove = -1;
        }
        else
        {
            xMove = 1;
        }

        return xMove * dodgeSpeed;
    }


    /// <summary>
    /// aaa
    /// </summary>
    /// <param name="touch">Current touch event</param>
    private void SwipeTeleport(Touch touch)
    {
        //test = 2;
        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
            //test++;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            //test = 4;
            Vector2 touchEnd = touch.position;

            float x = touchEnd.x - touchStart.x;

            if (Mathf.Abs(x) < minSwipeDistancePixels)
            {
                return;
            }

            Vector3 moveDirection;

            if (x < 0)
            {
                moveDirection = Vector3.left;
            }
            else
            {
                moveDirection = Vector3.right;
            }

            RaycastHit hit;

            if (!rb.SweepTest(moveDirection, out hit, swipeMove))
            {
                var movement = moveDirection * swipeMove;
                var newPos = rb.position + movement;

                rb.MovePosition(newPos);

                test = 10;
            }
        }
    }

    public int test = 1;

    private void ScalePlayer()
    {
        if (Input.touchCount != 2)
        {
            return;
        }
        else
        {
            Touch touch0 = Input.touches[0];
            Touch touch1 = Input.touches[1];

            Vector2 t0Pos = touch0.position;
            Vector2 t0Delta = touch0.deltaPosition;
            
            Vector2 t1Pos = touch1.position;
            Vector2 t1Delta = touch1.deltaPosition;

            Vector2 t0Prev = t0Pos - t0Delta;
            Vector2 t1Prev = t1Pos - t1Delta;

            float prevTDeltaMag = (t0Prev - t1Prev).magnitude;
            float tDeltaMag = (t0Pos - t1Pos).magnitude;

            float deltaMagDiff = prevTDeltaMag - tDeltaMag;

            float newScale = currentScale;
            newScale -= (deltaMagDiff * Time.deltaTime);

            newScale = Mathf.Clamp(newScale, minScale, maxScale);

            transform.localScale = Vector3.one * newScale;

            currentScale = newScale;
        }
    }


    /// <param name="screenPos">position of touch in screen space</param>
    private static void TouchObjects(Vector2 screenPos)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        int layerMask = ~0;

        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (Input.touchCount == 0)
            {
                hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
            }

            //hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }

    //Newly added test
    private static void TouchObjects(Vector2 screenPos, Touch touch)
    {
        Ray touchRay = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        int layerMask = ~0;

        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (touch.phase == TouchPhase.Began)
            {
                hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
            }

            //hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }
    }



    /// <param name="touch">our touch event</param>
    private static void TouchObjects(Touch touch)
    {

        if (Input.touchCount == 1)
        {
            Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            int layerMask = ~0;

            if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
                }

                hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
            }
        }
        
        /*Ray touchRay = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        int layerMask = ~0;

        if (Physics.Raycast(touchRay, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            hit.transform.SendMessage("PlayerTouch", SendMessageOptions.DontRequireReceiver);
        }*/
    }
}
