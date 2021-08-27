using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public FloatValue availableTime;
    public Text textUI;
    
    private float nextTimestamp;
    
    // Start is called before the first frame update
    void Start()
    {
        availableTime.runtimeValue = availableTime.initialValue;
        nextTimestamp = Time.time + 1;
        textUI.text = availableTime.initialValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (availableTime.runtimeValue > 0 && nextTimestamp < Time.time) {
            availableTime.runtimeValue -= 1;
            textUI.text = (availableTime.runtimeValue).ToString();
            nextTimestamp += 1; 
        }
    }
}
