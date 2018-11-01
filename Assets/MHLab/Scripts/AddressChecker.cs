using MHLab.Ethereum;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.UI
{
	public class AddressChecker : MonoBehaviour
    {
        public Text LogText;
        public UITextFader LogFader;
        public UIScaler Logo;
        public UITextFader Text1;
        public UITextFader Text2;
        public UIImageFader Background;
        public Text Debug;
        public Image StaticLogo;

        protected void Awake()
        {
            if (!string.IsNullOrEmpty(AccountManager.Address))
            {
                LogText.gameObject.SetActive(false);
                Logo.gameObject.SetActive(false);
                Text1.gameObject.SetActive(false);
                Text2.gameObject.SetActive(false);
                Background.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                StaticLogo.enabled = true;
            }
        }

        public void PerformLogin()
        {
            LogText.text = "Fetching account information...";
            try
            {
                AccountManager.GetAccount(OnLoginCompleted);
            }
            catch (Exception e)
            {
                LogText.text = "Something gone terribly wrong!";
            }
        }

        private void OnLoginCompleted(string account)
        {
            LogText.text = "Success!";

            Logo.enabled = true;
            Text1.enabled = true;
            Text2.enabled = true;
            Background.enabled = true;
            this.gameObject.SetActive(false);
            LogFader.enabled = true;

            Debug.text = AccountManager.Address;
        }
	}
}