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
        public Image imageComponent;
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
        public Image backgroundImageComponent;
        public Sprite defaultBackgroundImage;

        private void Start()
        {

//            previousDialogButton.OnClickAsObservable().Subscribe(_ =>
//            {
//                PreviousDialog();
//                Debug.Log("Play Previous Speech");
//            });
//            nextDialogButton.OnClickAsObservable().Subscribe(_ =>
//            {
//                NextDialog();
//                Debug.Log("Play Next Speech");
//            });
//            replayDialogButton.OnClickAsObservable().Subscribe(_ =>
//            {
//                ReplayVoiceTutorial();
//                Debug.Log("Replay current Speech");
//            });
            Init();
        }

        public void Init()
        {
            if (_startInit) return;
            _startInit = true;

            previousDialogButton.SetOnClickAction(o =>
            {
                PreviousDialog();
            });

            nextDialogButton.SetOnClickAction(o =>
            {
                NextDialog();
            });

            replayDialogButton.SetOnClickAction(o =>
            {
                ReplayVoiceTutorial();
            });

            backgroundImageComponent = GetComponent(typeof(Image)) as Image;

            characterComponent = GetComponentInChildren<ICharacterAnimation>();

            characterComponent.StartDefaultAnimation();

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
                StartDialogSystemSprite(currentDialogInfo);
            }, 1f);
        }

        public void UpdateImageBackground(Sprite spriteImage)
        {
            if (backgroundImageComponent == null) return;
            backgroundImageComponent.sprite = spriteImage;
        }

        public void StartDialogSystem()
        {
            StartDialogSystem();
        }

        public void StartDialogSystemSprite(Sprite backgroundImage, DialogInfo dialogInfo = null)
        {
            if (backgroundImage == null)
            {
                backgroundImageComponent.enabled = false;
            }
            else
            {
                backgroundImageComponent.sprite = backgroundImage;
                backgroundImageComponent.enabled = true;
            }

            StartDialogSystem(dialogInfo);
        }

        public void StartDialogSystemSprite(DialogInfo dialogInfo = null)
        {
            if (backgroundImageComponent.sprite == null)
            {
                backgroundImageComponent.enabled = false;
            }
            StartDialogSystem(dialogInfo);
        }

        [ButtonGroup("DialogControl")]
        public void PreviousDialog()
        {
            var temp = currentIndex - 1;
            if (temp < 0)
                temp = 0;
            DOTween.Kill("tutorialSystem");
            if (currentDialogInfo.speeches[currentIndex].speechEventSystem != null && currentDialogInfo.speeches[currentIndex].speechEventSystem.enabled)
            {
                currentDialogInfo.speeches[currentIndex].speechEventSystem.stopEvent.Raise();
            }
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
            transform.parent.gameObject.SetActive(true);
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
            DOTween.Kill("tutorialSystem");
            StartDialog(currentDialogInfo, currentIndex);
        }

        private void StartDialog(DialogInfo dialogInfo, int currentDialogIndex = 0)
        {
            indexEnd = dialogInfo.speeches.Count-1;
            currentIndex = currentDialogIndex;
            //TODO adicionar animação do personagem piscando e falando.    
            dialogSequenceAnimation = DOTween.Sequence();
            dialogSequenceAnimation.SetId("tutorialSystem");
            dialogSequenceAnimation.AppendInterval(.3f);
            DeAudioManager.Stop(DeAudioGroupId.Dialogue);

            for (var index = currentDialogIndex; index < currentDialogInfo.speeches.Count; index++)
            {
                var speech = currentDialogInfo.speeches[index];
                dialogSequenceAnimation.AppendCallback(() =>
                {
                    previousDialogButton.gameObject.SetActive(currentIndex != 0);
                    nextDialogButton.gameObject.SetActive(currentIndex != indexEnd);
                    textMeshComponent.SetText(string.Empty);
                    textMeshComponentHalfSized.SetText(string.Empty);
                    EventSystemHandler(index);
                });


                if (speech.speechAudioClip.enabled && speech.speechAudioClip != null)
                {
                    dialogSequenceAnimation.AppendCallback(() =>
                        {
                            if (GameConfig.Instance.isAudioVoiceOn)
                            {
                                DeAudioManager.Play(DeAudioGroupId.Dialogue, speech.speechAudioClip.audioClip);
                                FadeAudiosOnVoiceTo(0.10f);
                            }
                            characterComponent.StartTalkingAnimation();
                        });
                    dialogSequenceAnimation.Join(GetSelectedTextMeshComponent(speech).DOText(speech.speechText,
                        doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration).From(string.Empty));
                    AppendSpriteImage(speech, ref dialogSequenceAnimation);
                    dialogSequenceAnimation.AppendInterval(speech.speechAudioClip.audioClip.length);
                    dialogSequenceAnimation.AppendCallback(() =>
                    {
                        characterComponent.StopTalkingAnimation();
                        FadeAudiosOnVoiceTo(1f);
                    });
                } else if (speech.speechEventSystem.enabled)
                {
                    dialogSequenceAnimation.AppendCallback(() =>
                    {
                        speech.speechEventSystem.startEvent.Raise();
                                endTutorial += () => { speech.speechEventSystem.stopEvent.Raise(); };
                        });
                    dialogSequenceAnimation.AppendInterval(speech.speechEventSystem.eventDelay.eventDelay);
                }
                else
                {
                    dialogSequenceAnimation.Append(GetSelectedTextMeshComponent(speech).DOText(speech.speechText,
                            doTextDuration.enabled ? doTextDuration.timeBetweenSpeechs : dialogSetting.doTextDuration).From(string.Empty)
                        .OnStart(() => characterComponent.StartTalkingAnimation()).OnComplete(() => characterComponent.StopTalkingAnimation()));
                    AppendSpriteImage(speech, ref dialogSequenceAnimation);
                    //TODO add duration time default and by speech
                    dialogSequenceAnimation.AppendInterval(dialogSetting.textDefaultDuration);
                }

                dialogSequenceAnimation.AppendInterval(dialogSetting.timeBetweenSpeechs);

                dialogSequenceAnimation.AppendCallback(() => { currentIndex++; });
            }

            dialogSequenceAnimation.OnComplete(() => { currentIndex = indexEnd; });
            dialogSequenceAnimation.Play();
        }

        private static void FadeAudiosOnVoiceTo(float volumeTo)
        {
            DeAudioManager.FadeTo(DeAudioGroupId.Music, volumeTo, .3f);
            DeAudioManager.FadeTo(DeAudioGroupId.Ambient, volumeTo, .3f);
            DeAudioManager.FadeTo(DeAudioGroupId.FX, volumeTo, .3f);
        }

        private void EventSystemHandler(int index)
        {
            var (hasItem, nextSpeechInfo) = GetNextSpeech(currentDialogInfo, index);
            if (hasItem && nextSpeechInfo.speechEventSystem.enabled)
                nextSpeechInfo.speechEventSystem.stopEvent.Raise();
        }

        private (bool, SpeechInfo) GetNextSpeech(DialogInfo dialogInfo, int index) => index + 1 >= dialogInfo.speeches.Count - 1 ? (hasNext: false,speechInfo: null) : (hasNext:true,speechInfo: dialogInfo.speeches[index + 1]);

        public void AppendSpriteImage(SpeechInfo speech, ref Sequence sequence)
        {
            sequence.Join(imageComponent.DOFade(0f, .3f).From(imageComponent.color.a));
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
            characterComponent.StopTalkingAnimation();
            characterComponent.StopDefaultAnimation();
            dialogSequenceAnimation.Kill();
            transform.parent.gameObject.SetActive(false);
            endTutorial?.Invoke();
            eventBus.Publish(stopEventName);
            FadeAudiosOnVoiceTo(1f);
        }


    }

    [Serializable, Toggle("enabled")]
    public class DoTextDuration
    {
        public bool enabled;
        public float timeBetweenSpeechs;
    }

}
