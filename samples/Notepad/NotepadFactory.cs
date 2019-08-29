﻿using System;
using System.Text;
using System.Collections.Generic;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;
using Notepad.ViewModels.Documents;
using Notepad.ViewModels.Tools;

namespace Notepad
{
    public class NotepadFactory : Factory
    {
        public override IDock CreateLayout()
        {
            var untitledFileViewModel = new FileViewModel()
            {
                Path = string.Empty,
                Title = "Untitled",
                Text = "",
                Encoding = Encoding.Default.EncodingName
            };

            var findViewModel = new FindViewModel
            {
                Id = "Find",
                Title = "Find"
            };

            var replaceViewModel = new ReplaceViewModel
            {
                Id = "Replace",
                Title = "Replace"
            };

            var mainLayout = new ProportionalDock
            {
                Id = "Main",
                Title = "Main",
                Proportion = double.NaN,
                Orientation = Orientation.Horizontal,
                VisibleDockables = CreateList<IDockable>
                (
                    new DocumentDock
                    {
                        Id = "Files",
                        Title = "Files",
                        IsCollapsable = false,
                        Proportion = double.NaN,
                        ActiveDockable = untitledFileViewModel,
                        VisibleDockables = CreateList<IDockable>
                        (
                            untitledFileViewModel
                        )
                    },
                    new SplitterDock(),
                    new ProportionalDock
                    {
                        Proportion = 0.2,
                        Orientation = Orientation.Vertical,
                        VisibleDockables = CreateList<IDockable>
                        (
                            new ToolDock
                            {
                                Proportion = double.NaN,
                                ActiveDockable = findViewModel,
                                VisibleDockables = CreateList<IDockable>
                                (
                                    findViewModel
                                )
                            },
                            new SplitterDock(),
                            new ToolDock
                            {
                                Proportion = double.NaN,
                                ActiveDockable = replaceViewModel,
                                VisibleDockables = CreateList<IDockable>
                                (
                                    replaceViewModel
                                )
                            }
                        )
                    }
                )
            };

            var root = CreateRootDock();

            root.ActiveDockable = mainLayout;
            root.DefaultDockable = mainLayout;
            root.VisibleDockables = CreateList<IDockable>(mainLayout);
            root.Top = CreatePinDock();
            root.Top.Alignment = Alignment.Top;
            root.Bottom = CreatePinDock();
            root.Bottom.Alignment = Alignment.Bottom;
            root.Left = CreatePinDock();
            root.Left.Alignment = Alignment.Left;
            root.Right = CreatePinDock();
            root.Right.Alignment = Alignment.Right;

            return root;
        }

        public override void InitLayout(IDockable layout)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                ["Find"] = () => layout,
                ["Replace"] = () => layout
            };

            this.HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
            {
                [nameof(IDockWindow)] = () => new HostWindow()
            };

            this.DockableLocator = new Dictionary<string, Func<IDockable>>
            {
            };

            base.InitLayout(layout);
        }
    }
}
