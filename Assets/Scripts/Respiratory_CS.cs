using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Respiratory_CS : MonoBehaviour
{
    public GameObject Combine_Button;
    public GameObject Divide_Button;
    public GameObject Background_Box;
    GameObject[] Tank_Parts;
    Vector3 Ref = Vector3.zero;
    Vector3 Initial_location;
    Quaternion Initial_Rotation;
    int Counter = 0;
    public GameObject[] a;
    public GameObject b;
    public Main w;
    public GameObject[] c;
    public bool a1_;
    public bool s;
    public bool d;
    public bool f;
    public bool g;
    public Sprite asdfempty;
    public Sprite asdfstart;
    Quaternion initrot;
    private Vector3 initpos;
    private void Awake()
    {
        Tank_Parts = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Tank_Parts[i] = transform.GetChild(i).gameObject;
            Tank_Parts[i].GetComponent<Draggable>().enabled = false;
        }
    }

    void Start()
    {
        Tank_Parts = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Tank_Parts[i] = this.transform.GetChild(i).gameObject;
        }
        
        Combine_Button.GetComponent<EventTrigger>().enabled = false;
        Combine_Button.GetComponent<Image>().sprite = asdfempty;

    }
    public void 위치저장()
    {
        Initial_location = this.transform.position;
        Initial_Rotation = this.transform.rotation;

        initrot = transform.rotation;
        initpos = transform.position;
    }

    public void Divide()
    {
        a1_ = true;
        for (int i = 0; i < a.Length; i++)
        {
            a[i].SetActive(true);

        }
        gameObject.GetComponent<Animation>().enabled = false;
        if ((this.transform.position - Initial_location).magnitude > 0.001 || Quaternion.Angle(transform.rotation, Initial_Rotation) > 0.1)
        {
            StartCoroutine(Normalize());
        }
        GetComponent<Draggable>().enabled = false;
        Divide_Button.GetComponent<EventTrigger>().enabled = false;
        for (int i = 0; i < a.Length; i++)
        {
            a[i].GetComponent<EventTrigger>().enabled = false;

        }

        b.GetComponent<EventTrigger>().enabled = false;
        Divide_Button.GetComponent<Image>().sprite = asdfempty;
        foreach (GameObject A in Tank_Parts)
        {
            A.GetComponent<Respiratory_Part_CS>().StopAllCoroutines();
            A.GetComponent<MeshCollider>().enabled = false;
            if (A.name == "8")
            {
                A.GetComponent<Respiratory_Part_CS>().StartCoroutine("Wheel_Move");
            }
            else
            {
                A.GetComponent<Respiratory_Part_CS>().StartCoroutine("Divide_Move");
            }

            A.GetComponent<Draggable>().enabled = false;
        }
    }

    public void Divide_End()
    {
        Counter++;
        if (Counter >= 1)
        {
            Counter = 0;
            Combine_Button.GetComponent<EventTrigger>().enabled = true;
            Combine_Button.GetComponent<Image>().sprite = asdfstart;
            foreach (GameObject A in Tank_Parts)
            {
                A.GetComponent<Draggable>().enabled = true;
                A.GetComponent<MeshCollider>().enabled = true;
            }
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = true;
            }
            for (int i = 0; i < c.Length; i++)
            {
                c[i].GetComponent<EventTrigger>().enabled = true;
                c[i].GetComponent<Respiratory_Part_CS>().StartBT();
            }
            b.GetComponent<EventTrigger>().enabled = true;

        }
    }

    public void Combine()
    {
        a1_ = false;
        for (int i = 0; i < a.Length; i++)
        {
            a[i].SetActive(true);
        }
        Combine_Button.GetComponent<EventTrigger>().enabled = false;
        Combine_Button.GetComponent<Image>().sprite = asdfempty;
        for (int i = 0; i < a.Length; i++)
        {
            a[i].GetComponent<EventTrigger>().enabled = false;
        }
        foreach (GameObject A in Tank_Parts)
        {
            A.GetComponent<Respiratory_Part_CS>().StopAllCoroutines();
            A.GetComponent<Draggable>().enabled = false;
            A.GetComponent<MeshCollider>().enabled = false;
            A.GetComponent<Respiratory_Part_CS>().StartCoroutine("Combine_Move");


        }
        for (int i = 0; i < c.Length; i++)
        {
            c[i].GetComponent<EventTrigger>().enabled = false;
        }
    }

    public void Combine_End()
    {
        int Count = w.count;
        Counter++;
        if (Counter >= 1)
        {
            Counter = 0;
            foreach (GameObject A in Tank_Parts)
            {
                A.GetComponent<MeshCollider>().enabled = true;
            }

            Divide_Button.GetComponent<EventTrigger>().enabled = true;
            Divide_Button.GetComponent<Image>().sprite = asdfstart;
            gameObject.GetComponent<Animation>().enabled = true;
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = true;
            }



            GetComponent<Draggable>().enabled = true;


        }
    }

    public void option()
    {

        if (a1_ == false)
        {
            transform.position = initpos;
            transform.rotation = initrot;
        }
        if (a1_ == true)
        {
            Combine();
        }

    }

    IEnumerator Normalize()
    {
        while ((this.transform.position - Initial_location).magnitude > 0.001 || Quaternion.Angle(transform.rotation, Initial_Rotation) > 0.1)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, Initial_location, ref Ref, 0.2f);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Initial_Rotation, 8 * Time.deltaTime);
            yield return null;
        }
        this.transform.position = Initial_location;
        this.transform.rotation = Initial_Rotation;
    }

    public void ResePos()
    {
        if (a1_ == false)
        {
            transform.position = initpos;
            transform.rotation = initrot;

        }
        if (a1_ == true)
        {
            for (int i = 0; i < c.Length; i++)
            {
                c[i].GetComponent<Respiratory_Part_CS>().ResetPos();
            }
        }

    }
}
