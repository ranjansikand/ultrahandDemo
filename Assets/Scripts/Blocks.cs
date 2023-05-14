// Class attaches to manipulable objects


using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer), typeof(AudioSource))]
public class Blocks : MonoBehaviour, IManipulable
{
    Rigidbody _rigidbody;
    MeshRenderer _renderer;
    AudioSource _audiosource;

    public Material normal, controlled;
    public AudioClip collisionSound;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
        _audiosource = GetComponent<AudioSource>();

        normal = _renderer.material;
    }

    private void OnCollisionEnter(Collision collision) {
        float force = (collision.impulse / Time.fixedDeltaTime).magnitude;
        _audiosource.PlayOneShot(collisionSound, Mathf.Clamp01(force / 250f));
    }

    public void Float(float amount) {
        _rigidbody.useGravity = false;
        _rigidbody.AddForce(amount * Vector3.up, ForceMode.Force);
        _renderer.material = controlled;
    }

    public void Release() {
        _rigidbody.useGravity = true;
        _renderer.material = normal;
    }

    public void Launch(Vector3 target, float force) {
        Vector3 direction = Vector3.Normalize(target - _rigidbody.transform.position);
        _rigidbody.AddForce(force * direction, ForceMode.Impulse);
    }
}
