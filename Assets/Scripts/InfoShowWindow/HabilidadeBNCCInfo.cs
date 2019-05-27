using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(menuName = "Eduqbrinq/BNCC/Habilidade Info", fileName = "Habilidade")]
public class HabilidadeBNCCInfo : ScriptableObject
{
    public string codigo;
    public int id;
    [TextArea(3,50)]
    public string description;

    #if UNITY_EDITOR
    [Button("Salvar Informações")]
    public void SaveInfo()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}
