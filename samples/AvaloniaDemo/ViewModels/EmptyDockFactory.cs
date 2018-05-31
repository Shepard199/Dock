﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dock.Avalonia.Controls;
using Dock.Model;
using Dock.Model.Controls;
using Dock.Model.Controls.Editor;

namespace AvaloniaDemo.ViewModels
{
    /// <inheritdoc/>
    public class EmptyDockFactory : DockFactory
    {
        /// <inheritdoc/>
        public override IDock CreateLayout()
        {
            var view = new ViewStub
            {
                Id = nameof(IView),
                Width = double.NaN,
                Height = double.NaN,
                Title = nameof(IView)
            };

            return new RootDock
            {
                Id = nameof(IRootDock),
                Dock = "",
                Width = double.NaN,
                Height = double.NaN,
                Title = nameof(IRootDock),
                CurrentView = view,
                DefaultView = view,
                Views = new ObservableCollection<IView>
                {
                    view
                }
            };
        }

        /// <inheritdoc/>
        public override void InitLayout(IView layout, object context)
        {
            this.ContextLocator = new Dictionary<string, Func<object>>
            {
                [nameof(IRootDock)] = () => context,
                [nameof(ILayoutDock)] = () => context,
                [nameof(IDocumentDock)] = () => context,
                [nameof(IToolDock)] = () => context,
                [nameof(ISplitterDock)] = () => context,
                [nameof(IDockWindow)] = () => context
            };

            this.HostLocator = new Dictionary<string, Func<IDockHost>>
            {
                [nameof(IDockWindow)] = () => new HostWindow()
            };

            this.Update(layout, context, null);

            if (layout is IWindowsHost layoutWindowsHost)
            {
                layoutWindowsHost.ShowWindows();
                if (layout is IViewsHost layoutViewsHost)
                {
                    layoutViewsHost.CurrentView = layoutViewsHost.DefaultView;
                    if (layoutViewsHost.CurrentView is IWindowsHost currentViewWindowsHost)
                    {
                        currentViewWindowsHost.ShowWindows();
                    }
                }
            }
        }
    }
}