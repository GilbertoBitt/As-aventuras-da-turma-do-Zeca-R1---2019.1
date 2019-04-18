using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSystem : OverridableMonoBehaviour {
	#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN
	[SeparatorAttribute("Principal")]
	#endif
	public GameConfig config;
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
	public void SendGameLog(){

        config.SaveLOG(new DBOMINIGAMES_LOGS() {
            deviceID = SystemInfo.deviceUniqueIdentifier,
            faseLudica = faseLudica,
            tempoLudica = config.TimeToIntMilliseconds(tempoLudica),
            tempoDidatica = config.TimeToIntMilliseconds(tempoDidatica),
            pontosLudica = pontosLudica,
            pontosPedagogica = pontosPedagogica,
            pontosInteragindo = pontosInteragindo,
            idMinigame = idMinigame,
            personagem = config.GetCharName(PlayerPrefs.GetInt("characterSelected", 0)),
            online = 0,
            idUsuario = config.playerID,
            dataAcesso = config.ReturnCurrentDate()            
        });
         //(Log);

		int count = statistics.Count;

		if(count >= 1) {
            //config.SaveStatistic(statistics);
            config.SaveAllStatistic(statistics);         
        }


        
		
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

	public void SaveEstatistica(int _idGameDidatico, int _idDificuldade, bool _isRight){
        DBOESTATISTICA_DIDATICA statisticTemp = new DBOESTATISTICA_DIDATICA() {
            acertou = (_isRight) ? 1 : 0,
            idDificuldade = _idDificuldade,
            idGameDidatico = _idGameDidatico,
            idHabilidade = -1,
            idMinigame = idMinigame,
            dataInsert = config.ReturnCurrentDate()
        };
        if (config.currentUser != null) {
			statisticTemp.idUsuario = config.currentUser.idUsuario;
		}
		statistics.Add(statisticTemp);
	}

    public void SaveEstatistica(DBOESTATISTICA_DIDATICA temp) {
        DBOESTATISTICA_DIDATICA statisticTemp = temp;        
        statisticTemp.dataInsert = config.ReturnCurrentDate();
        if (config.currentUser != null) {
            statisticTemp.idUsuario = config.currentUser.idUsuario;
        }
        statistics.Add(statisticTemp);
    }

    public void SaveHighscore(int score, int idUser) {
        if(config.currentScore == null) {
            config.currentScore = new DBOPONTUACAO();
        }       
    }



}
