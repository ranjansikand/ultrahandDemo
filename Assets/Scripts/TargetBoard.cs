// Worldspace scoreboard above the target

using UnityEngine;
using TMPro;

public class TargetBoard : MonoBehaviour
{
    public static TargetBoard instance;
    [SerializeField] TMP_Text scoreText;
    bool ready = true;

    private void Awake() {
        instance = this;
    }

    public void UpdateText(TargetValue value) {
        if (ready) {
            ready = false;
            switch (value) {
                case (TargetValue.Low): scoreText.text = "5 points!"; break;
                case (TargetValue.Med): scoreText.text = "10 points!"; break;
                case (TargetValue.High): scoreText.text = "Bullseye!"; break;
                default: break;
            }
            Invoke(nameof(Reset), 0.1f);
        }
    }

    public void Reset() {
        ready = true;
    }
}
