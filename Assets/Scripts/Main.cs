using UnityEngine;
using System.Collections;
using zSpace.Core.Samples;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Main : MonoBehaviour
{
    public GameObject logo;
    public GameObject startbutton;
    public GameObject exitbutton;
    public GameObject decompositionbtn;
    public GameObject synthesisbtn;
    public GameObject sd;
    public GameObject popup;
    public GameObject pop;
    public GameObject gido;
    public GameObject rlrhkswl;
    public GameObject vP;
    public GameObject ghldrudakr;
    public GameObject[] a;
    public Text[] w;
    public GameObject quiz;
    public GameObject backbtn;
    public Image[] see;
    bool sd1;
    bool sd2;
    bool sd3;
    bool sd4;
    int noseerleh;
    int noseerlrhkswl;
    int noseevP;
    int noseeghldrurakr;
    public AudioSource bgm2;
    private Vector3 I_location;
    Quaternion I_Rotation;
    public Respiratory_CS a12;
    public int count = 0;
    public Material noseevPmat;
    public Material noseerlrhkswlmat;
    public Material noseeghldrurakrmat;
    public Material seevPmat;
    public Material seerlrhkswlmat;
    public Material seeghldrurakrmat;

    private void Awake()
    {

        
        for (int i = 0; i < a.Length; i++)
        {
            a[i].GetComponent<EventTrigger>().enabled = false;

        }

    }
    public void sta()
    {
        bgm2.GetComponent<AudioSource>().Play();

    }
    public void Startbtn()
    {
        count++;
        logo.SetActive(false);
        transform.localPosition = new Vector3(0, 0, 1.97f);
        sd.transform.localPosition = new Vector3(0, 0, 0);
        I_Rotation = transform.rotation;
        I_location = transform.position;
        sd.GetComponent<Respiratory_CS>().위치저장();
        for (int i = 0; i < a.Length; i++)
        {
            a[i].GetComponent<Respiratory_Part_CS>().StartBT();
        }
        startbutton.SetActive(false);
        exitbutton.SetActive(false);
        quiz.SetActive(false);
        decompositionbtn.SetActive(true);
        synthesisbtn.SetActive(true);
        backbtn.SetActive(true);
        popup.SetActive(true);
        for (int i = 0; i < a.Length; i++)
        {
            a[i].GetComponent<EventTrigger>().enabled = false;

        }
        see[4].GetComponent<EventTrigger>().enabled = false;
        w[4].GetComponent<EventTrigger>().enabled = false;
        w[4].GetComponent<colorchange>().bluecolor();
        see[4].GetComponent<imagecolorchange>().imagecolor();
    }
    public void back()
    {
        count--;
        if (count == 0)
        {
            transform.localPosition = new Vector3(2.23f, 0.09f, 1.97f);
            sd.GetComponent<Respiratory_CS>().Combine();
            sd.GetComponent<Respiratory_CS>().ResePos();
            
            sd.transform.localPosition = new Vector3(0, 0, 0);
            logo.SetActive(true);
            startbutton.SetActive(true);
            exitbutton.SetActive(true);
            quiz.SetActive(true);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            popup.SetActive(false);
            backbtn.SetActive(false);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<Draggable>().enabled = false;
            }



        }
        else if (count == 1)
        {

            for (int i = 0; i < a.Length; i++)
            {
                a[i].SetActive(true);
            }
            for (int i = 0; i < 4; i++)
            {
                w[i].GetComponent<colorchange>().blackcolor();
                see[i].GetComponent<imagecolorchange>().imagecolor();
            }
            w[4].GetComponent<colorchange>().bluecolor();
            see[4].GetComponent<imagecolorchange>().imagecolor();
            for (int i = 0; i < w.Length; i++)
            {
                w[i].GetComponent<EventTrigger>().enabled = true;
                see[i].GetComponent<EventTrigger>().enabled = true;
            }
            a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
            a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
            a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
            a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
            a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
            w[4].GetComponent<EventTrigger>().enabled = false;
            see[4].GetComponent<EventTrigger>().enabled = false;
            sd1 = false;
            sd2 = false;
            sd3 = false;
            sd4 = false;
            a12.s = false;
            a12.d = false;
            a12.f = false;
            a12.g = false;
            transform.position = I_location;
            transform.rotation = I_Rotation;
            noseerleh = 0;
            noseerlrhkswl = 0;
            noseevP = 0;
            noseeghldrurakr = 0;
            pop.SetActive(false);
            vP.SetActive(false);
            rlrhkswl.SetActive(false);
            gido.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(true);
            synthesisbtn.SetActive(true);
            sd.GetComponent<Respiratory_CS>().Combine();
            sd.GetComponent<Respiratory_CS>().ResePos();
        }
    }
    public void modoobogi()
    {

        for (int i = 0; i < a.Length; i++)
        {
            a[i].SetActive(true);
        }
        for (int i = 0; i < 4; i++)
        {
            w[i].GetComponent<colorchange>().blackcolor();
            see[i].GetComponent<imagecolorchange>().imagecolor();
        }
        w[4].GetComponent<colorchange>().bluecolor();
        see[4].GetComponent<imagecolorchange>().imagecolor();
        a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
        a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
        a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
        sd1 = false;
        sd2 = false;
        sd3 = false;
        sd4 = false;
        a12.s = false;
        a12.d = false;
        a12.f = false;
        a12.g = false;
        transform.position = I_location;
        transform.rotation = I_Rotation;
        noseerleh = 0;
        noseerlrhkswl = 0;
        noseevP = 0;
        noseeghldrurakr = 0;
        decompositionbtn.SetActive(true);
        synthesisbtn.SetActive(true);
        sd.transform.localPosition = new Vector3(0, 0, 0);
        count = 1;
        pop.SetActive(false);
        vP.SetActive(false);
        rlrhkswl.SetActive(false);
        gido.SetActive(false);
        ghldrudakr.SetActive(false);
        for (int i = 0; i < w.Length; i++)
        {
            w[i].GetComponent<EventTrigger>().enabled = true;
            see[i].GetComponent<EventTrigger>().enabled = true;
        }
        w[4].GetComponent<EventTrigger>().enabled = false;
        see[4].GetComponent<EventTrigger>().enabled = false;
        bool a1 = a12.a1_;
        if (a1 == false)
        {
            sd.GetComponent<Animation>().enabled = true;
            sd.GetComponent<Draggable>().enabled = true;
        }
        if (a1 == true)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = true;
                a[i].GetComponent<Respiratory_Part_CS>().ResetPos();
            }
        }
    }
    public void lung()
    {
        if (sd1 == false && sd2 == false && sd3 == false && sd4 == false)
        {
            if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
            {
                count++;
            }
            sd1 = true;
            sd2 = false;
            sd3 = false;
            sd4 = false;
            a[0].SetActive(true);
            a[1].SetActive(false);
            a[2].SetActive(true);
            a[4].SetActive(false);
            a[3].SetActive(false);
            pop.SetActive(true);
            vP.SetActive(true);
            rlrhkswl.SetActive(false);
            gido.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }

            w[0].GetComponent<colorchange>().blackcolor();
            w[1].GetComponent<colorchange>().blackcolor();
            w[3].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().bluecolor();
            w[4].GetComponent<colorchange>().blackcolor();
        }
        else
        {
            sd1 = true;
            sd2 = false;
            sd3 = false;
            sd4 = false;
            a[0].SetActive(true);
            a[1].SetActive(false);
            a[2].SetActive(true);
            a[4].SetActive(false);
            a[3].SetActive(false);
            pop.SetActive(true);
            vP.SetActive(true);
            rlrhkswl.SetActive(false);
            gido.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }
            w[0].GetComponent<colorchange>().blackcolor();
            w[1].GetComponent<colorchange>().blackcolor();
            w[3].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().bluecolor();
            w[4].GetComponent<colorchange>().blackcolor();
        }
        see[0].GetComponent<imagecolorchange>().imageblackcolor();
        see[1].GetComponent<imagecolorchange>().imageblackcolor();
        see[2].GetComponent<imagecolorchange>().imagecolor();
        see[3].GetComponent<imagecolorchange>().imageblackcolor();
        see[4].GetComponent<imagecolorchange>().imageblackcolor();
        for (int i = 0; i < w.Length; i++)
        {
            w[i].GetComponent<EventTrigger>().enabled = true;
            see[i].GetComponent<EventTrigger>().enabled = false;
        }
        w[2].GetComponent<EventTrigger>().enabled = false;
        see[4].GetComponent<EventTrigger>().enabled = true;
        a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
        a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
        a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
    }
    public void trachea()
    {
        if (sd1 == false && sd2 == false && sd3 == false && sd4 == false)
        {
            sd1 = false;
            sd2 = true;
            sd3 = false;
            sd4 = false;
            if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
            {
                count++;
            }
            a[3].SetActive(true);
            a[1].SetActive(false);
            a[4].SetActive(false);
            a[2].SetActive(false);
            a[0].SetActive(false);
            pop.SetActive(true);
            gido.SetActive(true);
            vP.SetActive(false);
            rlrhkswl.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }

            w[3].GetComponent<colorchange>().blackcolor();
            w[1].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().blackcolor();
            w[0].GetComponent<colorchange>().bluecolor();
            w[4].GetComponent<colorchange>().blackcolor();
        }
        else
        {
            sd1 = false;
            sd2 = true;
            sd3 = false;
            sd4 = false;
            a[3].SetActive(true);
            a[1].SetActive(false);
            a[4].SetActive(false);
            a[2].SetActive(false);
            a[0].SetActive(false);
            pop.SetActive(true);
            gido.SetActive(true);
            vP.SetActive(false);
            rlrhkswl.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }
            w[3].GetComponent<colorchange>().blackcolor();
            w[1].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().blackcolor();
            w[0].GetComponent<colorchange>().bluecolor();
            w[4].GetComponent<colorchange>().blackcolor();
        }
        see[2].GetComponent<imagecolorchange>().imageblackcolor();
        see[1].GetComponent<imagecolorchange>().imageblackcolor();
        see[0].GetComponent<imagecolorchange>().imagecolor();
        see[3].GetComponent<imagecolorchange>().imageblackcolor();
        see[4].GetComponent<imagecolorchange>().imageblackcolor();
        for (int i = 0; i < w.Length; i++)
        {
            w[i].GetComponent<EventTrigger>().enabled = true;
            see[i].GetComponent<EventTrigger>().enabled = false;
        }
        w[0].GetComponent<EventTrigger>().enabled = false;
        see[4].GetComponent<EventTrigger>().enabled = true;
        a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
        a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
        a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
    }
    public void trachea1()
    {
        if (sd1 == false && sd2 == false && sd3 == false && sd4 == false)
        {
            sd1 = false;
            sd2 = false;
            sd3 = true;
            sd4 = false;
            if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
            {
                count++;
            }
            a[1].SetActive(true);
            a[2].SetActive(false);
            a[3].SetActive(false);
            a[4].SetActive(false);
            a[0].SetActive(false);
            pop.SetActive(true);
            rlrhkswl.SetActive(true);
            vP.SetActive(false);
            gido.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }
            w[3].GetComponent<colorchange>().blackcolor();
            w[0].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().blackcolor();
            w[1].GetComponent<colorchange>().bluecolor();
            w[4].GetComponent<colorchange>().blackcolor();
        }
        else
        {
            sd1 = false;
            sd2 = false;
            sd3 = true;
            sd4 = false;
            a[0].SetActive(false);
            a[3].SetActive(false);
            a[4].SetActive(false);
            a[1].SetActive(true);
            a[2].SetActive(false);
            pop.SetActive(true);
            rlrhkswl.SetActive(true);
            vP.SetActive(false);
            gido.SetActive(false);
            ghldrudakr.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }
            w[3].GetComponent<colorchange>().blackcolor();
            w[0].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().blackcolor();
            w[1].GetComponent<colorchange>().bluecolor();
            w[4].GetComponent<colorchange>().blackcolor();
        }
        see[0].GetComponent<imagecolorchange>().imageblackcolor();
        see[2].GetComponent<imagecolorchange>().imageblackcolor();
        see[1].GetComponent<imagecolorchange>().imagecolor();
        see[3].GetComponent<imagecolorchange>().imageblackcolor();
        see[4].GetComponent<imagecolorchange>().imageblackcolor();
        for (int i = 0; i < w.Length; i++)
        {
            w[i].GetComponent<EventTrigger>().enabled = true;
            see[i].GetComponent<EventTrigger>().enabled = false;
        }
        w[1].GetComponent<EventTrigger>().enabled = false;
        see[4].GetComponent<EventTrigger>().enabled = true;
        a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
        a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
        a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
    }
    public void diaphragm()
    {
        if (sd1 == false && sd2 == false && sd3 == false && sd4 == false)
        {
            sd1 = false;
            sd2 = false;
            sd3 = false;
            sd4 = true;
            if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
            {
                count++;
            }
            a[4].SetActive(true);
            a[1].SetActive(false);
            a[2].SetActive(false);
            a[3].SetActive(false);
            a[0].SetActive(false);
            pop.SetActive(true);
            ghldrudakr.SetActive(true);
            vP.SetActive(false);
            rlrhkswl.SetActive(false);
            gido.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }
            w[4].GetComponent<colorchange>().blackcolor();
            w[0].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().blackcolor();
            w[3].GetComponent<colorchange>().bluecolor();
            w[1].GetComponent<colorchange>().blackcolor();
        }
        else
        {
            sd1 = false;
            sd2 = false;
            sd3 = false;
            sd4 = true;
            a[4].SetActive(true);
            a[1].SetActive(false);
            a[2].SetActive(false);
            a[3].SetActive(false);
            a[0].SetActive(false);
            pop.SetActive(true);
            ghldrudakr.SetActive(true);
            vP.SetActive(false);
            rlrhkswl.SetActive(false);
            gido.SetActive(false);
            decompositionbtn.SetActive(false);
            synthesisbtn.SetActive(false);
            sd.GetComponent<Respiratory_CS>().ResePos();
            sd.GetComponent<Draggable>().enabled = true;
            sd.GetComponent<Animation>().enabled = false;
            sd.transform.localPosition = new Vector3(0, 0, 0);
            for (int i = 0; i < a.Length; i++)
            {
                a[i].GetComponent<EventTrigger>().enabled = false;
            }
            w[4].GetComponent<colorchange>().blackcolor();
            w[0].GetComponent<colorchange>().blackcolor();
            w[2].GetComponent<colorchange>().blackcolor();
            w[3].GetComponent<colorchange>().bluecolor();
            w[1].GetComponent<colorchange>().blackcolor();
        }
        see[0].GetComponent<imagecolorchange>().imageblackcolor();
        see[1].GetComponent<imagecolorchange>().imageblackcolor();
        see[3].GetComponent<imagecolorchange>().imagecolor();
        see[2].GetComponent<imagecolorchange>().imageblackcolor();
        see[4].GetComponent<imagecolorchange>().imageblackcolor();
        for (int i = 0; i < w.Length; i++)
        {
            w[i].GetComponent<EventTrigger>().enabled = true;
            see[i].GetComponent<EventTrigger>().enabled = false;
        }
        w[3].GetComponent<EventTrigger>().enabled = false;
        see[4].GetComponent<EventTrigger>().enabled = true;
        a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
        a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
        a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
    }
    public void noseerleh1()
    {
        if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
        {
            count++;
        }
        noseerleh++;
        for (int i = 0; i < w.Length; i++)
        {
            see[i].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseerleh == 1)
        {
            a12.f = true;
            a[3].GetComponent<SkinnedMeshRenderer>().material = noseerlrhkswlmat;
            see[0].GetComponent<imagecolorchange>().imageblackcolor();
            see[4].GetComponent<imagecolorchange>().imageblackcolor();
            w[4].GetComponent<colorchange>().blackcolor();
            w[4].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseerleh == 2)
        {
            a12.f = false;
            a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
            see[0].GetComponent<imagecolorchange>().imagecolor();
            if (a12.s == false && a12.d == false && a12.g == false)
            {
                if (count == 2)
                {
                    sd.GetComponent<Draggable>().enabled = true;
                }
                see[4].GetComponent<imagecolorchange>().imagecolor();
                w[4].GetComponent<colorchange>().bluecolor();
                w[4].GetComponent<EventTrigger>().enabled = false;
                count--;
            }
            noseerleh = 0;
        }


    }
    public void noseerlrhkswl1()
    {
        if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
        {
            count++;
        }
        noseerlrhkswl++;
        for (int i = 0; i < w.Length; i++)
        {
            see[i].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseerlrhkswl == 1)
        {
            a12.d = true;
            a[1].GetComponent<MeshRenderer>().material = noseerlrhkswlmat;
            see[1].GetComponent<imagecolorchange>().imageblackcolor();
            see[4].GetComponent<imagecolorchange>().imageblackcolor();
            w[4].GetComponent<colorchange>().blackcolor();
            w[4].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseerlrhkswl == 2)
        {
            a12.d = false;
            a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
            see[1].GetComponent<imagecolorchange>().imagecolor();
            if (a12.s == false && a12.f == false && a12.g == false)
            {
                if (count == 2)
                {
                    sd.GetComponent<Draggable>().enabled = true;
                }
                see[4].GetComponent<imagecolorchange>().imagecolor();
                w[4].GetComponent<colorchange>().bluecolor();
                w[4].GetComponent<EventTrigger>().enabled = false;
                count--;
            }
            noseerlrhkswl = 0;
        }
    }
    public void noseevP1()
    {
        if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
        {
            count++;
        }
        noseevP++;
        for (int i = 0; i < w.Length; i++)
        {
            see[i].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseevP == 1)
        { 
            
            a12.s = true;
            a[0].GetComponent<SkinnedMeshRenderer>().material = noseevPmat;
            a[2].GetComponent<SkinnedMeshRenderer>().material = noseevPmat;
            see[2].GetComponent<imagecolorchange>().imageblackcolor();
            see[4].GetComponent<imagecolorchange>().imageblackcolor();
            w[4].GetComponent<colorchange>().blackcolor();
            w[4].GetComponent<EventTrigger>().enabled = true;


        }
        if (noseevP == 2)
        {
            a12.s = false;
            a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
            a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
            see[2].GetComponent<imagecolorchange>().imagecolor();
            if (a12.d == false && a12.f == false && a12.g == false)
            {
                if (count == 2)
                {
                    sd.GetComponent<Draggable>().enabled = true;
                }
                see[4].GetComponent<imagecolorchange>().imagecolor();
                w[4].GetComponent<colorchange>().bluecolor();
                w[4].GetComponent<EventTrigger>().enabled = false;
                count--;
            }
            noseevP = 0;
        }

    }
    public void noseeghldrurakr1()
    {
        if (a12.s == false && a12.d == false && a12.f == false && a12.g == false)
        {
            count++;
        }
        noseeghldrurakr++;
        for (int i = 0; i < w.Length; i++)
        {
            see[i].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseeghldrurakr == 1)
        {
            a12.g = true;
            a[4].GetComponent<SkinnedMeshRenderer>().material = noseeghldrurakrmat;
            see[3].GetComponent<imagecolorchange>().imageblackcolor();
            see[4].GetComponent<imagecolorchange>().imageblackcolor();
            w[4].GetComponent<colorchange>().blackcolor();
            w[4].GetComponent<EventTrigger>().enabled = true;
        }
        if (noseeghldrurakr == 2)
        {
            a12.g = false;
            a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
            see[3].GetComponent<imagecolorchange>().imagecolor();
            if (a12.s == false && a12.d == false && a12.f == false)
            {
                if (count == 2)
                {
                    sd.GetComponent<Draggable>().enabled = true;
                }
                see[4].GetComponent<imagecolorchange>().imagecolor();
                w[4].GetComponent<colorchange>().bluecolor();
                w[4].GetComponent<EventTrigger>().enabled = false;
                count--;
            }
            noseeghldrurakr = 0;
        }
    }
    public void optionon()
    {
        a[3].GetComponent<SkinnedMeshRenderer>().material = seerlrhkswlmat;
        a[1].GetComponent<MeshRenderer>().material = seerlrhkswlmat;
        a[0].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[2].GetComponent<SkinnedMeshRenderer>().material = seevPmat;
        a[4].GetComponent<SkinnedMeshRenderer>().material = seeghldrurakrmat;
    }
    public void optionoff()
    {
        if (a12.s)
        {
            a[0].GetComponent<SkinnedMeshRenderer>().material = noseevPmat;
            a[2].GetComponent<SkinnedMeshRenderer>().material = noseevPmat;
        }
        if (a12.g)
        {
            a[4].GetComponent<SkinnedMeshRenderer>().material = noseeghldrurakrmat;
        }
        if (a12.d)
        {
            a[1].GetComponent<MeshRenderer>().material = noseerlrhkswlmat;
        }
        if (a12.f)
        {
            a[3].GetComponent<SkinnedMeshRenderer>().material = noseerlrhkswlmat;
        }
    }
}
