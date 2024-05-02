using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class BaseCube : MonoBehaviour
{
    private const float MinLifetime = 2f;
    private const float MaxLifetime = 5f;

    private Material _material;
    private int _groundLayer;
    private bool _isColorChanged;

    private float _lifetime;
    private float _livedTime;

    public event Action<BaseCube> TimeEnded;

    private void Awake()
    {
        _groundLayer = LayerMask.NameToLayer("Ground");
        _material = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        _isColorChanged = false;
        _livedTime = 0;
        _lifetime = UnityEngine.Random.Range(MinLifetime, MaxLifetime);
    }

    private void LateUpdate()
    {
        _livedTime += Time.deltaTime;

        if (_livedTime >= _lifetime)
        {
            TimeEnded?.Invoke(this);
            _material.color = Color.white;
        }
    }

    public void StartInitialization(Vector3 position)
    {
        _isColorChanged = false;
        _livedTime = 0;
        SetTransform(position);
    }

    private void SetTransform(Vector3 position)
    {
        const int AxisCount = 3;
        const float MinTorque = 100f;
        const float MaxTorque = 1000f;

        int randomAxis = UnityEngine.Random.Range(0, AxisCount);
        Vector3 startTorque = new Vector3();
        startTorque[randomAxis] = UnityEngine.Random.Range(MinTorque, MaxTorque);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddTorque(startTorque);
        transform.position = position;
    }

    private void SwitchColor()
    {
        float red = UnityEngine.Random.Range(0f, 1f);
        float green = UnityEngine.Random.Range(0f, 1f);
        float blue = UnityEngine.Random.Range(0f, 1f);
        float alpha = 1;
        Color newColor = new Color(red, green, blue, alpha);
        _material.color = newColor;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == _groundLayer && _isColorChanged == false)
        {
            _isColorChanged = true;
            SwitchColor();
        }
    }
}