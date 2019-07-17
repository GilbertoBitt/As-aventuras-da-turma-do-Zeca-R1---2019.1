using UnityAtoms;
using UnityEngine;
using Void = UnityAtoms.Void;

namespace MiniGames.Rainbow.Scripts
{
    public class ShowDisplayItem : MonoBehaviour, IGameEventListener<Void>
    {
        public GameObject itemDisplay;
        public VoidEvent displayItemEvent;

        private void Start()
        {
            displayItemEvent.RegisterListener(this);
        }

        public void OnEventRaised(Void item)
        {
            itemDisplay.SetActive(true);
        }
    }
}
