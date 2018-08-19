using UnityEngine;

namespace MHLab.UI
{
    public class ObjectToggler :  MonoBehaviour
    {
        public GameObject Target;

        public void Toggle()
        {
            Target.SetActive(!Target.activeSelf);
        }
    }
}
