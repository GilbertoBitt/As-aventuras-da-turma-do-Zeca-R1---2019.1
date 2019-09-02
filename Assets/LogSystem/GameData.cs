using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData.asset", menuName = "Eduqbrinq/GameData")]
public class GameData : ScriptableObject
{
    public int id;
    public int idHabilidade;
    [TextArea(2,20)]
    [FoldoutGroup("1ª Ano")]
    [LabelText("Descrição")]
    public string description1ano;
    [TextArea(2,20)]
    [FoldoutGroup("2ª Ano")]
    [LabelText("Descrição")]
    public string description2ano;
    [TextArea(2,20)]
    [FoldoutGroup("3ª Ano")]
    [LabelText("Descrição")]
    public string description3ano;
}
