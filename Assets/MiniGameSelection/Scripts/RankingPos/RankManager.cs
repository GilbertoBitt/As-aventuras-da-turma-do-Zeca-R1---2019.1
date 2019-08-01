using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using TMPro;
using UnityEngine.UI;
public class RankManager : MonoBehaviour {

    public Canvas DetailCanvas;
    public Canvas RankingCanvas;

    public GameConfig config => GameConfig.Instance;
    public Transform parentContentTransform;
    public GameObject rankPrefabUI;
    public TextMeshProUGUI textTitleBar;
    public List<RankingUI> rankInstances = new List<RankingUI>();
    public RankingUI LastRank;
    public GameObject LastRankComponent;
    public List<DBORANKING> rankingResult = new List<DBORANKING>();
    public List<RankUser> rankUsers = new List<RankUser>();
    public GameDetailWindow gameDetailManager;
    public Color selfColorPos;
    public Color selfColorName;
    public int schoolID;
    public DBORANKING rankingUser;
    public ScrollRect scrollRect;
    public Button buttonGlobal;
    public Button buttonEscolar;
    public ColorBlock colorNormal;
    public ColorBlock colorBlockHighlight;
    public Color defaultColorName;
    public Color defaultColorHighlight;

    public void StartRanking(int idMinigame) {
        Timing.RunCoroutine(LoadRanking(gameDetailManager.idMinigame));
        Debug.Log("_idMinigame " + gameDetailManager.idMinigame);
    }

    public void LoadRanking() {
        StartRanking(gameDetailManager.idMinigame);
        CancasActivate();
        textTitleBar.text = config.allMinigames[gameDetailManager.idMinigame].name;
    }

    public void LoadRankingSchool() {
        Timing.RunCoroutine(LoadRankingSchool(gameDetailManager.idMinigame));
        CancasActivate();
        textTitleBar.text = config.allMinigames[gameDetailManager.idMinigame].name;
    }

    IEnumerator<float> LoadRankingSchool(int _idMinigame) {

        int tempIdMInigame = _idMinigame+1;

        buttonGlobal.colors = colorNormal;
        buttonEscolar.colors = colorBlockHighlight;

        DataService ds = config.OpenDb();
        Debug.Log("Ranking Escola.");
        bool userIsOnTop = false;

        DBOUSUARIOS users = config.OpenDb().GetUser(config.currentUser.idUsuario);
        yield return Timing.WaitForOneFrame;
        DBOTURMA turmaUser = config.OpenDb().GetClass(users.idTurma);
        yield return Timing.WaitForOneFrame;
        DBOESCOLA escolaUser = config.OpenDb().GetSchool(turmaUser.idEscola);

        yield return Timing.WaitForOneFrame;

        //rankingResult = ds.GetMinigameRanking(_idMinigame);
        List<DBORANKFILTER> rankings = ds.GetAllRankingsOfSchool(escolaUser.idEscola, tempIdMInigame);
        yield return Timing.WaitForOneFrame;
        rankUsers.Clear();
        yield return Timing.WaitForSeconds(0.1f);

        for (int i = 0; i < 10; i++) {
            rankInstances[i].UpdateEmptyRank();
            rankInstances[i].DefaulColor();
        }


        int countTemp = rankings.Count;
        for (int i = 0; i < countTemp; i++) {
            Debug.Log("rankingResult.Count " + countTemp.ToString());
            Debug.Log(rankings[i].ToString());          
            
            yield return Timing.WaitForSeconds(0.1f);
            rankUsers.Add(new RankUser() {
                nameUser = ds.GetUser(rankings[i].idUsuario).nomeJogador,
                idMinigame = rankings[i].idMinigame,
                scoreRank = rankings[i].highscore,
                idUsuario = rankings[i].idUsuario,
                posRank = i + 1,
            });

            rankInstances[i].dboUserSchool = escolaUser;
            rankInstances[i].ranking = rankUsers[i];
            rankInstances[i].UpdateUserRank();
            rankInstances[i].DefaulColor();
            if (rankings[i].idUsuario == config.playerID) {
                rankingUser = new DBORANKING() {
                    idUsuario = rankings[i].idUsuario,
                    idMinigame = rankings[i].idMinigame,
                    highscore = rankings[i].highscore
                };
                userIsOnTop = true;
                rankInstances[i].UpdateColors(selfColorName, selfColorPos);
            }
        }

        if (userIsOnTop == false) {
            //DBOUSUARIOS users = config.openDB().GetUser(config.currentUser.idUsuario);
            DBORANKING usersRank = config.OpenDb().GetRanking(tempIdMInigame, config.currentUser.idUsuario);

            if (config.currentSchool == null) {
                DBOTURMA turmaUser2 = config.OpenDb().GetClass(config.currentUser.idTurma);
                DBOESCOLA escolaUser2 = config.OpenDb().GetSchool(turmaUser.idEscola);
                config.currentSchool = escolaUser2;
            }
            yield return Timing.WaitForSeconds(0.1f);
            if (usersRank != null) {
                LastRank.ranking = new RankUser() {
                    nameUser = config.currentUser.nomeJogador,
                    idMinigame = usersRank.idMinigame,
                    scoreRank = usersRank.highscore,
                    idUsuario = usersRank.idUsuario,
                    posRank = usersRank.posicao,
                };
                LastRank.dboUserSchool = config.currentSchool;
                LastRank.UpdateUserRank();
                LastRankComponent.SetActive(true);
                LastRank.UpdateColors(selfColorName, selfColorPos);
            }
        }

        if (userIsOnTop && LastRankComponent.activeInHierarchy)
        {
            LastRankComponent.SetActive(false);
        }

        //DetailCanvas.enabled = false;
        RankingCanvas.enabled = true;
        if (gameDetailManager.rankingInformationGroupCanvas.alpha <= .5f) {
            gameDetailManager.ChangeGroupCanvas(gameDetailManager.rankingInformationGroupCanvas, gameDetailManager.rankingInformationCanvas);
        }
        Invoke("CancasActivate", 0.3f);
        Invoke("CancasActivate", 0.6f);
    }

    IEnumerator<float> LoadRanking(int _idMinigame) {

        int tempIdMInigame = _idMinigame+1;

        buttonGlobal.colors = colorBlockHighlight;
        buttonEscolar.colors = colorNormal;


        DataService ds = config.OpenDb();

        bool userIsOnTop = false;

        rankingResult = ds.GetMinigameRanking(tempIdMInigame);
        rankUsers.Clear();
        yield return Timing.WaitForSeconds(0.1f);

        for (int i = 0; i < 10; i++) {
            rankInstances[i].UpdateEmptyRank();
        }

        int countTemp = rankingResult.Count;
        for (int i = 0; i < countTemp; i++) {
//            Debug.Log("rankingResult.Count " + countTemp.ToString());
//            Debug.Log(rankingResult[i].ToString());
            DBOUSUARIOS users = config.OpenDb().GetUser(rankingResult[i].idUsuario);
            DBOTURMA turmaUser = config.OpenDb().GetClass(users.idTurma);
            DBOESCOLA escolaUser = config.OpenDb().GetSchool(turmaUser.idEscola);
            yield return Timing.WaitForSeconds(0.1f);
            rankUsers.Add(new RankUser() {
                nameUser = users.nomeJogador,                
                idMinigame = rankingResult[i].idMinigame,
                scoreRank = rankingResult[i].highscore,
                idUsuario = rankingResult[i].idUsuario,
                posRank = i+1,
            });
            rankInstances[i].dboUserSchool = escolaUser;
            rankInstances[i].ranking = rankUsers[i];
            rankInstances[i].UpdateUserRank();
            rankInstances[i].DefaulColor();
            if (rankingResult[i].idUsuario != config.playerID) continue;
            rankingUser = rankingResult[i];
            userIsOnTop = true;
            rankInstances[i].UpdateColors(selfColorName, selfColorPos);
        }

        if(userIsOnTop == false ) {
            //DBOUSUARIOS users = config.openDB().GetUser(config.currentUser.idUsuario);
            var usersRank = config.OpenDb().GetRanking(tempIdMInigame, config.currentUser.idUsuario);
           
            if (config.currentSchool == null) {
                DBOTURMA turmaUser = config.OpenDb().GetClass(config.currentUser.idTurma);
                DBOESCOLA escolaUser = config.OpenDb().GetSchool(turmaUser.idEscola);
                config.currentSchool = escolaUser;
            }
            yield return Timing.WaitForSeconds(0.1f);
            if (usersRank != null) {
                LastRank.ranking = new RankUser() {
                    nameUser = config.currentUser.nomeJogador,
                    idMinigame = usersRank.idMinigame,
                    scoreRank = usersRank.highscore,
                    idUsuario = usersRank.idUsuario,
                    posRank = usersRank.posicao,
                    
                };
                LastRank.dboUserSchool = config.currentSchool;
                LastRank.UpdateUserRank();
                LastRankComponent.SetActive(true);
                LastRank.UpdateColors(selfColorName, selfColorPos);
            }
        }

        //DetailCanvas.enabled = false;
        RankingCanvas.enabled = true;
        if (gameDetailManager.rankingInformationGroupCanvas.alpha <= .5f) {
            gameDetailManager.ChangeGroupCanvas(gameDetailManager.rankingInformationGroupCanvas, gameDetailManager.rankingInformationCanvas);
        }
        Invoke(nameof(CancasActivate), 0.3f);
        Invoke(nameof(CancasActivate), 0.6f);
    }

    public void CloseRankingWindow() {
        //RankingCanvas.enabled = false;
        //DetailCanvas.enabled = true;
        gameDetailManager.ChangeGroupCanvas(gameDetailManager.gameInformationGroupCanvas, gameDetailManager.gameInformationCanvas);
    }

    public void CancasActivate() {
        if(RankingCanvas.enabled == false) {
            RankingCanvas.enabled = true;
        }

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalScrollbar.value = 1f;
        scrollRect.verticalNormalizedPosition = 1f;
        Canvas.ForceUpdateCanvases();
    }

}
