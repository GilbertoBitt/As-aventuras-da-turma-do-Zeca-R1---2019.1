using System;
using System.Collections.Generic;
using com.csutil;
using DG.DeAudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TutorialSystem.Scripts
{
    public class DialogComponent : MonoBehaviour
    {
        [ReadOnly]
        public DialogSettings dialogSetting;
        public DoTextDuration doTextDuration;
        public bool autoStart;
        public DialogInfo currentDialogInfo;
        public int currentIndex;
        public int indexEnd;
        public string subscribeEventName = "tutorialLudica";
        public string stopEventName => $"{subscribeEventName}Stopped";
        public string startEventName => $"{subscribeEventName}Started";
        public TextMeshProUGUI textMeshComponent;
        [ReadOnly]
        public Sequence dialogSequenceAnimation;
        [ReadOnly] public EventBus eventBus;
        public Button previousDialogButton;
        public Button nextDialogButton;
        public Button replayDialogButton;
        [ReadOnly]
        public List<DialogWindowComponent> SubscribedDialogWindowComponents = new List<DialogWindowComponent>();


        public void Start()
        {

            eventBus = (EventBus) EventBus.instance;
            dialogSetting = IoC.inject.Get<DialogSettings>(this);
            if (dialogSetting == null)
            {
                dialogSetting = ResourcesV2.LoadScriptableObjectInstance<DialogSettings>("DialogSettings.asset");
                IoC.inject.SetSingleton(dialogSetting);
            }

            previousDialogButton.OnClickAsObservable().Subscribe(unit => { PreviousDialog(); });
            nextDialogButton.OnClickAsObservable().Subscribe(unit => { NextDialog(); });
            replayDialogButton.OnClickAsObservable().Subscribe(unit => { ReplayVoiceTutorial(); });


            if (!autoStart || currentDialogInfo == null) return;
            //TODO iniciar automaticamente no start
            eventBus.Publish(startEventName);
            DOTween.Kill("tutorialSystem");
            StartDialog();
        }




        [ButtonGroup("DialogControl")]
        public void PreviousDialog()
        {
            var temp = currentIndex--;
            if (temp < 0) return;
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo, currentIndex--);

        }

        [ButtonGroup("DialogControl")]
        public void StartDialog()
        {
            if (currentDialogInfo == null) return;
            eventBus.Publish(stopEventName);
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo);
        }

        [ButtonGroup("DialogControl")]
        public void NextDialog()
        {
            var temp = currentIndex++;
            if (temp >= currentDialogInfo.speeches.Count) return;
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo, currentIndex++);
        }

        [ButtonGroup("DialogControl")]
        public void ReplayVoiceTutorial()
        {
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo, currentIndex);
        }

        private void StartDialog(DialogInfo dialogInfo, int currentDialogIndex = 0)
        {
            indexEnd = dialogInfo.speeches.Count;
            currentIndex = currentDialogIndex;
            //TODO adicionar animação do personagem piscando e falando.    
            dialogSequenceAnimation = DOTween.Sequence();
            dialogSequenceAnimation.SetId("tutorialSystem");
            dialogSequenceAnimation.AppendInterval(.3f);
            DeAudioManager.Stop(DeAudioGroupId.Dialogue);

            for (var index = currentDialogIndex; index < currentDialogInfo.speeches.Count; index++)
            {
                dialogSequenceAnimation.AppendCallback(() =>
                {
                    previousDialogButton.gameObject.SetActive(currentIndex != 0);
                    nextDialogButton.gameObject.SetActive(currentIndex != indexEnd-1);
                });

                var speech = currentDialogInfo.speeches[index];
                if (speech.speechAudioClip.enabled && speech.speechAudioClip != null)
                {
                    dialogSequenceAnimation.AppendCallback(() =>
                        DeAudioManager.Play(DeAudioGroupId.Dialogue, speech.speechAudioClip.audioClip));
                    dialogSequenceAnimation.AppendInterval(speech.speechAudioClip.audioClip.length);
                    dialogSequenceAnimation.Join(textMeshComponent.DOText(speech.speechText,
                        doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration));
                }
                else
                {
                    dialogSequenceAnimation.Append(textMeshComponent.DOText(speech.speechText,
                        doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration));
                }

                dialogSequenceAnimation.AppendInterval(dialogSetting.timeBetweenSpeechs);

                dialogSequenceAnimation.AppendCallback(() => { currentIndex++; });

            }

            dialogSequenceAnimation.AppendCallback(() => { eventBus.Publish(stopEventName); });
            dialogSequenceAnimation.Play();
        }

        public void SetDialogInfo(DialogInfo dialogInfo)
        {
            currentDialogInfo = dialogInfo;
        }

        public void StopTutorial()
        {
            DOTween.Kill("tutorialSystem");
            eventBus.Publish(stopEventName);
        }


    }

    [Serializable, Toggle("enabled")]
    public class DoTextDuration
    {
        public bool enabled;
        public float timeBetweenSpeechs;
    }

}
