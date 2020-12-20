﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logical.Editor
{
    /// <summary>
    /// This class takes a NodeView and breaks it into small pieces to enable precise customization.
    /// The NodeViewDrawer takes this class and provides a very clean way to customize the appearance of custom nodes.
    /// </summary>
    public class NodeDisplayContainers
    {
        public NodeView NodeView { get; private set; }
        public VisualElement HeaderContainer { get; private set; }
        public VisualElement PreTitleContainer { get; private set; }
        public VisualElement PostTitleContainer { get; private set; }
        public VisualElement PrimaryBodyContainer { get; private set; }
        public InportContainer InportContainer { get; private set; }
        public List<OutportContainer> OutportContainers = new List<OutportContainer>();
        public VisualElement SecondaryBodyContainer { get; private set; }
        public VisualElement FooterContainer { get; private set; }

        private VisualElement AllOutportsContainer = null;

        public NodeDisplayContainers(NodeView nodeView)
        {
            NodeView = nodeView;

            HeaderContainer = new VisualElement();
            HeaderContainer.name = "header-container";
            NodeView.Insert(0, HeaderContainer);

            PreTitleContainer = CreateBaseElement("pre-title-container");
            NodeView.titleContainer.Insert(0, PreTitleContainer);

            PostTitleContainer = CreateBaseElement("post-title-container");
            NodeView.titleContainer.Add(PostTitleContainer);

            PrimaryBodyContainer = CreateBaseElement("upper-body-container");
            NodeView.Q<VisualElement>("contents")?.Insert(1, PrimaryBodyContainer);

            SecondaryBodyContainer = CreateBaseElement("body-container");
            NodeView.extensionContainer.Add(SecondaryBodyContainer);

            FooterContainer = new VisualElement();
            FooterContainer.name = "footer-container";
            NodeView.Add(FooterContainer);

            AllOutportsContainer = new VisualElement();
            AllOutportsContainer.name = "all-ports-container";
            AllOutportsContainer.style.backgroundColor = new Color(0.17578125f, 0.17578125f, 0.17578125f);
            NodeView.outputContainer.Add(AllOutportsContainer);
        }

        public VisualElement CreateBaseElement(string name)
        {
            VisualElement baseEle = new VisualElement();
            baseEle.name = name;
            baseEle.style.backgroundColor = new Color(0.15625f, 0.15625f, 0.15625f);
            return baseEle;
        }

        public void AddNewOutport(PortView outport)
        {
            OutportContainer outportContainer = new OutportContainer(outport);
            OutportContainers.Add(outportContainer);
            AllOutportsContainer.Add(outportContainer);
        }

        public void SetInport(PortView inport)
        {
            InportContainer = new InportContainer(inport);
            NodeView.inputContainer.Add(InportContainer);
            NodeView.inputContainer.style.flexGrow = 0;
        }

        public List<PortView> GetAllPorts()
        {
            List<PortView> portViews = new List<PortView>();
            if (InportContainer != null)
            {
                portViews.Add(InportContainer.PortView);
            }
            for (int i = 0; i < OutportContainers.Count; i++)
            {
                portViews.Add(OutportContainers[i].PortView);
            }
            return portViews;
        }

        public void ClearDisplays(bool deletePorts)
        {
            HeaderContainer.Clear();
            PreTitleContainer.Clear();
            PostTitleContainer.Clear();
            PrimaryBodyContainer.Clear();
            InportContainer?.ClearContainers();
            for(int i = 0; i < OutportContainers.Count; i++)
            {
                OutportContainers[i]?.ClearContainers();
            }
            if (deletePorts)
            {
                OutportContainers.Clear();
                AllOutportsContainer.Clear();
            }
            SecondaryBodyContainer.Clear();
            FooterContainer.Clear();
        }

        public void ResolveCollapsedPorts()
        {
            InportContainer?.ResolveCollapsed();
            for (int i = 0; i < OutportContainers.Count; i++)
            {
                OutportContainers[i].ResolveCollapsed();
            }
        }
    }

    public class OutportContainer : VisualElement
    {
        private const string OUTPORT_HEADER = "outport-header";
        private const string OUTPORT_FOOTER = "outport-footer";
        private const string OUTPORT_BODY = "outport-body";
        private const string OUTPORT_AREA = "outport-area";

        public VisualElement OutportHeader { get; private set; }
        public VisualElement OutportFooter { get; private set; }
        public VisualElement OutportBody { get; private set; }
        private VisualElement OutportArea { get; set; }
        public PortView PortView { get; private set; }

        public OutportContainer(PortView portView)
        {
            var uxmlAsset = Resources.Load<VisualTreeAsset>(ResourceAssetPaths.OutportContainer_UXML);
            uxmlAsset.CloneTree(this);

            OutportHeader = this.Q<VisualElement>(OUTPORT_HEADER);
            OutportFooter = this.Q<VisualElement>(OUTPORT_FOOTER);
            OutportBody = this.Q<VisualElement>(OUTPORT_BODY);
            OutportArea = this.Q<VisualElement>(OUTPORT_AREA);

            PortView = portView;
            OutportArea.Add(portView);
        }

        public void ClearContainers()
        {
            OutportHeader.Clear();
            OutportFooter.Clear();
            OutportBody.Clear();
        }

        public void ResolveCollapsed()
        {
            this.style.display = PortView.style.visibility == Visibility.Hidden ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }

    public class InportContainer : VisualElement
    {
        private const string INPORT_HEADER = "inport-header";
        private const string INPORT_FOOTER = "inport-footer";
        private const string INPORT_BODY = "inport-body";
        private const string INPORT_AREA = "inport-area";

        public VisualElement InportHeader { get; private set; }
        public VisualElement InportFooter { get; private set; }
        public VisualElement InportBody { get; private set; }
        private VisualElement InportArea { get; set; }
        public PortView PortView { get; private set; }

        public InportContainer(PortView portView)
        {
            var uxmlAsset = Resources.Load<VisualTreeAsset>(ResourceAssetPaths.ImportContainer_UXML);
            uxmlAsset.CloneTree(this);

            InportHeader = this.Q<VisualElement>(INPORT_HEADER);
            InportFooter = this.Q<VisualElement>(INPORT_FOOTER);
            InportBody = this.Q<VisualElement>(INPORT_BODY);
            InportArea = this.Q<VisualElement>(INPORT_AREA);

            PortView = portView;
            InportArea.Add(portView);
        }

        public void ClearContainers()
        {
            InportHeader.Clear();
            InportFooter.Clear();
            InportBody.Clear();
        }

        public void ResolveCollapsed()
        {
            this.style.display = PortView.style.visibility == Visibility.Hidden ? DisplayStyle.None : DisplayStyle.Flex;

        }
    }
}