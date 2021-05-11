using Managers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Story
{
    // StoryDrawerUIE
    [CustomPropertyDrawer(typeof(Story))]
    public class StoryDrawerUie : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var question = new PropertyField(property.FindPropertyRelative("question"));

            var chosen = new PropertyField(property.FindPropertyRelative("chosen"));

            var methodName = new PropertyField(property.FindPropertyRelative("methodName"));

            var className = new PropertyField(property.FindPropertyRelative("className"));

            // Add fields to the container.
            container.Add(question);

            container.Add(chosen);

            container.Add(methodName);

            container.Add(className);

            return container;
        }
    }

    // StoryDrawer
    [CustomPropertyDrawer(typeof(Story))]
    public class StoryDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var questionRect = new Rect(position.x, position.y, 30, position.height);
            var chosenRect = new Rect(position.x + 35, position.y, 50, position.height);
            var methodRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
            var classRect = new Rect(position.x, position.y, 30, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(questionRect, property.FindPropertyRelative("question"), GUIContent.none);

            EditorGUI.PropertyField(chosenRect, property.FindPropertyRelative("chosen"), GUIContent.none);

            EditorGUI.PropertyField(methodRect, property.FindPropertyRelative("methodName"), GUIContent.none);

            EditorGUI.PropertyField(classRect, property.FindPropertyRelative("className"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}