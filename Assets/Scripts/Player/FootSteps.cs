using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody _rigidboty;
    public float footstepThreshold;
    public float footstepRate;
    private float footstepTime;

    private void Start()
    {
        _rigidboty = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Mathf.Abs(_rigidboty.velocity.y) < 0.1f)
        {
            if (_rigidboty.velocity.magnitude > footstepThreshold)
            {
                if (Time.time - footstepTime > footstepRate)
                {
                    footstepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
                }
            }
        }
    }
}
