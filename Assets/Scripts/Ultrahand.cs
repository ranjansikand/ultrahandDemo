// Script governing player controls and interactions


using UnityEngine;

public class Ultrahand : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip grabSound, releaseSound;

    [SerializeField] LayerMask layerMask;  //  Only select grabbable objects
    IManipulable _controlledObject = null;

    // Particle effect for grabbing objects
    [SerializeField] ParticleSystem beam;
    [SerializeField] Transform particleField;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        // Select object
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (_controlledObject == null) GrabObject();
            else ReleaseObject();
        }

        // Rotate object
        if (Input.mouseScrollDelta != Vector2.zero && _controlledObject != null) {
            _controlledObject.Rotate(Input.mouseScrollDelta.y);
        }


        // Attach object
        if (Input.GetKeyDown(KeyCode.Mouse1) && _controlledObject != null) {
            if (_controlledObject.Attach()) {
                _controlledObject.Release();
                ReleaseObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse2) && _controlledObject != null) {
            _controlledObject.Detach();
        }
    }

    void GrabObject() {
        // Get ray in camera
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10, layerMask)) {
            // Grab object
            _controlledObject = hit.collider.gameObject.GetComponent<IManipulable>();
            _controlledObject.Grab();

            // Start beam effect
            beam.Play();
            particleField.parent = hit.transform;
            particleField.localPosition = Vector3.zero;
            // Play sound effect
            audioSource.PlayOneShot(grabSound, 0.5f);
        }
    }

    void ReleaseObject() {
        // Remove object
        _controlledObject.Release();
        _controlledObject = null;

        // End e
        beam.Stop();
        audioSource.PlayOneShot(releaseSound, 0.5f);
    }
}
