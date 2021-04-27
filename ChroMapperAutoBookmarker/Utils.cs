using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ChroMapperAutoBookmarker
{
    internal static class Utils
    {
        private const BindingFlags FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

        public static void SetField(this object obj, string fieldName, object value)
        {
            var prop = obj.GetType().GetField(fieldName, FLAGS);
            prop.SetValue(obj, value);
        }

        public static T GetField<T>(this object obj, string fieldName)
        {
            var prop = obj.GetType().GetField(fieldName, FLAGS);
            var value = prop.GetValue(obj);
            return (T)value;
        }

        public static void InvokeMethod(this object obj, string methodName, object[] methodParams)
        {
            MethodInfo dynMethod = obj.GetType().GetMethod(methodName, FLAGS);
            dynMethod.Invoke(obj, methodParams);
        }

        public static void SetActionMapsEnabled(bool enabled)
        {
            IEnumerable<Type> actionMaps = typeof(CMInput).GetNestedTypes().Where(x => x.IsInterface);
            if (enabled)
                CMInputCallbackInstaller.ClearDisabledActionMaps(typeof(Plugin), actionMaps);
            else
                CMInputCallbackInstaller.DisableActionMaps(typeof(Plugin), actionMaps);
        }

        public static Sprite LoadSprite(string resource)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
            texture.LoadImage(data);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100f);
        }
    }
}
