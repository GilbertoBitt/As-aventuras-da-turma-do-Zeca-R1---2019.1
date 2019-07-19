using DG.DeAudio;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PauseWindow.Scripts
{
    public class PauseWindowManager : MonoBehaviour
    {
        private static PauseWindowManager _internalInstance;
        public static PauseWindowManager Instance
        {
            get
            {
                if (_internalInstance == null)
                {
                    _internalInstance = FindObjectOfType(typeof(PauseWindowManager)) as PauseWindowManager;
                }

                return _internalInstance;
            }
        }

        [BoxGroup("Music Button")] public Button musicButton;
        [BoxGroup("Music Button")] public Image musicImage;
        [BoxGroup("FX Button")] public Button effectxButton;
        [BoxGroup("FX Button")] public Button effectxImage;
        [BoxGroup("Dialogue Button")] public Button voiceButton;
        [BoxGroup("Dialogue Button")] public Button voiceImage;

        public void Start()
        {
            musicButton.OnClickAsObservable().Subscribe(_ =>
                {
                    GameConfig.Instance.isAudioOn = !GameConfig.Instance.isAudioOn;
                    musicImage.DOFade(GameConfig.Instance.isAudioOn ? 0f : 1f, .6f).SetEase(Ease.InOutSine);
                });
        }

        public void SetConfigAudioSetting(
            ref BoolReactiveProperty targetAudio,
            ref Image targetImage,
            ref DeAudioGroupId targetGroup)
        {
            targetAudio.Value = !targetAudio.Value;
            targetImage.DOFade(targetAudio.Value ? 0f : 1f, .6f).SetEase(Ease.InOutSine);
        }
    }
}
