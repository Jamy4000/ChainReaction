using UnityEngine;

namespace DummyScripts
{
    public class DummyCaller : MonoBehaviour
    {
        private void Start() => ADummyScript.Instance.HelloWorld();
    }
}