using System;
using System.Collections.Generic;
using UnityEditor;
using GooglePlayServices;

public class JenkinsBuild
{
    public static void Android()
    {
        bool resolveResult = PlayServicesResolver.ResolveSync(true);
        if(resolveResult) AndroidWithoutResoulution();
    }

    public static void AndroidWithoutResoulution()
    {
        Build($"Builds/Android/", ".aab", BuildTarget.Android, BuildOptions.None);
    }

    public static void IOS()
    {
        Build($"Builds/iOS/", "/", BuildTarget.iOS, BuildOptions.None);
    }

    static void Build(string pathL, string pathR, BuildTarget target, BuildOptions options)
    {
        // Android build outputs settings - to export .aab and symbol files
        EditorUserBuildSettings.buildAppBundle = true;
        EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;

        // argument variables
        Dictionary<string, string> args;

        // Cache command line arguments to Dictionary
        string[] commandArguments = System.Environment.GetCommandLineArgs();
        args = GetArgsDict(commandArguments);

        // Set Android keystore passwords
        if (args.ContainsKey("-keyaliasPass")) PlayerSettings.Android.keyaliasPass = args["-keyaliasPass"];
        if (args.ContainsKey("-keystorePass")) PlayerSettings.Android.keystorePass = args["-keystorePass"];

        // BundleCode and Version
        string bundleCodeString = args["-bundleCode"];
        int bundleCodeInt = Int32.Parse(bundleCodeString);
        string version = GetVersionFromBundleCode(bundleCodeString);

        PlayerSettings.bundleVersion = version; // No matter which platform is being targeted.

        PlayerSettings.Android.bundleVersionCode = bundleCodeInt;
        PlayerSettings.iOS.buildNumber = bundleCodeString;

        // Common Build Options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        
        buildPlayerOptions.scenes = GetScenes();
        buildPlayerOptions.locationPathName = $"{pathL}{bundleCodeString}/{bundleCodeString}{pathR}";
        buildPlayerOptions.target = target;
        buildPlayerOptions.options = options;

        // Build
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static Dictionary<string, string> GetArgsDict(string[] args)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();

        int len = args.Length;

        for (int i = 0; i < len; ++i)
        {
            if (args[i][0] == '-') ret.Add(args[i], "");
            else if (i > 0) ret[args[i - 1]] = args[i];
        }

        return ret;
    }

    static string[] GetScenes()
    {
        List<string> scenes = new List<string>();
        foreach(var s in EditorBuildSettings.scenes)
        {
            if(s.enabled) scenes.Add(s.path);
        }
        return scenes.ToArray();
    }

    static string GetVersionFromBundleCode(string bc)
    {
        // bundleCode: NXXX -> version: N.XXX
        int l = bc.Length;
        string ret = "";
        for(int i=0 ; i<l-3 ; ++i) ret += bc[i];
        ret += ".";
        for(int i=l-3 ; i<l ; ++i) ret += bc[i];

        return ret;
    }
}