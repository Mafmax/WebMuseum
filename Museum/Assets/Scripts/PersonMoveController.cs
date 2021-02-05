using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMoveController : MonoBehaviour
{
    Transform person;
    [Range(1, 10)]
    public float speed = 1;
    Camera mainCam;



    void Awake()
    {
        person = transform;
        mainCam = Camera.main;
    }

    void Update()
    {
        CheckMove();
    }
    void CheckMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            person.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            person.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            person.position += new Vector3(0, 0,speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            person.position += new Vector3(0,0, -speed * Time.deltaTime);
        }
    }




}
