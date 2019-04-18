using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimRA2 : MonoBehaviour {

    Animator animPerson;
    int nubRandom;
    public GameObject img;
    void Start () {

        animPerson = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void randomAnim()
    {
        //nubRandom = Random.Range(2, 4);
       // animPerson.SetInteger("NumbAR", nubRandom);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "BoxColl")
        {
            img.SetActive(true);
            //Debug.Log("fedasf");
        }
    }
}
