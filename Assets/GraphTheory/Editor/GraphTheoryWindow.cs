﻿using GraphTheory.Editor.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphTheory.Editor
{
    public class GraphTheoryWindow : EditorWindow
    {
        private const string DATA_STRING = "GraphWindowData";
        private const string TOOLBAR = "toolbar";
        private const string MAIN_SPLITVIEW = "main-TwoPanelSplit";
        private const string MAIN_PANEL_LEFT = "main-panel-left";
        private const string MAIN_PANEL_RIGHT = "main-panel-right";

        private GraphWindowData m_graphWindowData = null;
        private NodeGraphView m_nodeGraphView = null;
        private Toolbar m_toolbar = null;
        private TwoPaneSplitView m_mainSplitView = null;
        private TabGroupElement m_mainTabGroup = null;

        [MenuItem("Graph/GraphTheory")]
        public static GraphTheoryWindow OpenWindow()
        {
            var window = GetWindow<GraphTheoryWindow>();
            window.titleContent = new GUIContent("NodeGraph");
            return window;
        }

        private void OnEnable() 
        {
            var xmlAsset = Resources.Load<VisualTreeAsset>("GraphTheoryWindow");
            xmlAsset.CloneTree(rootVisualElement);

            // Get all the elements
            m_mainSplitView = rootVisualElement.Q<TwoPaneSplitView>(MAIN_SPLITVIEW);
            VisualElement mainPanelRight = rootVisualElement.Q<VisualElement>(MAIN_PANEL_RIGHT);
            VisualElement mainPanelLeft = rootVisualElement.Q<VisualElement>(MAIN_PANEL_LEFT);
            m_toolbar = rootVisualElement.Q<Toolbar>(TOOLBAR);

            RegisterMainPanelLeft(mainPanelLeft);
            RegisterMainPanelRight(mainPanelRight);

            //Register toolbar last!
            RegisterToolbarButton_CreateNewGraph();

            DeserializeData();
        }

        private void DeserializeData()
        {
            string serializedData = EditorPrefs.GetString(DATA_STRING, "");
            if(string.IsNullOrEmpty(serializedData))
            {
                m_graphWindowData = new GraphWindowData(); 
            }
            else
            {
                m_graphWindowData = JsonUtility.FromJson<GraphWindowData>(serializedData);
            }
            Debug.Log("Deserialized data: " + serializedData);

            // Window siz 
            //Rect window = position; 
            //window.size = m_graphWindowData.WindowDimensions; 
            //position = window;

            // Main split view position
            m_mainSplitView.SetSplitPosition(m_graphWindowData.MainSplitViewPosition);
            m_mainTabGroup.DeserializeData(m_graphWindowData.MainTabGroup);
        }

        private void SerializeData()
        {
            m_graphWindowData.WindowDimensions = position.size;
            m_graphWindowData.MainSplitViewPosition = m_mainSplitView.SplitPosition;
            m_graphWindowData.MainTabGroup = m_mainTabGroup.GetSerializedData();

            Debug.Log("Serializing data: " + JsonUtility.ToJson(m_graphWindowData, true));
            EditorPrefs.SetString(DATA_STRING, JsonUtility.ToJson(m_graphWindowData, true));
        }

        private void OnDisable()
        {
            SerializeData();
        }

        private void RegisterToolbarButton_CreateNewGraph()
        { 
            var graphCreateButton = new Button(() =>
            {
                CreateNewGraphPopup.OpenWindow();
            });
            graphCreateButton.text = "Create Graph";
            m_toolbar.Add(graphCreateButton);
        }

        private void RegisterMainPanelLeft(VisualElement leftPanel)
        {
            List<(string, TabContent)> tabs = new List<(string, TabContent)>();
            tabs.Add(("Library", new TestContent()));
            tabs.Add(("Inspector", new TestContent()));

            m_mainTabGroup = new TabGroupElement(tabs);
            m_mainTabGroup.StretchToParentSize();
            leftPanel.Add(m_mainTabGroup);
        }

        private void RegisterMainPanelRight(VisualElement rightPanel)
        {
            m_nodeGraphView = new NodeGraphView
            {
                name = "NodeGraphView" 
            };
            m_nodeGraphView.StretchToParentSize();
            rightPanel.Add(m_nodeGraphView);
        }

        public void OpenGraph(string guid)
        {
            Debug.Log("Opening graph");
        }

        public void CloseGraph()
        {
            
        }
    }
}