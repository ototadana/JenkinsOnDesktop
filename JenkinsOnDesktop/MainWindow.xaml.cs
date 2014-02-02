using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using XPFriend.JenkinsOnDesktop.Core;
using XPFriend.JenkinsOnDesktop.Core.Folder;
using XPFriend.JenkinsOnDesktop.Util;

namespace XPFriend.JenkinsOnDesktop
{
    public partial class MainWindow : Window
    {
        private const string CallTheButlerMenuItem = "CallTheButler";
        private static MainWindow current;
        private DispatcherTimer timer = new DispatcherTimer();
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        private bool active;

        private Workspace Workspace { get { return Workspace.Current; } }
        internal double ScreenWidth { get { return SystemParameters.VirtualScreenWidth; } }
        internal double ScreenHeight { get { return SystemParameters.VirtualScreenHeight; } }

        internal static MainWindow Current
        {
            get
            {
                if (current == null)
                {
                    current = App.Instance.MainWindow as MainWindow;
                }
                return current;
            }

            set
            {
                current = value;
            }
        }

        #region "Window : Initializing"
        public MainWindow()
        {
            InitializeComponent();

            this.messageText.MouseLeftButtonDown += (sender, e) => ShowSourceUrl();
            this.butlerImage.MouseLeftButtonDown += (sender, e) => MoveWindow();
            this.configuration.MouseLeftButtonDown += (sender, e) => MoveWindow();
            this.exitButton.Click += (sender, e) => Exit(this.Workspace.Butler);
            this.showConfigurationButton.Click += (sender, e) => ShowConfiguration(this.Workspace.Butler);
            this.parameters.LostFocus += (sender, e) => this.businessExamplesPopup.IsOpen = false;
            this.parameters.GotFocus += (sender, e) => this.businessExamplesPopup.IsOpen = true;
            this.parameters.PreviewMouseDown += (sender, e) => this.businessExamplesPopup.IsOpen = true;
            this.MouseDown += (sender, e) => CloseBusinessExamples();

            this.ShowInTaskbar = false;

            this.notifyIcon.ContextMenuStrip = CreateContextMenu();
            this.notifyIcon.Visible = true;
            ApplySettings(this.Workspace);
            this.notifyIcon.DoubleClick += (sender, e) => ShowWindow();

            this.timer.Tick += (sender, e) => DoBusinessAndUpdateWindow(this.Workspace.Butler);
            StartTimer();
            this.Activated += (sender, e) => StopTimer();
            this.Deactivated += (sender, e) => StartTimer();
            if (!this.Workspace.HasConfigurationFile)
            {
                this.Loaded += (sender, e) => ShowConfiguration(this.Workspace.Butler);
            }
        }

        private void StopTimer()
        {
            this.timer.Stop();
        }

        private void StartTimer()
        {
            if (!this.configuration.IsVisible) 
            { 
                this.timer.Start(); 
            }
        }

        private void CloseBusinessExamples()
        {
            if (this.businessExamplesPopup.IsOpen) 
            {
                this.businessExamplesPopup.IsOpen = false;
            }
        }

        private System.Windows.Forms.ContextMenuStrip CreateContextMenu()
        {
            System.Windows.Forms.ContextMenuStrip contextMenu = 
                new System.Windows.Forms.ContextMenuStrip();

            contextMenu.Items.Add(CreateCallTheButlerMenuItem(contextMenu));
            contextMenu.Items.Add(CreateQuitMenuItem());
            return contextMenu;
        }

        private System.Windows.Forms.ToolStripMenuItem CreateQuitMenuItem()
        {
            System.Windows.Forms.ToolStripMenuItem quitMenuItem =
                new System.Windows.Forms.ToolStripMenuItem() 
                { 
                    Text = Properties.Resources.NotifyIcon_Quit 
                };

            quitMenuItem.Click += (sender, e) => Quit();
            return quitMenuItem;
        }

        private System.Windows.Forms.ToolStripMenuItem CreateCallTheButlerMenuItem(
            System.Windows.Forms.ContextMenuStrip contextMenu)
        {
            System.Windows.Forms.ToolStripMenuItem callTheButlerMenuItem =
                new System.Windows.Forms.ToolStripMenuItem()
                {
                    DoubleClickEnabled = true,
                    Text = string.Format(Properties.Resources.NotifyIcon_CallTheButler, "Jenkins")
                };

            callTheButlerMenuItem.Name = CallTheButlerMenuItem;
            callTheButlerMenuItem.Font =
                new System.Drawing.Font(callTheButlerMenuItem.Font.FontFamily,
                    callTheButlerMenuItem.Font.Size,
                    callTheButlerMenuItem.Font.Style ^ System.Drawing.FontStyle.Bold);
            callTheButlerMenuItem.Click += (sender, e) => ShowWindow();
            callTheButlerMenuItem.Visible = false;
            contextMenu.Opening += (sernder, e) => callTheButlerMenuItem.Select();
            return callTheButlerMenuItem;
        }

        private void ShowSourceUrl()
        {
            if (this.messageText.ToolTip != null)
            {
                Process.Start(this.messageText.ToolTip.ToString());
            }
        }

        private void MoveWindow()
        {
            try 
            { 
                this.DragMove(); 
            }
            catch (Exception) 
            { 
            }
        }
        #endregion

        #region "Window : ShowWindow"
        private void ShowWindow()
        {
            StopTimer();
            this.active = true;
            if (this.IsOnDesktop())
            {
                if (this.backgroundImage.ImageSource == null)
                {
                    UpdateWindow();
                }
                ActivateWindow();
            }
            else
            {
                ShowWindow(this.Workspace.Butler);
            }
        }

        private void ShowWindow(Butler butler)
        {
            butler.EnterAnimation.Children.Last().Completed += ActivateWindow;
            butler.EnterAnimation.Begin(this);
        }

        private void ActivateWindow(object sender, EventArgs e)
        {
            this.Workspace.Butler.EnterAnimation.Children.Last().Completed -= ActivateWindow;
            ActivateWindow();
        }

        private void ActivateWindow()
        {
            this.Topmost = this.Workspace.Butler.Topmost;
            if (this.active)
            {
                this.active = false;
                this.Activate();
            }
        }

        private bool IsOnDesktop()
        {
            return this.IsVisible && this.Opacity > 0.0 &&
                this.Left >= 0.0 && this.Left < this.ScreenWidth &&
                this.Top >= 0.0 && this.Top < this.ScreenHeight;
        }
        #endregion

        #region "Window : DoBusinessAndUpdateWindow"
        private void DoBusinessAndUpdateWindow(Butler butler)
        {
            butler.ExitAnimation.Children.Last().Completed += DoBusinessAndUpdateWindow;
            butler.ExitAnimation.Begin(this);
        }

        private void DoBusinessAndUpdateWindow(object sender, EventArgs e)
        {
            Butler butler = this.Workspace.Butler;
            butler.ExitAnimation.Children.Last().Completed -= DoBusinessAndUpdateWindow;
            this.Workspace.DoBusiness();
            if (butler.HasNews)
            {
                UpdateBalloonTipBy(butler, this.notifyIcon);
                UpdateWindowBy(butler);
            }
        }

        private void UpdateWindowBy(Butler butler)
        {
            ClearMessage();
            if (butler.HasMessage)
            {
                ShowWindow(butler);
            }
        }

        private void ClearMessage()
        {
            this.messageText.Text = null;
            this.messageText.ToolTip = null;
            this.message.Opacity = 0.0;
        }

        private static void UpdateBalloonTipBy(Butler butler, System.Windows.Forms.NotifyIcon notifyIcon)
        {
            notifyIcon.Icon = butler.Icon;
            if (string.IsNullOrEmpty(butler.BalloonTipText))
            {
                return;
            }
            notifyIcon.ShowBalloonTip(
                butler.BalloonTipTimeout * 1000,
                butler.BalloonTipTitle,
                butler.BalloonTipText,
                butler.ToolTipIcon);
        }
        #endregion

        #region "Window : Operations"
        internal void ShowMessage()
        {
            if (string.IsNullOrWhiteSpace(this.messageText.Text))
            {
                return;
            }
            Show(this.message, null);
        }

        internal void HideMessage()
        {
            Hide(this.message, null);
        }

        internal void Quit()
        {
            if (this.notifyIcon != null)
            {
                this.notifyIcon.Visible = false;
            }
            App.Instance.Shutdown();
        }

        internal void UpdateWindow()
        {
            Butler butler = this.Workspace.Butler;
            this.butlerImage.Source = butler.Image;
            this.configuration.Visibility = Visibility.Collapsed;
            UpdateMessage(this.message, this.messageText, butler);
            this.Topmost = true;
        }

        private static void UpdateMessage(Border message, TextBlock messageText, Butler butler)
        {
            message.Visibility = Visibility.Hidden;
            if (!butler.HasMessage)
            {
                return;
            }
            messageText.Text = butler.MessageText;
            messageText.FontFamily = butler.MessageStyle.FontFamily;
            messageText.FontSize = butler.MessageStyle.FontSize;
            messageText.ToolTip = butler.SourceUrl;
            messageText.Foreground = butler.MessageStyle.Foreground;
            message.Opacity = 1.0;
            message.Width = butler.MessageStyle.Width;
            message.Height = butler.MessageStyle.Height;
            message.Padding = butler.MessageStyle.Padding;
            message.Cursor = (messageText.ToolTip != null) ? Cursors.Hand : Cursors.Arrow;
            message.Background = butler.MessageStyle.Background;
            message.BorderBrush = butler.MessageStyle.BorderBrush;
            message.BorderThickness = butler.MessageStyle.BorderThickness;
            message.CornerRadius = butler.MessageStyle.CornerRadius;
            UpdateMessagePosition(message, butler.MessageStyle.Position);
        }

        private static void UpdateMessagePosition(FrameworkElement message, MessagePosition position)
        {
            if (position == MessagePosition.Left)
            {
                Grid.SetColumn(message, 0);
                Grid.SetRow(message, 1);
            }
            else if (position == MessagePosition.Top)
            {
                Grid.SetColumn(message, 1);
                Grid.SetRow(message, 0);
            }
            else if (position == MessagePosition.Bottom)
            {
                Grid.SetColumn(message, 1);
                Grid.SetRow(message, 2);
            }
            else // if (position == MessagePosition.Right)
            {
                Grid.SetColumn(message, 2);
                Grid.SetRow(message, 1);
            }
        }

        private void Exit(Butler butler)
        {
            if (this.configuration.IsVisible)
            {
                Hide(this.configuration, () => butler.ExitAnimation.Begin(this));
            }
            else
            {
                butler.ExitAnimation.Begin(this);
            }
        }

        #endregion

        #region "Configuration : Initialize"

        private void ShowConfiguration(Butler butler)
        {
            AddButlerNames();
            UpdateButlerInformations(butler);
            AddBusinessNames();
            this.configurationButlerTab.IsSelected = true;
            if (this.message.Visibility == Visibility.Visible)
            {
                Change(this.message, this.configuration, 
                    () => UpdateBusinessInformation(this.businessName.Text));
            }
            else
            {
                Show(this.configuration,
                    () => UpdateBusinessInformation(this.businessName.Text));
            }
        }

        private void AddButlerNames()
        {
            AddItems(this.butlerName, ButlersFolder.GetFolderNames(), 
                this.Workspace.Configuration.Butler);
        }

        private void AddBusinessNames()
        {
            AddItems(this.businessName, BusinessesFolder.GetFolderNames(), 
                this.Workspace.Configuration.Business);
        }

        private static void AddItems(ComboBox comboBox, IEnumerable<string> items, string text)
        {
            comboBox.Items.Clear();
            comboBox.Text = text;
            foreach (string item in items)
            {
                comboBox.Items.Add(item);
            }
        }

        private void UpdateButlerInformations(Butler butler)
        {
            this.butlerDisplayName.Text = butler.DisplayName;
            this.butlerLicense.Text = butler.License;
            this.butlerImageSample.Source = butler.TypicalAppearanceImage;
        }

        private void UpdateBusinessInformation(string name)
        {
            BusinessInformation businessInformation = BusinessInformation.GetInstance(name);
            this.businessSynopsis.Text = businessInformation.Synopsis;
            this.businessDescription.Text = businessInformation.Description;
            this.businessExamples.Text = businessInformation.Examples;

            Business businessConfiguration = Business.GetInstance(name);
            this.parameters.Text = businessConfiguration.Parameters;
            this.timerInterval.Value = businessConfiguration.TimerInterval;
        }

        private void Change(FrameworkElement oldElement, FrameworkElement newElement, Action action)
        {
            Hide(oldElement, () => Show(newElement, action));
        }

        private void Show(FrameworkElement element, Action action)
        {
            element.Opacity = 0.0;
            element.Visibility = Visibility.Visible;
            UpdateMessagePosition(element, this.Workspace.Butler.MessageStyle.Position);
            Storyboard storyBoard = CreateFadeAnimation(element, 0.0, 1.0);
            if (action != null)
            {
                storyBoard.Completed += (sender, e) => action();
            }
            storyBoard.Begin(this);
        }

        private void Hide(FrameworkElement element, Action action)
        {
            Storyboard storyBoard = CreateFadeAnimation(element, 1.0, 0.0);
            storyBoard.Completed += (sender, e) =>
            {
                element.Visibility = Visibility.Hidden;
                element.Opacity = 1.0;
                if (action != null)
                {
                    action();
                }
            };
            storyBoard.Begin(this);
        }

        private static Storyboard CreateFadeAnimation(FrameworkElement element, double from, double to)
        {
            Storyboard storyBoard = new Storyboard() { FillBehavior = FillBehavior.HoldEnd };
            Timeline timeline = new DoubleAnimation(from, to, new Duration(TimeSpan.FromSeconds(0.2)));
            Storyboard.SetTarget(timeline, element);
            Storyboard.SetTargetProperty(timeline, new PropertyPath("Opacity"));
            storyBoard.Children.Add(timeline);
            return storyBoard;
        }
        #endregion

        #region "Configuration : Events"

        private void timerInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.timerIntervalLabel.Content = 
                new DateTime(TimeSpan.FromSeconds(e.NewValue).Ticks).ToString("HH\\:mm\\:ss");
        }

        private void discardConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            Hide(this.configuration, () => this.Workspace.Butler.ExitAnimation.Begin(this));
        }

        private void applyConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAsFileName(butlerName) || !ValidateAsFileName(businessName))
            {
                return;
            }

            Hide(this.configuration, () =>
            {
                SaveConfigurations(this.Workspace);
                ApplySettings(this.Workspace);
                UpdateWindow();
                ActivateWindow();
                DoBusinessAndUpdateWindow(this.Workspace.Butler);
            });
        }

        private void SaveConfigurations(Workspace workspace)
        {
            workspace.Configuration.Business = this.businessName.Text;
            workspace.Configuration.Butler = this.butlerName.Text;
            workspace.Business.Parameters = this.parameters.Text;
            workspace.Business.TimerInterval = this.timerInterval.Value;
            workspace.SaveConfigurations();
        }

        private bool ValidateAsFileName(ComboBox comboBox)
        {
            if (string.IsNullOrEmpty(comboBox.Text))
            {
                comboBox.Focus();
                return false;
            }

            try
            {
                WorkspaceFolder.ValidateAsFileName(comboBox.Text);
                return true;
            }
            catch (ApplicationException e)
            {
                MessageBox.Show(e.Message, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
        }

        private void ApplySettings(Workspace workspace)
        {
            workspace.Initialize();
            this.timer.Interval = TimeSpan.FromSeconds(workspace.Business.TimerInterval);
            this.notifyIcon.Icon = workspace.Butler.Icon;
            this.Title = workspace.Butler.DisplayName;
            this.Icon = BitmapUtil.ToBitmapSource(workspace.Butler.Icon.ToBitmap());

            System.Windows.Forms.ToolStripItem callTheButlerMenuItem =
                this.notifyIcon.ContextMenuStrip.Items[CallTheButlerMenuItem];

            callTheButlerMenuItem.Text = 
                string.Format(Properties.Resources.NotifyIcon_CallTheButler, workspace.Butler.Nickname);

            callTheButlerMenuItem.Visible = true;
        }

        private void openButlerConfigurationFolder_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAsFileName(this.butlerName))
            {
                ButlersFolder.Open(this.butlerName.Text);
            }
        }

        private void openBusinessConfigurationFolder_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAsFileName(this.businessName))
            {
                BusinessesFolder.Open(this.businessName.Text);
            }
        }

        private void butlerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = (string)this.butlerName.SelectedValue;
            if (!string.IsNullOrEmpty(name))
            {
                UpdateButlerInformations(Butler.GetInstance(name));
            }
        }

        private void businessName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = (string)this.businessName.SelectedValue;
            if (!string.IsNullOrEmpty(name))
            {
                UpdateBusinessInformation(name);
            }
        }
        #endregion
    }
}
