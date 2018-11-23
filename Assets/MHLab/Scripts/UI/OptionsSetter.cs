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

        public GameObject ActivatedState;
        public GameObject DeactivatedState;

        public OptionsAllowedValues ValueType;
        public string KeyToQuery = "";
        public string ActivatedValue;
        public string DeactivatedValue;

        protected void Start()
        {
            if (!LocalStorage.HasKey(KeyToQuery))
            {
                LocalStorage.Store(KeyToQuery, ActivatedValue);
            }

            switch (ValueType)
            {
                case OptionsAllowedValues.Int:
                    var currentInt = LocalStorage.GetInt(KeyToQuery).Value;
                    if (currentInt == int.Parse(ActivatedValue))
                    {
                        ActivatedState.SetActive(true);
                        DeactivatedState.SetActive(false);
                    }
                    else
                    {
                        ActivatedState.SetActive(false);
                        DeactivatedState.SetActive(true);
                    }
                    break;
                case OptionsAllowedValues.Float:
                    var currentFloat = LocalStorage.GetFloat(KeyToQuery).Value;
                    if (Math.Abs(currentFloat - float.Parse(ActivatedValue)) < 0.01f)
                    {
                        ActivatedState.SetActive(true);
                        DeactivatedState.SetActive(false);
                    }
                    else
                    {
                        ActivatedState.SetActive(false);
                        DeactivatedState.SetActive(true);
                    }
                    break;
                case OptionsAllowedValues.String:
                    var currentString = LocalStorage.GetString(KeyToQuery).Value;
                    if (currentString == ActivatedValue)
                    {
                        ActivatedState.SetActive(true);
                        DeactivatedState.SetActive(false);
                    }
                    else
                    {
                        ActivatedState.SetActive(false);
                        DeactivatedState.SetActive(true);
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
                        ActivatedState.SetActive(false);
                        DeactivatedState.SetActive(true);
                    }
                    else
                    {
                        LocalStorage.Store(KeyToQuery, int.Parse(ActivatedValue));
                        ActivatedState.SetActive(true);
                        DeactivatedState.SetActive(false);
                    }
                    break;
                case OptionsAllowedValues.Float:
                    var currentFloat = LocalStorage.GetFloat(KeyToQuery).Value;
                    if (Math.Abs(currentFloat - float.Parse(ActivatedValue)) < 0.01f)
                    {
                        LocalStorage.Store(KeyToQuery, float.Parse(DeactivatedValue));
                        ActivatedState.SetActive(false);
                        DeactivatedState.SetActive(true);
                    }
                    else
                    {
                        LocalStorage.Store(KeyToQuery, float.Parse(ActivatedValue));
                        ActivatedState.SetActive(true);
                        DeactivatedState.SetActive(false);
                    }
                    break;
                case OptionsAllowedValues.String:
                    var currentString = LocalStorage.GetString(KeyToQuery).Value;
                    if (currentString == ActivatedValue)
                    {
                        LocalStorage.Store(KeyToQuery, DeactivatedValue);
                        ActivatedState.SetActive(false);
                        DeactivatedState.SetActive(true);
                    }
                    else
                    {
                        LocalStorage.Store(KeyToQuery, ActivatedValue);
                        ActivatedState.SetActive(true);
                        DeactivatedState.SetActive(false);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}