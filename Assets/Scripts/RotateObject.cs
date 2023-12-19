using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour
{
    GameObject Top_Parent;

    void OnMouseOver()
    {
        if (!Input.GetMouseButton(0) && Input.GetMouseButtonDown(1))
        {
            Top_Parent = transform.root.gameObject;
            StartCoroutine(Drag());
        }
    }

    IEnumerator Drag()
    {
        while (!Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            Top_Parent.transform.Rotate(Input.GetAxis("Mouse Y") * 5, Input.GetAxis("Mouse X") * -5, 0, Space.World);
            yield return null;
        }
    }
}