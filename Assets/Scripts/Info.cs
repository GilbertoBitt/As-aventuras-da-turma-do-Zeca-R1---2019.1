using System;
using UnityEngine;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;

using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

public class Info
{
    private static Info _instance;
    public static Info Instance => _instance ?? (_instance = new Info());

    public DateTime BuildTime { get; private set; }
    public String BuildDate { get; private set; }

    protected Info()
    {
        var txt = (UnityEngine.Resources.Load("BuildDate") as TextAsset);
        if (txt != null) BuildDate = txt.text.Trim();
    }

}

#if UNITY_EDITOR
public class AndroidBuildPrepartion : UnityEditor.Build.IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
    {
        using (var writer = new BinaryWriter(File.Open("Assets/Resources/BuildDate.txt", FileMode.OpenOrCreate)))
        {
            writer.Write(DateTime.Now.ToString("dd/MM/yyyy"));
        }
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }
}
#endif