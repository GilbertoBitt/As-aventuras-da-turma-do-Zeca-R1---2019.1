using SQLite4Unity3d;

[System.Serializable]
public class DBOESTATISTICA_DIDATICA {

    [PrimaryKey][AutoIncrement]
    public int ID { get; set; }
    public int idUsuario {get; set;}
	public string dataInsert {get; set;}
	public int idGameDidatico {get; set;}
    public int idHabilidade { get; set; }
    public int idDificuldade {get; set;}
	public int idMinigame {get; set;}
	public int acertou {get; set;}
    public int online { get; set; }
	

    public DBOESTATISTICA_DIDATICA() {
        idUsuario = 0;
        dataInsert = "";
        idGameDidatico = 0;
        idHabilidade = 0;
        idDificuldade = 0;
        idMinigame = 0;
        acertou = 0;
        online = 0;
    }

}
