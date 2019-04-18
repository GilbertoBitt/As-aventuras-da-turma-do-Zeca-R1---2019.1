﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Security;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

public class GameConfig : ScriptableObject {


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

        if (value == 1)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

   /* public void resetJogador()
    {
        jogador = new DBJogador();
        jogador.IdJogador = -1;
    }*/


    public DataService openDB()
    {
        db = new DataService(dbName);

        //Only SQLITE COMMAND.
        db.removeLinq = true;

        var dbv = db.GetDBVersion();

        if(dbv == null) {
            dbv = new DBOVERSION() {
                id = 1,
                version = sqliteVersion
            };
        }

        int temp = dbv.version;
        if (temp < sqliteVersion) {
           // db = new DataService(dbName);

            

            OnUpgrade(dbv);
            Debug.Log("Updating SQLITE: " + temp + " to " + sqliteVersion);
        }

        
        return db;
    }

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
		//currentUser = user;
		currentScore = openDB ().GetScore (currentUser.idUsuario);
		//BropsAmount = currentScore.brops;
		//TotalPoints = currentScore.pontuacaoTotal;
        playerID = currentUser.idUsuario;
        nickname = currentUser.login;
        namefull = currentUser.nomeJogador;
	}

	/// <summary>
	/// Updates the current.
	/// </summary>
	/// <param name="user">User.</param>
	public void UpdateCurrent(DBOUSUARIOS user){
		DataService dbi = openDB();
		currentUser = user;
        if (currentUser != null) {
            currentClass = dbi.GetClass(currentUser.idTurma);
        }
        if (currentClass != null) {
            currentYear = dbi.GetYears(currentClass.idAnoLetivo);
        }
        if (currentSchool != null) {
            currentClient = dbi.GetClient(currentSchool.idCliente);
        }

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
		DataService db = openDB();
		int countTemp = allMinigames.Count;
		for (int i = 0; i < countTemp; i++) {
			int id = i + 1;
			DBORANKING thisRank = new DBORANKING();
			if (hasCredentials) {
				thisRank = db.GetRanking(id, playerID);
			}
			if (thisRank != null) {
				allMinigames[i].highscore = thisRank.highscore;
                allMinigames[i].stars = thisRank.estrelas;

            } else {
				DBORANKING Rank = new DBORANKING();
				Rank.highscore = 0;
				allMinigames[i].highscore = 0;
                allMinigames[i].stars = 0;
                Rank.idMinigame = id;
				Rank.idUsuario = GetCurrentUserID();
				db.InsertRanking(Rank);
            }
		}

	}

    public DBORANKING GetGameInfo(int minigameID) {
        if (currentUser != null) {
            int userID = currentUser.idUsuario;

            DBORANKING rank = openDB().GetRanking(minigameID, userID);
            if(rank == null) {
                rank = new DBORANKING() {
                    highscore = 0,
                    idMinigame = minigameID,
                    idUsuario = userID
                };
            }

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
		openDB().InsertJogosLog(LOG);
	}

	public void SaveLOG(DBOMINIGAMES_LOGS _log){		
        if(currentUser != null) {
            _log.idUsuario = currentUser.idUsuario;
        }		

        if (isOnline) {
            netHelper.SetJogosLogM(_log);
        } else {
            _log.online = 0;
            openDB().InsertJogosLog(_log);
        };
            /*JogosLogSerializeble glog = new JogosLogSerializeble();
            glog.token = netHelper.token;
            glog.log = _log;
            glog.log.idUsuario = currentUser.idUsuario;           
            jogosLog.Add(glog);*/
     }

	public void Rank(int _idMinigame, int score, int _starsAmount) {

		DBORANKING rank = openDB().GetRanking (_idMinigame, GetCurrentUserID());

		if (rank != null) {
            Debug.Log(rank.ToString());
            bool hasUpdated = false;

			if (rank.highscore < score) {
                rank.highscore = score;
                rank.dataUpdate = ReturnCurrentDate();
                rank.idMinigame = _idMinigame;
                rank.idUsuario = playerID;
                hasUpdated = true;
            }

            if(rank.estrelas < _starsAmount) {
                rank.estrelas = _starsAmount;
                rank.idMinigame = _idMinigame;
                rank.idUsuario = playerID;
                rank.dataUpdate = ReturnCurrentDate();
                hasUpdated = true;
            }

            if (hasUpdated) {
               
                if (isOnline) {
                    rank.online = 1;
                    netHelper.RunSetRanking(rank, netHelper.token);
                    openDB().UpdateRanking(rank);
                } else {
                    rank.online = 0;
                    openDB().UpdateRanking(rank);
                }
            }
        } else {
            rank = new DBORANKING() {
                highscore = score,
                idMinigame = _idMinigame,
                estrelas = _starsAmount,
                idUsuario = playerID,
                dataInsert = ReturnCurrentDate(),
                dataUpdate = ReturnCurrentDate()
            };
            
            if (isOnline) {
                rank.online = 1;
                netHelper.RunSetRanking(rank, netHelper.token);
                openDB().InserRanking2(rank);
            } else {
                rank.online = 0;
                openDB().InserRanking2(rank);
            }
        }

        
    }

    public void UpdateScore(int _scoreBrops, int PontuacaoTOtal) {

        DBOPONTUACAO scores = openDB().GetScore(playerID);

        if (scores != null) {
            scores.idUsuario = playerID;
            scores.brops = _scoreBrops;
            scores.pontuacaoTotal = PontuacaoTOtal;
            scores.BropsDevice = BropsDeviceAmount;
            scores.PontuacaoTotalDevice = TotalPointsDevice;
            Debug.Log("scores.brops " + scores.brops);
            Debug.Log("scores.pontuacaoTotal " + scores.pontuacaoTotal);
            scores.dataUpdate = ReturnCurrentDate();
            openDB().UpdateScore(scores);
        } else {
            scores = new DBOPONTUACAO() {
                idUsuario = playerID
            };
            scores.brops = _scoreBrops;
            scores.pontuacaoTotal = PontuacaoTOtal;
            scores.BropsDevice = BropsDeviceAmount;
            scores.PontuacaoTotalDevice = TotalPointsDevice;
            Debug.Log("scores.brops " + scores.brops);
            Debug.Log("scores.pontuacaoTotal " + scores.pontuacaoTotal);
            scores.dataUpdate = ReturnCurrentDate();
            openDB().InsertScores(scores);
        }

        if (isOnline) {
            netHelper.RunsetPontuacao(scores, netHelper.token);
        } else {
            scores.online = 0;
            openDB().InsertOrReplateScore(scores);
           // queueLog.Add(() => netHelper.RunsetPontuacao(scores, netHelper.token));
        }
    }

	public void SaveStatistic(List<DBOESTATISTICA_DIDATICA> statisticTemp){
		openDB().InsertAllStatistic(statisticTemp);
	}

    public void SaveAllStatistic(List<DBOESTATISTICA_DIDATICA> statisticsTemp) {
      
        if (isOnline) {
            netHelper.RunStatistics(statisticsTemp);            
        } else {
            openDB().InsertAllStatistic(statisticsTemp);
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
		return (currentUser != null) ? currentUser.idUsuario : -1;
	}

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
        if(currentUser == null) {
            currentUser = openDB().GetUser(_idUser);
        }

        if (currentClass == null) {
            currentClass = openDB().GetClass(currentUser.idTurma);
        }

        if(currentSchool == null) {
            currentSchool = openDB().GetSchool(currentClass.idEscola);
        }

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
	