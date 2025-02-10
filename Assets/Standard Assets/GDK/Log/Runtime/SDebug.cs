

using UnityEngine;
using System;
using System.Diagnostics;
using D = UnityEngine.Debug;
using Object = UnityEngine.Object;

/// <summary>
/// 通过宏来开启调试日志,  断言Assert和普通Log在Editor下必定开启
/// LOG_ASSERT： 开启断言
/// LOG_ENABLED: 开启普能日志
/// LOG_NET ：网络日志
/// LOG_SKILL :技能日志
/// LOG_ACTOR : 角色日志
/// LOG_RVO :寻路日志
/// LOG_TEST :测试日志
/// LOG_AI : AI日志
/// LOG_MAP :地图日志
/// LOG_SYNC :同步日志
/// 
/// 如果以上日志类型都不能满足需求， 日志可按等级打印。一共分三级。
/// 
/// Oscar
/// </summary>
public class SDebug
{

    private class TagConst
    {
        public const string NET="Net";
        public const string SKILL = "Skill";
        public const string ACTOR = "Actor";
        public const string RVO = "Rvo";
        public const string TEST = "Test";
        public const string AI = "AI";
        public const string MAP = "Map";
        public const string SYNC = "Sync";
        public const string Sound = "Sound";
    }


    #region 断言

    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        if (!condition)
            D.unityLogger.Log(LogType.Assert, "Assertion failed");
    }


    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, Object context)
    {
        if (!condition)
            D.unityLogger.Log(LogType.Assert, (object)"Assertion failed", context);
    }


    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, object message)
    {
        if (!condition)
            D.unityLogger.Log(LogType.Assert, message);
    }

    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string message)
    {
        if (!condition)
            D.unityLogger.Log(LogType.Assert, message);
    }


    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, object message, Object context)
    {
        if (!condition)
            D.unityLogger.Log(LogType.Assert, message, context);
    }

    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string message, Object context)
    {
        if (!condition)
            D.unityLogger.Log(LogType.Assert, (object)message, context);
    }


    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void AssertFormat(bool condition, string format, params object[] args)
    {
        if (!condition)
            D.unityLogger.LogFormat(LogType.Assert, format, args);
    }


    [Conditional("LOG_ASSERT"), Conditional("UNITY_EDITOR")]
    public static void AssertFormat(bool condition, Object context, string format, params object[] args)
    {
        if (!condition)
            D.unityLogger.LogFormat(LogType.Assert, context, format, args);
    }
    #endregion

    #region  普通打印，通过LOG_ENABLED 开启

    /// <summary>
    /// 普通log,可通过宏LOG_ENABLED开启
    /// </summary>
    /// <param name="content"></param>
    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void Log(object message)
    {
        _Log( message);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void Log(object message, Object context)
    {
        _Log( message, context);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
        _LogFormat( format, args);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void LogFormat(Object context, string format, params object[] args)
    {
        _LogFormat(context, format, args);
    }


    #endregion

    #region  普通标签打印，通过LOG_ENABLED 开启

    /// <summary>
    /// 普通标签log,可通过宏LOG_ENABLED开启
    /// </summary>
    /// <param name="content"></param>
    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void LogTag(string tag, object message)
    {
        _LogTag(tag,message);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void LogTag(string tag, object message, Object context)
    {
        _LogTag(tag,message, context);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void LogTagFormat(string tag, string format, params object[] args)
    {
        _LogTagFormat(tag,format, args);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    public static void LogTagFormat(Object context, string tag, string format, params object[] args)
    {
        _LogTagFormat(context, tag,format, args);
    }


    #endregion

    #region  警告打印，开启

   // [Conditional("LOG_WARNING"), Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message)
    {
        D.unityLogger.Log(LogType.Warning, message);
    }

   // [Conditional("LOG_WARNING"), Conditional("UNITY_EDITOR")]
    public static void LogWarning(object message, Object context)
    {
        D.unityLogger.Log(LogType.Warning, message, context);
    }

   // [Conditional("LOG_WARNING"), Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Warning, format, args);
    }

    //[Conditional("LOG_WARNING"), Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(Object context, string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Warning, context, format, args);
    }

    #endregion


    #region  错误打印
    public static void LogError(object message)
    {
        D.unityLogger.Log(LogType.Error, message);
    }


    public static void LogError(object message, Object context)
    {
        D.unityLogger.Log(LogType.Error, message, context);
    }

   
    public static void LogErrorFormat(string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Error, format, args);
    }


    public static void LogErrorFormat(Object context, string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Error, context, format, args);
    }

    #endregion

    #region 异常打印


    public static void LogException(Exception exception)
    {
        D.unityLogger.LogException(exception, null);
    }
    public static void LogException(Exception exception,string message)
    {
        D.unityLogger.LogException(new Exception(message, exception), null);
    }
    public static void LogExceptionFormat(Exception exception, string format, params object[] args)
    {
        D.unityLogger.LogException(new Exception(string.Format(format, args), exception), null);
    }
    public static void LogException(Exception exception, Object context)
    {
        D.unityLogger.LogException(exception, context);
    }
    public static void LogException(Exception exception,string message, Object context)
    {
        D.unityLogger.LogException(new Exception(message, exception), context);
    }
    public static void LogException(Object context, Exception exception, string format,  params object[] args)
    {
        D.unityLogger.LogException(new Exception(string.Format(format, args), exception), context);
    }
    #endregion


    #region  网络日志，可通过LOG_NET开启
    /// <summary>
    /// 网络日志，可通过LOG_NET开启
    /// </summary>
    /// <param name="content"></param>
    [Conditional("LOG_NET")]
    public static void LogNet(object message)
    {
        _LogTag(TagConst.NET , message);
    }

    [Conditional("LOG_NET")]
    public static void LogNet(object message, Object context)
    {
        _LogTag(TagConst.NET, message, context);
    }

    [Conditional("LOG_NET")]
    public static void LogNet(object tag, object message)
    {
        _LogTagFormat(TagConst.NET, "{0}|{1}", tag, message);
    }

    [Conditional("LOG_NET")]
    public static void LogNetFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.NET, format, args);
    }

    [Conditional("LOG_ENABLED")]
    public static void LogNetFormat(Object context, string format, params object[] args)
    {
        _LogTagFormat(context, TagConst.NET, format, args);
    }

    #endregion

    #region 技能日志，可通过LOG_SKILL开启
    /// <summary>
    /// 技能日志，可通过LOG_SKILL开启
    /// </summary>
    [Conditional("LOG_SKILL")]
    public static void LogSkill(object message)
    {
        _LogTag(TagConst.SKILL, message);
    }

    [Conditional("LOG_SKILL")]
    public static void LogSkill(object message, Object context)
    {
        _LogTag(TagConst.SKILL, message, context);
    }

    [Conditional("LOG_SKILL")]
    public static void LogSkillFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.SKILL, format, args);
    }

    [Conditional("LOG_SKILL")]
    public static void LogSkillFormat(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.SKILL, format, args);
    }
    #endregion


    #region 角色日志，可通过LOG_ACTOR开启

    /// <summary>
    /// 角色日志，可通过LOG_ACTOR开启
    /// </summary>
    [Conditional("LOG_ACTOR")]
    public static void LogActor(object message)
    {
        _LogTag(TagConst.ACTOR, message);
    }

    [Conditional("LOG_ACTOR")]
    public static void LogActor(object message, Object context)
    {
        _LogTag(TagConst.ACTOR, message, context);
    }


    [Conditional("LOG_ACTOR")]
    public static void LogActor(string format, params object[] args)
    {
        _LogTagFormat(TagConst.ACTOR, format,args);
    }

    [Conditional("LOG_ACTOR")]
    public static void LogActor( Object context, string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.ACTOR, format, args);
    }
    #endregion

    #region 寻路日志，可通过LOG_RVO开启

    /// <summary>
    /// 寻路日志，可通过LOG_RVO开启
    /// </summary>
    [Conditional("LOG_RVO")]
    public static void LogRvo(object message)
    {
        _LogTag(TagConst.RVO, message);
    }

    [Conditional("LOG_RVO")]
    public static void LogRvo(object message, Object context)
    {
        _LogTag(TagConst.RVO, message, context);
    }

    [Conditional("LOG_RVO")]
    public static void LogRvoFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.RVO, format, args);
    }

    [Conditional("LOG_RVO")]
    public static void LogRvoFormat(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.RVO, format, args);
    }

    #endregion

    #region 测试日志，可通过LOG_TEST开启
    /// <summary>
    /// 测试日志，可通过LOG_TEST开启
    /// </summary>
    [Conditional("LOG_TEST")]
    public static void LogTest(object message)
    {
        _LogTag(TagConst.TEST, message);
    }

    [Conditional("LOG_TEST")]
    public static void LogTest(object message, Object context)
    {
        _LogTag(TagConst.TEST, message, context);
    }

    [Conditional("LOG_TEST")]
    public static void LogTestFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.TEST, format, args);
    }

    [Conditional("LOG_TEST")]
    public static void LogTestFormat(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.TEST, format, args);
    }

    #endregion


    #region AI日志，可通过LOG_AI开启

    /// <summary>
    /// AI日志，可通过LOG_AI开启
    /// </summary>
    [Conditional("LOG_AI")]
    public static void LogAI(object message)
    {
        _LogTag(TagConst.AI, message);
    }

    [Conditional("LOG_AI")]
    public static void LogAI(object message, Object context)
    {
        _LogTag(TagConst.AI, message, context);
    }


    [Conditional("LOG_AI")]
    public static void LogAIFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.AI, format, args);
    }

    [Conditional("LOG_AI")]
    public static void LogAIFormat(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.AI, format, args);
    }

    #endregion

    #region 地图日志，可通过LOG_MAP开启

    /// <summary>
    /// 地图日志，可通过LOG_MAP开启
    /// </summary>
    [Conditional("LOG_MAP")]
    public static void LogMap(object message)
    {
        _LogTag(TagConst.MAP, message);
    }

    [Conditional("LOG_MAP")]
    public static void LogMap(object message, Object context)
    {
        _LogTag(TagConst.MAP, message, context);
    }

    [Conditional("LOG_MAP")]
    public static void LogMap(string format, params object[] args)
    {
        _LogTagFormat(TagConst.MAP, format, args);
    }

    [Conditional("LOG_MAP")]
    public static void LogMap(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.MAP, format, args);
    }
    #endregion


    #region 同步日志，可通过LOG_SYNC开启
    /// <summary>
    /// 同步日志，可通过LOG_SYNC开启
    /// </summary>
    [Conditional("LOG_SYNC")]
    public static void LogSync(object message)
    {
        _LogTag(TagConst.SYNC, message);
    }

    [Conditional("LOG_SYNC")]
    public static void LogSync(object message, Object context)
    {
        _LogTag(TagConst.SYNC, message, context);
    }

    [Conditional("LOG_SYNC")]
    public static void LogSyncFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.SYNC, format, args);
    }

    [Conditional("LOG_SYNC")]
    public static void LogSyncFormat(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.SYNC, format, args);
    }

    #endregion
    
    
    #region 声音日志，Sound
    /// <summary>
    /// 同步日志，可通过LOG_SYNC开启
    /// </summary>
    [Conditional("LOG_Sound")]
    public static void LogSound(object message)
    {
        _LogTag(TagConst.Sound, message);
    }
    
    [Conditional("LOG_Sound")]
    public static void LogSound(object message, Object context)
    {
        _LogTag(TagConst.Sound, message, context);
    }

    [Conditional("LOG_Sound")]
    public static void LogSoundFormat(string format, params object[] args)
    {
        _LogTagFormat(TagConst.Sound, format, args);
    }

    [Conditional("LOG_Sound")]
    public static void LogSoundFormat(Object context,string format, params object[] args)
    {
        _LogTagFormat(context,TagConst.Sound, format, args);
    }


    #endregion


    #region  关键日志，跟warring ,error一样，一定要会打印的。

    /// <summary>
    /// 关键日志，跟warring ,error一样，一定要会打印的。
    /// </summary>

    public static void LogKey(object message)
    {
        D.unityLogger.Log(message);
        
    }


    public static void LogKey(object message, Object context)
    {
        D.unityLogger.Log(LogType.Log, message, context);
    }


    public static void LogKeyFormat(string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Log, format, args);
    }


    public static void LogKeyFormat(Object context, string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Log, context, format, args);
    }

 
    #endregion


    #region 私有方法

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _Log(object message)
    {
        D.unityLogger.Log(LogType.Log,message);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _Log(object message, Object context)
    {
        D.unityLogger.Log(LogType.Log, message, context);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _LogTag(string tag, object message)
    {
        D.unityLogger.Log(LogType.Log,tag, message);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _LogTag(string tag, object message, Object context)
    {
        D.unityLogger.Log(LogType.Log,tag, message,context);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _LogFormat(string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Log, format, args);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _LogFormat(Object context, string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Log, context, format, args);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _LogTagFormat(string tag, string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Log, tag+":"+format, args);
    }

    [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
    private static void _LogTagFormat(Object context, string tag, string format, params object[] args)
    {
        D.unityLogger.LogFormat(LogType.Log, context, tag + ":" + format, args);
    }
    #endregion
}

