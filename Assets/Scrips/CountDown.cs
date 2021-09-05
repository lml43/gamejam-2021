using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public FloatValue availableTime;
    public Text textUI;
    public BoolValue isPaused;
    
    private float nextTimestamp;
    
    // Start is called before the first frame update
    void Start()
    {
        availableTime.runtimeValue = availableTime.initialValue;
        textUI.text = availableTime.initialValue.ToString();
        nextTimestamp = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused.runtimeValue && availableTime.runtimeValue > 0 && nextTimestamp < Time.time) {
            availableTime.runtimeValue -= 1;
            textUI.text = (availableTime.runtimeValue).ToString();
            nextTimestamp += 1; 
        } else {

        }
    }

    public void UpdateNextTimestamp() {
        nextTimestamp = Time.time + 1;
    }
}
