using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{

    [SerializeField] private AudioSource introMusic;
    // Start is called before the first frame update
    void Start()
    {
        introMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
