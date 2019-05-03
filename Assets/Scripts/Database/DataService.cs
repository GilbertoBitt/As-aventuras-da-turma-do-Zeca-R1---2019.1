using UnityEngine;
using System.Collections;
using System.Linq;
using System.Globalization;
//using SQLite;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using com.csutil;
using UniRx.Async;
#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
using UnityEditor;
#endif

public class DataService {

    public bool removeLinq = false;

	private SQLiteConnection _connection;
    private SQLiteAsyncConnection _AsyncConnection;
	
	public DataService(string DatabaseName){
        // check if file exists in Application.persistentDataPath
		var filepath = $"{Application.persistentDataPath}/{DatabaseName}";
		if (File.Exists(filepath)){
        	_connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _AsyncConnection = new SQLiteAsyncConnection(filepath);
            //Debug.Log("<color=#ffffff>Final PATH:</color> <color=#33bfea>" + filepath + "</color>"); 
		} else {
    #if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_STANDALONE
            var _loaddb = $"{Application.streamingAssetsPath}/{DatabaseName}";
            Debug.Log(_loaddb);
            var filepathTo = $"{Application.persistentDataPath}/{DatabaseName}";
            var bytesTOLoad = File.ReadAllBytes(_loaddb);
            Debug.Log(filepathTo);
            //File.Copy(_loaddb,filepathTo,true);
            File.WriteAllBytes(filepathTo, bytesTOLoad);
            filepath = filepathTo;
#elif UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
           var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
           // then save to Application.persistentDataPath
           File.Copy(loadDb, filepath);
#elif UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_STANDALONE
            var _loaddb = string.Format("{0}/{1}", Application.streamingAssetsPath, DatabaseName);
            Debug.Log(_loaddb);            
            var filepathTo = string.Format("{0}/{1}", Application.persistentDataPath,DatabaseName);
            var bytesTOLoad = File.ReadAllBytes(_loaddb);
            Debug.Log(filepathTo);
            //File.Copy(_loaddb,filepathTo,true);
            File.WriteAllBytes(filepathTo, bytesTOLoad);
            filepath = filepathTo;
#endif
            _connection = new SQLiteConnection(filepath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            _AsyncConnection = new SQLiteAsyncConnection(filepath, key: "eduqbrinqlp");
            //Debug.Log("<color=#ffffff>Final PATH:</color> <color=#33bfea>" + filepath + "</color>"); 
		}       

	}

    #region DBOUSUARIO

	    /// <summary>
	    /// Gets the user.
	    /// </summary>
	    /// <returns>The user.</returns>
	    /// <param name="login">Login.</param>
	    public DBOUSUARIOS GetUser(string loginUser){
		    return _AsyncConnection.Table<DBOUSUARIOS>().Where(x => x.login == loginUser).FirstOrDefaultAsync().Result;
            //test                           
	    }

        public DBOUSUARIOS GetUser(int userID) {
            return _AsyncConnection.Table<DBOUSUARIOS>().Where(x => x.idUsuario == userID).FirstOrDefaultAsync().Result;
        }


        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <returns>The user list.</returns>
        /// <param name="ClassID">Class I.</param>
        public List<DBOUSUARIOS> GetUserList (int ClassID){
            var query = _AsyncConnection.Table<DBOUSUARIOS>().Where(x => x.idTurma == ClassID);
            var result = query.ToListAsync();
            var resultList = result.Result;
            return resultList;
	    }

       
        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="USER">USE.</param>
        public void UpdateUser(DBOUSUARIOS USER){
		    _AsyncConnection.UpdateAsync(USER);
	    }

        public void InsertUser(DBOUSUARIOS user) {
            _AsyncConnection.InsertOrReplaceAsync(user);
        }

    #endregion

    #region DBOPONTUACAO

	    /// <summary>
	    /// Gets the score.
	    /// </summary>
	    /// <returns>The score.</returns>
	    /// <param name="userID">User I.</param>
	    public DBOPONTUACAO GetScore(int userID){
		    DBOPONTUACAO score = _AsyncConnection.Table<DBOPONTUACAO>().Where(x => x.idUsuario == userID).FirstOrDefaultAsync().Result ??
                                 new DBOPONTUACAO();
            return score;
	    }

        public List<DBOPONTUACAO> GetallScoresOffline() {
            IEnumerable<DBOPONTUACAO> inventory = _AsyncConnection.Table<DBOPONTUACAO>().Where(x => x.online == 0).ToListAsync().Result;
            return inventory.ToList();
        }

        public void DeleteScore(DBOPONTUACAO _delete) {
            _AsyncConnection.DeleteAsync(_delete);
        }



        /// <summary>
        /// Update Score of user.
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore(DBOPONTUACAO score) {
            score.dataUpdate = ReturnCurrentDate();
            InsertOrReplateScore(score);
        }

        public void InsertScores(DBOPONTUACAO scory) {
            scory.dataUpdate = ReturnCurrentDate();
            InsertOrReplateScore(scory);
        }

        public void InsertOrReplateScore(DBOPONTUACAO _score) {
            //Connection.Query("UPDATE Book SET Name = '" + inputfield.text + "' WHERE Id = " + intBookNo);
//            object[] argss = new object[7];
//            argss[0] = _score.idUsuario;
//            argss[1] = _score.brops;
//            argss[2] = _score.pontuacaoTotal;
//            argss[3] = _score.dataUpdate;
//            argss[4] = _score.BropsDevice;
//            argss[5] = _score.PontuacaoTotalDevice;
//            argss[6] = _score.online;
            //object[] argss02 = new object[6];


//            String command01 = "REPLACE INTO DBOPONTUACAO (idUsuario, brops, pontuacaoTotal, dataUpdate, BropsDevice, PontuacaoTotalDevice, online) VALUES(?,?,?,?,?,?,?);";
            //String commandFull = command01 + command02;
            // _connection.Query(commansd, argss);
            //_connection.CreateCommand(commansd,argss);
//            _connection.Execute(command01, argss);
            //_connection.Execute(command02, argss2);
            _AsyncConnection.InsertOrReplaceAsync(_score);
        }

        public void UpdateToOnlineScore(DBOPONTUACAO _score)
        {
            _AsyncConnection.UpdateAsync(_score);
//            //Connection.Query("UPDATE Book SET Name = '" + inputfield.text + "' WHERE Id = " + intBookNo);
//            object[] argss = new object[7];
//            argss[0] = _score.idUsuario;
//            argss[1] = _score.brops;
//            argss[2] = _score.pontuacaoTotal;
//            argss[3] = _score.dataUpdate;
//            argss[4] = _score.BropsDevice;
//            argss[5] = _score.PontuacaoTotalDevice;
//            argss[6] = 1;
//            //object[] argss02 = new object[6];
//
//
//            String command01 = "REPLACE INTO DBOPONTUACAO (idUsuario, brops, pontuacaoTotal, dataUpdate, BropsDevice, PontuacaoTotalDevice, online) VALUES(?,?,?,?,?,?,?);";
//            //String commandFull = command01 + command02;
//            // _connection.Query(commansd, argss);
//            //_connection.CreateCommand(commansd,argss);
//            _connection.Execute(command01, argss);
//            //_connection.Execute(command02, argss2);
        }

        #endregion

    #region DBESCOLA

    /// <summary>
    /// Gets the school list.
    /// </summary>
    /// <returns>The school list.</returns>
    /// <param name="idCliente">Identifier cliente.</param>
    public List<DBOESCOLA> GetSchoolList (int idCliente){
		IEnumerable<DBOESCOLA> result = _AsyncConnection.Table<DBOESCOLA>().Where(x => x.idCliente == idCliente).ToListAsync().Result;
		List<DBOESCOLA> resultList = result.ToList();
		return resultList;
	}


	/// <summary>
	/// Gets the school.
	/// </summary>
	/// <returns>The school.</returns>
	/// <param name="idEscola">Identifier escola.</param>
	public DBOESCOLA GetSchool (int idEscola){
		var result = _AsyncConnection.Table<DBOESCOLA>().Where(x => x.idEscola == idEscola).FirstOrDefaultAsync().Result;
		return result;
	}



    public void AddAllEscolas(List<DBOESCOLA> escolas) {
        foreach (var dboescola in escolas)
        {
            _AsyncConnection.InsertOrReplaceAsync(dboescola);
        }
    }

#endregion

    #region DBOCLIENTE

	    /// <summary>
	    /// Gets the name of the client.
	    /// </summary>
	    /// <returns>The client name.</returns>
	    /// <param name="clientID">Client I.</param>
	    public string GetClientName(int clientID)
        {
            var cliente = _AsyncConnection.Table<DBOCLIENTES>().Where(x => x.idCliente == clientID).FirstOrDefaultAsync().Result;
            return cliente != null ? cliente.nomeCliente : "";
        }

	    /// <summary>
	    /// Gets the client.
	    /// </summary>
	    /// <returns>The client.</returns>
	    /// <param name="clientID">Client I.</param>
	    public DBOCLIENTES GetClient(int clientID){
		    DBOCLIENTES cliente = _AsyncConnection.Table<DBOCLIENTES>().Where(x => x.idCliente == clientID).FirstOrDefaultAsync().Result;
		    return cliente;
	    }

    #endregion

    #region DBOANOLETIVO

	    /// <summary>
	    /// Gets the years list.
	    /// </summary>
	    /// <returns>The years list.</returns>
	    /// <param name="SchoolID">School ID.</param>
	    public List<DBOANOLETIVO> GetYearsList (){
            IEnumerable<DBOANOLETIVO> result = _AsyncConnection.Table<DBOANOLETIVO>().ToListAsync().Result;
		    List<DBOANOLETIVO> resultList = result.ToList();
		    return resultList;
	    }


	    /// <summary>
	    /// Gets the class.
	    /// </summary>
	    /// <returns>The class.</returns>
	    /// <param name="idTurma">Identifier turma.</param>
	    public DBOANOLETIVO GetYears (int idAnoLetivo){
		    DBOANOLETIVO result = _AsyncConnection.Table<DBOANOLETIVO>().Where(x => x.idAnoLetivo == idAnoLetivo).FirstOrDefaultAsync().Result;
		    return result;
	    }

  

        public void AddAllAnoLetivo(List<DBOANOLETIVO> anosLetivos) {
            _AsyncConnection.InsertAllAsync(anosLetivos);
        }

    #endregion

    #region DBOTURMA

	    /// <summary>
	    /// Gets the class list.
	    /// </summary>
	    /// <returns>The class list.</returns>
	    /// <param name="YearID">Year I.</param>
	    public List<DBOTURMA> GetClassList (int YearID){
		    IEnumerable<DBOTURMA> result = _AsyncConnection.Table<DBOTURMA>().Where(x => x.idAnoLetivo == YearID).ToListAsync().Result;
		    List<DBOTURMA> resultList = result.ToList();
		    return resultList;
	    }


	    /// <summary>
	    /// Gets the class.
	    /// </summary>
	    /// <returns>The class.</returns>
	    /// <param name="idTurma">Identifier turma.</param>
	    public DBOTURMA GetClass (int idTurma)
        {
            return _AsyncConnection.Table<DBOTURMA>().Where(x => x.idTurma == idTurma).FirstOrDefaultAsync().Result;
        }

        public List<DBOTURMA> GetClassBySchoolID(int idSchool)
        {
            return _AsyncConnection.Table<DBOTURMA>().Where(x => x.idEscola == idSchool).ToListAsync().Result;
        }


        public void AddAllTurmas(List<DBOTURMA> turmas) {
            _AsyncConnection.InsertAllAsync(turmas);
        }

    #endregion

    #region DBORANKING

	    /// <summary>
	    /// Gets the ranking.
	    /// </summary>
	    /// <returns>The ranking.</returns>
	    /// <param name="idMinigame">Identifier minigame.</param>
	    /// <param name="idUsuario">Identifier usuario.</param>
	    public DBORANKING GetRanking(int _idMinigame, int _idUsuario){
            Log.d("tentar pegar ranking existente");
            Log.d($"Minigame ID: {_idMinigame} | ID Usuário: {_idUsuario}");
            var query = _AsyncConnection.Table<DBORANKING>().Where(x => x.idMinigame == _idMinigame && x.idUsuario == _idUsuario).FirstOrDefaultAsync();
            var result = query;
            Log.d(JsonWriter.GetWriter().Write(query.Result));
		    return query.Result;
	    }

        public List<DBORANKING> GetMinigameRanking(int idMinigame) {
            var query = _AsyncConnection.Table<DBORANKING>().Where(x => x.idMinigame == idMinigame && x.idUsuario != 0).OrderByDescending(x => x.highscore).Take(10);
            return query.ToListAsync().Result;
        }

        public List<DBORANKING> GetAllUserRanks(int idUsuario) {
            IEnumerable<DBORANKING> minigamesRankingDB = _AsyncConnection.Table<DBORANKING>().Where(x => x.idUsuario == idUsuario).OrderBy(x => x.idMinigame).Take(5).ToListAsync().Result;
            return minigamesRankingDB.ToList();
        }

        public List<DBORANKING> GetAllOfflineRanks() {
            IEnumerable<DBORANKING> minigamesRankingDB = _AsyncConnection.Table<DBORANKING>().Where(x => x.online == 0).ToListAsync().Result;
            return minigamesRankingDB.ToList();
        }

        public List<DBORANKFILTER> GetAllRankingsOfSchool(int idSchool, int idMinigame) {

            object[] objs = new object[2];
            objs[0] = idMinigame;
            objs[1] = idSchool;
            string cmdText = "SELECT * FROM DBORANKFILTER WHERE idMinigame = ? AND idEscola = ? ORDER BY DBORANKFILTER.highscore DESC LIMIT 10";
            return _AsyncConnection.QueryAsync<DBORANKFILTER>(cmdText, objs).Result;
            
            
        }


        /// <summary>
        /// Inserts the ranking.
        /// </summary>
        /// <param name="_p">P.</param>
        public void InsertRanking(DBORANKING _p){
		    _p.dataInsert = ReturnCurrentDate ();
		    _p.dataUpdate = ReturnCurrentDate ();
            InserRanking2(_p);
	    }

        public void InsertRanking(List<DBORANKING> _list) {
            int _countTemp = _list.Count;
            for (int i = 0; i < _countTemp; i++) {
                if (_list[i] != null) {
                    InserRanking2(_list[i]);
                }
            }
        }

        public void UpdateRankings(List<DBORANKING> _list) {
            int _countTemp = _list.Count;
            for (int i = 0; i < _countTemp; i++) {
                InserRanking2(_list[i]);
            }
        }

        public void UpdateRankings2(List<DBORANKING> _list) {
            int _countTemp = _list.Count;
            for (int i = 0; i < _countTemp; i++) {
                InserRanking2(_list[i]);
            }
        }

        public void UpdateOrReplaceDBORANKING(DBORANKING _ranking) {
            //Connection.Query("UPDATE Book SET Name = '" + inputfield.text + "' WHERE Id = " + intBookNo);
//            object[] argss = new object[8];
//            argss[0] = _ranking.idMinigame;
//            argss[1] = _ranking.idUsuario;
//            argss[2] = _ranking.highscore;
//            argss[3] = _ranking.dataInsert;
//            argss[4] = _ranking.dataUpdate;
//            argss[5] = _ranking.posicao;
//            argss[6] = _ranking.estrelas;
//            argss[7] = _ranking.online;
//            //object[] argss02 = new object[6];
//
//
//            String command01 = "REPLACE INTO DBORANKING (idMinigame, idUsuario, highscore, dataInsert, dataUpdate, posicao, estrelas, online) VALUES(?,?,?,?,?,?,?,?);";
//            //String commandFull = command01 + command02;
//            // _connection.Query(commansd, argss);
//            //_connection.CreateCommand(commansd,argss);
//            _connection.Execute(command01, argss);
            //_connection.Execute(command02, argss2);
            _AsyncConnection.InsertOrReplaceAsync(_ranking);
        }

        public void UpdateRanking(DBORANKING _ranking) {
//            object[] argss = new object[8];
//            argss[0] = _ranking.highscore;
//            argss[1] = _ranking.dataInsert;
//            argss[2] = _ranking.dataUpdate;
//            argss[3] = _ranking.posicao;
//            argss[4] = _ranking.estrelas;
//            argss[5] = _ranking.online;
//            argss[6] = _ranking.idMinigame;
//            argss[7] = _ranking.idUsuario;
//
//            String command02 = "UPDATE DBORANKING SET highscore=?,dataInsert=?,dataUpdate=?,posicao=?,estrelas=?,online=? WHERE idMinigame=? AND idUsuario=?;";
//            _connection.Execute(command02, argss);
            _AsyncConnection.InsertOrReplaceAsync(_ranking);
        }

        public void InserRanking2(DBORANKING _ranking) {
//            object[] argss = new object[8];
//            argss[0] = _ranking.idMinigame;
//            argss[1] = _ranking.idUsuario;
//            argss[2] = _ranking.highscore;
//            argss[3] = _ranking.dataInsert;
//            argss[4] = _ranking.dataUpdate;
//            argss[5] = _ranking.posicao;
//            argss[6] = _ranking.estrelas;
//            argss[7] = _ranking.online;
//
//            String command01 = "REPLACE INTO DBORANKING (idMinigame, idUsuario, highscore, dataInsert, dataUpdate, posicao, estrelas, online) VALUES(?,?,?,?,?,?,?,?);";
//            _connection.Execute(command01, argss);
            _AsyncConnection.InsertOrReplaceAsync(_ranking);
        }



	    /*public void UpdateRanking(DBORANKING _rank){
		    _rank.dataUpdate = ReturnCurrentDate ();
            UpdateOrReplaceDBORANKING(_rank);
	    }*/

    #endregion

    #region DBOPERGUNTAS
	    /// <summary>
	    /// Gets the question list.
	    /// </summary>
	    /// <returns>The question list.</returns>
	    /// <param name="idBook">Identifier book.</param>
	    public List<DBOPERGUNTAS> GetQuestionList (int idBook){
		    var result = _AsyncConnection.Table<DBOPERGUNTAS>().Where(x => x.idLivro == idBook).ToListAsync().Result;
//		    List<DBOPERGUNTAS> resultList = result.ToList();
		    return result;
	    }

        public List<DBOPERGUNTAS> GetQuestionListE(int idBook, int _idCliente) {
            return _AsyncConnection.Table<DBOPERGUNTAS>().Where(x => x.idLivro == idBook && x.idCliente == _idCliente).ToListAsync().Result;
        }

        public List<DBOPERGUNTAS> GetQuestionList(int idBook, int _idCliente) {
            IEnumerable<DBOPERGUNTAS> result = _connection.Table<DBOPERGUNTAS>().Where(x => x.idLivro == idBook && (x.idCliente == 1 || x.idCliente == _idCliente));
            List<DBOPERGUNTAS> resultList = result.ToList();
            return resultList;
        }

        public void DeleteQuestion(int _idPerguntas) {
            DBOPERGUNTAS temp = new DBOPERGUNTAS() {
                idPergunta = _idPerguntas
            };
            _AsyncConnection.DeleteAsync(temp);
            //_AsyncConnection.DeleteAsync(idPerguntas);
        }

    #endregion

    #region DBORESPOSTAS

	    /// <summary>
	    /// Gets the answers list.
	    /// </summary>
	    /// <returns>The answers list.</returns>
	    /// <param name="idPergunta">Identifier pergunta.</param>
	    public List<DBORESPOSTAS> GetAnswersList (int idPergunta){
		    IEnumerable<DBORESPOSTAS> result = _connection.Table<DBORESPOSTAS>().Where(x => x.idPergunta == idPergunta);
		    List<DBORESPOSTAS> resultList = result.ToList();
		    return resultList;
	    }

        public void DelAnswer(int _idRespostas) {
            DBORESPOSTAS temp = new DBORESPOSTAS() {
                idResposta = _idRespostas
            };
            _AsyncConnection.DeleteAsync(temp);
        }

    #endregion

    #region DBOJOGOS_LOGS

	    public void InsertJogosLog(DBOMINIGAMES_LOGS LOG){
		    LOG.dataAcesso = ReturnCurrentDate ();
            InsertMinigamesLOG(LOG);
		    //Debug.Log(LOG.ToString());
	    }

        public List<DBOMINIGAMES_LOGS> GetAllMinigamesLog() {
            IEnumerable<DBOMINIGAMES_LOGS> result = _AsyncConnection.Table<DBOMINIGAMES_LOGS>().ToListAsync().Result;
            List<DBOMINIGAMES_LOGS> resultList = result.ToList();
            return resultList;
        }
        
        public void DeleteMinigamesLog(DBOMINIGAMES_LOGS _minigameLog) {
            _AsyncConnection.DeleteAsync(_minigameLog);
        }

        public void DeleteMinigamesLogs(List<DBOMINIGAMES_LOGS> logs)
        {
            _AsyncConnection.DeleteAsync<DBOMINIGAMES_LOGS>(logs);
        }

        public void InsertMinigamesLOG(DBOMINIGAMES_LOGS _minigamesLog) {
            _AsyncConnection.InsertOrReplaceAsync(_minigamesLog);
        }

        public async UniTask InsertLogAsync(DBOMINIGAMES_LOGS minigamesLog)
        {
            await _AsyncConnection.InsertOrReplaceAsync(minigamesLog);
        }

        #endregion

    #region DBOGAMESLOG

    public void InserGamesLOG(DBOGAMES_LOGS LOG) {
        LOG.dataAcesso = ReturnCurrentDate();
        InsertDBOGAMES_LOG(LOG);
    }

    public List<DBOGAMES_LOGS> GetAllGamesLOG() {
        IEnumerable<DBOGAMES_LOGS> result = _AsyncConnection.Table<DBOGAMES_LOGS>().ToListAsync().Result;
        List<DBOGAMES_LOGS> resultList = result.ToList();
        return resultList;
    }

    public void DeleteGamesLog(DBOGAMES_LOGS _minigameLog) {
        _AsyncConnection.DeleteAsync(_minigameLog);
    }
    
    /// <summary>
    /// Deleta regostrps DBOMINIGAMES_LOGS em massa.
    /// </summary>
    /// <param name="logs"> List de logs</param>
    public void DeleteGamesLogs(List<DBOGAMES_LOGS> logsGames_logs) {
        _AsyncConnection.DeleteAsync<DBOGAMES_LOGS>(logsGames_logs);
    }

    public void InsertDBOGAMES_LOG(DBOGAMES_LOGS _gamesLog) {
        _AsyncConnection.InsertAsync(_gamesLog);
    }

    #endregion

    #region DBOESTATISTICA_DITATICA

    /// <summary>
    /// Inserts the statistic.
    /// </summary>
    /// <param name="Statistic">Statistic.</param>
    public void InsertStatistic(DBOESTATISTICA_DIDATICA Statistic){
		//Statistic.dataInsert = System.DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss'.'fff");
		Statistic.dataInsert = ReturnCurrentDate();
        InsertStatistic2(Statistic);
		Debug.Log(Statistic.ToString());
	}

    public void InsertAllStatistic(List<DBOESTATISTICA_DIDATICA> statistics) {
        //_connection.InsertAll(statistics);
        int countTemp = statistics.Count;
        for (int i = 0; i < countTemp; i++) {
            InsertStatistic2(statistics[i]);
        }
    }

    public void InsertStatistic2(DBOESTATISTICA_DIDATICA _statistic)
    {
        _AsyncConnection.InsertAsync(_statistic);
    }

    public async Task InsertLogAsync(DBOESTATISTICA_DIDATICA log)
    { 
        await _AsyncConnection.InsertAsync(log);
    }

    public List<DBOESTATISTICA_DIDATICA> GetAllStatisticDidatica() {
        IEnumerable<DBOESTATISTICA_DIDATICA> result = _AsyncConnection.Table<DBOESTATISTICA_DIDATICA>().ToListAsync().Result;
        List<DBOESTATISTICA_DIDATICA> resultList = result.ToList();
        return resultList;
    }

    public void DeleteEstatistica(DBOESTATISTICA_DIDATICA _minigameLog) {
        _AsyncConnection.DeleteAsync(_minigameLog);
    }
    
    public void DeleteEstatisticas(List<DBOESTATISTICA_DIDATICA> @logs)
    {
        _AsyncConnection.DeleteAsync<DBOESTATISTICA_DIDATICA>(@logs);
    }



    #endregion

    #region DBOSINCRONIZACAO

    public DBOSINCRONIZACAO GetSync(int idCliente) {
        DBOSINCRONIZACAO sinc = _AsyncConnection.Table<DBOSINCRONIZACAO>().Where(x => x.idCliente == idCliente).FirstOrDefaultAsync().Result ??
                                new DBOSINCRONIZACAO();
        return sinc;
    }

    public void UpdateSync(DBOSINCRONIZACAO syncTemp) {
        _AsyncConnection.InsertOrReplaceAsync(syncTemp);
    }

    /// <summary>
    /// Returns the current date.
    /// </summary>
    /// <returns>The current date.</returns>
    string ReturnCurrentDate(){
        return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        //return System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

#endregion

    #region DBOPERGUNTAS


        public void AddAllPerguntas(List<DBOPERGUNTAS> perguntas) {
            int size = perguntas.Count;
            for (int i = 0; i < size; i++) {
                _AsyncConnection.InsertOrReplaceAsync(perguntas[i]);
            }
            //_connection.InsertAll(perguntas);
        }

    public void AddStatisticasDidaticaMock(List<DBOESTATISTICA_DIDATICA> statisticas) {
        _AsyncConnection.InsertAllAsync(statisticas);
    }

    public void AddMinigamesLogs(List<DBOMINIGAMES_LOGS> @logs) {
        _AsyncConnection.InsertAllAsync(@logs);
    }
        
    public void AddPerguntaOrReplace(DBOPERGUNTAS _pergunta) {
        _AsyncConnection.InsertOrReplaceAsync(_pergunta);
    }

        public List<DBOPERGUNTAS> PerguntasToDownload() {

            return _connection.Query<DBOPERGUNTAS>("SELECT * FROM DBOPERGUNTAS WHERE downloaded == 0 AND ativo = 1 AND audio = 1", new object[0]);

        }
    #endregion

    #region DBORESPOSTAS



        public void AddAllRespostas(List<DBORESPOSTAS> respostas) {
            //_connection.InsertAll(respostas);
            int size = respostas.Count;
            for (int i = 0; i < size; i++) {
                _AsyncConnection.InsertOrReplaceAsync(respostas[i]);
            }
        }

    public void AddRespostaOrReplace(DBORESPOSTAS _resposta) {
        _AsyncConnection.InsertOrReplaceAsync(_resposta);
    }

    public List<DBORESPOSTAS> RespostaNotDownloaded() {
        return _connection.Query<DBORESPOSTAS>("SELECT * FROM DBORESPOSTAS WHERE downloaded == 0 AND ativo = 1 AND audio = 1", new object[0]);
    }

    #endregion
    #region category_item

    public DBOITENS_CATEGORIAS[] GetCategoryItem() {
    IEnumerable<DBOITENS_CATEGORIAS> categorys = _connection.Table<DBOITENS_CATEGORIAS>();
    return categorys.ToArray();
    }

    public DBOITENS_CATEGORIAS[] GetCategoryItem(int _idGame) {
    IEnumerable<DBOITENS_CATEGORIAS> categorys = _connection.Table<DBOITENS_CATEGORIAS>().Where(x => x.idGame == _idGame);
    return categorys.ToArray();
    }

    public void AddItensCategory(List<DBOITENS_CATEGORIAS> _itensCategory) {
    int _tempCount = _itensCategory.Count;
    for (int i = 0; i < _tempCount; i++) {
        _AsyncConnection.InsertOrReplaceAsync(_itensCategory[i]);
    }
    //_connection.InsertAll(_itensCategory);
    }

    #endregion

    #region storeItem

    public DBOITENS[] GetItensStore() {
        IEnumerable<DBOITENS> storeItens = _AsyncConnection.Table<DBOITENS>().Where(x => x.ativo == 1).ToListAsync().Result;
        return storeItens.ToArray();
    }

    public DBOITENS GetItemStore(int _idItem) {
        DBOITENS storeItens = _AsyncConnection.Table<DBOITENS>().Where(x => x.idItem == _idItem).FirstOrDefaultAsync().Result ;
        return storeItens;
    }

    public List<DBOITENS> GetItensStoreList() {
        IEnumerable<DBOITENS> storeItens = _AsyncConnection.Table<DBOITENS>().Where(x => x.ativo == 1).ToListAsync().Result;
        return storeItens.ToList();
    }

    public void AddItens(List<DBOITENS> _itens) {
        int _countTemp = _itens.Count;
        for (int i = 0; i < _countTemp; i++) {
            _AsyncConnection.InsertOrReplaceAsync(_itens[i]);
        }
    }

    public List<DBOITENS> NotDownloadedItens() {
        string commandSQLITE = "SELECT * FROM DBOITENS WHERE downloaded == 0 AND ativo = 1";
        return _AsyncConnection.QueryAsync<DBOITENS>(commandSQLITE,new object[0]).Result;
    }

    #endregion

    #region Inventario

    public List<DBOINVENTARIO> GetAllInventory() {
        IEnumerable<DBOINVENTARIO> inventory = _AsyncConnection.Table<DBOINVENTARIO>().Where(x => x.online == 0).ToListAsync().Result;
        return inventory.ToList();
    }

    public DBOINVENTARIO[] GetInventory(int _idUser) {
        IEnumerable<DBOINVENTARIO> inventory = _AsyncConnection.Table<DBOINVENTARIO>().Where(x => x.idUsuario == _idUser && x.quantidade >= 1).ToListAsync().Result;
        return inventory.ToArray();
    }

    public void SetInventario(DBOINVENTARIO _item) {
        UpdateOrReplateInventory(_item);
    }

    public void SetInventario(List<DBOINVENTARIO> _itens) {
        int tempCount = _itens.Count;
        for (int i = 0; i < tempCount; i++) {
            UpdateOrReplateInventory(_itens[i]);
        }
    }

    public void UpdateItem(DBOINVENTARIO _temp) {
        UpdateOrReplateInventory(_temp);
    }

    public void UpdateOrReplateInventory(DBOINVENTARIO _itemUpdate)
    {
        _AsyncConnection.InsertOrReplaceAsync(_itemUpdate);
    }
    #endregion

    public void UpdateMinigames(List<DBOMINIGAMES> _minigames) {
        
        int tempCount = _minigames.Count;
        for (int i = 0; i < tempCount; i++) {
            _AsyncConnection.InsertOrReplaceAsync(_minigames[i]);
        }
    }

    public async Task<int> InsertOrReplaceAsyncMiniGames(DBOMINIGAMES minigame)
    {
        return await _AsyncConnection.InsertOrReplaceAsync(minigame);
    }

    public List<DBOMINIGAMES> GetAllMinigames() {
        IEnumerable<DBOMINIGAMES> inventory = _AsyncConnection.Table<DBOMINIGAMES>().Where(x => x.ativo == 1).ToListAsync().Result;
        return inventory.ToList();
    }


    public void RunQuery(string query) {
        object[] argsss = new object[1];
        //var cmd = _connection.CreateCommand(query, argsss);
        _connection.Execute(query, argsss);
        //_connection.Query(query, argsss);
    }

    public void ExecuteSQLITECommmand(string sqliteQuery, object[] variables) {
        //object[] argsss = new object[1];
        //var cmd = _connection.CreateCommand(query, argsss);
        _connection.Execute(sqliteQuery, variables);
        //_connection.Query(query, argsss);
    }

    public DBOVERSION GetDBVersion() {
        return _AsyncConnection.Table<DBOVERSION>().Where(x => x.id == 1).FirstOrDefaultAsync().Result;       
    }

    public void UpdateDBVersion(DBOVERSION _dbv) {
        _AsyncConnection.UpdateAsync(_dbv);
    }

    public void InsertOrReplaceRecentItem(int userID, int itemId) {

        object[] argss = new object[2];

        argss[0] = userID;
        argss[1] = itemId;

        string command01 = "REPLACE INTO DBORECENTITEM VALUES(?,?);";

        _connection.Execute(command01, argss);
    }

    public List<DBORECENTITEM> GerAllRecentOfUser(int userID) {
        IEnumerable<DBORECENTITEM> recentItens = _AsyncConnection.Table<DBORECENTITEM>().Where(x => x.userId == userID).ToListAsync().Result;
        return recentItens.ToList();
    }


}
