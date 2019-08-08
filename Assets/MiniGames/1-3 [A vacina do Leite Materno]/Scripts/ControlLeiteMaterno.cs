using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;
using MEC;
using MiniGames.Scripts._1_3A;
using UniRx.Triggers;

public class ControlLeiteMaterno : MonoBehaviour {

    public Manager1_3A Manager1_3AR;
    Collider2D colliderBox;
    public Vector2 posLeite;
    public Vector2 posPlayer;
    public AreaEffector2D areaEffector;
    public NurseFollow1_3A NurseFollow1_3A2;
    private static readonly int DelizandoLeite = Animator.StringToHash("DelizandoLeite");
    private static readonly int Levandando = Animator.StringToHash("Levandando");
    private static readonly int BatendoCaixa = Animator.StringToHash("BatendoCaixa");


    void Start () {
      
        this.Manager1_3AR = Camera.main.GetComponent<Manager1_3A>();
        this.NurseFollow1_3A2 = Manager1_3AR.enfermeiraG.GetComponent<NurseFollow1_3A>();
        colliderBox = this.GetComponent<Collider2D>();
        areaEffector = this.GetComponent<AreaEffector2D>();
        areaEffector.enabled = false;



    }


    private void Update() {
        if (Manager1_3AR.nurseManager.hasCollide==true && NurseFollow1_3A2.endGameP == true) {
            colliderBox.enabled = false;
           // Manager1_3AR.checkPiscar = true;
            StopCoroutine(TimerVolta());
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("PlayerPe") || !(Manager1_3AR.playerRigidbody.velocity.y <= 0) ||
            Manager1_3AR.plataformController2d.isFlying) return;
        areaEffector.enabled = true;
        Manager1_3AR.plataformController2d.checkDeslizando = true;
        Manager1_3AR.animPerson.SetBool(BatendoCaixa, false);
        Manager1_3AR.animPerson.SetInteger("NumCaindo", 1);
        Manager1_3AR.animPerson.SetBool("DelizandoLeite", true);
        Manager1_3AR.ButtonsEnable(false);
        Manager1_3AR.deslizandoLeite = true;
        Invoke(nameof(CallTimeVoltar), 2f);
    }

    void CallTimeVoltar()
    {
        if(!Manager1_3AR.deslizandoLeite) return;
        StartCoroutine(TimerVolta());
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("PlayerPe")) {
          StartCoroutine(TimerVolta());

        }
      
    }


    IEnumerator TimerVolta()
    {
        colliderBox.enabled = false;
        Manager1_3AR.checkPiscar = true;
        Manager1_3AR.imgPersonAnimP.SetBool("Piscar",true);
        //  Manager1_3AR.imgPersonAnimP.enabled = true;
        yield return new WaitForSeconds(0.5f);
       // colliderBox.enabled = false;
        Manager1_3AR.numCaindo = 2;
        Manager1_3AR.StopRunning();      
        Manager1_3AR.animPerson.SetBool(DelizandoLeite, false);
        Manager1_3AR.animPerson.SetBool(BatendoCaixa, false);
        Manager1_3AR.animPerson.SetInteger("NumCaindo", 1);
        yield return new WaitForSeconds(1f);
        Manager1_3AR.animPerson.SetBool(DelizandoLeite, false);
        colliderBox.enabled = false;
        CrossPlatformInputManager.SetAxisZero("Vertical");
        yield return new WaitForSeconds(1f);
        Manager1_3AR.animPerson.SetBool(DelizandoLeite, false);
        Manager1_3AR.deslizandoLeite = false;
        //Manager1_3AR.ButtonsEnable(true);
        Manager1_3AR.checkPiscar = false;
        Manager1_3AR.imgPersonAnimP.SetBool("Piscar", false);
        Manager1_3AR.BackRunning();

    }



}
