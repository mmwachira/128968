using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhysicsPickup : MonoBehaviour
{
    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Transform PickupTarget;
    [Space]
    [SerializeField] private float PickupRange;
    private Rigidbody CurrentObject;
    public Transform Player;
    public bool picked;
    public GameObject updatetext;
    public TMP_Text updateText;

    // Start is called before the first frame update
    void Start()
    {
        picked = false;
    }

    

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown("e"))
        {
            if(CurrentObject)
            {
                CurrentObject.useGravity = true;
                CurrentObject = null;
                return;
            }

            Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            if(Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
            {
                CurrentObject = HitInfo.rigidbody;
                CurrentObject.useGravity = false;
            }
        }
    }

    // void OnMouseOver()
    // {
    //   {
    //           if (Player)
	// 	  		{
	// 	  			float dist = Vector3.Distance(Player.position, transform.position);
	// 	  			if (dist < 3)
	// 	  			{
    //                       if (picked == false)
    //                       {
    //                         updatetext.SetActive(true);
	// 	  				    updateText.text = "Press [E] to pick up object";
    //                       }  
							
	// 	  			}

	// 	  			if (dist > 3)
	// 	  			{
    //                       if(picked == false || picked == true)
    //                       {
    //                         updatetext.SetActive(false);
	// 	  				    updateText.text = "";
    //                       }
						
	// 	  			}
    //             }
    //     }  
    // }

    void FixedUpdate()
    {
        if (Player)
		  		{
		  			float dist = Vector3.Distance(Player.position, transform.position);
		  			if (dist < 2)
		  			{
                          if (picked == false)
                          {
                            updatetext.SetActive(true);
		  				    updateText.text = "Press [E] to pick up object";
                          }  
							
		  			}

		  			if (dist > 2)
		  			{
                          if(picked == false || picked == true)
                          {
                            updatetext.SetActive(false);
		  				    updateText.text = "";
                          }
						
		  			}
                }

        if(CurrentObject)
        {
            Vector3 DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint;
        }
        
    }
}
