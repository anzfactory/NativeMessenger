/*********************************
 2015-12-23
*********************************/
using UnityEngine;
using System.Runtime.InteropServices;

namespace Anz.Utils
{

    public class NativeMessenger
    {
        private static int counter = 0;

        public static void ShowToast(string message)
        {
            var adapter = NativeMessenger.factory();
            if (adapter != null) {
                adapter.ShowToast(message);
            }
        }

        public static void ShowAlert(string message, System.Action callback)
        {
            var adapter = NativeMessenger.factory();
            if (adapter != null) {
                adapter.ShowAlert(message, callback);
            }
        }

        private static NativeMessageAdapter factory()
        {
            NativeMessageAdapter messageAdapter = null;
            GameObject gameObject = new GameObject(NativeMessageAdapter.OBJECT_NAME + "_" + counter++);

            #if UNITY_EDITOR
            messageAdapter = gameObject.AddComponent<EditorMessageAdapter>();
            #elif UNITY_IPHONE
            messageAdapter = gameObject.AddComponent<IOSNativeMessageAdapter>();
            #elif UNITY_ANDROID
            messageAdapter = gameObject.AddComponent<AndroidNativeMessageAdapter>();
            #endif

            return messageAdapter;
        }
    }


    public abstract class NativeMessageAdapter : UnityEngine.MonoBehaviour
    {

        public static readonly string OBJECT_NAME = "NativeMessenger";
        protected static readonly string METHOD_NAME = "Dismiss";

        protected System.Action callback;

        public abstract void ShowToast(string message);
        public abstract void ShowAlert(string message, System.Action callback);

        public void Dismiss()
        {
            if (this.callback != null) {
                this.callback();
            }

            Destroy(this.gameObject);
        }

    }

    #if UNITY_IPHONE
    public class IOSNativeMessageAdapter : NativeMessageAdapter
    {

        [DllImport("__Internal")]
        private static extern void showToast(string message);
        [DllImport("__Internal")]
        private static extern void showAlert(string message, string callbackObjectName, string callbackMethodName);

        public override void ShowToast(string message)
        {
            showToast(message);
            Dismiss();
        }

        public override void ShowAlert(string message, System.Action callback)
        {
            this.callback = callback;
            showAlert(message, this.gameObject.name, METHOD_NAME);
        }
    }
    #endif

    #if UNITY_ANDROID
    public class AndroidNativeMessageAdapter : NativeMessageAdapter
    {
        public override void ShowToast(string message)
        {
            using ( UnityEngine.AndroidJavaClass plugin = new UnityEngine.AndroidJavaClass("xyz.anzfactory.MessageManager") ) {
                plugin.CallStatic("showToast", message);
                Dismiss();
            }
        }

        public override void ShowAlert(string message, System.Action callback)
        {
            this.callback = callback;
            using ( AndroidJavaClass plugin = new AndroidJavaClass("xyz.anzfactory.MessageManager") ) {
                plugin.CallStatic("showAlert", message, this.gameObject.name, METHOD_NAME);
            }
        }
    }
    #endif

    public class EditorMessageAdapter : NativeMessageAdapter
    {
        public override void ShowToast(string message)
        {
            Debug.Log("ShowToast:" + message);
            Dismiss();
        }

        public override void ShowAlert(string message, System.Action callback)
        {
            this.callback = callback;
            Debug.Log("ShowAlert:" + message);
            Dismiss();
        }
    }

}
