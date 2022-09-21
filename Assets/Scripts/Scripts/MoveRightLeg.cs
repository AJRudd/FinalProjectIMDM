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

public class MoveRightLeg : MonoBehaviour
{

    [SerializeField] private AudioSource attractionSoundFour;
    new Camera camera;
    
    private bool _holdObject;
    
    private float movementSpeed = .001f;
    private int _prevId, _currId;
    
    private Vector2 _prevPosition, _currPosition;
    
    private GameObject _robotTwo;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("ARNetworkingSceneManager/ARSceneCamera").GetComponent<Camera>();
        _holdObject = false;
        EnhancedTouchSupport.Enable();
        _prevId = -1;
        _currId = -1;
        attractionSoundFour.Play();
    }
    bool TryGetTouchDrag(out Vector2 touchPosition, ref int touchId) {
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
            Destroy(_robotTwo.gameObject);
        }
    }

    void GrabObject()
    {
       
        if (!TryGetTouchDrag(out _currPosition, ref _currId) || _prevId != _currId || _robotTwo == null)
        {
            if (_prevId != _currId)
            {
                Debug.Log("Word");
                _robotTwo = HitObject(_currPosition);
                Debug.Log("robot: " + _robotTwo);
            }
            else
            {
                _robotTwo = null;
            }
            
            _prevId = _currId;
            _prevPosition = _currPosition;
            return;
        }

        Vector2 diffPosition = _currPosition - _prevPosition;
        Vector3 movement = new Vector3(diffPosition.x, 0, diffPosition.y);
        Vector3 angles = transform.rotation.eulerAngles;
        movement = Quaternion.Euler(0, angles.y, 0) * movement;
        _robotTwo.transform.Translate(movement * movementSpeed * Time.deltaTime);
    }

    GameObject HitObject(Vector2 touchPosition)
    {
        Debug.Log("Chu");
        Ray ray = camera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Boozle: " + hit.transform.name);
            if (hit.transform.name.Contains("rightLegParent"))
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
    }

    //Update is called once per frame
    void Update()
    {
        GrabObject();
    }
}
