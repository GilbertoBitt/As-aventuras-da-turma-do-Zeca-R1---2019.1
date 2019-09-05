using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

public class LogSystem : OverridableMonoBehaviour {
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Principal")]
	#endif
	public int idMinigame;
	public int idHabilidade;
	public int idDificuldade;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Timers")]
	#endif
	public float tempoLudica;
	public float tempoDidatica;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Pontuação")]
	#endif
	public int pontosLudica;
	public int pontosPedagogica;
	public int pontosInteragindo;

	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Fase")]
	#endif
	public int faseLudica;

	private bool isTimerL = false;
	private bool isTimerD = false;

	private List<DBOESTATISTICA_DIDATICA> statistics = new List<DBOESTATISTICA_DIDATICA> ();
	
	//TODO ScriptableSingleton Object. avoid loosing data when scene context change.

    public override void UpdateMe() {
		if (isTimerL) {
			tempoLudica += Time.deltaTime;
		}

		if (isTimerD) {
			tempoDidatica += Time.deltaTime;
		}
	}


	/// <summary>
	/// Clears all.
	/// </summary>
	public void ClearAll(){
		tempoLudica = 0f;
		tempoDidatica = 0f;
		pontosPedagogica = 0;
		pontosInteragindo = 0;
		faseLudica = 0;
		isTimerL = false;
		isTimerD = false;
	}


	/// <summary>
	/// Sends the game log.
	/// </summary>
	public void SendGameLog()
	{
		Timing.RunCoroutine(SendAllLogs());
	}

	private IEnumerator<float> SendAllLogs()
	{
		var log = new DBOMINIGAMES_LOGS()
		{
			deviceID = SystemInfo.deviceUniqueIdentifier,
			faseLudica = faseLudica,
			tempoLudica = GameConfig.Instance.TimeToIntMilliseconds(tempoLudica),
			tempoDidatica = GameConfig.Instance.TimeToIntMilliseconds(tempoDidatica),
			pontosLudica = pontosLudica,
			pontosPedagogica = pontosPedagogica,
			pontosInteragindo = pontosInteragindo,
			idMinigame = idMinigame,
			personagem = GameConfig.Instance.GetCharName(PlayerPrefs.GetInt("characterSelected", 0)),
			online = EduqbrinqLogger.Instance.IsOnlineInt,
			idUsuario = GameConfig.Instance.playerID,
			dataAcesso = GameConfig.Instance.ReturnCurrentDate()
		};
		var info = EduqbrinqLogger.Instance.GenerateGameInfo(log, statistics);
		EduqbrinqLogger.Instance.AddGameInfo(info);
		EduqbrinqLogger.Instance.SendDataLog();
//		config.SaveLOG();
//                 //(Log);
//        
//        		int count = statistics.Count;
//        
//        		if(count >= 1) {
//                    //config.SaveStatistic(statistics);
//                    config.SaveAllStatistic(statistics);         
//                }
//
                yield return Timing.WaitForOneFrame;
	}

	/// <summary>
	/// Adds the pontos ludica.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void AddPontosLudica(int amount){
		pontosLudica += amount;
	}

	/// <summary>
	/// Adds the pontos pedagogica.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void AddPontosPedagogica(int amount){
		pontosPedagogica += amount;
	}

	/// <summary>
	/// Adds the pontos interagindo.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public void AddPontosInteragindo(int amount){
		pontosInteragindo += amount;
	}

	/// <summary>
	/// Starts the timer ludica.
	/// </summary>
	/// <param name="enable">If set to <c>true</c> enable.</param>
	public void StartTimerLudica(bool enable){
		isTimerL = enable;
	}


	/// <summary>
	/// Starts the timer didatica.
	/// </summary>
	/// <param name="enable">If set to <c>true</c> enable.</param>
	public void StartTimerDidatica(bool enable){
		isTimerD = enable;
	}

	public void SaveEstatistica(int _idGameDidatico, int _idDificuldade, bool _isRight)
	{
		Timing.RunCoroutine(SaveStatisticaCoroutine(_idGameDidatico, _idDificuldade, _isRight));
	}
	public void SaveEstatistica(int _idGameDidatico, int habilidade, int _idDificuldade, bool _isRight)
	{
		Timing.RunCoroutine(SaveStatisticaCoroutine(_idGameDidatico, habilidade,_idDificuldade, _isRight));
	}
	
	private IEnumerator<float> SaveStatisticaCoroutine(int _idGameDidatico, int habilidade, int _idDificuldade, bool isRight)
	{
		var statisticTemp = new DBOESTATISTICA_DIDATICA{
			acertou = (isRight) ? 1 : 0,
			idDificuldade = _idDificuldade,
			idGameDidatico = _idGameDidatico,
			idHabilidade = habilidade,
			idMinigame = idMinigame,
			online = EduqbrinqLogger.Instance.IsOnlineInt,
			dataInsert = GameConfig.Instance.ReturnCurrentDate()
		};
		if (GameConfig.Instance.currentUser != null) {
			statisticTemp.idUsuario = GameConfig.Instance.currentUser.idUsuario;
		}
		statistics.Add(statisticTemp);
		yield return Timing.WaitForOneFrame;
	}


	private IEnumerator<float> SaveStatisticaCoroutine(int _idGameDidatico, int _idDificuldade, bool isRight)
	{
		var statisticTemp = new DBOESTATISTICA_DIDATICA{
			acertou = (isRight) ? 1 : 0,
			idDificuldade = _idDificuldade,
			idGameDidatico = _idGameDidatico,
			idHabilidade = -1,
			idMinigame = idMinigame,
			online = EduqbrinqLogger.Instance.IsOnlineInt,
			dataInsert = GameConfig.Instance.ReturnCurrentDate()
		};
		if (GameConfig.Instance.currentUser != null) {
			statisticTemp.idUsuario = GameConfig.Instance.currentUser.idUsuario;
		}
		statistics.Add(statisticTemp);
		yield return Timing.WaitForOneFrame;
	}

	public void SaveEstatistica(DBOESTATISTICA_DIDATICA temp)
	{
		Timing.RunCoroutine(SaveOnStatistica(temp));
	}

	private IEnumerator<float> SaveOnStatistica(DBOESTATISTICA_DIDATICA temp)
	{
		DBOESTATISTICA_DIDATICA statisticTemp = temp;        
                statisticTemp.dataInsert = GameConfig.Instance.ReturnCurrentDate();
                if (GameConfig.Instance.currentUser != null) {
                    statisticTemp.idUsuario = GameConfig.Instance.currentUser.idUsuario;
                }
                statistics.Add(statisticTemp);
                yield return Timing.WaitForOneFrame;
	}

	public void SaveHighscore(int score, int idUser) {
        if(GameConfig.Instance.currentScore == null) {
	        GameConfig.Instance.currentScore = new DBOPONTUACAO();
        }       
    }



}
