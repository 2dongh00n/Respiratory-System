using UnityEngine;
using UnityEngine.UI;

public class Quiz5_Controller : MonoBehaviour
{
    public bool Answer;
    public bool Switch;

    public void Button_Pressed()
    {
        if (Switch)
        {
            Switch = false;
            this.GetComponent<Text>().color = new Color(0f, 0f, 0f);
            GameObject.Find("Quiz_Manager").GetComponent<Quiz_Manager>().Quiz5_Counter--;
            if (Answer)
            {
                GameObject.Find("Quiz_Manager").GetComponent<Quiz_Manager>().Quiz5_Right_Answer_Counter--;
            }
        }
        else
        {
            Switch = true;
            this.GetComponent<Text>().color = new Color(1, 0, 0);
            GameObject.Find("Quiz_Manager").GetComponent<Quiz_Manager>().Quiz5_Counter++;
            if (Answer)
            {
                GameObject.Find("Quiz_Manager").GetComponent<Quiz_Manager>().Quiz5_Right_Answer_Counter++;
            }
        }
        GameObject.Find("Quiz_Manager").GetComponent<Quiz_Manager>().Quiz5_Controll();
    }
}
