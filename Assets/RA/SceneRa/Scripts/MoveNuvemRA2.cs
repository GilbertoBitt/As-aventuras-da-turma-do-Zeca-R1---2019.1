using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNuvemRA2 : MonoBehaviour {

    Vector3 offsetFollow;
    Vector3 defaultOffset;
    float transitionDuration;
    public AnimationCurve transitionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public GameObject myNuven;
    Transform posIni;
    public Transform posEnd;
    bool checknuvem = true;
    public bool checknuvem2 = false;
    void Start () {
        transitionDuration = Random.Range(20f,30f);

        posIni = GetComponent<Transform>();
        offsetFollow = new Vector3(posIni.position.x, posIni.position.y, posIni.position.z);
        defaultOffset = new Vector3(posEnd.position.x, posEnd.position.y, posEnd.position.z);
        StartCoroutine(MoverNuvem());
    }

    void Update()
    {

        if (checknuvem2) {
            checknuvem2 = false;
            StartCoroutine(MoverNuvem());
        }
    }
    public IEnumerator MoverNuvem()    {
        Vector3 startOffset = offsetFollow;
        Vector3 endOffset = defaultOffset;
        float times = 0.0f;
       
            while (times < transitionDuration)
            {
                times += Time.deltaTime;
                float s = times / transitionDuration;
                myNuven.transform.position = Vector3.Lerp(startOffset, endOffset, transitionCurve.Evaluate(s));
                yield return 1f;
            }
        checknuvem2 = true;
       
    }
 }
