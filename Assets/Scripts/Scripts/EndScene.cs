using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    [SerializeField] private GameObject animatedRobot;
    [SerializeField] private AudioSource robotCompletion;

    private Animation anim;
    
    // Start is called before the first frame update
    void Start()
    {
        var animatedStatuePosition = new Vector3(0, 0, 0);
        Instantiate(animatedRobot, animatedStatuePosition, Quaternion.identity);
        anim = animatedRobot.GetComponent<Animation>();
        anim.Play("thisOne");
        robotCompletion.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
