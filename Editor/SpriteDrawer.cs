using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tymski
{
    [CustomPropertyDrawer(typeof(Sprite))]
    public class SpriteDrawer : PropertyDrawer
    {

        public static bool Expanded;
        private static GUIStyle s_TempStyle = new GUIStyle();
        float height;
        float width;
        Rect position;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
            if (property.serializedObject.isEditingMultipleObjects) return;
            if (property.objectReferenceValue == null) return;

            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 2 && position.Contains(e.mousePosition))
            {
                Expanded ^= true;
                CalculateRect(property);
            }
            if (!Expanded) return;

            if (Event.current.type != EventType.Repaint) return;

            this.position = position;

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect inspectorSprite;
            inspectorSprite = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            inspectorSprite.y += EditorGUIUtility.singleLineHeight + 2;

            Sprite sprite = property.objectReferenceValue as Sprite;
            CalculateRect(property);


            inspectorSprite.width = width;
            inspectorSprite.height = height;
            inspectorSprite.x = position.x + position.width - inspectorSprite.width;

            s_TempStyle.normal.background = sprite.texture;
            s_TempStyle.Draw(inspectorSprite, GUIContent.none, false, false, false, false);

            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.serializedObject.isEditingMultipleObjects)
            {
                return base.GetPropertyHeight(property, label);
            }
            CalculateRect(property);
            if (height == 0) return base.GetPropertyHeight(property, label);
            return base.GetPropertyHeight(property, label) + height + 4;
        }

        void CalculateRect(SerializedProperty property)
        {
            if (property.objectReferenceValue == null) return;
            if (!Expanded)
            {
                height = 0;
                position.height = 0;
            }

            Sprite sprite = property.objectReferenceValue as Sprite;

            float ratioX = position.width / sprite.rect.width;
            float ratioY = 200f / sprite.rect.height;
            float ratio = Mathf.Min(ratioX, ratioY);
            ratio = Mathf.Min(ratio, 1f);

            height = sprite.rect.height * ratio;
            width = sprite.rect.width * ratio;
        }
    }
}