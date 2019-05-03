using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatabaseExtensions
{
    public static WWWForm ToForm(this DBOESTATISTICA_DIDATICA log, (string token, int id) UserInfo)
    {
        var form = new WWWForm();
        form.AddField("token", UserInfo.token);
        form.AddField("idUsuario", log.idUsuario);
        form.AddField("dataInsert", log.dataInsert ?? EduqbrinqLogger.GetCurrentDateTime());
        form.AddField("idHabilidade", log.idHabilidade);
        form.AddField("idGameDidatico", log.idGameDidatico);
        form.AddField("idDificuldade", log.idDificuldade);
        form.AddField("idMinigame", log.idMinigame);
        form.AddField("acertou", log.acertou);
        form.AddField("online", log.online);
        form.AddField("idUsuarioOnline", UserInfo.id);
        return form;
    }
    
    public static WWWForm ToForm(this DBOMINIGAMES_LOGS log, (string token, int id) UserInfo)
    {
        var form = new WWWForm();
        form.AddField("token", UserInfo.token);
        form.AddField("idUsuario", log.idUsuario);
        form.AddField("idMinigame", log.idMinigame);
        form.AddField("pontosLudica", log.pontosLudica);
        form.AddField("pontosPedagogica", log.pontosPedagogica);
        form.AddField("pontosInteragindo", log.pontosInteragindo);
        form.AddField("personagem", log.personagem);
        form.AddField("dataAcesso", log.dataAcesso);
        form.AddField("tempoLudica", log.tempoLudica);
        form.AddField("tempoDidatica", log.tempoDidatica);
        form.AddField("faseLudica", log.faseLudica);
        form.AddField("deviceID", log.deviceID);
        form.AddField("online", log.online);
        form.AddField("idUsuarioOnline", UserInfo.id);
        return form;
    }
}
