using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace UnityCommonLibrary {
    public static class FullIntentHistory {
        public static string output;
        public static bool record = true;

        internal static readonly Dictionary<object, IntentHistory> fullHistory = new Dictionary<object, IntentHistory>();

        static FullIntentHistory() {
            output = Directory.GetParent(Application.dataPath).FullName + "/history_" + Guid.NewGuid().ToString() + ".csv";
        }

        public static void CreateIntentHistory(object obj) {
            if(!record) {
                return;
            }
            var history = new IntentHistory(obj);
            fullHistory.Add(history.obj, history);
        }

        public static void PushIntent(object obj, string intent, params object[] targets) {
            if(!record) {
                return;
            }
            fullHistory[obj].PushIntent(intent, targets);
        }

        public static void WriteToDisk() {
            var text = "Time,Source,Intent,Result,Targets" + Environment.NewLine + string.Join(Environment.NewLine, fullHistory.Select(h => h.Value.ToCSV()).ToArray()).Trim();
            File.WriteAllText(output, text);
        }

        public static void PostResult(object obj, IntentResult result) {
            if(!record) {
                return;
            }
            fullHistory[obj].PostResult(result);
        }
    }

    internal class IntentHistory {
        internal List<Intent> history = new List<Intent>();
        internal readonly string guid = Guid.NewGuid().ToString();
        internal object obj;

        bool waitingForResult;

        internal IntentHistory(object obj) {
            this.obj = obj;
        }

        internal void PushIntent(string intent, params object[] targets) {
            if(waitingForResult) {
                PostResult(IntentResult.Unknown);
            }
            else {
                waitingForResult = true;
            }

            history.Add(new Intent(obj, intent, targets));
        }

        internal void PostResult(IntentResult result) {
            waitingForResult = false;
            var intent = history[history.Count - 1];
            intent.result = result;
            history[history.Count - 1] = intent;
        }

        internal string ToCSV() {
            return string.Join(Environment.NewLine, history.Select(h => h.ToCSV()).ToArray()).Trim();
        }

        internal struct Intent {
            public object obj;
            public object[] targets;
            public float time;
            public IntentResult result;
            public string intent;

            public Intent(object obj, string intent, params object[] target) {
                this.obj = obj;
                this.intent = intent;
                this.targets = target;
                time = Time.unscaledTime;
                result = IntentResult.InProgress;
            }

            internal string ToCSV() {
                return string.Join(",",
                    new string[] {
                        time.ToString(),
                        obj.ToString(),
                        intent,
                        result.ToString(),
                        string.Join(" | ", targets.Select(t => t.ToString()).ToArray())
                    }
                );
            }
        }
    }

    public enum IntentResult {
        InProgress,
        Success,
        Unknown,
        Failed,
        CheckPass,
        CheckFail
    }

}