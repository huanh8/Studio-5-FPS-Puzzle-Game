using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTest : MonoBehaviour
{

    public void OpenTheDooe()
    {
        if (transform.position.y <= 3.5f)
            transform.Translate(Vector3.up * Time.deltaTime);
        else
            transform.position = new Vector3(transform.position.x, 3.5f, transform.position.z);
    }
    public void CloseTheDooe()
    {
        if (transform.position.y >= 1.5f)
            transform.Translate(Vector3.down * Time.deltaTime);
        else
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
    }
}
