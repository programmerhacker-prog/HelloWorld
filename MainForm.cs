using System;
using System.Drawing;
using System.Windows.Forms;

namespace HelloWorld;

public sealed class MainForm : Form
{
    private readonly TextBox _firstNameTextBox = new();
    private readonly TextBox _lastNameTextBox = new();
    private readonly Button _greetButton = new();
    private readonly Label _promptLabel = new();
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _trayMenu;
    private bool _isExiting;

    public MainForm()
    {
        Text = "Hello in the Tray";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(420, 220);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        _promptLabel.Text = "Enter your first and last name:";
        _promptLabel.AutoSize = true;
        _promptLabel.Location = new Point(20, 20);

        _firstNameTextBox.PlaceholderText = "First name";
        _firstNameTextBox.Location = new Point(20, 55);
        _firstNameTextBox.Width = 170;

        _lastNameTextBox.PlaceholderText = "Last name";
        _lastNameTextBox.Location = new Point(210, 55);
        _lastNameTextBox.Width = 170;

        _greetButton.Text = "Say hello";
        _greetButton.Location = new Point(20, 100);
        _greetButton.Click += GreetButtonOnClick;

        Controls.AddRange(new Control[] { _promptLabel, _firstNameTextBox, _lastNameTextBox, _greetButton });

        _trayMenu = new ContextMenuStrip();
        _trayMenu.Items.Add("Open", null, (_, _) => ShowFromTray());
        _trayMenu.Items.Add("Exit", null, (_, _) => ExitFromTray());

        _notifyIcon = new NotifyIcon
        {
            Text = "Hello in the Tray",
            Icon = SystemIcons.Application,
            Visible = true,
            ContextMenuStrip = _trayMenu
        };
        _notifyIcon.DoubleClick += (_, _) => ShowFromTray();

        Resize += OnResize;
        FormClosing += OnFormClosing;
    }

    private void GreetButtonOnClick(object? sender, EventArgs e)
    {
        var firstName = _firstNameTextBox.Text.Trim();
        var lastName = _lastNameTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            MessageBox.Show("Please enter both your first and last name.", "Missing info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show($"Hello, {firstName} {lastName}!", "Greeting", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void OnResize(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Minimized)
        {
            Hide();
        }
    }

    private void ShowFromTray()
    {
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
    }

    private void ExitFromTray()
    {
        _isExiting = true;
        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        Application.Exit();
    }

    private void OnFormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_isExiting)
        {
            return;
        }

        e.Cancel = true;
        Hide();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _notifyIcon.Dispose();
            _trayMenu.Dispose();
        }

        base.Dispose(disposing);
    }
}
