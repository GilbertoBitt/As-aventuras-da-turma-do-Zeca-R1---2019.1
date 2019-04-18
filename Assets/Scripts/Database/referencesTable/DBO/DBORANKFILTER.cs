using SQLite4Unity3d;

[System.Serializable]
public class DBORANKFILTER {

    [PrimaryKey, Unique]
    public int idMinigame { get; set; }
    public int idUsuario { get; set; }
    public int highscore { get; set; }
    public int idEscola { get; set; }
    public string nomeEscola { get; set; }


    public override string ToString() {
        return string.Format(
            "<color=#ffffff> -------------------------------------------------------------</color>\n" +
            "<color=#ffffff>idMinigame - </color><color=#4286f4>{0}</color>\n" +
            "<color=#ffffff>idUsuario - </color><color=#4286f4>{1}</color>\n" +
            "<color=#ffffff>highscore - </color><color=#4286f4>{2}</color>\n" +
            "<color=#ffffff>idEscola - </color><color=#4286f4>{3}</color>\n" +
            "<color=#ffffff>nomeEscola- </color><color=#4286f4>{4}</color>\n" +
            "<color=#ffffff> -------------------------------------------------------------</color>\n",
            idMinigame,
            idUsuario,
            highscore,
            idEscola,
            nomeEscola);
    }
}