// Interface that allows objects to be controlled with telekinesis

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManipulable
{
    public void Float(float force);
    public void Launch(Vector3 target, float force);
    public void Release();
}
