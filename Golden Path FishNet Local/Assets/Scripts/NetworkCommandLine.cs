using FishNet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Class to start a server, host or client from the commandline. Still used to launch the program with "-mlapi server" as arguments.
    /// </summary>
    public class NetworkCommandLine : MonoBehaviour
    {
        /// <summary>
        /// Awake method. Starts the server, client or host.
        /// </summary>
        void Awake()
        {
            if (Application.isEditor) return;
            var args = GetCommandlineArgs();
            /*foreach (var kvp in args)
            {
                Debug.Log(kvp.Key + " : " + kvp.Value);
            }*/

            if (args.TryGetValue("-server", out _))
            {
                Debug.Log("Starting server");
                InstanceFinder.ServerManager.StartConnection();

            }
        }

        /// <summary>
        /// Gets the commandline arguments.
        /// </summary>
        /// <returns>The commandline arguments, in a Dictionary<argument, value>, both strings</argument></returns>
        private Dictionary<string, string> GetCommandlineArgs()
        {
            Dictionary<string, string> argDictionary = new Dictionary<string, string>();
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i].ToLower();
                if (arg.StartsWith("-"))
                {
                    string value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                    value = (value?.StartsWith("-") ?? false) ? null : value;

                    argDictionary.Add(arg, value);
                }
            }
            return argDictionary;
        }
    }
}

