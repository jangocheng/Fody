using System;
using System.Text;
using Microsoft.Build.Framework;

public class BuildLogger : MarshalByRefObject, ILogger
{
    public IBuildEngine BuildEngine { get; set; }
    public MessageImportance MessageImportance { get; set; }

    StringBuilder stringBuilder;
 
    public virtual void Initialise(string messageImportance)
    {
        stringBuilder = new StringBuilder();
        MessageImportance messageImportanceEnum;
        if (!Enum.TryParse(messageImportance, out messageImportanceEnum))
        {
            throw new WeavingException(string.Format("Invalid MessageImportance in config. Should be 'Low', 'Normal' or 'High' but was '{0}'.", messageImportance));
        }
        MessageImportance = messageImportanceEnum;
    }

    public virtual void LogWarning(string message)
    {
        BuildEngine.LogWarningEvent(new BuildWarningEventArgs("", "", "", 0, 0, 0, 0, string.Format("{0}: {1}", "Fody", message), "", "Fody"));
    }

    public virtual void LogInfo(string message)
    {
        stringBuilder.AppendLine("  " + message);
    }

    public virtual void LogError(string currentWeaver, string message)
    {
        stringBuilder.AppendLine("  Error: " + message);
        string format;

        if (currentWeaver == null)
        {
            format = string.Format("Fody: {0}:", message);
        }
        else
        {
            format = string.Format("Fody/{0}: {1}", currentWeaver.Replace(".Fody",""), message);
        }
        BuildEngine.LogErrorEvent(new BuildErrorEventArgs("", "", "", 0, 0, 0, 0, format, "", "Fody"));
    }

    public virtual void Flush()
    {
        var message = stringBuilder.ToString();
        //message = message.Substring(0, message.Length - 2);
        BuildEngine.LogMessageEvent(new BuildMessageEventArgs(message, "", "Fody", MessageImportance));
    }


}