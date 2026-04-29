using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public GameObject ObjectToFollow;
    private Vector3 offset;
    private float smooth = 0.15f;
    private Vector3 velocity = Vector3.zero;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SetTargetToDefault();
        offset = transform.position - ObjectToFollow.transform.position;
    }
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, ObjectToFollow.transform.position + offset, ref velocity, smooth);
    }
    public void SetNewTarget(GameObject newTarget)
    {
        ObjectToFollow = newTarget;
    }
    public void SetTargetToDefault()
    {
        ObjectToFollow = CharacterTracker.Instance.player.gameObject;
    }
}
