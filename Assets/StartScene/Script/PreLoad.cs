using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;

public class PreLoad :MonoBehaviour {

    public float preloadRotationSpeed;
    public Image preloaderImage;
    public GameObject eventSystemComp;
    public GameObject CameraMain;
    public Text textPercentageComp;
    public Text textBuildVersion;

    public Texture2D newCursors;
    public Vector2 hotspot;

    // Use this for initialization
    void Start() {
        //Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        textBuildVersion.text = "Build Version: " + Application.version;
        StartCoroutine(ChangeScene("StartScene"));

        Cursor.SetCursor(newCursors,hotspot,CursorMode.Auto);
        
    }



    IEnumerator<float> AsyncLoad(string scene) {

        AsyncOperation loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        loading.allowSceneActivation = false;

        while (loading.progress < 0.90f) {
            yield return Timing.WaitForOneFrame;
        }


        loading.allowSceneActivation = true;

    }

    /*
*
*  NB: this is a copy/paste from my last commit. There may
*     be errors. Any improvements I find I'll update this code
*
*/

    // you MUST start ALL coroutines before the loadasync call

    public IEnumerator ChangeScene(string sceneName) {
        yield return new WaitForSeconds(.3f);
        // MUST save a ref to old scene - needed later!
        Scene oldScene = SceneManager.GetActiveScene();
        // NB: Unity has major bugs in the "by name" call; they recommend you use "by path"
        //  .. which is a workaround to their bugs, but it makes your game hardcoded: if you
        //  .. change your folder structure, your game stops working. It's a nasty hack.
        //  .. Thanks, Unity!
       // Scene newScene = SceneManager.GetSceneByName(sceneName);

        // You MUST de-tag your Main-Camera, or Unity's own code crashes
        // ..e.g: nasty bug in Unity's FPS controller: it permanently breaks mouselook!
        // ... if you disable it, your screen will go black / flicker, so detag instead
        //Camera oldMainCamera = Camera.main; // we need this later
        //Camera.main.tag = "Untagged";

        // Start the async load
        // NOTE: Unity does not allow you to pass-in the scene to load as a parameter. WTF?
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone) // note: this will crash at 0.9 if you're not careful
        {
            //asyncOperation.allowSceneActivation = true;
            // progressBar.value = asyncOperation.progress;
            //int percentage = (int)asyncOperation.progress * 100;
            textPercentageComp.text = string.Format("{0:N0}%", asyncOperation.progress * 100f);
            // 0.9f is a hardcoded magic number inside the SceneManager API
            // ... this is OFFICIAL! Not a hack!
                if (asyncOperation.progress == 0.9f) {
                textPercentageComp.text = "100%";
                // asyncOperation.allowSceneActivation = true;

                // Unity WILL CORRUPT DATA INTERNALLY AT END OF THIS FRAME
                // ...unless you do several things here.

                // You MUST disable all AudioListeners, or you will get spammed with errors
                // FindObjectOfType<AudioListener>().enabled = false;

                // Because we're at 0.9f, and scene is about to flip-over,
                //  you MUST disable your live cameras
                //oldMainCamera.gameObject.SetActive(false);
                Scene newScene = SceneManager.GetSceneByName(sceneName);
                // MUST either delete the old scene,
                //   OR disable every root gameobject 
                /* for (int i = 0; i < objsToDelete.Length; i++) {
                     objsToDelete[i].SetActive(false);
                 }*/


                SceneManager.MoveGameObjectToScene(CameraMain, newScene);
                //SceneManager.MoveGameObjectToScene(eventSystemComp, newScene);

                // Scene has now loaded; but you MUST manually "activate" it
				yield return new WaitUntil(() => asyncOperation.isDone);

                SceneManager.SetActiveScene(newScene);
                //yield return new WaitForFixedUpdate();
                
                StartSceneManager manager = FindObjectOfType(typeof(StartSceneManager)) as StartSceneManager;
                if(manager != null) {
                    manager.StartDelayed();
                } else {
                    Debug.Log("Null Exception", this);
                }
                SceneManager.UnloadSceneAsync(oldScene);
                //oldMainCamera.enabled = false;
                yield return null;
                }

            //textPercentageComp.text = string.Format("{0:N0}%", asyncOperation.progress * 100f);
        }
        }
    }


    
