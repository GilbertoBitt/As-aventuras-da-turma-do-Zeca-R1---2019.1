using SQLite;

[System.Serializable]
public class DBOVERSION {

    [PrimaryKey][NotNull][Unique]
    public int id { get; set; }
    public int version { get; set; }

}
