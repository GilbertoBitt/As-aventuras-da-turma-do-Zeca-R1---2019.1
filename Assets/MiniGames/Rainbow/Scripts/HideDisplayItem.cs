using UnityAtoms;
using UnityEngine;

namespace MiniGames.Rainbow.Scripts
{
    public class HideDisplayItem : MonoBehaviour, IGameEventListener<Void>
    {
        public GameObject itemDisplay;
        public VoidEvent hideDisplayEvent;

        private void Start()
        {
            hideDisplayEvent.RegisterListener(this);
        }

        public void OnEventRaised(Void item)
        {
            itemDisplay.SetActive(false);
        }
    }
}
