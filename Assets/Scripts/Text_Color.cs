using UnityEngine;
using UnityEngine.UI;

public class Text_Color : MonoBehaviour
{
    public Text[] Black_Text;

    public void Select()
    {
        this.GetComponent<Text>().color = new Color(1, 0, 0);
        foreach (Text B in Black_Text)
        {
            B.color= new Color(0, 0 , 0);
        }
    }

    public void Red_Color_For_5Quiz()
    {
        this.GetComponent<Text>().color = new Color(1, 0, 0);
    }

}
