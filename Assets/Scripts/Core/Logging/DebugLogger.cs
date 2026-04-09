using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Core.Logging
{
    public class DebugLogger
    {
        private const string DEBUG_LOGGER_NAME = "DebugLogger";
        
        private const string DEFAULT_COLOR = "#ffffff";
        private const string INSTALLER_COLOR = "#FFBE98";
        private const string STATE_MACHINE_COLOR = "#F05A7E";
        private const string MEDIATOR_COLOR = "#BED1CF";
        private const string FACTORY_COLOR = "#125B9A";
        private const string SERVICE_COLOR = "#0B8494";
        
        public static void LogMessage(string message, object sender = null)
        {
#if UNITY_EDITOR
            Debug.Log(GetString(message, sender));
#endif
        }

        public static void LogWarning(string message, object sender = null)
        {
#if UNITY_EDITOR
            Debug.LogWarning(GetString(message, sender));
#endif
        }

        public static void LogError(string message, object sender = null)
        {
#if UNITY_EDITOR
            Debug.LogError(GetString(message, sender));
#endif
        }

        private static string GetString(string message, object sender)
        {
            bool isSenderNull = sender == null;
            
            string color = isSenderNull ? DEFAULT_COLOR : GetHexColor(sender.GetType());
            string name = isSenderNull ? DEBUG_LOGGER_NAME : sender.GetType().Name;
            
            return $"<b><i><color={color}>{name}: </color></i></b> {message}";
        }

        private static string GetHexColor(Type sender) =>
            sender.Namespace switch
            {
                var x when Regex.IsMatch(x, @".*Installers.*") => INSTALLER_COLOR,
                var x when Regex.IsMatch(x, @".*GameLoop.*") => STATE_MACHINE_COLOR,
                var x when Regex.IsMatch(x, @".*Mediator.*") => MEDIATOR_COLOR,
                var x when Regex.IsMatch(x, @".*Factories.*") => FACTORY_COLOR,
                var x when Regex.IsMatch(x, @".*Services.*") => SERVICE_COLOR,
                _ => DEFAULT_COLOR
            };
    }
}