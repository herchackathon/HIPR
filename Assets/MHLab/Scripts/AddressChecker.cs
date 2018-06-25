using MHLab.SlidingTilePuzzle.Data;
using MHLab.Web.Storage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MHLab.Nethereum
{
    public class AddressChecker : MonoBehaviour
    {
        // The scene index to load after a successful login.
        public int SceneToLoad = 1;

        // The input field that contains the address.
        public InputField AddressText;
        // The input field that contains the private key.
        public InputField PrivateKeyText;

        public Text VerifyingText;
        public Text ErrorText;

        protected void Awake()
        {
            SceneManager.LoadScene(SceneToLoad);

            if (LocalStorage.HasKey(StorageKeys.AccountAddressKey))
            {
                var address = LocalStorage.GetString(StorageKeys.AccountAddressKey);
                if (!address.HasError)
                {
                    // Set the UI field accordingly.
                    AddressText.text = address.Value;
                }
            }

            if (LocalStorage.HasKey(StorageKeys.AccountPrivateKey))
            {
                var privateKey = LocalStorage.GetString(StorageKeys.AccountPrivateKey);
                if (!privateKey.HasError)
                {
                    // Set the UI field accordingly.
                    PrivateKeyText.text = privateKey.Value;
                }
            }

            // On startup, we check if an address and a private key are already stored.
            if (LocalStorage.HasKey(StorageKeys.AccountAddressKey) && LocalStorage.HasKey(StorageKeys.AccountPrivateKey))
            {
                // If so, we retrieve them.
                var address = LocalStorage.GetString(StorageKeys.AccountAddressKey);
                var privateKey = LocalStorage.GetString(StorageKeys.AccountPrivateKey);

                if (!address.HasError && !privateKey.HasError)
                {
                    // We start the login process with the stored address.
                    StartCoroutine(AccountManager.Login(address.Value, privateKey.Value, OnLoginCompleted));

                    if (VerifyingText != null)
                        VerifyingText.gameObject.SetActive(true);
                    if (AddressText != null)
                        AddressText.readOnly = true;
                }
            }
        }

        public void PerformLogin()
        {
            if (ErrorText != null)
                ErrorText.gameObject.SetActive(false);
            if (AddressText != null)
                AddressText.readOnly = true;
            StartCoroutine(AccountManager.Login(AddressText.text, PrivateKeyText.text, OnLoginCompleted));
        }

        private void OnLoginCompleted(bool success)
        {
            if (success)
            {
                if (VerifyingText != null)
                    VerifyingText.gameObject.SetActive(false);
                SceneManager.LoadScene(SceneToLoad);
            }
            else
            {
                if (ErrorText != null)
                    ErrorText.gameObject.SetActive(true);
                if (AddressText != null)
                    AddressText.readOnly = false;
            }
        }
    }
}