using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class imagecolorchange : MonoBehaviour
{
    public Sprite checkon;
    public Sprite checkoff;
    public void imagecolor()
    {
        gameObject.GetComponent<Image>().sprite = checkon;
    }
    public void imageblackcolor()
    {
        gameObject.GetComponent<Image>().sprite = checkoff;
    }
}
