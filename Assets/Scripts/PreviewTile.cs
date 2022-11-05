using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewTile : MonoBehaviour
{
    public Vector3 target;
    public float smoothTime = 5f;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, smoothTime * Time.deltaTime);
    }
}
