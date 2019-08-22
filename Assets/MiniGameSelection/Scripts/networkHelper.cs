using MEC;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using com.csutil;
using MiniJSON;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class networkHelper {

    public bool isDoingOperation = false;
    public SyncTable currentSyncDB;
    public DBOUSUARIOS currentUser;
    public bool HasCredentials = false;
    public string token;
    public int userID;
    public GameConfig config => GameConfig.Instance;
    private bool operationFailed = false;
    private string tempToken;
    private StringFast stringfast = new StringFast();
    public static event Action<bool, string, string> LoginFailedEvent;
    public StartSceneManager startScene;
    public void NetworkVeryfier(GameConfig configs) {
        configs.isVerifingNetwork = true;

        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
//                configs.isOn = false;
                configs.isVerifingNetwork = false;
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
            case NetworkReachability.ReachableViaLocalAreaNetwork:
//                configs.isOn = true;
                configs.isVerifingNetwork = false;
                break;
        }
        configs.isVerifingNetwork = false;
    }

    

    #region dboSync
    /*public void GetDBOSYNC(string dbosyncURI, int idCliente) {
        isDoingOperation = true;
        //Timing.RunCoroutine(DBOSYNCDOWN(dbosyncURI, idCliente));
    }*/

    public IEnumerator<float> DBOSYNCDOWN(string dbosyncURI, int idCliente, int _idCliente) {
        isDoingOperation = true;
        startScene.MessageStatus("Verificando atualizações");
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(DBOSYNCWEBREQUEST(dbosyncURI, idCliente, _idCliente)));
        isDoingOperation = false;
    }

    IEnumerator<float> DBOSYNCWEBREQUEST(string dbosyncURI, int idCliente, int _idClient) {
        //Debug.Log("Getting DBOSYNC");
        WWWForm form = new WWWForm();

        form.AddField("idCliente", idCliente);
        form.AddField("idGame", _idClient);
        form.AddField("gameKey", config.returnDecryptKey());

        using (var request = UnityWebRequest.Post(dbosyncURI, form)) {
            request.timeout = 10;
            request.redirectLimit = 2;
            yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            } else {
                string response = request.downloadHandler.text;
                //Debug.Log(response);
                currentSyncDB = DecodeJson(response, idCliente);
            }
            request.Dispose();
        }

       



    }


    public SyncTable DecodeJson(string JSONs, int idCliente) {
        
        var N = JSON.Parse(JSONs);
        SyncTable temp = new SyncTable() {
            //DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            escola = N["escola"],
            turma = N["turma"],
            usuarios = N["usuarios"],
            minigames = N["miniGames"],
            perguntasG = N["perguntasG"],
            respostasG = N["respostasG"],
            perguntasE = N["perguntasE"],
            respostasE = N["respostasG"],
            itensG = N["itensG"],
            itensE = N["itensE"],
            itens_categorias = N["itensCategorias"],
            ranking = N["ranking"],
            idDelPergunta = N["idDelPergunta"],
            idDelResposta = N["idDelResposta"],
            sincModePerguntas = N["sincModePerguntas"],
            sincModeItens = N["sincModeitens"]
        };
        return temp;
    }

    #endregion

    #region retrieveToken

    /*public void GetToken(string URI,string login, string password, string deviceID) {
        isDoingOperation = true;
        Timing.RunCoroutine(GetTokenSync(URI, login, password, deviceID));
    }*/

    /*public IEnumerator<float> GetTokenSync(string uri, string login, string password, string deviceID) {
        Debug.Log("Getting Token");
        isDoingOperation = true;
        yield return Timing.WaitUntilDone(Timing.RunCoroutine(GetTokenIE(uri, login, password, deviceID)));
        isDoingOperation = false;

    }*/

    public void UpdatePlayer(DBOUSUARIOS _user) {
        config.playerID = _user.idUsuario;
        config.nickname = _user.login;
        config.namefull = _user.nomeJogador;
    }

    public IEnumerator<float> GetTokenIE(string uri,string _idCliente, string login, string password, string deviceID, StartSceneManager sceneStart) {
        isDoingOperation = true;
        //Debug.Log("Getting Token");
        WWWForm form = new WWWForm();

        startScene.MessageStatus("Autenticando Credenciais");

        form.AddField("login", login);
        form.AddField("senha", password);
        form.AddField("deviceID", deviceID);
        form.AddField("idGame", config.gameID);
        form.AddField("gameKey", config.returnDecryptKey());
        form.AddField("versao", Application.version);
        form.AddField("DeviceModel", SystemInfo.deviceModel);
        form.AddField("OperatingSystem", SystemInfo.operatingSystem);
        form.AddField("ScreenWidth", Screen.width);
        form.AddField("ScreenHeight", Screen.height);
        form.AddField("SystemMemorySize", SystemInfo.systemMemorySize);
        form.AddField("idCliente", _idCliente);

        UnityWebRequest request = UnityWebRequest.Post(uri, form);
        request.timeout = 30;
        request.redirectLimit = 3;
        
        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));
        
        string response = request.downloadHandler.text;

        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            //Debug.Log(request.error);

            if(request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
                sceneStart.OnlineAcessFailed();
            } else {
                sceneStart.OnlineAcessFailed();
            }
            
            //Timing.KillCoroutines();
            //Timing.KillCoroutines("LoginRoutine");
        } else {           
            //Debug.Log(response);           

            yield return Timing.WaitForSeconds(0.2f);

            token = result["token"].Value;
            userID = result["idUsuario"].AsInt;
            config.playerID = result["idUsuario"].AsInt;

            if (!string.IsNullOrEmpty(token)) {
                HasCredentials = false;
                //string error = result["erro"];
                //create Games_LOGS register;
                //TODO here
               // config.isOnline = false;
                //sceneStart.OnlineAcessFailed();
                //Timing.KillCoroutines();

                DBOUSUARIOS user = config.OpenDb().GetUser(login);
                if (user != null) {
                    config.currentUser = user;
                    UpdatePlayer(user);


                }


                //
                //Debug.Log(error);
            } else {
                //sceneStart.OnlineAcessSucess(login, password);
                //HasCredentials = true;
                sceneStart.OnlineAcessFailed();

            }
        }

        isDoingOperation = false;
    }

    #endregion

    #region setMethods

    public void SetJogosLogM(DBOMINIGAMES_LOGS LOGs){
        Timing.RunCoroutine(SetJogosLog(LOGs, token));
    }

    public void RunStatistics(List<DBOESTATISTICA_DIDATICA> _list) {
        Timing.RunCoroutine(SendStatisticsList(_list));
    }

    public IEnumerator<float> SendStatisticsList(List<DBOESTATISTICA_DIDATICA> _statisticaToSend) {
        yield return Timing.WaitForSeconds(0.1f);

        int countTemp = _statisticaToSend.Count;

        if (countTemp >= 1 && config.isOn) {
            if (startScene != null) {
                startScene.MessageStatus("Sincronizando Estatísticas");
            }
            do {
                yield return Timing.WaitUntilDone(Timing.RunCoroutine(StatisticSave(_statisticaToSend[0], _statisticaToSend)));
                yield return Timing.WaitForSeconds(0.1f);
            } while (config.isOn && _statisticaToSend.Count > 0);
        }
    }

    public IEnumerator<float> SyncInfo(Action<bool, string, string> failed) {
        LoginFailedEvent = failed;
        startScene.MessageStatus("Sincronizando Game");

        DataService ds = GameConfig.Instance.OpenDb();
        List<DBOMINIGAMES_LOGS> _logToSend = ds.GetAllMinigamesLog();
//        yield return Timing.WaitForSeconds(0.1f);
//        if (_logToSend.Count >= 1) {
//            yield return Timing.WaitUntilDone(Timing.RunCoroutine(SetJogosLogs(_logToSend)));
//        }

        var eduqbrinq = EduqbrinqLogger.Instance;
        eduqbrinq.SendRequest(_logToSend).GetAwaiter();

        
        
        int countTemp = 0;
        List<DBORANKING> _rankToSend = ds.GetAllOfflineRanks();
//        yield return Timing.WaitForSeconds(0.1f);
//        countTemp = _rankToSend.Count;
//        if (countTemp >= 1 && config.isOnline) {
//            startScene.MessageStatus("Sincronizando Ranking");
//            do {
//
//                yield return Timing.WaitUntilDone(Timing.RunCoroutine(setRanking(_rankToSend[0], _rankToSend)));
//                yield return Timing.WaitForSeconds(0.1f);
//
//            } while (_rankToSend.Count >= 1 && config.isOnline);
//        }
        eduqbrinq.SendRequest(_rankToSend).GetAwaiter();


        List<DBOESTATISTICA_DIDATICA> _statisticaToSend = ds.GetAllStatisticDidatica();
        yield return Timing.WaitForSeconds(0.1f);
        countTemp = _statisticaToSend.Count;

        if (countTemp >= 1 && config.isOn) {
            yield return Timing.WaitUntilDone(Timing.RunCoroutine(SetEstatisticasDidaticas(_statisticaToSend)));
        }

        List<DBOGAMES_LOGS> _logsGames = ds.GetAllGamesLOG();
        yield return Timing.WaitForSeconds(0.1f);
        countTemp = _logsGames.Count;
        if (countTemp >= 1 && config.isOn) {
            startScene.MessageStatus("Sincronizando Estatísticas");
            do {
                yield return Timing.WaitUntilDone(Timing.RunCoroutine(SendGamesLog(_logsGames[0], _logsGames)));
                yield return Timing.WaitForSeconds(0.1f);
            } while (config.isOn && _logsGames.Count > 0);
        }

        List<DBOPONTUACAO> userScoreUpdates = ds.GetallScoresOffline();
        yield return Timing.WaitForSeconds(0.1f);

        startScene.MessageStatus("Sincronizando Pontuação");
        eduqbrinq.SendRequest(userScoreUpdates).GetAwaiter();

        List<DBOINVENTARIO> userInventoryUdpdate = ds.GetAllInventory();
        yield return Timing.WaitForSeconds(0.1f);

        countTemp = userInventoryUdpdate.Count;
        if (countTemp >= 1 && config.isOn) {
            startScene.MessageStatus("Sincronizando Inventário");
            do {
                DBOITENS item = config.OpenDb().GetItemStore((userInventoryUdpdate[0].idItem));
                yield return Timing.WaitUntilDone(Timing.RunCoroutine(SetInventario(userInventoryUdpdate[0], userInventoryUdpdate, item.valor)));
                yield return Timing.WaitForSeconds(0.1f);
            } while (userInventoryUdpdate.Count >= 1 && config.isOn);
        }

    }

    public IEnumerator<float> SetJogosLog(DBOMINIGAMES_LOGS logTemp, string TokenT) {

        isDoingOperation = true;
        WWWForm form = new WWWForm();
        logTemp.online = 1;
        form.AddField("token", token);
        form.AddField("idUsuario", logTemp.idUsuario);
        form.AddField("idMinigame", logTemp.idMinigame);
        form.AddField("pontosLudica", logTemp.pontosLudica);
        form.AddField("pontosPedagogica", logTemp.pontosPedagogica);
        form.AddField("pontosInteragindo", logTemp.pontosInteragindo);
        form.AddField("personagem", logTemp.personagem);
        form.AddField("dataAcesso", logTemp.dataAcesso);
        form.AddField("tempoLudica", logTemp.tempoLudica);
        form.AddField("tempoDidatica", logTemp.tempoDidatica);
        form.AddField("faseLudica", logTemp.faseLudica);
        form.AddField("deviceID", logTemp.deviceID);
        form.AddField("online", 1);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setJogosLogs", form);


        yield return Timing.WaitUntilDone(request.SendWebRequest());

        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);
        if (request.isNetworkError || request.isHttpError) {                   
            //config.isOnline = false;
            logTemp.online = 0;
            config.OpenDb().InsertJogosLog(logTemp);
        } else {
            
        }

        isDoingOperation = false;
    }

    public IEnumerator<float> SetJogosLog(DBOMINIGAMES_LOGS jogosTemp, List<DBOMINIGAMES_LOGS> _list) {

        isDoingOperation = true;
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idUsuario", jogosTemp.idUsuario);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);
        form.AddField("idMinigame", jogosTemp.idMinigame);
        form.AddField("pontosLudica", jogosTemp.pontosLudica);
        form.AddField("pontosPedagogica", jogosTemp.pontosPedagogica);
        form.AddField("pontosInteragindo", jogosTemp.pontosInteragindo);
        form.AddField("personagem", jogosTemp.personagem);
        form.AddField("dataAcesso", jogosTemp.dataAcesso);
        form.AddField("tempoLudica", jogosTemp.tempoLudica);
        form.AddField("tempoDidatica", jogosTemp.tempoDidatica);
        form.AddField("faseLudica", jogosTemp.faseLudica);
        form.AddField("deviceID", jogosTemp.deviceID);
        form.AddField("online", 0);


        using (UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setJogosLogs", form)) {
            request.timeout = 10;
            request.redirectLimit = 2;

            yield return Timing.WaitUntilDone(request.SendWebRequest());

            string response = request.downloadHandler.text;
            var result = JSON.Parse(response);
            if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {

                if (request.isNetworkError || request.isHttpError) {
//                    config.isOn = false;
                }
                //LoginFailedEvent(true, config.currentUser.login, config.currentUser.senha);
            } else {
                _list.RemoveAt(0);
                config.OpenDb().DeleteMinigamesLog(jogosTemp);
            }

            isDoingOperation = false;
            request.Dispose();
        }
    }  
    
    //TODO MinigamesLog
    public IEnumerator<float> SetJogosLogs(List<DBOMINIGAMES_LOGS> ListOfLogs) {

        isDoingOperation = true;
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);
        var JsonValue = JsonSerialize(ListOfLogs);
        form.AddField("JsonValue", JsonValue);
        var HashValue = GenerateHashInJson(JsonValue);
        form.AddField("HashValue", HashValue);

        //TODO atualizar link/rota da api de envio em massa de logs.
        using (UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setJogosOnlineLogs", form)) {
            request.timeout = 10;
            request.redirectLimit = 2;

            yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

            var response = request.downloadHandler.text;
            
            if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
                if (request.isNetworkError || request.isHttpError) {
//                    config.isOn = false;
                }
            } else {
                //TODO deletar todos os logs offline recentemente enviados.
                config.OpenDb().DeleteMinigamesLogs(ListOfLogs);
            }

            isDoingOperation = false;
            request.Dispose();
        }
    }

    IEnumerator<float> StatisticSave(EstatisticaDidaticaSerialazeble _listStatistc, DBOESTATISTICA_DIDATICA _statistic, string tokens) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        _statistic.online = 0;
        
        form.AddField("token", token);
        form.AddField("idUsuario", _statistic.idUsuario);
        form.AddField("dataInsert", _statistic.dataInsert);
        form.AddField("idHabilidade", _statistic.idGameDidatico);
        form.AddField("idGameDidatico", _statistic.idGameDidatico);
        form.AddField("idDificuldade", _statistic.idDificuldade);
        form.AddField("idMinigame", _statistic.idMinigame);
        form.AddField("acertou", _statistic.acertou);
        form.AddField("online", 0);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);


        UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setEstatisticaDidatica", form);


        yield return Timing.WaitUntilDone(request.SendWebRequest());

        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            }
        } else {
            _listStatistc.estatisticas.RemoveAt(0);

        }

        isDoingOperation = false;
    }

    IEnumerator<float> StatisticSaveOffline(DBOESTATISTICA_DIDATICA _statistic, List<DBOESTATISTICA_DIDATICA> _list) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();

        form.AddField("token", token);
        form.AddField("idUsuario", _statistic.idUsuario);
        form.AddField("dataInsert", _statistic.dataInsert != null ? _statistic.dataInsert : config.ReturnCurrentDate());
        form.AddField("idHabilidade", _statistic.idHabilidade);
        form.AddField("idGameDidatico", _statistic.idGameDidatico);
        form.AddField("idDificuldade", _statistic.idDificuldade);
        form.AddField("idMinigame", _statistic.idMinigame);
        form.AddField("acertou", _statistic.acertou);
        form.AddField("online", 0);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setEstatisticaDidatica", form);


        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));
        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
//            config.isOn = false;
        } else {
            _list.RemoveAt(0);
            config.OpenDb().DeleteEstatistica(_statistic);
        }

        isDoingOperation = false;
    }
    
    IEnumerator<float> SetEstatisticasDidaticas(List<DBOESTATISTICA_DIDATICA> @logs) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();

        form.AddField("token", token);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);
        var jsonValue = JsonSerialize(@logs);
        form.AddField("JsonValue", jsonValue);
        var hashValue = GenerateHashInJson(jsonValue);
        form.AddField("HashValue", hashValue);
        
        var objects = new object[4];
        objects[0] = token;
        objects[1] = config.currentUser.idUsuario;
        objects[2] = jsonValue;
        objects[3] = hashValue;
        Log.d("WWWForm details", objects);

        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setEstatisticaDidatica2", form);


        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));
        string response = request.downloadHandler.text;
        
        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            }
        } else {
            GameConfig.Instance.OpenDb().DeleteEstatisticas(@logs);
        }
        request.Dispose();
        isDoingOperation = false;
    }

    IEnumerator<float> StatisticSave(DBOESTATISTICA_DIDATICA _statistic, List<DBOESTATISTICA_DIDATICA> _list) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        _statistic.online = 1;

        form.AddField("token", token);
        form.AddField("idUsuario", _statistic.idUsuario);
        form.AddField("dataInsert", _statistic.dataInsert);
        form.AddField("idHabilidade", _statistic.idHabilidade);
        form.AddField("idGameDidatico", _statistic.idGameDidatico);
        form.AddField("idDificuldade", _statistic.idDificuldade);
        form.AddField("idMinigame", _statistic.idMinigame);
        form.AddField("acertou", _statistic.acertou);
        form.AddField("online", 1);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setEstatisticaDidatica", form);

        yield return Timing.WaitUntilDone(request.SendWebRequest());
        
        string response = request.downloadHandler.text;
        
        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
//            config.isOn = false;
            _statistic.online = 0;
            config.OpenDb().InsertStatistic2(_statistic);
        } else {
            Debug.Log("StatisticSave" + response);
        }
        request.Dispose();
        isDoingOperation = false;
    }

    IEnumerator<float> SendGamesLog(DBOGAMES_LOGS _log, List<DBOGAMES_LOGS> _list) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idUsuario", _log.idUsuario);
        form.AddField("idGame", config.gameID);
        form.AddField("dataAcesso", _log.dataAcesso);
        form.AddField("online", 0);
        form.AddField("versao", _log.versao);
        form.AddField("deviceID", _log.deviceID);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        //TODO GamesLOG API LINK
        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setGamesLogs", form);

        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            }
        } else {
            _list.RemoveAt(0);
            config.OpenDb().DeleteGamesLog(_log);
        }
        request.Dispose();
        isDoingOperation = false;
    }
    
    //TODO envio em massa de DBOGames_log
    IEnumerator<float> SendGamesLogs(List<DBOGAMES_LOGS> logs) {
        isDoingOperation = true;
        var form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);
        var jsonValue = JsonSerialize(logs);
        form.AddField("JsonValue", jsonValue);
        form.AddField("HashValue", GenerateHashInJson(jsonValue));

        //TODO GamesLOGs API LINK
        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setGamesLogs", form);

        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

        var response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            }
        } else {
            config.OpenDb().DeleteGamesLogs(logs);
        }
        request.Dispose();
        isDoingOperation = false;
    }

    public void RunSetRanking(DBORANKING ranking, string _token) {
        Timing.RunCoroutine(setRanking(ranking, _token));
    }

    IEnumerator<float> setRanking(DBORANKING ranking, string _token) {
        isDoingOperation = true;
        
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idMinigame", ranking.idMinigame);
        form.AddField("idUsuario", ranking.idUsuario);
        form.AddField("highscore", ranking.highscore);
        form.AddField("idGame", config.gameID);
        form.AddField("idCliente", config.clientID);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);
        form.AddField("estrelas", ranking.estrelas);

        UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setRanking", form);


        yield return Timing.WaitUntilDone(request.SendWebRequest());

        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
//            config.isOn = false;
            ranking.online = 0;
            config.OpenDb().InsertRanking(ranking);
            Log.d("Ranking offline Atualizado");
        } else {
            Log.d("Ranking Online Atualizdo");
        }
        
        request.Dispose();

        isDoingOperation = false;
    }

    IEnumerator<float> setRanking(DBORANKING rankLog, List<DBORANKING> _list) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idMinigame", rankLog.idMinigame);
        form.AddField("idUsuario", rankLog.idUsuario);
        form.AddField("highscore", rankLog.highscore);
        form.AddField("idCliente", config.clientID);
        form.AddField("idGame", config.gameID);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);
        form.AddField("estrelas", rankLog.estrelas);

        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setRanking", form);


        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

        string response = request.downloadHandler.text;

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            //Debug.Log(request.error);
            //config.isOnline = false;
            //LoginFailedEvent(true, config.currentUser.login, config.currentUser.senha);

            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            }

        } else {
           
            //Debug.Log(response);

            _list.RemoveAt(0);
            rankLog.online = 1;
            config.OpenDb().InsertRanking(rankLog);
        }
        request.Dispose();
        isDoingOperation = false;
    }
    

    public void RunsetPontuacao(DBOPONTUACAO score, string token) {
        Timing.RunCoroutine(setPontuacao(score));
    }

    IEnumerator<float> setPontuacao(DBOPONTUACAO score, List<DBOPONTUACAO> _list) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        form.AddField("token", token);     
        form.AddField("idUsuario", score.idUsuario);
        form.AddField("deviceBrops", score.BropsDevice);
        form.AddField("devicePoints", score.PontuacaoTotalDevice);
        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setPontuacao", form);


        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
        } else {
            if (response.Contains("success")) {
                config.BropsDeviceAmount = 0;
                config.TotalPointsDevice = 0;
                _list.RemoveAt(0);
                score.online = 1;
                score.PontuacaoTotalDevice = 0;
                score.BropsDevice = 0;
                config.OpenDb().UpdateToOnlineScore(score);
            }            
        }

        isDoingOperation = false;
    }

    IEnumerator<float> setPontuacao(DBOPONTUACAO score) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("idUsuario", score.idUsuario);
        form.AddField("deviceBrops", score.BropsDevice);
        form.AddField("devicePoints", score.PontuacaoTotalDevice);
        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setPontuacao", form);


        yield return Timing.WaitUntilDone(request.SendWebRequest());

        string response = request.downloadHandler.text;
        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
                score.online = 0;
                config.OpenDb().InsertOrReplateScore(score);
            }
        } else {
            config.BropsDeviceAmount = 0;
            config.TotalPointsDevice = 0;
        }
        request.Dispose();
        isDoingOperation = false;
    }

    public void SetInventory(DBOINVENTARIO _inv, int valu) {
        Timing.RunCoroutine(SetInventario(_inv,valu));
        config.BropsDeviceAmount -= valu;
        //config.TotalPointsDevice -= valu;
        Timing.RunCoroutine(setPontuacao(new DBOPONTUACAO() {
            brops = config.BropsAmount,
            pontuacaoTotal = config.TotalPoints,
            idUsuario = config.currentUser.idUsuario,
            dataUpdate = config.ReturnCurrentDate(),
            BropsDevice = config.BropsDeviceAmount,
            PontuacaoTotalDevice = config.TotalPointsDevice            
        }));
    }

    IEnumerator<float> SetInventario(DBOINVENTARIO _inv, int value) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        
        form.AddField("idUsuario", _inv.idUsuario);
        form.AddField("idItem", _inv.idItem);
        form.AddField("quantidade", _inv.quantidade);
        form.AddField("ativo", _inv.ativo);
        form.AddField("deviceID", SystemInfo.deviceUniqueIdentifier);
        form.AddField("valor", value);
        form.AddField("token", token);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        //TODO adicionar link da API SetInventario.
        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ2/setInventario", form);

        yield return Timing.WaitUntilDone(request.SendWebRequest());
        string response = request.downloadHandler.text;

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
//            config.isOn = false;
            _inv.online = 0;
            config.OpenDb().UpdateOrReplateInventory(_inv);
        } else {
            _inv.online = 1;
            _inv.deviceQuantity = 0;
            config.OpenDb().UpdateOrReplateInventory(_inv);
        }
        request.Dispose();
        isDoingOperation = false;
    }

    IEnumerator<float> SetInventario(DBOINVENTARIO inv, List<DBOINVENTARIO> _list, int itemValue) {
        isDoingOperation = true;
        WWWForm form = new WWWForm();
        
        form.AddField("idUsuario", inv.idUsuario);
        form.AddField("idItem", inv.idItem);
        form.AddField("quantidade", inv.deviceQuantity);
        form.AddField("ativo", inv.ativo);
        form.AddField("deviceID", SystemInfo.deviceUniqueIdentifier);
        form.AddField("valor", itemValue);
        form.AddField("token", token);
        form.AddField("idUsuarioOnline", config.currentUser.idUsuario);

        //TODO adicionar link da API SetInventario.
        var request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ2/setInventario", form);

        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

        string response = request.downloadHandler.text;
        if (request.isNetworkError || request.isHttpError || response.Contains("response")) {
            if (request.isNetworkError || request.isHttpError) {
//                config.isOn = false;
            }
        } else {
            //config.usersIDInventoryUpdate.Remove(inv);
            _list.RemoveAt(0);
            inv.online = 1;
            inv.deviceQuantity = 0;
            config.OpenDb().UpdateItem(inv);
        }
        request.Dispose();
        isDoingOperation = false;
    }

    #endregion

    #region SyncBeforeLogin
    public IEnumerator<float> GettingDBOSBeforeLogin(int idCliente, DataService db, string URI, int idGame) {
        isDoingOperation = true;

        List<string> tables = new List<string>();

        config.sincModeItens = 1;
        config.sincModePerguntas = 1;

        yield return Timing.WaitForSeconds(1f);

        DBOSINCRONIZACAO dboI = GameConfig.Instance.OpenDb().GetSync(idCliente);
        DBOSINCRONIZACAO dboG = GameConfig.Instance.OpenDb().GetSync();

        bool hasTables = false;

        if (config.sincModePerguntas == 1 || config.sincModePerguntas == 3) {
            if (LocalIsOlder(dboG.perguntas, currentSyncDB.perguntasG)) {
                tables.Add("perguntasG");
                hasTables = true;
            }

            if (LocalIsOlder(dboG.respostas, currentSyncDB.respostasG)) {
                tables.Add("respostasG");
                hasTables = true;
            }
        }

        if (config.sincModePerguntas == 1 || config.sincModePerguntas == 2) {
            if (LocalIsOlder(dboI.perguntas, currentSyncDB.perguntasE)) {
                tables.Add("perguntasE");
                hasTables = true;
            }

            if (LocalIsOlder(dboI.respostas, currentSyncDB.respostasE)) {
                tables.Add("respostasE");
                hasTables = true;
            }
        }

        if (config.sincModeItens == 1 || config.sincModeItens == 2) {
            if (LocalIsOlder(dboI.itens, currentSyncDB.itensE)) {
                tables.Add("itensE");
                hasTables = true;
            }
        }

        if (config.sincModeItens == 1 || config.sincModeItens == 3) {
            if (LocalIsOlder(dboG.itens, currentSyncDB.itensG)) {
                tables.Add("itensG");
                hasTables = true;
            }

            if (LocalIsOlder(dboG.itens_categorias, currentSyncDB.itens_categorias)) {
                tables.Add("itens_categorias");
                hasTables = true;
            }
        }

        if (LocalIsOlder(dboG.minigames, currentSyncDB.minigames)) {
            tables.Add("minigames");
            hasTables = true;
        }

        if (hasTables == false) {
            tables.Add("");
        }

        WWWForm form = new WWWForm();

        form.AddField("idGame", config.gameID);
        form.AddField("idCliente", idCliente);
        form.AddField("gameKey", config.returnDecryptKey());

        for (int i = 0; i < tables.Count; i++) {
            stringfast.Clear();
            stringfast.Append("tables[").Append(i).Append("]");
            string temp = stringfast.ToString();
            form.AddField(temp, tables[i]);
        }
        form.AddField("minigames", dboG.minigames);
        form.AddField("perguntasG", dboG.perguntas);
        form.AddField("respostasG", dboG.respostas);
        form.AddField("perguntasE", dboI.perguntas);
        form.AddField("respostasE", dboI.respostas);
        form.AddField("itensG", dboG.itens);
        form.AddField("itens_categorias", dboG.itens_categorias);
        form.AddField("itensE", dboI.itens);

        var requestData = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/getTabsSincronizacao", form);
        requestData.timeout = 10;
        requestData.redirectLimit = 2;

        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(requestData));

        if (requestData.isNetworkError || requestData.isHttpError) {
//            config.isOn = false;
        } else {
            string response = requestData.downloadHandler.text;
            var result = JSON.Parse(response);

            var array1 = result["tabelas"];
            var perguntas = array1["perguntas"];
            var respostas = array1["respostas"];
            var itensJson = array1["itens"];
            var minigamesJson = array1["miniGames"];
            var itens_categoriasJson = array1["itensCategorias"];

            int size;

            DataService data = config.OpenDb();

            size = itensJson.Count;

            if (size >= 1) {
                List<DBOITENS> _itensDB = new List<DBOITENS>();
                startScene.MessageStatus("Atualizando Itens");
                for (int i = 0; i < size; i++) {
                    _itensDB.Add(new DBOITENS() {
                        idItem = itensJson[i]["idItem"].AsInt,
                        idCliente = itensJson[i]["idCliente"].AsInt,
                        idCategoriaItem = itensJson[i]["idCategoriaItem"].AsInt,
                        infoItem = itensJson[i]["infoItem"],
                        nomeItem = itensJson[i]["nomeItem"],
                        ativo = itensJson[i]["ativo"].AsBool ? 1 : 0,
                        valor = itensJson[i]["valor"].AsInt,
                        downloaded = 0
                    });
                }               
                for (int i = 0; i < size; i++) {
                    //TODO async WhenAllTask();
                    startScene.MessageStatus("Atualizando Itens " + (i + 1) + "/" + size + "");

                    if (_itensDB[i].ativo != 1 || config.isOn != true) continue;
                    bool downloaded = false;
                    yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadImageItem(MyResult => downloaded = MyResult, _itensDB[i].idItem)));
                    _itensDB[i].downloaded = downloaded ? 1 : 0;
                    //Debug.Log(_itensDB[i].ToString());
                }

                data.AddItens(_itensDB);
                yield return Timing.WaitForOneFrame;

                if (config.isOn == true) {
                     if (currentSyncDB.sincModeItens == 1 || currentSyncDB.sincModeItens == 3) {
                         dboG.itens = currentSyncDB.itensG;
                     }

                     if (currentSyncDB.sincModeItens == 1 || currentSyncDB.sincModeItens == 2) {
                         dboI.itens = currentSyncDB.itensE;
                     }
                }
            }

            //Baixar imagem de itens previamente não baixado por motivos de erro de conexão ou afins.
            List<DBOITENS> itensPreviousNotDownloaded = data.NotDownloadedItens();

            size = itensPreviousNotDownloaded.Count;
            if (size >= 1 && config.isOn) {
                for (int i = 0; i < size; i++) {
                    startScene.MessageStatus("Download de Itens Restantes " + (i + 1) + "/" + size + "");
                    bool downloaded = false;
                    if (!File.Exists(config.fullPatchItemIcon + itensPreviousNotDownloaded[i].idItem + ".png")) {
                        yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadImageItem(MyResult => downloaded = MyResult, itensPreviousNotDownloaded[i].idItem)));
                    } else {
                        downloaded = true;
                    }
                    itensPreviousNotDownloaded[i].downloaded = downloaded ? 1 : 0;
                }

                data.AddItens(itensPreviousNotDownloaded);
                yield return Timing.WaitForOneFrame;
                //Termino do download de imagens de itens previamente não baixados.
            }
            size = itens_categoriasJson.Count;

            if (size >= 1) {

                List<DBOITENS_CATEGORIAS> itensCategoryDB = new List<DBOITENS_CATEGORIAS>();
                startScene.MessageStatus("Atualizando Categorias");
                for (int i = 0; i < size; i++) {
                    itensCategoryDB.Add(new DBOITENS_CATEGORIAS() {
                        idCategoriaItem = itens_categoriasJson[i]["idCategoriaItem"].AsInt,
                        nomeCategoria = itens_categoriasJson[i]["nomeCategoria"],
                        infoCategoria = itens_categoriasJson[i]["infoCategoria"],
                        ativo = itens_categoriasJson[i]["ativo"].AsBool ? 1 : 0,
                        colecionaveis = itens_categoriasJson[i]["colecionaveis"].AsBool ? 1 : 0
                    });

                }
                    

                data.AddItensCategory(itensCategoryDB);
                yield return Timing.WaitForOneFrame;
                dboG.itens_categorias = currentSyncDB.itens_categorias;

            }

            size = perguntas.Count;

            if (size >= 1) {

                List<DBOPERGUNTAS> perguntasDB = new List<DBOPERGUNTAS>();
                startScene.MessageStatus("Atualizando Perguntas");
                for (int i = 0; i < size; i++) {

                    perguntasDB.Add(new DBOPERGUNTAS() {
                        idCliente = perguntas[i]["idCliente"].AsInt,
                        idPergunta = perguntas[i]["idPergunta"].AsInt,
                        idHabilidade = perguntas[i]["idHabilidade"].AsInt,
                        idLivro = perguntas[i]["idLivro"].AsInt,
                        idDificuldade = perguntas[i]["idDificuldade"].AsInt,
                        textoPergunta = perguntas[i]["textoPergunta"],
                        ativo = perguntas[i]["ativo"].AsBool ? 1 : 0,
                        audio = perguntas[i]["audio"].AsBool ? 1 : 0,
                        downloaded = 0
                    });

                }


                for (int i = 0; i < size; i++) {
                    if (perguntasDB[i].ativo == 1 && config.isOn && perguntasDB[i].audio == 1) {
                        startScene.MessageStatus("Atualizando Perguntas " + i + "/" + size);
                        stringfast.Clear();
                        stringfast.Append(config.fullAudioClipDestinationQuestions).Append(perguntasDB[i].idPergunta).Append(".ogg");
                        bool downloaded = false;
                        yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadQuestionSound(MyResult => downloaded = MyResult, perguntasDB[i].idPergunta)));
                        perguntasDB[i].downloaded = downloaded ? 1 : 0;
                    }
                }


                data.AddAllPerguntas(perguntasDB);
                yield return Timing.WaitForOneFrame;

                if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 3) {
                    dboG.perguntas = currentSyncDB.perguntasG;
                }
                if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 2) {
                    dboI.perguntas = currentSyncDB.perguntasE;
                }

            }

            // Rotina de download de perguntas previamente não baixadas ou por erro ou por falta de conexão com a internet.
            List<DBOPERGUNTAS> notDownloadedPerguntas = data.PerguntasToDownload();
            size = notDownloadedPerguntas.Count;
            for (int i = 0; i < size; i++) {
                stringfast.Clear();
                stringfast.Append(config.fullAudioClipDestinationQuestions).Append(notDownloadedPerguntas[i].idPergunta).Append(".ogg");
                bool downloaded = false;
                if (!File.Exists(stringfast.ToString())) {
                    yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadQuestionSound(MyResult => downloaded = MyResult, notDownloadedPerguntas[i].idPergunta)));
                } else {
                    downloaded = true;
                }

                notDownloadedPerguntas[i].downloaded = downloaded ? 1 : 0;
                data.AddPerguntaOrReplace(notDownloadedPerguntas[i]);
                yield return Timing.WaitForOneFrame;

            }

            //rotina de download das respostas.

                size = respostas.Count;

                if (size >= 1) {

                    List<DBORESPOSTAS> respostasDB = new List<DBORESPOSTAS>();
                    startScene.MessageStatus("Atualizando Respostas");
                    for (int i = 0; i < size; i++) {
                        respostasDB.Add(new DBORESPOSTAS() {
                            idResposta = respostas[i]["idResposta"].AsInt,
                            idPergunta = respostas[i]["idPergunta"].AsInt,
                            textoResposta = respostas[i]["textoResposta"],
                            correta = respostas[i]["correta"].AsBool ? 1 : 0,
                            audio = perguntas[i]["audio"].AsBool ? 1 : 0,
                            downloaded = 0
                        });
                    }
                    

                    for (int i = 0; i < size; i++) {
                        if (respostasDB[i].ativo == 1 && config.isOn && respostasDB[i].audio == 1) {
                            startScene.MessageStatus("Atualizando Perguntas " + i + "/" + size);
                            bool downloaded = false;
                            yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadAnswerID(MyResult => downloaded = MyResult, respostasDB[i].idResposta)));
                            respostasDB[i].downloaded = downloaded ? 1 : 0;
                        }
                    }

                    
                    data.AddAllRespostas(respostasDB);
                    yield return Timing.WaitForOneFrame;

                    if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 3) {
                        dboG.respostas = currentSyncDB.respostasG;
                    }

                    if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 2) {
                        dboI.respostas = currentSyncDB.respostasE;
                    }
                }

            // rotina de respostas previamente não baixadas por questões de falha na conexão ou problemas no meio.
            List<DBORESPOSTAS> notDownloadedRespotas = data.RespostaNotDownloaded();
            size = notDownloadedRespotas.Count;

            for (int i = 0; i < size; i++) {
                stringfast.Clear();
                stringfast.Append(config.fullAudioClipDestinationAnswers).Append(notDownloadedRespotas[i].idResposta).Append(".ogg");
                bool downloaded = false;
                if (!File.Exists(stringfast.ToString())) {
                    yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadAnswerID(MyResult => downloaded = MyResult, notDownloadedRespotas[i].idResposta)));
                } else {
                    downloaded = true;
                }
                notDownloadedRespotas[i].downloaded = downloaded ? 1 : 0;
                data.AddRespostaOrReplace(notDownloadedRespotas[i]);
            }



            size = minigamesJson.Count;

            if (size >= 1) {

                //Criar List de Minigames
                //List<MinigameStruct> minigamesDB = new List<MinigameStruct>();
                List<DBOMINIGAMES> minigamesDB = new List<DBOMINIGAMES>();
                startScene.MessageStatus("Atualizando informações do Jogo");

                for (int i = 0; i < size; i++) {
                    minigamesDB.Add(new DBOMINIGAMES() {
                        ativo = minigamesJson[i]["ativo"].AsBool ? 1 : 0,
                        idMinigames = minigamesJson[i]["idMiniGame"].AsInt,
                        idLivro = minigamesJson[i]["idLivro"].AsInt,
                        nomeMinigame = minigamesJson[i]["nomeMiniGame"],
                        infoMinigame = minigamesJson[i]["infoMiniGame"]
                    });
                }

                data.UpdateMinigames(minigamesDB);

                yield return Timing.WaitForOneFrame;

                if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 3) {
                    dboG.minigames = currentSyncDB.minigames;
                }

                if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 2) {
                    dboI.minigames = currentSyncDB.minigames;
                }


            }

        }



        if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 3) {
            db.UpdateSync(dboG);
        }

        if (currentSyncDB.sincModePerguntas == 1 || currentSyncDB.sincModePerguntas == 2) {
            db.UpdateSync(dboI);
        }



        isDoingOperation = false;
        requestData.Dispose();
       }


    #endregion

    #region afterLoginSync
    public IEnumerator<float> SyncAfterLogin(int _idCliente, DataService db, string URI, string tokenTemp, StartSceneManager _startScene) {
        isDoingOperation = true;
        startScene.MessageStatus("Sincronizando Informações");
        WWWForm form = new WWWForm();
        DBOSINCRONIZACAO dbo = db.GetSync(_idCliente);
        bool hasTables = false;
        //form.AddField("idUsuario", )
        form.AddField("idCliente", _idCliente);
        form.AddField("token", token);
        form.AddField("idUsuario", config.playerID);
        form.AddField("usuarios", dbo.usuarios);
        form.AddField("idGame", config.gameID);
        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);

        List<string> tables = new List<string>();

        yield return Timing.WaitForSeconds(0.1f);

        if (LocalIsOlder(dbo.escola, currentSyncDB.escola)) {
            tables.Add("escola");
            hasTables = true;
        }

        if (LocalIsOlder(dbo.turma, currentSyncDB.turma)) {
            tables.Add("turma");
            hasTables = true;
        }

        if (LocalIsOlder(dbo.usuarios, currentSyncDB.usuarios)) {
            tables.Add("usuarios");
            hasTables = true;
        }

        if (LocalIsOlder(dbo.ranking, currentSyncDB.ranking)) {
            tables.Add("ranking");
            hasTables = true;
        }

        if (hasTables == false) {
            tables.Add("");
        }

        for (int i = 0; i < tables.Count; i++) {
            stringfast.Clear();
            stringfast.Append("tables[").Append(i).Append("]");
            string temp = stringfast.ToString();
            form.AddField(temp, tables[i]);
        }

        var request = UnityWebRequest.Post(URI, form);
        request.timeout = 20;
        request.redirectLimit = 2;
        yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

        string response = request.downloadHandler.text;
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            //Debug.Log(request.error);
            if (request.isNetworkError) {
//                config.isOn = false;
                _startScene.OnlineAcessFailed();
            } else {
                _startScene.OnlineAcessFailed();
            }
        } else {

            var tablesJSON = result["tabelas"];
            //yield return Timing.WaitForSeconds(0.2f);
            var score = tablesJSON["pontuacao"];
            var escolas = tablesJSON["escola"];
            var turmas = tablesJSON["turma"];
            var usuarios = tablesJSON["usuarios"];
            var ranking = tablesJSON["ranking"];
            var ranking2 = tablesJSON["rankingusuario"];
            var inventory = tablesJSON["inventario"];


            //yield return Timing.WaitForSeconds(0.5f);

            startScene.MessageStatus("Sincronizando Pontuação");
            DBOPONTUACAO newSCORE = new DBOPONTUACAO() {
                pontuacaoTotal = score[0]["pontuacaoTotal"].AsInt,
                brops = score[0]["brops"].AsInt,
                dataUpdate = score[0]["dataUpdate"],
                idUsuario = config.playerID,
                BropsDevice = 0,
                PontuacaoTotalDevice = 0
            };

            config.currentScore = newSCORE;

            config.TotalPoints = newSCORE.pontuacaoTotal;
            config.BropsAmount = newSCORE.brops;
            config.BropsDeviceAmount = 0;
            config.TotalPointsDevice = 0;

            newSCORE.online = 1;
            db.UpdateScore(newSCORE);

            int size = escolas.Count;

            if (size >= 1) {

                List<DBOESCOLA> escolasDB = new List<DBOESCOLA>();
                startScene.MessageStatus("Atualizando Lista de Escolas");
                for (int i = 0; i < size; i++) {
                    escolasDB.Add(new DBOESCOLA() {
                        idEscola = escolas[i]["idEscola"].AsInt,
                        nomeEscola = escolas[i]["nomeEscola"],
                        dataUpdate = escolas[i]["dataUpdate"],
                        idCliente = _idCliente
                    });
                }

                db.AddAllEscolas(escolasDB);

                dbo.escola = currentSyncDB.escola;

            }

            size = turmas.Count;

            if (size >= 1) {
                List<DBOTURMA> turmasDB = new List<DBOTURMA>();
                startScene.MessageStatus("Atualizando Lista de Turmas");
                for (int i = 0; i < size; i++) {
                    turmasDB.Add(new DBOTURMA() {
                        idTurma = turmas[i]["idTurma"].AsInt,
                        idAnoLetivo = turmas[i]["idAnoLetivo"].AsInt,
                        idEscola = turmas[i]["idEscola"].AsInt,
                        descTurma = turmas[i]["descTurma"],
                        dataUpdate = turmas[i]["dataUpdate"],
                    });
                }
                db.AddAllTurmas(turmasDB);

                dbo.turma = currentSyncDB.turma;
            }

            yield return Timing.WaitForSeconds(0.2f);

            size = usuarios.Count;

            if (size >= 1)
            {
                var listUsers = new List<DBOUSUARIOS>();
                startScene.MessageStatus("Atualizando Lista de Usuários");
                for (int i = 0; i < size; i++) {
                    DBOUSUARIOS userTemp = new DBOUSUARIOS
                    {
                        idUsuario = usuarios[i]["idUsuario"].AsInt,
                        idCliente = _idCliente,
                        tipoUsuario = usuarios[i]["tipoUsuario"] == null ? 0 : usuarios[i]["tipoUsuario"].AsInt,
                        idTurma = usuarios[i]["idTurma"].AsInt,
                        nomeJogador = usuarios[i]["nomeJogador"],
                        login = usuarios[i]["login"],
                        senha = usuarios[i]["senha"],
                        dataUpdate = usuarios[i]["dataUpdate"],
                        ativo = usuarios[i]["ativo"].AsBool ? 1 : 0
                    };
                    listUsers.Add(userTemp);

                }
                dbo.usuarios = currentSyncDB.usuarios;
                db.AddAllUser(listUsers);

            }

            yield return Timing.WaitForSeconds(0.2f);

            size = ranking.Count;

            if (size >= 1) {

                List<DBORANKING> rankingDB = new List<DBORANKING>();
                startScene.MessageStatus("Atualizando Ranking dos Minigames");
                for (int i = 0; i < size; i++) {
                    rankingDB.Add(new DBORANKING() {
                        idMinigame = ranking[i]["idMiniGame"].AsInt,
                        idUsuario = ranking[i]["idUsuario"].AsInt,
                        highscore = ranking[i]["highScore"].AsInt,
                        posicao = -1,
                        online = 1
                    });
                }
//                db.ClearRanking();
                db.InsertRanking(rankingDB);
//                EduqbrinqLogger.Instance.SendRequest(rankingDB).GetAwaiter();
                dbo.ranking = currentSyncDB.ranking;

            }

            yield return Timing.WaitForSeconds(0.2f);

            size = ranking2.Count;

            //Own Rank Loop.
            if (size >= 1) {


                List<DBORANKING> rankingDB = new List<DBORANKING>();
                startScene.MessageStatus("Atualizando Ranking dos Minigames");

                for (int i = 0; i < size; i++) {
                    DBORANKING temp = new DBORANKING() {
                        idMinigame = ranking2[i]["idMiniGame"].AsInt,
                        idUsuario = userID,
                        highscore = ranking2[i]["highscore"].AsInt,
                        posicao = ranking2[i]["posicao"].AsInt,
                        estrelas = ranking2[i]["estrelas"].AsInt,
                        online = 1
                    };

                    rankingDB.Add(temp);
                }
//                EduqbrinqLogger.Instance.SendRequest(rankingDB).GetAwaiter();
                db.InsertRanking(rankingDB);

            }


            yield return Timing.WaitForSeconds(0.2f);

            size = inventory.Count;

            if (size >= 1) {
                startScene.MessageStatus("Atualizando Inventário");
                List<DBOINVENTARIO> inventoryDB = new List<DBOINVENTARIO>();
                for (int i = 0; i < size; i++) {
                    inventoryDB.Add(new DBOINVENTARIO() {
                        idItem = inventory[i]["idItem"],
                        idUsuario = userID,
                        quantidade = inventory[i]["quantidade"],
                        ativo = inventory[i]["ativo"].AsBool ? 1 : 0,
                        dataUpdate = inventory[i]["dataUpdate"],
                        online = 1,
                        deviceQuantity = 0
                    });
                }

                db.SetInventario(inventoryDB);
            }

            db.UpdateSync(dbo);

            _startScene.OnlineAcessSucess();
            request.Dispose();
        }
    }

    public IEnumerator<float> SyncAfterLogin2(int _idCliente, DataService db, string URI, string tokenTemp, StartSceneManager _startScene) {
        isDoingOperation = true;
        startScene.MessageStatus("Sincronizando Informações");
        WWWForm form = new WWWForm();
        DBOSINCRONIZACAO dbo = db.GetSync(_idCliente);
        bool hasTables = false;
        //form.AddField("idUsuario", )
        form.AddField("idCliente", _idCliente);
        form.AddField("token", token);
        form.AddField("idUsuario", config.playerID);
        form.AddField("usuarios", dbo.usuarios);
        form.AddField("idGame", config.gameID);
        form.AddField("deviceId", SystemInfo.deviceUniqueIdentifier);

        List<string> tables = new List<string>();

        yield return Timing.WaitForSeconds(0.1f);

        if (LocalIsOlder(dbo.escola, currentSyncDB.escola)) {
            tables.Add("escola");
            hasTables = true;
        }

        if (LocalIsOlder(dbo.turma, currentSyncDB.turma)) {
            tables.Add("turma");
            hasTables = true;
        }

        if (LocalIsOlder(dbo.usuarios, currentSyncDB.usuarios)) {
            tables.Add("usuarios");
            hasTables = true;
        }

        if (LocalIsOlder(dbo.ranking, currentSyncDB.ranking)) {
            tables.Add("ranking");
            hasTables = true;
        }

        if (hasTables == false) {
            tables.Add("");
        }

        for (int i = 0; i < tables.Count; i++) {
            stringfast.Clear();
            stringfast.Append("tables[").Append(i).Append("]");
            string temp = stringfast.ToString();
            form.AddField(temp, tables[i]);
        }

        var request = UnityWebRequest.Post(URI, form);

        yield return Timing.WaitUntilDone(request.SendWebRequest());

        string response = request.downloadHandler.text;
        Log.d(response);
        var result = JSON.Parse(response);

        if (request.isNetworkError || request.isHttpError || response.Contains("erro")) {
            if (request.isNetworkError) {
//                config.isOn = false;
                _startScene.OnlineAcessFailed();
            } else {
                _startScene.OnlineAcessFailed();
            }
        } else {

            var tablesJSON = result["tabelas"];
            //yield return Timing.WaitForSeconds(0.2f);
            var score = tablesJSON["pontuacao"];
            var escolas = tablesJSON["escola"];
            var turmas = tablesJSON["turma"];
            Log.d($"Turma Json: {turmas}");
            var usuarios = tablesJSON["usuarios"];
            var ranking = tablesJSON["ranking"];
            var ranking2 = tablesJSON["rankingusuario"];
            var inventory = tablesJSON["inventario"];


            //yield return Timing.WaitForSeconds(0.5f);

            startScene.MessageStatus("Sincronizando Pontuação");
            DBOPONTUACAO newSCORE = new DBOPONTUACAO() {
                pontuacaoTotal = score[0]["pontuacaoTotal"].AsInt,
                brops = score[0]["brops"].AsInt,
                dataUpdate = score[0]["dataUpdate"],
                idUsuario = config.playerID,
                BropsDevice = 0,
                PontuacaoTotalDevice = 0
            };

            config.currentScore = newSCORE;

            config.TotalPoints = newSCORE.pontuacaoTotal;
            config.BropsAmount = newSCORE.brops;
            config.BropsDeviceAmount = 0;
            config.TotalPointsDevice = 0;

            newSCORE.online = 1;
            db.UpdateScore(newSCORE);

            int size = escolas.Count;

            if (size >= 1) {

                List<DBOESCOLA> escolasDB = new List<DBOESCOLA>();
                startScene.MessageStatus("Atualizando Lista de Escolas");
                for (int i = 0; i < size; i++) {
                    escolasDB.Add(new DBOESCOLA() {
                        idEscola = escolas[i]["idEscola"].AsInt,
                        nomeEscola = escolas[i]["nomeEscola"],
                        dataUpdate = escolas[i]["dataUpdate"],
                        idCliente = _idCliente
                    });
                }

                db.AddAllEscolas(escolasDB);

                dbo.escola = currentSyncDB.escola;

            }

            size = turmas.Count;

            if (size >= 1) {
                List<DBOTURMA> turmasDB = new List<DBOTURMA>();
                startScene.MessageStatus("Atualizando Lista de Turmas");
                for (int i = 0; i < size; i++) {
                    turmasDB.Add(new DBOTURMA() {
                        idTurma = turmas[i]["idTurma"].AsInt,
                        idAnoLetivo = turmas[i]["idAnoLetivo"].AsInt,
                        idEscola = turmas[i]["idEscola"].AsInt,
                        descTurma = turmas[i]["descTurma"],
                        dataUpdate = turmas[i]["dataUpdate"],
                    });
                }
                db.AddAllTurmas(turmasDB);

                dbo.turma = currentSyncDB.turma;
            }

            yield return Timing.WaitForSeconds(0.2f);

            size = usuarios.Count;

            if (size >= 1) {

                startScene.MessageStatus("Atualizando Lista de Usuarios");
                for (int i = 0; i < size; i++) {
                    DBOUSUARIOS userTemp = new DBOUSUARIOS() {
                        idUsuario = usuarios[i]["idUsuario"].AsInt,
                        idCliente = _idCliente,
                        tipoUsuario = (usuarios[i]["tipoUsuario"] == null) ? 0 : usuarios[i]["tipoUsuario"].AsInt,
                        idTurma = usuarios[i]["idTurma"].AsInt,
                        nomeJogador = usuarios[i]["nomeJogador"],
                        login = usuarios[i]["login"],
                        senha = usuarios[i]["senha"],
                        dataUpdate = usuarios[i]["dataUpdate"],
                        ativo = (usuarios[i]["ativo"].AsBool) ? 1 : 0
                    };

                    db.InsertUser(userTemp);

                }

                dbo.usuarios = currentSyncDB.usuarios;

            }

            yield return Timing.WaitForSeconds(0.2f);

            size = ranking.Count;

            if (size >= 1) {

                List<DBORANKING> rankingDB = new List<DBORANKING>();
                startScene.MessageStatus("Atualizando Ranking dos Minigames");
                for (int i = 0; i < size; i++) {
                    rankingDB.Add(new DBORANKING() {
                        idMinigame = ranking[i]["idMiniGame"].AsInt,
                        idUsuario = ranking[i]["idUsuario"].AsInt,
                        highscore = ranking[i]["highScore"].AsInt,
                        posicao = -1,
                        online = 1,
                    });
                }

//                db.ClearRanking();
                db.InsertRanking(rankingDB);

                dbo.ranking = currentSyncDB.ranking;

            }

            yield return Timing.WaitForSeconds(0.2f);

            size = ranking2.Count;

            //Own Rank Loop.
            if (size >= 1) {


                List<DBORANKING> rankingDB = new List<DBORANKING>();
                startScene.MessageStatus("Atualizando Ranking dos Minigames 2");

                for (int i = 0; i < size; i++) {
                    DBORANKING temp = new DBORANKING() {
                        idMinigame = ranking2[i]["idMiniGame"].AsInt,
                        idUsuario = userID,
                        highscore = ranking2[i]["highscore"].AsInt,
                        posicao = ranking2[i]["posicao"].AsInt,
                        estrelas = ranking2[i]["estrelas"].AsInt,
                        online = 1
                    };

                    rankingDB.Add(temp);
                }

                db.InsertRanking(rankingDB);

            }


            yield return Timing.WaitForSeconds(0.2f);

            size = inventory.Count;

            if (size >= 1) {
                startScene.MessageStatus("Atualizando Inventário");
                List<DBOINVENTARIO> inventoryDB = new List<DBOINVENTARIO>();
                for (int i = 0; i < size; i++) {
                    inventoryDB.Add(new DBOINVENTARIO() {
                        idItem = inventory[i]["idItem"],
                        idUsuario = userID,
                        quantidade = inventory[i]["quantidade"],
                        ativo = inventory[i]["ativo"].AsBool ? 1 : 0,
                        dataUpdate = inventory[i]["dataUpdate"],
                        online = 1,
                        deviceQuantity = 0
                    });
                }

                db.SetInventario(inventoryDB);
            }

            db.UpdateSync(dbo);

            _startScene.OnlineAcessSucess2();
            request.Dispose();
        }

    }

    #endregion

    #region DelPergunta e Resposta

    public IEnumerator<float> DelsSync(int clientID) {
        isDoingOperation = true;
        clientID = 1;
        DataService db = GameConfig.Instance.OpenDb();
        startScene.MessageStatus("Verificando Remoção de Perguntas");
        DBOSINCRONIZACAO dbo = GameConfig.Instance.OpenDb().GetSync(clientID);

        WWWForm form = new WWWForm();

        form.AddField("idCliente", clientID);
        form.AddField("idPerguntaDel", dbo.idDelPergunta);
        form.AddField("idRespostaDel", dbo.idDelResposta);
        form.AddField("gameKey", GameConfig.Instance.returnDecryptKey());

        using (UnityWebRequest request = UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/GetPRRemovidas", form)) {
            request.timeout = 10;
            request.redirectLimit = 2;
            yield return Timing.WaitUntilDone(startScene.StartProgressWebRequest(request));

            if (request.isNetworkError || request.isHttpError) {

            } else {

                var jsonDecode = JSON.Parse(request.downloadHandler.text);
                var decode = jsonDecode["PR_Removidas"];
                var delPergunta = decode["perguntasDel"];
                var delRespsota = decode["respostasDel"];

                int size = delPergunta.Count;

                for (int i = 0; i < size; i++) {
                    db.DeleteQuestion(delPergunta[i]["idPergunta"].AsInt);
                }

                size = delRespsota.Count;

                for (int i = 0; i < size; i++) {
                    db.DelAnswer(delRespsota[i]["idResposta"].AsInt);
                }

                dbo.idDelPergunta = currentSyncDB.idDelPergunta;
                dbo.idDelResposta = currentSyncDB.idDelResposta;

                db.UpdateSync(dbo);
            }

            request.Dispose();
        }
        
        isDoingOperation = false;
    }

    #endregion


    public DateTime DateFromString(string dateString) {
        //Debug.Log(dateString);
        if(dateString == null) {
            dateString = "2017-01-01 13:31:23.937";
        }
        return DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

    public bool LocalIsOlder(string onlineDate, string offlineDate) {
        if (DateTime.Compare(DateFromString(onlineDate), DateFromString(offlineDate)) < 0) {
            return true;
        } else {
            return false;
        }
    }

    public string Md5Sum(string strToEncrypt) {
        UTF8Encoding ue = new UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++) {
            hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public void RegisterLogLogin(DBOUSUARIOS _user) {
        if (_user != null) {
            DBOGAMES_LOGS temp = new DBOGAMES_LOGS {
                dataAcesso = config.ReturnCurrentDate(),
                deviceID = SystemInfo.deviceUniqueIdentifier,
                idGame = config.gameID,
                idUsuario = _user.idUsuario,
                online = 0,
                versao = Application.version,
                ID = 0,
            };

            config.OpenDb().InserGamesLOG(temp);
        }
    }

    public void LoadImageItemIcon(int itemID) {
        //Timing.RunCoroutine(LoadImageItem(itemID));


    }


    IEnumerator<float> LoadImageItem(Action<bool> MyResult, int idItem) {
        stringfast.Clear();
        stringfast.Append("https://api.eduqbrinq.com.br/midias/itens/").Append(idItem).Append(".png");
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(stringfast.ToString())){
            www.redirectLimit = 2;
            www.timeout = 30;
            yield return Timing.WaitUntilDone(www.SendWebRequest());

            if (www.isNetworkError || www.isHttpError) {



                MyResult(false);
                www.Dispose();
            } else {
                var bytes = ((DownloadHandlerTexture)www.downloadHandler).texture.EncodeToPNG();
                File.WriteAllBytes(config.fullPatchItemIcon + idItem + ".png", bytes);
                MyResult(true);
                www.Dispose();
            }
        }
       
    }

    IEnumerator<float> LoadQuestionSound(Action<bool> MyResult, int QuestionID)
    {
        var www = UnityWebRequestMultimedia.GetAudioClip("https://api.eduqbrinq.com.br/midias/perguntas/" + QuestionID + ".ogg", AudioType.OGGVORBIS);
        www.timeout = 20;
        www.redirectLimit = 2;
        yield return Timing.WaitUntilDone(www.SendWebRequest());

        

        if (www.isNetworkError || www.isHttpError) {
            //config.isOnline = false;
            //config.QuestionIDtoDownload.Add(QuestionID);
            MyResult(false);
        }
        else {
            var bytes = ((DownloadHandlerAudioClip)www.downloadHandler).data;
            if (bytes.Length > 2000) {
                File.WriteAllBytes(config.fullAudioClipDestinationQuestions + QuestionID + ".ogg", bytes);
                MyResult(true);
            } else {
                if (!config.QuestionIDtoDownload.Contains(QuestionID)) {
                    config.QuestionIDtoDownload.Add(QuestionID);
                    MyResult(false);
                }
            }
        }

        www.Dispose();
    }

    IEnumerator<float> LoadAnswerID(Action<bool> MyResult, int AnswerID)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("https://api.eduqbrinq.com.br/midias/respostas/" + AnswerID + ".ogg", AudioType.OGGVORBIS);
        www.timeout = 20;
        www.redirectLimit = 2;
        yield return Timing.WaitUntilDone(www.SendWebRequest());

        if (www.isNetworkError || www.isHttpError)
        {
            //config.isOnline = false;
            //config.AnswersIDtoDoownload.Add(AnswerID);
            MyResult(false);
        } else
        {
            var bytes = ((DownloadHandlerAudioClip)www.downloadHandler).data;
            if (bytes.Length > 2000)
            {
                File.WriteAllBytes(config.fullAudioClipDestinationAnswers + AnswerID + ".ogg", bytes);
                MyResult(true);
            } else
            {
                MyResult(false);
            }
        }
        www.Dispose();
    }

    public string JsonSerialize(object toSerialization)
    {
        var jsonValue = JsonWriter.GetWriter().Write(toSerialization);
        #if UNITY_EDITOR
        Debug.Log($"{StackTraceUtility.ExtractStackTrace()}{'\n'}{jsonValue}");
        #endif
        return jsonValue;
    }

    public string GenerateHashInJson(string jsonValue) {
        
        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] Bytes = UE.GetBytes(jsonValue);
        SHA1Managed SHhash = new SHA1Managed();
        byte[] HashValue = SHhash.ComputeHash(Bytes);
        int[] BytesInInt = new int[HashValue.Length];
        for (int i = 0; i < HashValue.Length; i++) {
            BytesInInt[i] = HashValue[i];
        }
        var JsonHash = JsonSerialize(BytesInInt);
        #if UNITY_EDITOR
        Debug.Log($"{StackTraceUtility.ExtractStackTrace()}{'\n'}{JsonHash}");
        #endif
        return JsonHash;
    }


}
