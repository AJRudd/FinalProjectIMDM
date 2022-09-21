using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Grapeshot;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.XR.ARFoundation;
using Quaternion = UnityEngine.Quaternion;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI;
using Niantic.ARDK.Networking.HLAPI.Authority;
using Niantic.ARDK.Networking.HLAPI.Data;
using Niantic.ARDK.Networking.HLAPI.Object;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDK.Networking.HLAPI.Routing;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;

namespace Niantic.ARDKExamples.Pong
{
    
}
public class GamePlay : MonoBehaviour
{
    //[SerializeField] private AudioSource targetSound;

    //[SerializeField] private GameObject Interactable;
    [SerializeField] private GameObject leftArm;
    [SerializeField] private GameObject rightArm;
    [SerializeField] private GameObject leftLeg;
    [SerializeField] private GameObject rightLeg;
    [SerializeField] private GameObject statue;
    

    
    /*[SerializeField] new Camera camera;
    
    private bool _holdObject;
    
    private float movementSpeed = .001f;
    private int _prevId, _currId;
    
    private Vector2 _prevPosition, _currPosition;
    
    private GameObject _robot;*/


    // Start is called before the first frame update
    void Start()
    {
        var statuePosition = new Vector3(7, 0, 0);
        var itemPosition = Random.insideUnitCircle * 1;
        var secondItemPosition = Random.insideUnitSphere * 1;
        Debug.Log(statuePosition);
        Instantiate(statue, statuePosition, Quaternion.identity);
        Instantiate(leftArm, itemPosition, Quaternion.identity);
        Instantiate(rightArm, secondItemPosition, Quaternion.identity);
        //targetSound.Play();
        //_holdObject = false;
        //EnhancedTouchSupport.Enable();
        //_prevId = -1;
        //_currId = -1;
        StartCoroutine(MinuteCoroutine());
    }

    IEnumerator MinuteCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp: " + Time.time);
        yield return new WaitForSecondsRealtime(105);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        var thirdItemPosition = Random.insideUnitCircle * 5;
        var fourthItemPosition = Random.insideUnitSphere * 5;
        Instantiate(leftLeg, thirdItemPosition, Quaternion.identity);
        Instantiate(rightLeg, fourthItemPosition, Quaternion.identity);
    }
    
    
    // Set up the initial conditions

    /*bool TryGetTouchDrag(out Vector2 touchPosition, ref int touchId) {
        if (Touch.activeTouches.Count == 1 && !Touch.activeTouches[0].isTap) {
            touchPosition = Touch.activeTouches[0].screenPosition;
            touchId = Touch.activeTouches[0].touchId;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Bin")
            
        {
            Destroy(_robot.gameObject);
        }
    }

    void GrabObject()
    {
       
        if (!TryGetTouchDrag(out _currPosition, ref _currId) || _prevId != _currId || _robot == null)
        {
            if (_prevId != _currId)
            {
                Debug.Log("Word");
                _robot = HitObject(_currPosition);
                Debug.Log("robot: " + _robot);
            }
            else
            {
                _robot = null;
            }
            
            _prevId = _currId;
            _prevPosition = _currPosition;
            return;
        }

        Vector2 diffPosition = _currPosition - _prevPosition;
        Vector3 movement = new Vector3(diffPosition.x, 0, diffPosition.y);
        Vector3 angles = transform.rotation.eulerAngles;
        movement = Quaternion.Euler(0, angles.y, 0) * movement;
        _robot.transform.Translate(movement * movementSpeed * Time.deltaTime);
    }

    GameObject HitObject(Vector2 touchPosition)
    {
        Debug.Log("Chu");
        Ray ray = camera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Boozle: " + hit.transform.name);
            if (hit.transform.name.Contains("leftArmParent"))
            {
                Debug.Log("YoYoMa");
                _holdObject = true;
                if (_holdObject)
                {
                    Debug.Log("JesusIsMyHomie");
                    //targetSound.Stop();
                    return hit.transform.gameObject;
                }
                
            }
        }

        return null;
    }*/

    //Update is called once per frame
    void Update()
    {
      
}
}

