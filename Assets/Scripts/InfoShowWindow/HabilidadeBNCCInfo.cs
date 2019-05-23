using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Eduqbrinq/BNCC/Habilidade Info", fileName = "Habilidade")]
public class HabilidadeBNCCInfo : ScriptableObject
{
    public string codigo;
    public int id;
    [TextArea(3,50)]
    public string description;
}
