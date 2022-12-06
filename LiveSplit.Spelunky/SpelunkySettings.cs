

using LiveSplit.Spelunky.Categories;
using LiveSplit.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.Spelunky
{
  public class SpelunkySettings : UserControl
  {
    public static readonly System.Type[] CategoryTypes = new System.Type[3]
    {
      typeof (AllShortcuts),
      typeof (AllJournalEntries),
      typeof (AllCharacters)
    };
    private const bool DefaultAutoSplittingEnabled = true;
    private const bool DefaultAutoLoadSaveFile = false;
    private const bool DefaultUseAlternativeSaveFile = false;
    private const bool DefaultShowJournalTracker = false;
    private const int DefaultJournalTrackerScaleIndex = 0;
    private const bool DefaultShowCharactersTracker = false;
    private const int DefaultCharactersTrackerScaleIndex = 0;
    private const string DefaultSaveFile = "";
    private const int DefaultRunCategoryIndex = 0;
    private IContainer components;
    private GroupBox MainGroup;
    private Label RunCategorySelectionLabel;
    private CheckBox AutoSplittingEnabledCheckBox;
    private ComboBox RunCategoryNameComboBox;
    private Label SaveFilePrefixLabel;
    private CheckBox AutoLoadSaveCheckBox;
    private Button SaveFileBrowseButton;
    private TextBox SaveFileTextBox;
    private LinkLabel AlternativeSaveFileLinkLabel;
    private CheckBox ForceAlternativeSaveFileCheckBox;
    private CheckBox ShowJournalTrackerCheckBox;
    private GroupBox groupBox1;
    private ComboBox JournalTrackerScaleComboBox;
    private Label ScaleLabel;
    private GroupBox groupBox2;
    private CheckBox ShowCharactersTrackerCheckBox;
    private Label ScaleCharsLabel;
    private ComboBox CharactersTrackerScaleComboBox;
    private LinkLabel AboutSpelunkySplitterLabel;

    public bool AutoSplittingEnabled => this.AutoSplittingEnabledCheckBox.Checked;

    public double JournalTrackerScale
    {
      get
      {
        string selectedItem = (string) this.JournalTrackerScaleComboBox.SelectedItem;
        return double.Parse(selectedItem.Substring(0, selectedItem.LastIndexOf('x')));
      }
    }

    public bool ShowJournalTracker => this.ShowJournalTrackerCheckBox.Checked;

    public System.Type CurrentRunCategoryType => SpelunkySettings.CategoryTypes[this.RunCategoryNameComboBox.SelectedIndex];

    public string SaveFile => this.SaveFileTextBox.Text;

    public bool AutoLoadSaveFile => this.AutoLoadSaveCheckBox.Checked;

    public bool ForceAlternativeSaveFile => this.ForceAlternativeSaveFileCheckBox.Checked;

    public double CharactersTrackerScale
    {
      get
      {
        string selectedItem = (string) this.CharactersTrackerScaleComboBox.SelectedItem;
        return double.Parse(selectedItem.Substring(0, selectedItem.LastIndexOf('x')));
      }
    }

    public bool ShowCharactersTracker => this.ShowCharactersTrackerCheckBox.Checked;

    public event SpelunkySettings.PropertyChangedHandler PropertyChanged;

    public SpelunkySettings()
    {
      this.InitializeComponent();
      this.RunCategoryNameComboBox.DataSource = (object) ((IEnumerable<System.Type>) SpelunkySettings.CategoryTypes).Select<System.Type, string>((Func<System.Type, string>) (categoryType => Category.GetFriendlyName(categoryType))).ToArray<string>();
      this.AutoSplittingEnabledCheckBox.Checked = true;
      this.RunCategoryNameComboBox.SelectedIndex = 0;
      this.SaveFileTextBox.Text = "";
      this.AutoLoadSaveCheckBox.Checked = false;
      this.ForceAlternativeSaveFileCheckBox.Checked = false;
      this.JournalTrackerScaleComboBox.SelectedIndex = 0;
      this.CharactersTrackerScaleComboBox.SelectedIndex = 0;
      this.AutoSplittingEnabledCheckBox.CheckedChanged += new EventHandler(this.HandleAutoSplittingCheckedChanged);
      this.AutoLoadSaveCheckBox.CheckedChanged += new EventHandler(this.HandleAutoLoadSaveFileCheckedChanged);
      this.ForceAlternativeSaveFileCheckBox.CheckedChanged += new EventHandler(this.HandleForceAlternativeSaveFileCheckedChanged);
      this.ShowJournalTrackerCheckBox.CheckedChanged += new EventHandler(this.HandleShowJournalTrackerCheckBoxCheckedChanged);
      this.JournalTrackerScaleComboBox.SelectedIndexChanged += new EventHandler(this.HandleJournalTrackerScaleComboBoxSelectedIndexChanged);
      this.ShowCharactersTrackerCheckBox.CheckedChanged += new EventHandler(this.HandleShowCharactersTrackerCheckBoxCheckedChanged);
      this.CharactersTrackerScaleComboBox.SelectedIndexChanged += new EventHandler(this.HandleCharactersTrackerScaleComboBoxSelectedIndexChanged);
      this.RunCategoryNameComboBox.SelectedIndexChanged += new EventHandler(this.HandleRunSelectedIndexChanged);
      this.SaveFileBrowseButton.Click += new EventHandler(this.HandleSaveFileBrowseButtonClick);
      this.AlternativeSaveFileLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.HandleAlternativeSaveFileLinkLabelClicked);
      this.AboutSpelunkySplitterLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.HandleAboutSpelunkySplitterLinkClicked);
    }

    private void HandleSaveFileBrowseButtonClick(object sender, EventArgs args)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Title = "Select a save file";
      openFileDialog1.Filter = "Spelunky Save File (*.sav)|*.sav";
      openFileDialog1.FilterIndex = 0;
      openFileDialog1.RestoreDirectory = true;
      openFileDialog1.CheckFileExists = true;
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog() != DialogResult.OK)
        return;
      try
      {
        openFileDialog2.OpenFile().Close();
        this.SaveFileTextBox.Text = openFileDialog2.FileName;
        if (!this.AutoLoadSaveFile)
          return;
        this.PropertyChanged((object) this, EventArgs.Empty);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed to read file: " + ex.Message);
      }
    }

    private void HandleAutoLoadSaveFileCheckedChanged(object sender, EventArgs args)
    {
      if (this.AutoLoadSaveCheckBox.Checked && !File.Exists(this.SaveFile))
      {
        int num = (int) MessageBox.Show(this.SaveFile.Length == 0 ? "Select a valid save file before enabling auto-load." : "The selected auto-load save file does not exist.");
        this.AutoLoadSaveCheckBox.Checked = false;
      }
      else
        this.PropertyChanged((object) this, EventArgs.Empty);
    }

    private void HandleAlternativeSaveFileLinkLabelClicked(
      object sender,
      LinkLabelLinkClickedEventArgs args)
    {
      int num = (int) MessageBox.Show("This option enables changing the save file the game uses from 'spelunky_save.sav' to 'splitter_save.sav'. This allows for maintaining a separate speedrunning save file and normal gameplay file. After LiveSplit is closed Spelunky will use the normal save file again. Enabling this is recommended when using save file auto-load.", "Force alternative save file");
    }

    private void HandleAboutSpelunkySplitterLinkClicked(
      object sender,
      LinkLabelLinkClickedEventArgs args)
    {
      int num = (int) new AboutDialog().ShowDialog((IWin32Window) this);
    }

    private void HandleAutoSplittingCheckedChanged(object sender, EventArgs args) => this.PropertyChanged((object) this, EventArgs.Empty);

    private void HandleForceAlternativeSaveFileCheckedChanged(object sender, EventArgs args) => this.PropertyChanged((object) this, EventArgs.Empty);

    private void HandleShowJournalTrackerCheckBoxCheckedChanged(object sender, EventArgs args) => this.PropertyChanged((object) this, EventArgs.Empty);

    private void HandleJournalTrackerScaleComboBoxSelectedIndexChanged(
      object sender,
      EventArgs args)
    {
      this.PropertyChanged((object) this, EventArgs.Empty);
    }

    private void HandleShowCharactersTrackerCheckBoxCheckedChanged(object sender, EventArgs args) => this.PropertyChanged((object) this, EventArgs.Empty);

    private void HandleCharactersTrackerScaleComboBoxSelectedIndexChanged(
      object sender,
      EventArgs args)
    {
      this.PropertyChanged((object) this, EventArgs.Empty);
    }

    private void HandleRunSelectedIndexChanged(object sender, EventArgs args)
    {
      this.ShowJournalTrackerCheckBox.Checked = this.CurrentRunCategoryType.Equals(typeof (AllJournalEntries));
      this.ShowCharactersTrackerCheckBox.Checked = this.CurrentRunCategoryType.Equals(typeof (AllCharacters));
      this.PropertyChanged((object) this, EventArgs.Empty);
    }

    public XmlNode GetSettings(XmlDocument doc)
    {
      XmlElement element = doc.CreateElement("Settings");
      element.AppendChild((XmlNode) XMLSettings.ToElement<string>(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
      element.AppendChild((XmlNode) XMLSettings.ToElement<bool>(doc, "AutoSplittingEnabledCheckBox", this.AutoSplittingEnabledCheckBox.Checked));
      element.AppendChild((XmlNode) XMLSettings.ToElement<string>(doc, "RunCategoryNameComboBox", this.CurrentRunCategoryType.Name));
      element.AppendChild((XmlNode) XMLSettings.ToElement<string>(doc, "SaveFileTextBox", this.SaveFileTextBox.Text));
      element.AppendChild((XmlNode) XMLSettings.ToElement<bool>(doc, "AutoLoadSaveCheckBox", this.AutoLoadSaveCheckBox.Checked));
      element.AppendChild((XmlNode) XMLSettings.ToElement<bool>(doc, "ForceAlternativeSaveFileCheckBox", this.ForceAlternativeSaveFileCheckBox.Checked));
      element.AppendChild((XmlNode) XMLSettings.ToElement<bool>(doc, "ShowJournalTrackerCheckBox", this.ShowJournalTrackerCheckBox.Checked));
      element.AppendChild((XmlNode) XMLSettings.ToElement<string>(doc, "JournalTrackerScaleComboBox", (string) this.JournalTrackerScaleComboBox.SelectedItem));
      element.AppendChild((XmlNode) XMLSettings.ToElement<bool>(doc, "ShowCharactersTrackerCheckBox", this.ShowCharactersTrackerCheckBox.Checked));
      element.AppendChild((XmlNode) XMLSettings.ToElement<string>(doc, "CharactersTrackerScaleComboBox", (string) this.CharactersTrackerScaleComboBox.SelectedItem));
      return (XmlNode) element;
    }

    private int ParseRunCategoryTypeIndex(string strRunCategoryType)
    {
      try
      {
        return Array.IndexOf<System.Type>(SpelunkySettings.CategoryTypes, ((IEnumerable<System.Type>) SpelunkySettings.CategoryTypes).Where<System.Type>((Func<System.Type, bool>) (categoryType => categoryType.Name == strRunCategoryType)).First<System.Type>());
      }
      catch (InvalidOperationException ex)
      {
        throw new Exception(string.Format("Failed to parse category type '{0}'", (object) strRunCategoryType), (Exception) ex);
      }
    }

    private string ParseSaveFile(string strSaveFile) => strSaveFile;

    private int ParseJournalTrackerScaleIndex(string strScale)
    {
      int num = this.JournalTrackerScaleComboBox.Items.IndexOf((object) strScale);
      return num != -1 ? num : throw new Exception(string.Format("Failed to parse journal tracker scale value '{0}'", (object) strScale));
    }

    private int ParseCharactersTrackerScaleIndex(string strScale)
    {
      int num = this.CharactersTrackerScaleComboBox.Items.IndexOf((object) strScale);
      return num != -1 ? num : throw new Exception(string.Format("Failed to parse characters tracker scale value '{0}'", (object) strScale));
    }

    public void SetSettings(XmlNode settings)
    {
      this.AutoSplittingEnabledCheckBox.Checked = XMLSettings.Parse<bool>(settings["AutoSplittingEnabledCheckBox"], true, new XMLSettings.ExceptingParser<bool>(bool.Parse));
      this.RunCategoryNameComboBox.SelectedIndex = XMLSettings.Parse<int>(settings["RunCategoryNameComboBox"], 0, new XMLSettings.ExceptingParser<int>(this.ParseRunCategoryTypeIndex));
      this.SaveFileTextBox.Text = XMLSettings.Parse<string>(settings["SaveFileTextBox"], "", new XMLSettings.ExceptingParser<string>(this.ParseSaveFile));
      this.AutoLoadSaveCheckBox.Checked = XMLSettings.Parse<bool>(settings["AutoLoadSaveCheckBox"], false, new XMLSettings.ExceptingParser<bool>(bool.Parse));
      this.ForceAlternativeSaveFileCheckBox.Checked = XMLSettings.Parse<bool>(settings["ForceAlternativeSaveFileCheckBox"], false, new XMLSettings.ExceptingParser<bool>(bool.Parse));
      this.ShowJournalTrackerCheckBox.Checked = XMLSettings.Parse<bool>(settings["ShowJournalTrackerCheckBox"], false, new XMLSettings.ExceptingParser<bool>(bool.Parse));
      this.JournalTrackerScaleComboBox.SelectedIndex = XMLSettings.Parse<int>(settings["JournalTrackerScaleComboBox"], 0, new XMLSettings.ExceptingParser<int>(this.ParseJournalTrackerScaleIndex));
      this.ShowCharactersTrackerCheckBox.Checked = XMLSettings.Parse<bool>(settings["ShowCharactersTrackerCheckBox"], false, new XMLSettings.ExceptingParser<bool>(bool.Parse));
      this.CharactersTrackerScaleComboBox.SelectedIndex = XMLSettings.Parse<int>(settings["CharactersTrackerScaleComboBox"], 0, new XMLSettings.ExceptingParser<int>(this.ParseCharactersTrackerScaleIndex));
      this.PropertyChanged((object) this, EventArgs.Empty);
    }

    protected override void OnParentChanged(EventArgs e)
    {
      base.OnParentChanged(e);
      if (this.Parent?.Parent?.Parent == null || !this.Parent.Parent.Parent.GetType().Equals(typeof (ComponentSettingsDialog)))
        return;
      Control parent = this.Parent.Parent.Parent;
      parent.Text = "SpelunkySplitter Settings";
      Size size = this.Size + new Size(40, 110);
      parent.MinimumSize = size;
      parent.Size = size;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.MainGroup = new System.Windows.Forms.GroupBox();
            this.AboutSpelunkySplitterLabel = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.JournalTrackerScaleComboBox = new System.Windows.Forms.ComboBox();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.ShowJournalTrackerCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CharactersTrackerScaleComboBox = new System.Windows.Forms.ComboBox();
            this.ScaleCharsLabel = new System.Windows.Forms.Label();
            this.ShowCharactersTrackerCheckBox = new System.Windows.Forms.CheckBox();
            this.AlternativeSaveFileLinkLabel = new System.Windows.Forms.LinkLabel();
            this.ForceAlternativeSaveFileCheckBox = new System.Windows.Forms.CheckBox();
            this.SaveFileBrowseButton = new System.Windows.Forms.Button();
            this.SaveFileTextBox = new System.Windows.Forms.TextBox();
            this.SaveFilePrefixLabel = new System.Windows.Forms.Label();
            this.AutoLoadSaveCheckBox = new System.Windows.Forms.CheckBox();
            this.AutoSplittingEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.RunCategorySelectionLabel = new System.Windows.Forms.Label();
            this.RunCategoryNameComboBox = new System.Windows.Forms.ComboBox();
            this.MainGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGroup
            // 
            this.MainGroup.Controls.Add(this.AboutSpelunkySplitterLabel);
            this.MainGroup.Controls.Add(this.groupBox1);
            this.MainGroup.Controls.Add(this.groupBox2);
            this.MainGroup.Controls.Add(this.AlternativeSaveFileLinkLabel);
            this.MainGroup.Controls.Add(this.ForceAlternativeSaveFileCheckBox);
            this.MainGroup.Controls.Add(this.SaveFileBrowseButton);
            this.MainGroup.Controls.Add(this.SaveFileTextBox);
            this.MainGroup.Controls.Add(this.SaveFilePrefixLabel);
            this.MainGroup.Controls.Add(this.AutoLoadSaveCheckBox);
            this.MainGroup.Controls.Add(this.AutoSplittingEnabledCheckBox);
            this.MainGroup.Controls.Add(this.RunCategorySelectionLabel);
            this.MainGroup.Controls.Add(this.RunCategoryNameComboBox);
            this.MainGroup.Location = new System.Drawing.Point(3, 3);
            this.MainGroup.Name = "MainGroup";
            this.MainGroup.Size = new System.Drawing.Size(287, 247);
            this.MainGroup.TabIndex = 1;
            this.MainGroup.TabStop = false;
            this.MainGroup.Text = "SpelunkySplitter";
            // 
            // AboutSpelunkySplitterLabel
            // 
            this.AboutSpelunkySplitterLabel.AutoSize = true;
            this.AboutSpelunkySplitterLabel.Location = new System.Drawing.Point(5, 224);
            this.AboutSpelunkySplitterLabel.Name = "AboutSpelunkySplitterLabel";
            this.AboutSpelunkySplitterLabel.Size = new System.Drawing.Size(114, 13);
            this.AboutSpelunkySplitterLabel.TabIndex = 18;
            this.AboutSpelunkySplitterLabel.TabStop = true;
            this.AboutSpelunkySplitterLabel.Text = "About SpelunkySplitter";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.JournalTrackerScaleComboBox);
            this.groupBox1.Controls.Add(this.ScaleLabel);
            this.groupBox1.Controls.Add(this.ShowJournalTrackerCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(7, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 45);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Journal Tracker";
            // 
            // JournalTrackerScaleComboBox
            // 
            this.JournalTrackerScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.JournalTrackerScaleComboBox.FormattingEnabled = true;
            this.JournalTrackerScaleComboBox.Items.AddRange(new object[] {
            "1x",
            "1.25x",
            "1.5x",
            "1.75x",
            "2x",
            "2.25x",
            "2.5x",
            "2.75x",
            "3x",
            "3.25x",
            "3.5x",
            "3.75x",
            "4x"});
            this.JournalTrackerScaleComboBox.Location = new System.Drawing.Point(173, 18);
            this.JournalTrackerScaleComboBox.Name = "JournalTrackerScaleComboBox";
            this.JournalTrackerScaleComboBox.Size = new System.Drawing.Size(95, 21);
            this.JournalTrackerScaleComboBox.TabIndex = 15;
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.AutoSize = true;
            this.ScaleLabel.Location = new System.Drawing.Point(135, 22);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(37, 13);
            this.ScaleLabel.TabIndex = 14;
            this.ScaleLabel.Text = "Scale:";
            // 
            // ShowJournalTrackerCheckBox
            // 
            this.ShowJournalTrackerCheckBox.AutoSize = true;
            this.ShowJournalTrackerCheckBox.Location = new System.Drawing.Point(8, 20);
            this.ShowJournalTrackerCheckBox.Name = "ShowJournalTrackerCheckBox";
            this.ShowJournalTrackerCheckBox.Size = new System.Drawing.Size(56, 17);
            this.ShowJournalTrackerCheckBox.TabIndex = 13;
            this.ShowJournalTrackerCheckBox.Text = "Visible";
            this.ShowJournalTrackerCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CharactersTrackerScaleComboBox);
            this.groupBox2.Controls.Add(this.ScaleCharsLabel);
            this.groupBox2.Controls.Add(this.ShowCharactersTrackerCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(7, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 45);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Characters Tracker";
            // 
            // CharactersTrackerScaleComboBox
            // 
            this.CharactersTrackerScaleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CharactersTrackerScaleComboBox.FormattingEnabled = true;
            this.CharactersTrackerScaleComboBox.Items.AddRange(new object[] {
            "1x",
            "1.25x",
            "1.5x",
            "1.75x",
            "2x",
            "2.25x",
            "2.5x",
            "2.75x",
            "3x",
            "3.25x",
            "3.5x",
            "3.75x",
            "4x"});
            this.CharactersTrackerScaleComboBox.Location = new System.Drawing.Point(173, 18);
            this.CharactersTrackerScaleComboBox.Name = "CharactersTrackerScaleComboBox";
            this.CharactersTrackerScaleComboBox.Size = new System.Drawing.Size(95, 21);
            this.CharactersTrackerScaleComboBox.TabIndex = 18;
            // 
            // ScaleCharsLabel
            // 
            this.ScaleCharsLabel.AutoSize = true;
            this.ScaleCharsLabel.Location = new System.Drawing.Point(135, 22);
            this.ScaleCharsLabel.Name = "ScaleCharsLabel";
            this.ScaleCharsLabel.Size = new System.Drawing.Size(37, 13);
            this.ScaleCharsLabel.TabIndex = 17;
            this.ScaleCharsLabel.Text = "Scale:";
            // 
            // ShowCharactersTrackerCheckBox
            // 
            this.ShowCharactersTrackerCheckBox.AutoSize = true;
            this.ShowCharactersTrackerCheckBox.Location = new System.Drawing.Point(8, 20);
            this.ShowCharactersTrackerCheckBox.Name = "ShowCharactersTrackerCheckBox";
            this.ShowCharactersTrackerCheckBox.Size = new System.Drawing.Size(56, 17);
            this.ShowCharactersTrackerCheckBox.TabIndex = 16;
            this.ShowCharactersTrackerCheckBox.Text = "Visible";
            this.ShowCharactersTrackerCheckBox.UseVisualStyleBackColor = true;
            // 
            // AlternativeSaveFileLinkLabel
            // 
            this.AlternativeSaveFileLinkLabel.AutoSize = true;
            this.AlternativeSaveFileLinkLabel.Location = new System.Drawing.Point(211, 41);
            this.AlternativeSaveFileLinkLabel.Name = "AlternativeSaveFileLinkLabel";
            this.AlternativeSaveFileLinkLabel.Size = new System.Drawing.Size(13, 13);
            this.AlternativeSaveFileLinkLabel.TabIndex = 12;
            this.AlternativeSaveFileLinkLabel.TabStop = true;
            this.AlternativeSaveFileLinkLabel.Text = "?";
            // 
            // ForceAlternativeSaveFileCheckBox
            // 
            this.ForceAlternativeSaveFileCheckBox.AutoSize = true;
            this.ForceAlternativeSaveFileCheckBox.Location = new System.Drawing.Point(9, 40);
            this.ForceAlternativeSaveFileCheckBox.Name = "ForceAlternativeSaveFileCheckBox";
            this.ForceAlternativeSaveFileCheckBox.Size = new System.Drawing.Size(208, 17);
            this.ForceAlternativeSaveFileCheckBox.TabIndex = 11;
            this.ForceAlternativeSaveFileCheckBox.Text = "Force game to use alternative save file";
            this.ForceAlternativeSaveFileCheckBox.UseVisualStyleBackColor = true;
            // 
            // SaveFileBrowseButton
            // 
            this.SaveFileBrowseButton.Location = new System.Drawing.Point(206, 196);
            this.SaveFileBrowseButton.Name = "SaveFileBrowseButton";
            this.SaveFileBrowseButton.Size = new System.Drawing.Size(76, 22);
            this.SaveFileBrowseButton.TabIndex = 10;
            this.SaveFileBrowseButton.Text = "Browse";
            this.SaveFileBrowseButton.UseVisualStyleBackColor = true;
            // 
            // SaveFileTextBox
            // 
            this.SaveFileTextBox.Location = new System.Drawing.Point(59, 197);
            this.SaveFileTextBox.Name = "SaveFileTextBox";
            this.SaveFileTextBox.ReadOnly = true;
            this.SaveFileTextBox.Size = new System.Drawing.Size(145, 20);
            this.SaveFileTextBox.TabIndex = 9;
            // 
            // SaveFilePrefixLabel
            // 
            this.SaveFilePrefixLabel.AutoSize = true;
            this.SaveFilePrefixLabel.Location = new System.Drawing.Point(5, 200);
            this.SaveFilePrefixLabel.Name = "SaveFilePrefixLabel";
            this.SaveFilePrefixLabel.Size = new System.Drawing.Size(54, 13);
            this.SaveFilePrefixLabel.TabIndex = 8;
            this.SaveFilePrefixLabel.Text = "Save File:";
            // 
            // AutoLoadSaveCheckBox
            // 
            this.AutoLoadSaveCheckBox.AutoSize = true;
            this.AutoLoadSaveCheckBox.Location = new System.Drawing.Point(9, 60);
            this.AutoLoadSaveCheckBox.Name = "AutoLoadSaveCheckBox";
            this.AutoLoadSaveCheckBox.Size = new System.Drawing.Size(261, 17);
            this.AutoLoadSaveCheckBox.TabIndex = 7;
            this.AutoLoadSaveCheckBox.Text = "Auto-load save file before run (backup suggested)";
            this.AutoLoadSaveCheckBox.UseVisualStyleBackColor = true;
            // 
            // AutoSplittingEnabledCheckBox
            // 
            this.AutoSplittingEnabledCheckBox.AutoSize = true;
            this.AutoSplittingEnabledCheckBox.Checked = true;
            this.AutoSplittingEnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoSplittingEnabledCheckBox.Location = new System.Drawing.Point(9, 19);
            this.AutoSplittingEnabledCheckBox.Name = "AutoSplittingEnabledCheckBox";
            this.AutoSplittingEnabledCheckBox.Size = new System.Drawing.Size(127, 17);
            this.AutoSplittingEnabledCheckBox.TabIndex = 2;
            this.AutoSplittingEnabledCheckBox.Text = "Auto splitting enabled";
            this.AutoSplittingEnabledCheckBox.UseVisualStyleBackColor = true;
            // 
            // RunCategorySelectionLabel
            // 
            this.RunCategorySelectionLabel.AutoSize = true;
            this.RunCategorySelectionLabel.Location = new System.Drawing.Point(6, 177);
            this.RunCategorySelectionLabel.Name = "RunCategorySelectionLabel";
            this.RunCategorySelectionLabel.Size = new System.Drawing.Size(52, 13);
            this.RunCategorySelectionLabel.TabIndex = 1;
            this.RunCategorySelectionLabel.Text = "Category:";
            // 
            // RunCategoryNameComboBox
            // 
            this.RunCategoryNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RunCategoryNameComboBox.FormattingEnabled = true;
            this.RunCategoryNameComboBox.Location = new System.Drawing.Point(59, 173);
            this.RunCategoryNameComboBox.Name = "RunCategoryNameComboBox";
            this.RunCategoryNameComboBox.Size = new System.Drawing.Size(222, 21);
            this.RunCategoryNameComboBox.TabIndex = 0;
            // 
            // SpelunkySettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainGroup);
            this.Name = "SpelunkySettings";
            this.Size = new System.Drawing.Size(293, 253);
            this.MainGroup.ResumeLayout(false);
            this.MainGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

    }

    public delegate void PropertyChangedHandler(object sender, EventArgs args);
  }
}
