using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using VRC;
using VRC.Core;
using VRC.DataModel;
using VRC.DataModel.Core;
using VRC.SDKBase;

namespace Utils
{
    public static class Extensions
    {
        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }
        public static string GetPath(this GameObject gameObject)
        {
            string path = "/" + gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                path = "/" + gameObject.name + path;
            }
            return path;
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                if (exclude == null || exclude(transform.GetChild(i)))
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        public static void DestroyChildren(this Transform transform) => DestroyChildren(transform, null);

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }
        public static Vector3 SetY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static float RoundAmount(this float i, float nearestFactor)
        {
            return (float)Math.Round(i / nearestFactor) * nearestFactor;
        }

        public static Vector3 RoundAmount(this Vector3 i, float nearestFactor)
        {
            return new Vector3(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor), i.z.RoundAmount(nearestFactor));
        }

        public static Vector2 RoundAmount(this Vector2 i, float nearestFactor)
        {
            return new Vector2(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor));
        }

        public static void DelegateSafeInvoke(this Delegate @delegate, params object[] args)
        {
            Delegate[] delegates = @delegate.GetInvocationList();
            for (int i = 0; i < delegates.Length; i++)
            {
                try
                {
                    delegates[i].DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    Melonlogger.Error("Error while executing delegate:\n" + ex.ToString());
                }
            }
        }
    }
}