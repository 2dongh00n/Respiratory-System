using System.Collections;
using UnityEngine;

public class Respiratory_Part_CS : MonoBehaviour
{
    GameObject Tank;
    Vector3 Ref = Vector3.zero;
    Vector3 Initial_location;
    float Distance;
    Vector3 Target_Direction;
    public Vector3 Secondary_Location;

        

    void Start()
    {
        Initial_location = transform.localPosition;
        Target_Direction = (transform.localPosition - new Vector3(0, -0.1146159f, -0.377149f)).normalized;
        Distance = (transform.localPosition - new Vector3(0, -0.1146159f, -0.377149f)).magnitude;
        Tank = transform.parent.gameObject;
    }

    IEnumerator Divide_Move()
    {
        while ((transform.localPosition - Target_Direction * Distance * 2).magnitude > 0.001f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Target_Direction*Distance*2, ref Ref, 0.2f);
            this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(Vector3.zero), 8 * Time.deltaTime);
            yield return null;
        }
        this.transform.localPosition = Target_Direction * Distance * 2;
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Tank.GetComponent<Respiratory_CS>().Divide_End();
    }

    IEnumerator Wheel_Move()
    {
        while ((transform.localPosition - new Vector3(1.23f * (transform.localPosition.x / Mathf.Abs(transform.localPosition.x)), 0.14f, 0.7557463f)).magnitude > 0.001f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, new Vector3(1.23f * (transform.localPosition.x / Mathf.Abs(transform.localPosition.x)), 0.14f, 0.7557463f), ref Ref, 0.2f);
            this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(Vector3.zero), 8 * Time.deltaTime);
            yield return null;
        }
        this.transform.localPosition = new Vector3(1.23f * (transform.localPosition.x / Mathf.Abs(transform.localPosition.x)), 0.14f, 0.7557463f);
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Tank.GetComponent<Respiratory_CS>().Divide_End();
    }

    IEnumerator Combine_Move()
    {
        while ((this.transform.localPosition - Initial_location).magnitude > 0.001 || Quaternion.Angle(transform.localRotation, Quaternion.Euler(Vector3.zero)) > 0.1)
        {
            this.transform.localPosition = Vector3.SmoothDamp(this.transform.localPosition, Initial_location, ref Ref, 0.2f);
            this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(Vector3.zero), 8 * Time.deltaTime);
            yield return null;
        }
        this.transform.localPosition = Initial_location;
        this.transform.localRotation = Quaternion.Euler(Vector3.zero);
        this.GetComponent<MeshCollider>().enabled = true;
        Tank.GetComponent<Respiratory_CS>().Combine_End();
    }
    public void StartBT()
    {
        inirot = transform.rotation;
        inipos = transform.position;
    }
    public void ResetPos()
    {
        transform.position = inipos;
        transform.rotation = inirot;
    }
    Quaternion inirot;
    private Vector3 inipos;
}
