using MHLab.Web.Storage;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.UI
{
    public class OptionsSetter : MonoBehaviour
    {
        public enum OptionsAllowedValues
        {
            Int,
            Float,
            String
        }

        public string ActivatedText = "ACTIVATED";
        public string DeactivatedText = "DEACTIVATED";
        public Text Text;

        public OptionsAllowedValues ValueType;
        public string KeyToQuery = "";
        public string ActivatedValue;
        public string DeactivatedValue;

        protected void Awake()
        {
            if (!LocalStorage.HasKey(KeyToQuery))
            {
                LocalStorage.Store(KeyToQuery, ActivatedValue);
                return;
            }

            switch (ValueType)
            {
                case OptionsAllowedValues.Int:
                    var currentInt = LocalStorage.GetInt(KeyToQuery).Value;
                    if (currentInt == int.Parse(ActivatedValue))
                    {
                        Text.text = ActivatedText;
                    }
                    else
                    {
                        Text.text = DeactivatedText;
                    }
                    break;
                case OptionsAllowedValues.Float:
                    var currentFloat = LocalStorage.GetFloat(KeyToQuery).Value;
                    if (Math.Abs(currentFloat - float.Parse(ActivatedValue)) < 0.01f)
                    {
                        Text.text = ActivatedText;
                    }
                    else
                    {
                        Text.text = DeactivatedText;
                    }
                    break;
                case OptionsAllowedValues.String:
                    var currentString = LocalStorage.GetString(KeyToQuery).Value;
                    if (currentString == ActivatedValue)
                    {
                        Text.text = ActivatedText;
                    }
                    else
                    {
                        Text.text = DeactivatedText;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnButtonPressed()
        {
            switch (ValueType)
            {
                case OptionsAllowedValues.Int:
                    var currentInt = LocalStorage.GetInt(KeyToQuery).Value;
                    if (currentInt == int.Parse(ActivatedValue))
                    {
                        LocalStorage.Store(KeyToQuery, int.Parse(DeactivatedValue));
                        Text.text = DeactivatedText;
                    }
                    else
                    {
                        LocalStorage.Store(KeyToQuery, int.Parse(ActivatedValue));
                        Text.text = ActivatedText;
                    }
                    break;
                case OptionsAllowedValues.Float:
                    var currentFloat = LocalStorage.GetFloat(KeyToQuery).Value;
                    if (Math.Abs(currentFloat - float.Parse(ActivatedValue)) < 0.01f)
                    {
                        LocalStorage.Store(KeyToQuery, float.Parse(DeactivatedValue));
                        Text.text = DeactivatedText;
                    }
                    else
                    {
                        LocalStorage.Store(KeyToQuery, float.Parse(ActivatedValue));
                        Text.text = ActivatedText;
                    }
                    break;
                case OptionsAllowedValues.String:
                    var currentString = LocalStorage.GetString(KeyToQuery).Value;
                    if (currentString == ActivatedValue)
                    {
                        LocalStorage.Store(KeyToQuery, DeactivatedValue);
                        Text.text = DeactivatedText;
                    }
                    else
                    {
                        LocalStorage.Store(KeyToQuery, ActivatedValue);
                        Text.text = ActivatedText;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}