﻿using UnityEditor;

namespace Toolbox.Editor
{
    using Editor = UnityEditor.Editor;

    //TODO: new name
    public class BasicToolboxEditor : IToolboxEditor
    {
        public BasicToolboxEditor(Editor contextEditor)
        {
            ContextEditor = contextEditor;
        }

        public void DrawCustomProperty(SerializedProperty property)
        {
            ToolboxEditorGui.DrawToolboxProperty(property);
        }

        public void DrawCustomInspector()
        {
            DrawCustomInspector(ContextEditor.serializedObject);
        }

        public void DrawCustomInspector(SerializedObject serializedObject)
        {
            if (!ToolboxDrawerModule.ToolboxDrawersAllowed)
            {
                ContextEditor.DrawDefaultInspector();
                return;
            }

            serializedObject.Update();
            var property = serializedObject.GetIterator();
            //enter to the 'Base' property
            if (property.NextVisible(true))
            {
                var isScript = PropertyUtility.IsDefaultScriptProperty(property);
                //try to draw the first property (m_Script)
                using (new EditorGUI.DisabledScope(isScript))
                {
                    DrawCustomProperty(property.Copy());
                }

                //iterate over all other serialized properties
                //NOTE: every child will be handled internally
                while (property.NextVisible(false))
                {
                    DrawCustomProperty(property.Copy());
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        public Editor ContextEditor { get; }
    }
}
