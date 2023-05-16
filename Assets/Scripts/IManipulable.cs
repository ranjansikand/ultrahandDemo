// Interface that allows objects to be controlled
// Grab: Actions executed on pickup
// Release: Actions executed when dropped

using UnityEngine;

public interface IManipulable
{
    public void Grab(bool byParent = true);
    public void Release(bool byParent = true);
    public bool Attach();
    public void Attach(FixedJoint joint);
    public void Detach();
    public void Detach(FixedJoint joint);
    public void Rotate(float upDown);
}
