using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEditor;
using UnityEngine;
using AppleAuth.Editor;

public sealed class IOSAuthPostprocessor
{
    public static bool enableBitcode = false;

    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        if (target != BuildTarget.iOS) return;

        SetAppleAuth(path);
    }

    static void SetAppleAuth(string path)
    {

        var projectPath = PBXProject.GetPBXProjectPath(path);

        var project = new PBXProject();
        project.ReadFromString(System.IO.File.ReadAllText(projectPath));
        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, project.GetUnityMainTargetGuid());
        manager.AddSignInWithApple();
        manager.WriteToFile();
    }

}