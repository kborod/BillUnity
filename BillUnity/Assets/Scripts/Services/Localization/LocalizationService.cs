using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

namespace Kborod.Services.Localization
{
    public class LocalizationService : MonoBehaviour
    {
        public event Action LanguageChanged;

        [Header("Language"), SerializeField] private Language language = Language.En;
        [Header("Localization assets path"), SerializeField] private string assetPath;

        private Dictionary<Language, Dictionary<string, string>> localizations = new Dictionary<Language, Dictionary<string, string>>();

        public Language Language => language;

        private Dictionary<Type, string> enumTypeToName = new Dictionary<Type, string>();

        private void Awake()
        {
            Read(assetPath);
        }

        public void SetLanguage(Language newLanguage)
        {
            if (language == newLanguage)
            {
                return;
            }

            language = newLanguage;
            LanguageChanged?.Invoke();
        }

        public string GetTranslationById(string id)
        {
            var localization = localizations[language];

            if (localization.ContainsKey(id))
            {
                return localization[id];
            }

            Debug.LogError($"[Localization] Translate not found for id: {id.ToString()}", this);
            return "---NotFound---";
        }

        public string GetTranslationByEnum<T>(T value, TranslationForm translationForm = TranslationForm.Standart) where T : Enum
        {
            var id = GetIdOfEnum(value, translationForm);
            return GetTranslationById(id);
        }

        public string GetIdOfEnum<T>(T value, TranslationForm translationForm = TranslationForm.Standart) where T : Enum
        {
            if (enumTypeToName.TryGetValue(typeof(T), out var typeName) == false)
            {
                var typeFullName = typeof(T).ToString();
                int pos = typeFullName.LastIndexOf(".") + 1;
                typeName = typeFullName.Substring(pos, typeFullName.Length - pos);
                enumTypeToName.Add(typeof(T), typeName);
            }

            var result = $"{typeName}.{value}";

            if (translationForm != TranslationForm.Standart)
            {
                result += $".{translationForm}";
            }

            return result;
        }

        public string GetTranslationOfTimePeriod(long seconds, TimeFieldFormat format)
        {
            return GetTranslationOfTimePeriod(TimeSpan.FromSeconds(seconds), format);
        }

        public string GetTranslationOfTimePeriod(TimeSpan t, TimeFieldFormat format)
        {
            format = GetFactFormat(format, t);

            if (format == TimeFieldFormat.XX)
            {
                if (t.Days > 0)
                    return $"{DaysString(t.Days)}";
                else if (t.Hours > 0)
                    return $"{HoursString(t.Hours)}";
                else if (t.Minutes > 0)
                    return $"{MinutesString(t.Minutes)}";
                else
                    return $"{SecondsString(t.Seconds)}";
            }
            else if (format == TimeFieldFormat.XX_XX)
            {
                if (t.Days > 0)
                    return $"{DaysString(t.Days)} {HoursString(t.Hours)}";
                else if (t.Hours > 0)
                    return $"{HoursString(t.Hours)} {MinutesString(t.Minutes)}";
                else
                    return $"{MinutesString(t.Minutes)} {SecondsString(t.Seconds)}";
            }
            else
            {
                if (t.Days > 0)
                    return $"{DaysString(t.Days)} : {HoursString(t.Hours)} : {MinutesString(t.Minutes)}";
                else
                    return $"{HoursString(t.Hours)} : {MinutesString(t.Minutes)} : {SecondsString(t.Seconds)}";
            }

            string DaysString(int days) => $"{days} {GetTranslationById("Time.days")}";

            string HoursString(int hours) => $"{hours} {GetTranslationById("Time.hours")}";

            string MinutesString(int minutes) => $"{minutes} {GetTranslationById("Time.minutes")}";

            string SecondsString(int seconds) => $"{seconds} {GetTranslationById("Time.seconds")}";

            TimeFieldFormat GetFactFormat(TimeFieldFormat format, TimeSpan t)
            {
                if (format != TimeFieldFormat.LEAST)
                    return format;
                if (t.Hours <= 0)
                    return TimeFieldFormat.XX;
                else if (t.Days <= 0)
                    return TimeFieldFormat.XX_XX;
                return TimeFieldFormat.XX_XX_XX;
            }
        }

        private void Read(string path)
        {
            if (localizations.Count > 0) return;

            var textAssets = Resources.LoadAll<TextAsset>(path);

            foreach (var textAsset in textAssets)
            {
                var text = textAsset.text.Replace("\\n", "\n").Replace("\"\"", "[quotes]");
                var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");

                foreach (Match match in matches)
                {
                    text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[comma]").Replace("\n", "[newline]"));
                }

                var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                var languagesLine = lines[0].Split(',');
                List<Language> languages = new List<Language>();
                var firstTranslationColumnNum = 2;
                for (int i = firstTranslationColumnNum; i < languagesLine.Length; i++)
                {
                    languages.Add((Language)Enum.Parse(typeof(Language), languagesLine[i].Trim()));
                }

                languages.ForEach(language =>
                {
                    if (!localizations.ContainsKey(language))
                    {
                        localizations.Add(language, new Dictionary<string, string>());
                    }
                });

                var keyColumnNum = 1;
                for (var i = keyColumnNum; i < lines.Length; i++)
                {
                    var columns = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[comma]", ",").Replace("[newline]", "\n").Replace("[quotes]", "\"")).ToList();

                    var keyStr = columns[1].Trim();
                    if (string.IsNullOrEmpty(keyStr)) continue;

                    for (var j = 0; j < languages.Count; j++)
                    {
                        localizations[languages[j]].Add(keyStr, columns[j + 2]);
                    }
                }
            }
        }
    }
}
