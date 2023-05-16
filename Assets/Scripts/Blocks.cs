// Class governing manipulable objects
// Contains scripts for objects to be selected, moved, edited, and detached


using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer), typeof(AudioSource))]
public class Blocks : MonoBehaviour, IManipulable
{
    Rigidbody _rigidbody;
    MeshRenderer _renderer;
    AudioSource _audiosource;

    // Sound effects and material swaps to make changes more clear
    public Material normal, controlled;
    public AudioClip collisionSound, attachSound, detachSound;

    // While held, will  attempt to maintain a constant distance from the camera's
    // focal point to this object
    float distanceToCamera;
    bool held;

    // Governs connections between other objects
    // joint is the unit this is connected out to, while the list contains all units
    // connected into this one
    FixedJoint joint;
    public List<FixedJoint> neighborJoints {get; set;} = new List<FixedJoint>(); 
    [SerializeField] LayerMask layerMask;


    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
        _audiosource = GetComponent<AudioSource>();

        normal = _renderer.material;
    }

    private void Update() {
        if (held) {  // If player is actively carring this object
            // Get center screenpoint
            Vector3 targetPos = Camera.main.ViewportToWorldPoint(
                new Vector3(0.5f, 0.5f, distanceToCamera));
            // Get direction to screenpoint
            Vector3 direction = (targetPos - transform.position).normalized; 
            // Scale speed by distance to desired point
            float scaledForce = 5 * Vector3.Distance(targetPos, transform.position);

            _rigidbody.velocity = direction * scaledForce;
        }
    }

    private void OnCollisionEnter(Collision other) {
        _audiosource.PlayOneShot(collisionSound, 0.25f);
    }


    // Functions required by the interface

    public void Grab(bool byParent = true) {
        // Change material and update gravity
        _renderer.material = controlled;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        

        // Only update control in user-selected object
        if (byParent) {
            // Update variables to use in update
            distanceToCamera = Vector3.Distance(Camera.main.transform.position, transform.position);
            held = true;
            
            // Grab joined object
            if (joint != null) {
                joint.connectedBody.gameObject.GetComponent<IManipulable>().Grab(false);
            }

            // Grab other objects joined to this
            if (neighborJoints.Count > 0) {
                foreach (FixedJoint jointObj in neighborJoints) {
                    if (jointObj != null)
                        jointObj.gameObject.GetComponent<IManipulable>().Grab(false);
                }
            }
        }
        
    }

    public void Release(bool byParent = true) {
        // Swap back to original state
        _renderer.material = normal;
        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.None;

        

        // Only update control in user-selected object
        if (byParent) {
            // Stop movement
            held = false;
            
            // Release jointed object
            if (joint != null) {
                joint.connectedBody.gameObject.GetComponent<IManipulable>().Release(false);
            }

            // Release other objects jointed to this
            if (neighborJoints.Count > 0) {
                foreach (FixedJoint jointObj in neighborJoints) {
                    if (jointObj != null)
                        jointObj.gameObject.GetComponent<IManipulable>().Release(false);
                }
            }
        }
        
    }

    public bool Attach() {
        // Find objects in connection range
        Collider[] results = new Collider[5];
        if (Physics.OverlapSphereNonAlloc(transform.position, 2, results, layerMask) > 1) {
            Rigidbody obj = null;
            float dist = 5;

            // Sort through results to find nearest object
            foreach (Collider collider in results) {
                // Error catching
                if (collider != null && collider.attachedRigidbody != _rigidbody) {
                    float tempDist = Vector3.Distance(transform.position, collider.transform.position);
                    if (tempDist < dist) {
                        Debug.Log("Nearest: " + collider.attachedRigidbody + " | Self: " + _rigidbody);
                        obj = collider.attachedRigidbody;
                        dist = tempDist;
                    }
                }
            }

            // Create and attach joint
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = obj;
            // Let other object know it's connected to this
            results[1].gameObject.GetComponent<IManipulable>().Attach(joint);

            _audiosource.PlayOneShot(attachSound, 1);
            return true;
        }

        // No applicable object found
        return false;
    }

    public void Attach(FixedJoint joint) {
        // Add reference to connection
        neighborJoints.Add(joint);
    }

    public void Detach() {
        bool somethingWasDetached = false;

        if (joint != null) {
            // Disconnect from other objects
            var obj = joint.connectedBody.gameObject.GetComponent<IManipulable>();
            obj.Release(false);
            obj.Detach(joint);

            // Remove joint
            Destroy(joint);
            somethingWasDetached = true;
        }

        if (neighborJoints.Count > 0) {
            // Destroy joints on other objects that connect to this one
            foreach (FixedJoint joints in neighborJoints) {
                joints.gameObject.GetComponent<IManipulable>().Release(false);
                Destroy(joints);
            }
            somethingWasDetached = true;
        }

        joint = null;
        neighborJoints.Clear();

        // Only play a sound if something was detached
        if (somethingWasDetached) _audiosource.PlayOneShot(detachSound, 1); 
    }

    public void Detach(FixedJoint joint) {
        neighborJoints.Remove(joint);
    }

    public void Rotate(float rotation) {
        transform.Rotate(Camera.main.transform.right * rotation * 10, Space.Self);
    }
}
