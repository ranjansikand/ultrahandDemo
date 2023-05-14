// Power control system for the player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telekinesis : MonoBehaviour
{
    AudioSource audiosource;

    // Scanning and adding objects
    Collider[] results = new Collider[20];
    List<IManipulable> controlledObjects = new List<IManipulable>();
    int numberOfObjects = 0;
    [SerializeField] LayerMask layerMask;

    // Using powers
    Coroutine telekineticPower;  // Stores whichever power is in use
    WaitForSeconds liftDelay = new WaitForSeconds(0.05f), launchDelay = new WaitForSeconds(0.2f),
        startDelay = new WaitForSeconds(0.5f);

    // Effects
    [SerializeField] ParticleSystem liftEffect;
    [SerializeField] AudioClip liftSound, releaseSound;
    
    private void Awake() {
        audiosource = GetComponent<AudioSource>();
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (controlledObjects.Count == 0) {  
                // Scan for objects in radius
                numberOfObjects = Physics.OverlapSphereNonAlloc(transform.position, 5, results, layerMask);

                // Add objects to control
                for (int i = 0; i < numberOfObjects; i++) {
                    IManipulable objectToAdd = results[i].GetComponent<IManipulable>();
                    controlledObjects.Add(objectToAdd);
                }

                // Start lifting
                telekineticPower = StartCoroutine(LiftObjectsInRadius());
            } else {  
                // Play effect
                audiosource.PlayOneShot(releaseSound, 0.5f);

                // Drop objects
                foreach (IManipulable obj in controlledObjects) {
                    obj.Release();
                }
                controlledObjects.Clear();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (controlledObjects.Count > 0) {
                // Look for target
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Assign target to found position or forward direction
                Vector3 target;
                if (Physics.Raycast(ray, out hit)) target = hit.point;
                else target = Camera.main.transform.forward * 25f;

                // Start launching
                telekineticPower = StartCoroutine(ShootObjectsTowardsPosition(target));
            }
        }
    }


    IEnumerator LiftObjectsInRadius() {
        // Start Effects
        liftEffect.Play();
        audiosource.PlayOneShot(liftSound, 0.5f);

        yield return launchDelay;

        // Lift Objects
        foreach (IManipulable obj in controlledObjects) {
            obj.Float(Random.Range(2.5f, 6f));
            yield return liftDelay;
        }
        telekineticPower = null;
    }

    IEnumerator ShootObjectsTowardsPosition(Vector3 target) {
        foreach (IManipulable obj in controlledObjects) {
            obj.Launch(target, 20f);
            obj.Release();
            yield return launchDelay;
        }

        telekineticPower = null;
        controlledObjects.Clear();
    }
}
