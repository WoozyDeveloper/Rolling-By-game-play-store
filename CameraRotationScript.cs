using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationScript : MonoBehaviour
{
    //TODO see usage


    // Start is called before the first frame update
    void Start()
    {
        Vector3 rotationVector = new Vector3(0, 30, 0);
        Quaternion rotation = Quaternion.Euler(rotationVector);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.zero, 10.0f * Time.deltaTime);
    }

   
}
