using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace TutorialSystem.Scripts
{
    public class DialogSplitter : MonoBehaviour
    {
        [TableList]
        public List<DialogInfoData> dialogList = new List<DialogInfoData>();

        public DialogInfo dialog => dialogList.First(x => x.anoLetivo == GameConfig.Instance.GetAnoLetivo()).dialogInfo;
    }

    [System.Serializable]
    public class DialogInfoData
    {
        [TableColumnWidth(30, Resizable = false)]
        public int anoLetivo;
        public DialogInfo dialogInfo;
    }
}
