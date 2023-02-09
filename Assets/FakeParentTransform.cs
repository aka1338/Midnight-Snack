using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeParentTransform : MonoBehaviour
{
    public Transform parentTransform;
    private void LateUpdate()
    {
        transform.position = parentTransform.position;
        transform.rotation = parentTransform.rotation;
    }
}
