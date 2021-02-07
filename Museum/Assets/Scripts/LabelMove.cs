using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelMove : MonoBehaviour
{
    [Range(2, 20)]
    public float speed = 2;
    [Range(0.25f, 2)]
    public float amplitude = 0.25f;

    public Transform person;

    private float time = 0;
    private Transform label;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        label = transform;
        startPosition = transform.position;
        Debug.LogError(label.forward);
    }

    // Update is called once per frame
    void Update()
    {
        label.forward = new Vector3(person.position.x, 0, person.position.z) - label.position;
        time += Time.deltaTime;
        label.position = startPosition + new Vector3(0, amplitude * Mathf.Cos(speed * time), 0);
    }
}
