using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Net;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DG.DeAudio;
using MEC;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Events;
using TMPro;
using UnityEngine.Playables;
using Segment = MEC.Segment;

public class gameSelection : MonoBehaviour {


	public int bropsAmount = 0;

	public Transform listViewPort;
	public LoadManager loadManager;

	public int selectedMinigame = 0;
	public menuState stateMenu;
	private menuState oldStateMenu;
	public Canvas[] Panels;

	public Toggle[] Toggles;

	public Text bropsText;

	public StatusTexts texts;
	public Text ProgressText;
	public Text profileNameText;
	public Image AvatarImage;
	public GameObject syncButton;
    public TextMeshProUGUI usernameText;
	public UnityEvent onAdministratorLoggin;
    public TextMeshProUGUI[] textsUpdate;

	[Header("CharSelections")]
	public int selectedChars = -1;
	public ScrollSnap charScroll;
	public Text CharSelectedText;
	public GameDetailWindow gameDetailWindow;

    [Header("Store")]
    public StoreManager storeManager;
    public Canvas storePanel;
    public GameObject btsLogo;
    public StoreData storeData;

    public Text texBrops;
    public Text texPoints;
    public Text texNameAluno;
    public Text textSchool;
    public Text textClass;
    public Text textYear;
    public GameObject[] textNomeGame;
	public Text textComponentBuildVersion;
    public Text textComponentCliente;
    public PlayableDirector director;
    public double currentTime;
    [ReadOnly]
    public SoundManager soundManager;

    public void Awake() {

        soundManager = FindObjectOfType(typeof(SoundManager)) as SoundManager;
        Timing.RunCoroutine(LoadStoreData());
    }

    public void TrocaUsu() {
        GameConfig.Instance.UpdateCurrent(GameConfig.Instance.currentUser);
        texBrops.text = $"{GameConfig.Instance.BropsAmount}";
        texPoints.text = $"{GameConfig.Instance.TotalPoints}";
        texNameAluno.text = $"{GameConfig.Instance.namefull}";
        textClass.text = GameConfig.Instance.currentClass.descTurma;
        textSchool.text = GameConfig.Instance.currentSchool.nomeEscola;
        textYear.text = $"{GameConfig.Instance.currentClass.idAnoLetivo} ANO";
    }

    public void ActiveNomeGame(bool check) {

        if (check)
        {
	        foreach (var t in textNomeGame)
	        {
		        t.SetActive(true);
	        }
        } 
        else
        {
	        foreach (var t in textNomeGame)
	        {
		        t.SetActive(false);
	        }
        }
       

    }

    IEnumerator<float> waitToStart(){
		yield return Timing.WaitForSeconds(1f);
		GameConfig.Instance.BropsAmount = GameConfig.Instance.currentScore.brops;
		GameConfig.Instance.TotalPoints = GameConfig.Instance.currentScore.pontuacaoTotal;
        usernameText.text = GameConfig.Instance.currentUser.nomeJogador;
        stateMenu = menuState.main;
		selectedChars = -1;
		UpdateState (stateMenu);
		BropsTextUpdate ();
        foreach (var t in textsUpdate)
        {
	        t.ForceMeshUpdate();
	        t.UpdateFontAsset();
	        t.SetAllDirty();
        }

        //gameConfigs.AddOrReplateMinigame
        List<DBOMINIGAMES> dboMinigames = new List<DBOMINIGAMES>();
        dboMinigames = GameConfig.Instance.OpenDb().GetAllMinigames();

        int tempCount = dboMinigames.Count;
        for (int i = 0; i < tempCount; i++) {
	        GameConfig.Instance.AddOrReplateMinigame(new MinigameStruct
            {
                idMinigame = dboMinigames[i].idMinigames,
                idLivro = dboMinigames[i].idLivro,
                infoMinigame = dboMinigames[i].infoMinigame,
                nomeMinigame = dboMinigames[i].nomeMinigame,
                ativo = dboMinigames[i].ativo
            });
        }

        yield return Timing.WaitForSeconds(0.1f);
        LoadMyOwnRank();

    }

    // Use this for initialization
    public void StartLate () {
        TrocaUsu();
        GameConfig.Instance.currentScore = GameConfig.Instance.OpenDb().GetScore(GameConfig.Instance.currentUser.idUsuario);
        

		Timing.RunCoroutine(waitToStart(), Segment.SlowUpdate);
        Resources.UnloadUnusedAssets();
		textComponentBuildVersion.text = "Versão do Jogo: " + Application.version;

        textComponentCliente.text = "Cliente: " + GameConfig.Instance.ClientName;

    }

	public void  SairGame () {
        PlayerPrefs.SetString("PlayerLastLogin", "");
        PlayerPrefs.DeleteKey("PlayerProfile");
		PlayerPrefs.DeleteAll();
		Application.Quit();
        Debug.Log("exit!");
	}

    public void changeUser() {
        PlayerPrefs.SetString("PlayerLastLogin", "");
        PlayerPrefs.DeleteKey("PlayerProfile");
        PlayerPrefs.DeleteAll();
    }

	

	public void changeScene(string nameScene){
		SceneManager.LoadScene (nameScene);
	}

	public void changeScene(int scene){
		SceneManager.LoadScene (scene);
	}

	public void buttonShowMinigame(int minigameID){
		gameDetailWindow.LoadMinigame (minigameID);
		selectedMinigame = minigameID;
		UpdateState (menuState.selected);
	}

	public void UpdateSelectChar(){
		selectedChars = charScroll.CurrentPage ();
		CharSelectedText.text = GetNameCharacter (selectedChars);
		PlayerPrefs.SetInt("characterSelected", selectedChars);
		//Save character selec;
	}


	public void backFromConfig(){
		GameConfig.Instance.SavePrefs ();
		UpdateState (menuState.main);
	}

	public void UpdateState(menuState state){

		switch (state) {
		case menuState.selected:
			PanelsActivation((int)state);
			selectedChars = -1;
			break;
		case menuState.loginScreen:
			PanelsActivationOver((int)state);
			break;
		default:
			PanelsActivation((int)state);
			break;
		}

	}

	/// <summary>
	/// 0 - Tela Principal.
	/// 1 - Tela de Configuração.
	/// 2 - Tela do MiniJogo Selecionado.
	/// 3 - Tela de Login/Acesso.
	/// </summary>
	/// <param name="i">The other Collider involved in this collision.</param>
	public void	PanelsActivation(int panelIndex)
	{
		foreach (var t in Panels)
		{
			t.enabled = t == Panels[panelIndex];
		}
	}

	public void	PanelsActivationOver(int panelsIndex)
	{
		foreach (var t in Panels)
		{
			if(t == Panels[panelsIndex]){
				t.enabled  = true;
			}
		}
	}


	public enum menuState
	{
		main,
		config,
		selected,
		loginScreen,
	}

	public void backgroundMusicValidate()
	{
		GameConfig.Instance.isAudioOn = !Toggles [0].isOn;

	}


	public void soundFXValidate()
	{
		GameConfig.Instance.isAudioFXOn = !Toggles [1].isOn;
	}

	public void VoicesValidate()
	{
		GameConfig.Instance.isAudioVoiceOn = !Toggles [2].isOn;
	}

	public void OpenConfig(){

		Toggles [0].isOn = !GameConfig.Instance.isAudioOn;

		Toggles [1].isOn = !GameConfig.Instance.isAudioFXOn;

		Toggles [2].isOn = !GameConfig.Instance.isAudioVoiceOn;

		UpdateState (menuState.config);

	}

	public void BropsTextUpdate(){

		bropsText.text = "B$: " + bropsAmount;

	}

	public void backFromSelect(){
		stateMenu = menuState.main;
		UpdateState (menuState.main);
	}

	public string GetNameCharacter(int character){
		string name = "";

		switch (character) {
		case 0:
			name = "Zeca";
			return name;
			break;
		case 1:
			name = "Tati";
			return name;
			break;
		case 2:
			name = "Paulo";
			return name;
			break;
		case 3:
			name = "Manu";
			return name;
			break;
		case 4:
			name = "João";
			return name;
			break;
		case 5:
			name = "Bia";
			return name;
			break;
		default:
			name = "Error";
			return name;
			break;
		}

	}

	public enum CharName
	{
		zeca,
		manu,
		paulo,
		joao,
		bia,
		tati
	}

	public void networkVeryfier(){
		GameConfig.Instance.isVerifingNetwork = true;
		GameConfig.Instance.isOnline = true;
		string HtmlText = GetHtmlFromUri("http://google.com");
		if(HtmlText == "")     {
			//No connection
			GameConfig.Instance.isOnline = false;
		}
		else if(!HtmlText.Contains("schema.org/WebPage")){
			//Redirecting since the beginning of googles html contains that 
			//phrase and it was not found
			GameConfig.Instance.isOnline = false;
		}
		else{
			GameConfig.Instance.isOnline = true;
			//success
		}
		GameConfig.Instance.isVerifingNetwork = false;
	}

	public string GetHtmlFromUri(string resource){
     string html = string.Empty;
     HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
     try
     {
         using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
         {
             bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
             if (isSuccess)
             {
                 using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                 {
                     //We are limiting the array to 80 so we don't have
                     //to parse the entire html document feel free to 
                     //adjust (probably stay under 300)
                     char[] cs = new char[80];
                     reader.Read(cs, 0, cs.Length);
                     foreach(char ch in cs)
                     {
                         html +=ch;
                     }
                 }
             }
         }
     }
     catch
     {
         return "";
     }
     return html;
 }

 	 /// <summary>
	 /// Callback sent to all game objects before the application is quit.
	 /// </summary>
	 void OnApplicationQuit()
	 {
		 /*gameConfigs.jogador = new DBJogador();
		 gameConfigs.jogador.IdJogador = -1;*/
	 }

	 void OnEnable(){
	#if UNITY_EDITOR
		EditorApplication.playmodeStateChanged += StateChange;
	#endif
	}
	#if UNITY_EDITOR
	void StateChange(){
		if (EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.isPlaying){
			/*gameConfigs.jogador = new DBJogador();
		 	gameConfigs.jogador.IdJogador = -1;*/
		}
	}
	#endif

    public void StoreItens(){
        
        storePanel.enabled = true;
        storeManager.enabled = true;
        btsLogo.SetActive(false);
        storeManager.OpenStore();
    }

    public void LoadMyOwnRank() {
        List<DBORANKING> myRanks = GameConfig.Instance.OpenDb().GetAllUserRanks(GameConfig.Instance.currentUser.idUsuario);

        for (int i = 0; i < myRanks.Count; i++) {
	        GameConfig.Instance.allMinigames[i].highscore = myRanks[i].highscore;
        }
    }


    public IEnumerator<float> LoadInventoryData() {
        storeData.buyedItens.Clear();
        DBOINVENTARIO[] _inventory = GameConfig.Instance.OpenDb().GetInventory(GameConfig.Instance.GetCurrentUserID());
        yield return Timing.WaitForSeconds(0.2f);
        int tempCount = _inventory.Length;
        for (int i = 0; i < tempCount; i++) {
            storeData.buyedItens.Add(_inventory[i].idItem);
        }
    }

    public IEnumerator<float> LoadStoreData() {

        yield return Timing.WaitUntilDone(Timing.RunCoroutine(LoadInventoryData()));
        Debug.Log("Loading Store Data.");
        storeData.SetDataService(GameConfig.Instance.OpenDb());
        List<DBOITENS> _itensOnStore = GameConfig.Instance.OpenDb().GetItensStoreList();
        //yield return Timing.WaitForSeconds(0.2f);
        int tempCount = _itensOnStore.Count;
        Debug.Log("Itens To Load On Store:" + tempCount);

        storeData.itensOnStore.Clear();
        //List<int> DeleteFromList = new List<int>();
        /* for (int i = 0; i < tempCount; i++) {
             if (File.Exists(config.fullPatchItemIcon + _itensOnStore[i].idItem + ".png") == false) {
                 DeleteFromList.Add(i);
             }
         }*/
        StringFast streamingAssetPatch = new StringFast();
        //Loop nos itens da loja.
        for (int i = 0; i < tempCount; i++) {
            if (_itensOnStore[i].ativo == 1) {
                streamingAssetPatch.Clear();
                //Converter Informações do SQLITE em Classe StoreItem.
                StoreItem item = storeData.dboItensToStoreItens(_itensOnStore[i]);

                //Verificação da imagem do item dentro do streaming assets.
                var persistentPathItemIcon = string.Format("{0}{1}.png", GameConfig.Instance.fullPatchItemIcon, _itensOnStore[i].idItem);

                //Verificar se arquivo de imagem existe no PersistentPath.
                //Adicionar FLAG de verificação de download. [ ] [WARNING]
                if (File.Exists(persistentPathItemIcon)) {
                    item.itemIcon = StoreData.LoadPNG(persistentPathItemIcon);
                } else {

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_STANDALONE 
                    // PC/Unity Editor Streaming Asset.
                    streamingAssetPatch.Clear();
                    //Gerarando caminho do streaming assets folder para PC.
                    streamingAssetPatch.Append(Application.streamingAssetsPath).Append("/medias/item/").Append(_itensOnStore[i].idItem).Append(".png");
                    Debug.LogWarning(streamingAssetPatch.ToString(), this);
                    //Carregando bytes do arquivo de imagem em streaming assets.
                    var itemIconBytesLoaded = File.ReadAllBytes(streamingAssetPatch.ToString());
                    //Salvando bytes de streaming asset em PersistentDataPath.
                    File.WriteAllBytes(persistentPathItemIcon, itemIconBytesLoaded);
#elif UNITY_ANDROID
                    //Android Streaming Asset.
                    streamingAssetPatch.Append(Application.streamingAssetsPath).Append("/medias/item/").Append(_itensOnStore[i].idItem).Append(".png");
                    Debug.LogWarning(streamingAssetPatch.ToString(), this);

                    //Carregar do StreamingAssets 'apk' arquivo de imagem.
                    var loadDb = new WWW(streamingAssetPatch.ToString());
                    while (!loadDb.isDone) { }
                    //Copiar dados carregados do streaming assets 
                    File.WriteAllBytes(persistentPathItemIcon, loadDb.bytes);
                    // Android Android Ended.
#elif UNITY_IOS
                    streamingAssetPatch.Clear();
                    streamingAssetPatch.Append(Application.streamingAssetsPath).Append("/medias/item/").Append(_itensOnStore[i].idItem).Append(".png");
                    Debug.LogWarning(streamingAssetPatch.ToString(), this);
                    var ItemIconPath = streamingAssetPatch.ToString();
                    File.Copy(ItemIconPath, persistentPathItemIcon);
#endif
                    //Carrega o item agora no PersistentData.
                    item.itemIcon = StoreData.LoadPNG(persistentPathItemIcon);

                }

                streamingAssetPatch.Clear();
                streamingAssetPatch.Append("Item Name: ").Append(item.itemID).Append(" | Item File: ").Append(GameConfig.Instance.fullPatchItemIcon).Append(_itensOnStore[i].idItem).Append(".png");
                Debug.Log(streamingAssetPatch.ToString(), this);
                storeData.itensOnStore.Add(item);
            }
        }


        /* for (int i = 0; i < tempCount; i++) {
             if (_itensOnStore[i].ativo == 1) {
                 StoreItem item = storeData.dboItensToStoreItens(_itensOnStore[i]);
                 item.itemIcon = StoreData.LoadPNG(config.fullPatchItemIcon + _itensOnStore[i].idItem + ".png");
                 Debug.Log("Item Name: " + item.itemID + " | Item File: " + config.fullPatchItemIcon + _itensOnStore[i].idItem + ".png");
                 storeData.itensOnStore.Add(item);
             }
         }*/

        LoadStoreCategoryItem();
    }

    public void LoadStoreCategoryItem() {
        storeData.itensCategory = GameConfig.Instance.OpenDb().GetCategoryItem();
    }

}
