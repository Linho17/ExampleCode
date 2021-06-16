using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StatusSwipe
{
    None,
    SliceUP,
    SliceDown,
    HoldUp,
    HoldDown
};

public class Swipe : MonoBehaviour
{

    [SerializeField] private float minSwipeDistance;
    [SerializeField] private float MaxSwipeDuration;

    private float timeSwipeStart;
    private float timeSwipeEnd;
    private float swipeTime;
    

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipeLength;

    
    public StatusSwipe TouchReader()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                timeSwipeStart = Time.time;
                startSwipePosition = touch.position;
            }

            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                
                endSwipePosition = touch.position;
                swipeLength = (endSwipePosition - startSwipePosition).magnitude;
                
                if (swipeLength > minSwipeDistance)
                {
                    if ((endSwipePosition - startSwipePosition).y > 0)
                    {
                        return StatusSwipe.HoldUp;
                    }
                    else
                    {
                        return StatusSwipe.HoldDown;
                    }
                    
                }

            }

            else if(touch.phase == TouchPhase.Ended)
            {

                timeSwipeEnd = Time.time;
                swipeTime = timeSwipeEnd - timeSwipeStart;

                endSwipePosition = touch.position;
                swipeLength = (endSwipePosition - startSwipePosition).magnitude;

                if (swipeTime < MaxSwipeDuration && swipeLength > minSwipeDistance)
                {
                    if ((endSwipePosition - startSwipePosition).y > 0)
                    {
                        return StatusSwipe.SliceUP;
                    }
                    else
                    {
                        return StatusSwipe.SliceDown;
                    }
                }
            }
        }

        return StatusSwipe.None;

    }
}
