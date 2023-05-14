// Rings of the target

using UnityEngine;


public enum TargetValue { Low, Med, High }

public class Target : MonoBehaviour
{
    public TargetValue thisValue;

    private void OnCollisionEnter(Collision other) {
        TargetBoard.instance.UpdateText(thisValue);
    }
}
