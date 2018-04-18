using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class PostBuildWebGLFixer : MonoBehaviour
{
    [PostProcessBuild]
    public static void OnPostProcessWebGLBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.WebGL)
        {
            DisableWebGLWarningOnMobile(path);
        }
    }

    private static void DisableWebGLWarningOnMobile(string path)
    {
        var loaderPath = Path.Combine(Path.Combine(path, "Build"), "UnityLoader.js");
        var content = File.ReadAllText(loaderPath);

        var actualContentRegex = @"compatibilityCheck:function\(e,t,r\)\{.+,Blobs:\{\},loadCode";
        var contentToReplaceRegex = "compatibilityCheck:function(e,t,r){t()},Blobs:{},loadCode";

        content = Regex.Replace(content, actualContentRegex, contentToReplaceRegex);

        File.WriteAllText(loaderPath, content);
    }
}
