using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;
using com.csutil;
using UniRx;
using UnityEditor;

public class GameConfig : ScriptableObject
{

    private static GameConfig _config;
    public static GameConfig Instance
    {
        get
        {
            if (_config == null)
            {
                _config = ResourcesV2.LoadScriptableObjectInstance<GameConfig>("GameConfig.asset");
                IoC.inject.SetSingleton(_config);
            }
            return _config;
        }
    }
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Principal")]
	#endif
	public string dbName;
    public string ClientName;
	public int clientID;
    public int gameID;
    public int sincModePerguntas;
    public int sincModeItens;
    public int usageCounter;
    public int usageLimit;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN

    [SeparatorAttribute("Informações do Usuario!")]
#endif
    public string nickname;
    public string namefull;
    public int playerID;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Informações de Conexão")]
	#endif
    public bool isOnline;
    public bool isVerifingNetwork;
    public string sessionID;
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Perfil Atual")]
	#endif
	public bool hasCredentials;
	public DBOUSUARIOS currentUser;
	public DBOCLIENTES currentClient;
	public DBOESCOLA currentSchool;
	public DBOTURMA currentClass;
	public DBOANOLETIVO currentYear;
	public DBOPONTUACAO currentScore;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Audio Config")]
	#endif
    public bool isAudioOn = true;
    public bool isAudioFXOn = true;
    public bool isAudioVoiceOn = true;

    public BoolReactiveProperty isAudioOnReactive = new BoolReactiveProperty(true);
    public BoolReactiveProperty isAudioFxOnReactive = new BoolReactiveProperty(true);
    public BoolReactiveProperty isAudioVoiceOnOnReactive = new BoolReactiveProperty(true);

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("BROPS")]
	#endif
    public int BropsAmount;
    public int BropsDeviceAmount;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("TOTAL POINTS")]
	#endif
    public int TotalPoints;
    public int TotalPointsDevice;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Minigames")]
	#endif
    public List<Minigames> allMinigames = new List<Minigames>();
    public List<MinigameStruct> minigames = new List<MinigameStruct>();

	public List<Action> queueLog = new List<Action>();
	public DBOMINIGAMES_LOGS LOG = new DBOMINIGAMES_LOGS();
    public networkHelper netHelper;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("queue")]
#endif
    public List<int> itensIDToDownloadImage = new List<int>();
    public List<int> QuestionIDtoDownload = new List<int>();
    public List<int> AnswersIDtoDoownload = new List<int>();

    [Header("LinksTo")]
	public string ForgetUserOrPasswordLink;
	DataService db;

    [TextArea]
    public string GameKey;
    private string encryptKey;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("Store")]
#endif
    public string mainMediaDirectory;
    public string destinationFolder;
    public string audioDestination;
    public string fullPatchItemIcon;
    public string fullAudioClipDestinationAnswers;
    public string fullAudioClipDestinationQuestions;

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
    [SeparatorAttribute("SQLITE")]
#endif
    public int sqliteVersion = 0;
    [TextArea]
    public string[] querys;
    
    
    [TextArea]
    public string jsonToMd5Array;
    
    [TextArea]
    public string jsonResult;

    [ContextMenu("Do Json")]
    public void ToJson() {
        jsonResult = "";
        var nethelp = new networkHelper();
        jsonResult = nethelp.GenerateHashInJson(jsonToMd5Array);
    }

    void Start()
    {	
        //Carregar Audio de PlayerPrefs.
        isAudioOn = GetBool("isAudioOn");
        isAudioFXOn = GetBool("isAudioOn");
        isAudioVoiceOn = GetBool("isAudioOn");

       // BropsAmount = PlayerPrefs.GetInt("BropsAmount");
    }
    public int GetAnoLetivo()
    {
        UpdateCurrent(GetCurrentUser());
        var r = currentYear ?? new DBOANOLETIVO{idAnoLetivo = 1};
        return r.idAnoLetivo;
    }

    public void SavePrefs(){
        //Salvar Audio em PlayerPrefs.
        SetBool("isAudioOn", isAudioOn);
        SetBool("isAudioFXOn", isAudioFXOn);
        SetBool("isAudioVoiceOn", isAudioVoiceOn);
       // PlayerPrefs.SetInt("BropsAmount", BropsAmount);
        PlayerPrefs.Save();
    }

    //Boolean Workaround para PlayerPrefs.
    public static void SetBool(string key, bool state)
    {
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }

    public static bool GetBool(string key)
    {
        var value = PlayerPrefs.GetInt(key, 1);

        return value == 1;
    }

   /* public void resetJogador()
    {
        jogador = new DBJogador();
        jogador.IdJogador = -1;
    }*/


    public DataService OpenDb() => db ?? (db = new DataService(dbName));

    public void OnUpgrade(DBOVERSION dbv) {
        for (int i = dbv.version; i < sqliteVersion; i++) {            
            db.RunQuery(querys[i]);
            Debug.LogWarning("Executando Query: " + querys[i]);
        }
        dbv.version = sqliteVersion;
        //PlayerPrefs.SetInt("sqliteVersion", sqliteVersion);
        db.UpdateDBVersion(dbv);
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    public void OnValidate()
    {
        if (dbName.Contains(".db3") == false)
        {
            dbName += ".db3";
        }
    }

	/// <summary>
	/// Updates the current.
	/// </summary>
	/// <param name="client">Client.</param>
	/// <param name="school">School.</param>
	/// <param name="year">Year.</param>
	/// <param name="classe">Classe.</param>
	/// <param name="user">User.</param>
	public void UpdateCurrent(DBOESCOLA school, DBOANOLETIVO year, DBOTURMA classe, DBOUSUARIOS user){
		currentSchool = school;
		currentYear = year;
		currentUser = user;
		currentClass = classe;
		currentScore = OpenDb ().GetScore (currentUser.idUsuario);
        playerID = currentUser.idUsuario;
        nickname = currentUser.login;
        namefull = currentUser.nomeJogador;
	}

	/// <summary>
	/// Updates the current.
	/// </summary>
	/// <param name="user">User.</param>
	public void UpdateCurrent(DBOUSUARIOS user){
		currentUser = user;
        if (currentUser == null) return;
        var getTurmaTask = OpenDb().GetClass(currentUser.idTurma);
        currentClass = getTurmaTask;
        if (currentClass == null) return;
        
        var getCurrentClassTask = OpenDb().GetYears(currentClass.idAnoLetivo);
        currentYear = getCurrentClassTask;
        currentSchool = OpenDb().GetSchool(currentClass.idEscola);
//        if (currentYear == null) return;
//        var getCurrentSchoolTask = openDB().GetClient(currentSchool.idCliente);
//        currentClient = getCurrentSchoolTask;
    }

    public void UpdateScore(DBOUSUARIOS user) {
            currentUser = user;
            nickname = currentUser.login;
            namefull = currentUser.nomeJogador;
            playerID = currentUser.idUsuario;
            /*currentScore = db.GetScore(user.idUsuario);
            if (currentScore == null) {
            currentScore = new DBOPONTUACAO() {
                idUsuario = user.idUsuario
            };
            }
           // BropsAmount = currentScore.brops;
           // TotalPoints = currentScore.pontuacaoTotal;*/

    }  

	public void UpdateMinigames(){
		var LocalDB = OpenDb();
		int countTemp = allMinigames.Count;
		for (int i = 0; i < countTemp; i++) {
			int id = i + 1;
			DBORANKING thisRank = new DBORANKING();
			if (hasCredentials) {
				thisRank = LocalDB.GetRanking(id, playerID);
			}
			if (thisRank != null) {
				allMinigames[i].highscore = thisRank.highscore;
                allMinigames[i].stars = thisRank.estrelas;

            } else {
				var Rank = new DBORANKING();
				Rank.highscore = 0;
				allMinigames[i].highscore = 0;
                allMinigames[i].stars = 0;
                Rank.idMinigame = id;
				Rank.idUsuario = GetCurrentUserID();
				LocalDB.InsertRanking(Rank);
            }
		}

	}

    public DBORANKING GetGameInfo(int minigameID) {
        if (currentUser != null) {
            int userID = currentUser.idUsuario;

            DBORANKING rank = OpenDb().GetRanking(minigameID, userID) ?? new DBORANKING() {
                highscore = 0,
                idMinigame = minigameID,
                idUsuario = userID
            };

            return rank;
        } else {
            return new DBORANKING();
        }
    }

	public string GetCharName(int characterSelected){
		switch (characterSelected) {
			case 0:
				return "Zeca";
				break;
			case 1:
				return "Tati";
				break;
			case 2:
				return "Paulo";
				break;
			case 3:
				return "Manu";
				break;
			case 4:
				return "Joao";
				break;
			case 5:
				return "Bia";
				break;
			default:
				return "Zeca";
				break;
		}
	}

	public void SaveLOG(){
		OpenDb().InsertJogosLog(LOG);
	}

	public void SaveLOG(DBOMINIGAMES_LOGS _log){		
        if(currentUser != null) {
            _log.idUsuario = currentUser.idUsuario;
        }		

        if (isOnline) {
            netHelper.SetJogosLogM(_log);
        } else {
            _log.online = 0;
            OpenDb().InsertJogosLog(_log);
        };
        /*JogosLogSerializeble glog = new JogosLogSerializeble();
        glog.token = netHelper.token;
        glog.log = _log;
        glog.log.idUsuario = currentUser.idUsuario;           
        jogosLog.Add(glog);*/
     }

	public void Rank(int _idMinigame, int score, int _starsAmount) {

        var eduqbrinqLog = EduqbrinqLogger.Instance;
    
		DBORANKING rank = OpenDb().GetRanking (_idMinigame, GetCurrentUserID()) ?? new DBORANKING();

        bool hasUpdated = false;

		if (rank.highscore < score) {
            rank.highscore = score;
            hasUpdated = true;
            Log.d("Highscore Atualizado");
        }

        if(rank.estrelas < _starsAmount) {
            rank.estrelas = _starsAmount;
            hasUpdated = true;
            Log.d("Estrelas Atualizadas");
        }

        rank.dataUpdate = ReturnCurrentDate();
        rank.idMinigame = _idMinigame;
        rank.idUsuario = playerID;

        if (!hasUpdated) return;
        eduqbrinqLog.UpdateDatabase(rank);
    }

    //TODO Updated Score

    public void UpdateScore(int scoreBrops, int pontuacaoTotal) {

        DBOPONTUACAO scores = OpenDb().GetScore(playerID) ?? new DBOPONTUACAO();

        scores.idUsuario = playerID;
        scores.brops = scoreBrops;
        scores.pontuacaoTotal = pontuacaoTotal;
        scores.BropsDevice = BropsDeviceAmount;
        scores.PontuacaoTotalDevice = TotalPointsDevice;
        Debug.Log("scores.brops " + scores.brops);
        Debug.Log("scores.pontuacaoTotal " + scores.pontuacaoTotal);
        scores.dataUpdate = ReturnCurrentDate();

        EduqbrinqLogger.Instance.UpdateDatabase(scores);
    }

	public void SaveStatistic(List<DBOESTATISTICA_DIDATICA> statisticTemp){
		OpenDb().InsertAllStatistic(statisticTemp);
	}

    public void SaveAllStatistic(List<DBOESTATISTICA_DIDATICA> statisticsTemp) {
      
        if (isOnline) {
            netHelper.RunStatistics(statisticsTemp);            
        } else {
            OpenDb().InsertAllStatistic(statisticsTemp);
        }
    }



	public int TimeToIntMilliseconds(float time){
		float temp = time * 1000;
		int resultInte = Mathf.FloorToInt (temp);
		return resultInte;
	}

	public string ReturnCurrentDate(){
        return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

	public int GetCurrentUserID(){
		return currentUser?.idUsuario ?? -1;
	}

    public DBOUSUARIOS GetCurrentUser() => currentUser ?? OpenDb().GetDefaultUser();

    [ContextMenu("Encrypt")]
    public void EncryptText() {

        string encryptText = GameKey;
        encryptText = Encrypt("Chave", encryptText);
        GameKey = Encrypt("Chave", encryptText);
    }

    //[ContextMenu("Decrypt")]
    public string returnDecryptKey() {
        string decryptText = Decrypt("Chave", GameKey);
        string decryptTheDecrypt = Decrypt("Chave", decryptText);
        //Debug.Log(decryptTheDecrypt);
        return decryptTheDecrypt;
    }

    public string Encrypt(string key, string data) {
        string encData = null;
        byte[][] keys = GetHashKeys(key);

        try {
            encData = EncryptStringToBytes_Aes(data, keys[0], keys[1]);
        } catch (CryptographicException) { } catch (ArgumentNullException) { }

        return encData;
    }

    public string Decrypt(string key, string data) {
        string decData = null;
        byte[][] keys = GetHashKeys(key);

        try {
            decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
        } catch (CryptographicException) { } catch (ArgumentNullException) { }

        return decData;
    }

    private byte[][] GetHashKeys(string key) {
        byte[][] result = new byte[2][];
        Encoding enc = Encoding.UTF8;

        SHA256 sha2 = new SHA256CryptoServiceProvider();

        byte[] rawKey = enc.GetBytes(key);
        byte[] rawIV = enc.GetBytes(key);

        byte[] hashKey = sha2.ComputeHash(rawKey);
        byte[] hashIV = sha2.ComputeHash(rawIV);

        Array.Resize(ref hashIV, 16);

        result[0] = hashKey;
        result[1] = hashIV;

        return result;
    }

    private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV) {
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        byte[] encrypted;

        using (AesManaged aesAlg = new AesManaged()) {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream()) {
                using (CryptoStream csEncrypt =
                        new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return Convert.ToBase64String(encrypted);
    }

    //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
    private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV) {
        byte[] cipherText = Convert.FromBase64String(cipherTextString);

        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        string plaintext = null;

        using (Aes aesAlg = Aes.Create()) {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
                using (CryptoStream csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

    [MethodImplAttribute(MethodImplOptions.NoInlining)]
    public void ShowDestinationItemImage() {
        CreateDirectoryNeeded();
    }

    [MethodImplAttribute(MethodImplOptions.NoInlining)]
    public void CreateDirectoryNeeded(){


        string mainDirectory = string.Format("{0}/{1}", Application.persistentDataPath, "medias");
        if (!Directory.Exists(mainDirectory)) {
            Directory.CreateDirectory(mainDirectory);
            mainMediaDirectory = mainDirectory;
        }

        fullAudioClipDestinationQuestions = string.Format("{0}/{1}", mainDirectory, "perguntas/");
        if (!Directory.Exists(fullAudioClipDestinationQuestions)){
            Directory.CreateDirectory(fullAudioClipDestinationQuestions);
        }

        fullAudioClipDestinationAnswers = string.Format("{0}/{1}", mainDirectory, "respostas/");
        if (!Directory.Exists(fullAudioClipDestinationAnswers)) {
            Directory.CreateDirectory(fullAudioClipDestinationAnswers);
        }


        fullPatchItemIcon = string.Format("{0}/{1}", mainDirectory, "item/");
        if (!Directory.Exists(fullPatchItemIcon)) {
            Directory.CreateDirectory(fullPatchItemIcon);
        }

    }
    

    public void TestDownloadImage() {
        Directory.CreateDirectory(fullPatchItemIcon);
        //netHelper.LoadImageItemIcon(1);
    }

    public void AddOrReplateMinigame(MinigameStruct  _newMinigame){

        int _tempCount = minigames.Count;
        bool minigameExist = false;

        for (int i = 0; i < _tempCount; i++){
            if(minigames[i].idMinigame == _newMinigame.idMinigame){
                minigames[i] = _newMinigame;
                minigameExist = true;
            }
        }

        if(minigameExist == false){
            minigames.Add(_newMinigame);
        }


    }


    public void UpdateAll(int _idUser) {
        if (currentUser != null) return;
        var taskUser = OpenDb().GetUser(_idUser);
        currentUser = taskUser;
        if (currentClass != null) return;
        var taskClass = OpenDb().GetClass(currentUser.idTurma);
        currentClass = taskClass;
        if (currentSchool != null) return;
        var taskSchool = OpenDb().GetSchool(currentClass.idEscola);
        currentSchool = taskSchool;
    }

    public MinigameStruct GetMinigameID(int _idMinigame) {

        int countTemp = minigames.Count;


        for (int i = 0; i < countTemp; i++) {
            if(minigames[i].idMinigame == _idMinigame){
                return minigames[i];
            }
        }

        return null;        

    }

}
	