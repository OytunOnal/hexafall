using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HexFall
{
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            object[] attributes = methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true);
            for(int i = 0; i < attributes.Length; i++)
            {
                if(attributes != null)
                {
                    ButtonAttribute buttonAttribute = (ButtonAttribute)attributes[i];
                    string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

                    if (GUILayout.Button(buttonText))
                    {
                        object[] attributeParams = buttonAttribute.Params;
                        if(attributeParams.Length > 0)
                        {
                            ParameterInfo[] methodParams = methodInfo.GetParameters();
                            if(attributeParams.Length == methodParams.Length)
                            {
                                bool allowInvoke = true;
                                for(int p = 0; p < attributeParams.Length; p++)
                                {
                                    if(attributeParams[p].GetType() != methodParams[p].ParameterType)
                                    {
                                        allowInvoke = false;

                                        Debug.LogWarning(string.Format("Invalid parameters are specified ({0})", buttonText), target);

                                        break;
                                    }
                                }

                                if(allowInvoke)
                                {
                                    methodInfo.Invoke(target, buttonAttribute.Params);
                                }
                            }
                            else
                            {
                                Debug.LogWarning(string.Format("Invalid parameters are specified ({0})", buttonText), target);
                            }
                        }
                        else
                        {
                            methodInfo.Invoke(target, null);
                        }
                    }
                }
            }
        }
    }
}
