using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {

    public CharacterEnum character;
    public string name;
    public Sprite spriteImage;
    
}

public enum CharacterEnum {
    Zeca,
    Tati,
    Paulo,
    Manu,
    Joao,
    Bia,
}


