using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class colorchange : MonoBehaviour
{   
    public void bluecolor()
    {
        this.GetComponent<Text>().color = new Color(1, 0, 0);
    }
    public void blackcolor()
    {
        this.GetComponent<Text>().color = new Color(0, 0, 0);
    }
}
