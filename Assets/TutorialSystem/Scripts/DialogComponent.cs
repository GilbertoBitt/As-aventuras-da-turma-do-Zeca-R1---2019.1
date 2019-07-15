using System;
using System.Collections.Generic;
using com.csutil;
using DG.DeAudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TutorialSystem.Scripts
{
    public class DialogComponent : MonoBehaviour
    {
        public TextMeshProUGUI characterNameComponent;
        public ICharacterAnimation characterComponent;
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
        public TextMeshProUGUI textMeshComponentHalfSized;
        public Image spriteImage;
        [ReadOnly]
        public Sequence dialogSequenceAnimation;
        [ReadOnly] public EventBus eventBus;
        public Button previousDialogButton;
        public Button nextDialogButton;
        public Button replayDialogButton;
        public Button startGame;
        [FormerlySerializedAs("SubscribedDialogWindowComponents")] [ReadOnly]
        public List<DialogWindowComponent> subscribedDialogWindowComponents = new List<DialogWindowComponent>();

        public Action startTutorial;
        public Action endTutorial;

        private bool _startInit = false;
        public Image imageComponent;

        private void Start()
        {

            previousDialogButton.OnClickAsObservable().Subscribe(_ =>
            {
                PreviousDialog();
                Debug.Log("Play Previous Speech");
            });
            nextDialogButton.OnClickAsObservable().Subscribe(_ =>
            {
                NextDialog();
                Debug.Log("Play Next Speech");
            });
            replayDialogButton.OnClickAsObservable().Subscribe(_ =>
            {
                ReplayVoiceTutorial();
                Debug.Log("Replay current Speech");
            });
            Init();
        }

        public void Init()
        {
            if (_startInit) return;
            _startInit = true;
            imageComponent = GetComponent(typeof(Image)) as Image;

            characterComponent = GetComponentInChildren<ICharacterAnimation>();
            startGame.OnClickAsObservable().Subscribe(unit =>
            {
                StopTutorial();
            });
            eventBus = (EventBus) EventBus.instance;
            dialogSetting = IoC.inject.Get<DialogSettings>(this);
            if (dialogSetting == null)
            {
                dialogSetting = ResourcesV2.LoadScriptableObjectInstance<DialogSettings>("DialogSettings.asset");
                IoC.inject.SetSingleton(dialogSetting);
            }

            characterNameComponent.SetText(characterComponent.GetCharacterName());

            if (!autoStart || currentDialogInfo == null) return;

            this.ExecuteDelayed(() =>
            {
                currentIndex = 0;
                eventBus.Publish(startEventName);
                DOTween.Kill("tutorialSystem");
                StartDialogSystem();
            }, 1f);
        }

        [ButtonGroup("DialogControl")]
        public void PreviousDialog()
        {
            var temp = currentIndex - 1;
            if (temp < 0) return;
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo, temp);

        }

        [ButtonGroup("DialogControl")]
        public void StartDialogSystem(DialogInfo dialogInfo = null)
        {

            Init();
            currentIndex = 0;
            if (dialogInfo != null)
            {
                currentDialogInfo = dialogInfo;
            }
            if (!transform.parent.gameObject.activeInHierarchy)
            {
                transform.parent.gameObject.SetActive(true);
            }
            characterComponent.StartLoopAnimation();
            eventBus.Publish(startEventName);
            startTutorial?.Invoke();
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo);
        }

        [ButtonGroup("DialogControl")]
        public void NextDialog()
        {
            var temp = currentIndex + 1;
            if (temp >= currentDialogInfo.speeches.Count) return;
            dialogSequenceAnimation.Kill();
            StartDialog(currentDialogInfo, temp);
        }

        [ButtonGroup("DialogControl")]
        public void ReplayVoiceTutorial()
        {
            dialogSequenceAnimation.Kill();
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
                    textMeshComponent.SetText(string.Empty);
                    textMeshComponentHalfSized.SetText(string.Empty);
                });

                var speech = currentDialogInfo.speeches[index];
                if (speech.speechAudioClip.enabled && speech.speechAudioClip != null)
                {
                    dialogSequenceAnimation.AppendCallback(() =>
                        {
                            DeAudioManager.Play(DeAudioGroupId.Dialogue, speech.speechAudioClip.audioClip);
                            characterComponent.StartTalking();
                        });
                    dialogSequenceAnimation.Join(GetSelectedTextMeshComponent(speech).DOText(speech.speechText,
                        doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration).From(string.Empty));
                    AppendSpriteImage(speech, ref dialogSequenceAnimation);
                    dialogSequenceAnimation.AppendInterval(speech.speechAudioClip.audioClip.length);
                    dialogSequenceAnimation.AppendCallback(() => characterComponent.StopTalking());
                }
                else
                {
                    dialogSequenceAnimation.Append(GetSelectedTextMeshComponent(speech).DOText(speech.speechText,
                            doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration).From(string.Empty)
                        .OnStart(() => characterComponent.StartTalking()).OnComplete(() => characterComponent.StopTalking()));
                    AppendSpriteImage(speech, ref dialogSequenceAnimation);
                    //TODO add duration time default and by speech
                    dialogSequenceAnimation.AppendInterval(dialogSetting.textDefaultDuration);
                }

                dialogSequenceAnimation.AppendInterval(dialogSetting.timeBetweenSpeechs);

                dialogSequenceAnimation.AppendCallback(() => { currentIndex++; });

            }
            dialogSequenceAnimation.Play();
        }

        public void AppendSpriteImage(SpeechInfo speech, ref Sequence sequence)
        {
            if (textMeshComponentHalfSized.color.a >= 0.1f)
            {
                sequence.Join(textMeshComponentHalfSized.DOFade(0f, .3f));
            }
            if (!speech.speechSpriteAsset.enabled) return;
            sequence.Join(imageComponent.DOFade(1f, .3f).OnStart(() =>
                {
                    imageComponent.sprite = speech.speechSpriteAsset.spriteAsset;
                }).From(0f));
        }

        public TextMeshProUGUI GetSelectedTextMeshComponent(SpeechInfo speechInfo)
        {
            return speechInfo.speechSpriteAsset.enabled ? textMeshComponentHalfSized : textMeshComponent;
        }

        public void SetDialogInfo(DialogInfo dialogInfo)
        {
            currentDialogInfo = dialogInfo;
        }

        public void StopTutorial()
        {
            DeAudioManager.Stop(DeAudioGroupId.Dialogue);
            characterComponent.StopTalking();
            characterComponent.StopLoopAnimation();
            dialogSequenceAnimation.Kill();
            transform.parent.gameObject.SetActive(false);
            endTutorial?.Invoke();
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
