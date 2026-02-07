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
    private readonly Label _headerLabel = new();
    private readonly Label _firstNameLabel = new();
    private readonly Label _lastNameLabel = new();
    private readonly Label _greetingLabel = new();
    private readonly TableLayoutPanel _layout = new();
    private readonly NotifyIcon _notifyIcon;
    private readonly ContextMenuStrip _trayMenu;
    private bool _isExiting;

    public MainForm()
    {
        Text = "Hello in the Tray";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(440, 280);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        BackColor = Color.White;

        _headerLabel.Text = "Welcome!";
        _headerLabel.AutoSize = true;
        _headerLabel.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point);
        _headerLabel.ForeColor = Color.FromArgb(44, 62, 80);

        _promptLabel.Text = "Enter your first and last name to receive a greeting.";
        _promptLabel.AutoSize = true;
        _promptLabel.ForeColor = Color.FromArgb(95, 99, 104);

        _firstNameLabel.Text = "First name";
        _firstNameLabel.AutoSize = true;
        _firstNameLabel.ForeColor = Color.FromArgb(44, 62, 80);

        _lastNameLabel.Text = "Last name";
        _lastNameLabel.AutoSize = true;
        _lastNameLabel.ForeColor = Color.FromArgb(44, 62, 80);

        _firstNameTextBox.PlaceholderText = "Jane";
        _firstNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        _lastNameTextBox.PlaceholderText = "Doe";
        _lastNameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        _greetButton.Text = "Say hello";
        _greetButton.BackColor = Color.FromArgb(52, 152, 219);
        _greetButton.ForeColor = Color.White;
        _greetButton.FlatStyle = FlatStyle.Flat;
        _greetButton.FlatAppearance.BorderSize = 0;
        _greetButton.Height = 34;
        _greetButton.Click += GreetButtonOnClick;

        _greetingLabel.Text = "Your greeting will appear here.";
        _greetingLabel.AutoSize = true;
        _greetingLabel.ForeColor = Color.FromArgb(88, 110, 117);

        _layout.ColumnCount = 2;
        _layout.RowCount = 6;
        _layout.Dock = DockStyle.Fill;
        _layout.Padding = new Padding(24);
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        _layout.Controls.Add(_headerLabel, 0, 0);
        _layout.SetColumnSpan(_headerLabel, 2);
        _layout.Controls.Add(_promptLabel, 0, 1);
        _layout.SetColumnSpan(_promptLabel, 2);
        _layout.Controls.Add(_firstNameLabel, 0, 2);
        _layout.Controls.Add(_firstNameTextBox, 1, 2);
        _layout.Controls.Add(_lastNameLabel, 0, 3);
        _layout.Controls.Add(_lastNameTextBox, 1, 3);
        _layout.Controls.Add(_greetButton, 1, 4);
        _layout.Controls.Add(_greetingLabel, 0, 5);
        _layout.SetColumnSpan(_greetingLabel, 2);

        Controls.Add(_layout);

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

        var greeting = $"Hello, {firstName} {lastName}!";
        _greetingLabel.Text = greeting;
        MessageBox.Show(greeting, "Greeting", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
