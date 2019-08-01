using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using com.csutil;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Networking;

public class EduqbrinqLogger : MonoBehaviour
{
    private static GameConfig _config => GameConfig.Instance;
    private static string UserToken => _config.netHelper.token;
    private static int UserId => _config.GetCurrentUserID();
    private static int ClientId => _config.clientID;
    private static int GameId => _config.gameID;
//    private static (string token, int id) UserInfo => (UserToken, UserId);
    private static (string token, int id, int gameId, int clientId) UserInfo => (UserToken, UserId, ClientId, GameId);

    private Queue<EduqbringLogGameInfo> _gameInfos;

    private static EduqbrinqLogger _instance;

    private static bool IsQueueEmpty => _instance._gameInfos.Count >= 1;
    
    public static EduqbrinqLogger Instance
    {
        get
        {
            if (_instance != null && _instance.gameObject) return _instance;
            var instanceHome = GameObject.Find("Eduqbrinq Controller");

            if (instanceHome == null)
            {
                instanceHome = new GameObject { name = "Eduqbrinq Controller" };
                DontDestroyOnLoad(instanceHome);
            }

            _instance = instanceHome.GetComponent<EduqbrinqLogger>() ?? instanceHome.AddComponent<EduqbrinqLogger>();
            IoC.inject.SetSingleton(_instance);
            IoC.inject.GetOrAddSingleton<EduqbrinqLogger>(_instance);
            return _instance;
        }
//        set => _instance = value;
    }

    public void AddGameInfo(EduqbringLogGameInfo info)
    {
        if(_gameInfos == null)
            _gameInfos = new Queue<EduqbringLogGameInfo>();

        _gameInfos?.Enqueue(info);
    }

    public async UniTask SendRequest(List<DBORANKING> logs)
    {
        var toSend = new List<UniTask>();
        foreach (var log in logs)
        {
            toSend.Add(SendRequestSync(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setRanking",
                    log.ToForm(UserInfo)), log));
        }
        await UniTask.WhenAll(toSend);
    }

    async UniTask SendRequestSync(UnityWebRequest req, DBORANKING rank)
    {
        var op = await req.SendWebRequest();
        if (IsValidRequest(op))
        {
            rank.online = 1;
        }
        _config.OpenDb().UpdateRanking(rank).ToCoroutine();
    }

    public void UpdateDatabase(DBORANKING updatedRank)
    {
        var task = SaveDatabase(updatedRank).GetAwaiter();
        task.OnCompleted(() => Log.d($"Rank Updated! Info: {JsonWriter.GetWriter().Write(updatedRank)}"));
    }

    async UniTask SaveDatabase(DBORANKING rankUpdated)
    {
        if (IsOnline)
        {
            await SendRequest(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setRanking",
                    rankUpdated.ToForm(UserInfo)), rankUpdated);
        }
        else
        {
            rankUpdated.online = IsOnlineInt;
            _config.OpenDb().UpdateRanking(rankUpdated).ToCoroutine();
        }
    }

    public async UniTask SendRequest(List<DBOPONTUACAO> logs)
    {
        var toSend = new List<UniTask>();
        foreach (var log in logs)
        {
            toSend.Add(SendRequestSync(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setRanking",
                    log.ToForm(UserInfo)), log));
        }
        await UniTask.WhenAll(toSend);
    }

    async UniTask SendRequestSync(UnityWebRequest req, DBOPONTUACAO score)
    {
        var op = await req.SendWebRequest();
        if (IsValidRequest(op))
        {
            score.online = 1;
        }
        _config.OpenDb().UpdateScore(score);
    }


    async UniTask SendRequest(UnityWebRequest req, DBORANKING rank)
    {
        var op = await req.SendWebRequest();
        if (!IsValidRequest(op))
        {
            rank.online = 0;
        }
        _config.OpenDb().UpdateRanking(rank).ToCoroutine();
//        Log.d($"Sended or Saved Sucess of [DBOMINIGAMES_LOGS]: {JsonWriter.GetWriter().Write(log)}");
    }



    public void UpdateDatabase(DBOPONTUACAO updatedScore)
    {
        var task = SaveDatabase(updatedScore).GetAwaiter();
        task.OnCompleted(() => Log.d($"Score Updated! Info: {JsonWriter.GetWriter().Write(updatedScore)}"));
    }

    async UniTask SaveDatabase(DBOPONTUACAO score)
    {
        if (IsOnline)
        {
            await SendRequest(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setPontuacao",
                    score.ToForm(UserInfo)), score);
        }
        else
        {
            score.online = IsOnlineInt;
            _config.OpenDb().UpdateScore(score);
        }
    }

    async UniTask SendRequest(UnityWebRequest req, DBOPONTUACAO score)
    {
        var op = await req.SendWebRequest();
        if (!IsValidRequest(op))
        {
            score.online = 0;

        }
        _config.OpenDb().UpdateScore(score);
    }

    public EduqbringLogGameInfo GenerateGameInfo(DBOMINIGAMES_LOGS log, List<DBOESTATISTICA_DIDATICA> estatisticas)
    {
        return new EduqbringLogGameInfo{ GameLog = log, Estatisticas = estatisticas};
    }

    public void SendDataLog()
    {
        var current = _gameInfos.Dequeue();
        var task = SendLogs(current).GetAwaiter();
        task.OnCompleted(() => Log.d("Send Data Log Completed!"));
    }

    public static string GetCurrentDateTime() => System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);


    async UniTask SendLogs(EduqbringLogGameInfo info)
    {
        await UniTask.WhenAll(SendEstatisticas(info.Estatisticas),
            GameLog(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setJogosLogs", info.GameLog.ToForm(UserInfo)), info.GameLog));
    }

    public async UniTask SendRequest(List<DBOMINIGAMES_LOGS> logs)
    {
        var toSend = new List<UniTask>();
        foreach (var log in logs)
        {
            toSend.Add(GameLogSync(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setJogosLogs",
                    log.ToForm(UserInfo)), log));
        }
        await UniTask.WhenAll(toSend);
    }

    async UniTask GameLogSync(UnityWebRequest req, DBOMINIGAMES_LOGS log)
    {
        var op = await req.SendWebRequest();
        if (IsValidRequest(op))
        {
            _config.OpenDb().DeleteMinigamesLog(log);
        }

//        Log.d($"Sended or Saved Sucess of [DBOMINIGAMES_LOGS]: {JsonWriter.GetWriter().Write(log)}");
    }
    
    
    async UniTask GameLog(UnityWebRequest req, DBOMINIGAMES_LOGS log)
    {
        var op = await req.SendWebRequest();
        if (!IsValidRequest(op))
        {
            log.online = 0;

        }
        _config.OpenDb().InsertLogAsync(log);
//        Log.d($"Sended or Saved Sucess of [DBOMINIGAMES_LOGS]: {JsonWriter.GetWriter().Write(log)}");
    }

    async UniTask GameLog(UnityWebRequest req, DBOESTATISTICA_DIDATICA estatistica)
    {
        var op = await req.SendWebRequest();
        if (IsValidRequest(op))
        {
            estatistica.online = 1;
        }
        _config.OpenDb().InsertLogAsync(estatistica).ToCoroutine();
//        Log.d($"Sended or Saved Sucess of [DBOESTATISTICA_DIDATICA]: {JsonWriter.GetWriter().Write(estatistica)}");
    }
    

    async UniTask SendEstatisticas(List<DBOESTATISTICA_DIDATICA> estatisticas)
    {
        var toSend = new List<UniTask>();
        foreach (var estatistica in estatisticas)
        {
            toSend.Add(GameLog(
                UnityWebRequest.Post("https://api.eduqbrinq.com.br/eduqbrinqApi01/EduqbrinqAZ/setEstatisticaDidatica",
                    estatistica.ToForm(UserInfo)), estatistica));
        }
        await UniTask.WhenAll(toSend);
    }

    private static bool IsValidRequest(UnityWebRequest req) => !req.isHttpError  &&  !req.isNetworkError && !req.downloadHandler.text.Contains("erro");

    public static bool IsOnline => Application.internetReachability != NetworkReachability.NotReachable;
    public int IsOnlineInt => IsOnline ? 1 : 0;
}


public enum EduqbrinqAPI
{
    SetJogosLogs,
    SetEstatisticaDidatica,
}


public struct EduqbringLogGameInfo
{
    public DBOMINIGAMES_LOGS GameLog;
    public List<DBOESTATISTICA_DIDATICA> Estatisticas;
}
