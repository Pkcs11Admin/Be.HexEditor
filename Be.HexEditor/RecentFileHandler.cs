﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Be.HexEditor.Properties;

namespace Be.HexEditor
{
    public partial class RecentFileHandler : Component
    {
        public class FileMenuItem : ToolStripMenuItem
        {
            string fileName;

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }

            public FileMenuItem(string fileName)
            {
                this.fileName = fileName;
            }

            public override string Text
            {
                get
                {
                    ToolStripMenuItem parent = (ToolStripMenuItem)this.OwnerItem;
                    int index = parent.DropDownItems.IndexOf(this);
                    return string.Format("{0} {1}", index+1, fileName);
                }
                set
                {
                }
            }
        }

        public const int MaxRecentFiles = 24;

        public RecentFileHandler()
        {
            InitializeComponent();

            Init();
        }

        public RecentFileHandler(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            Init();
        }

        void Init()
        {
            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Default_PropertyChanged);
        }

        public void AddFile(string fileName)
        {
            if (this.recentFileToolStripItem == null)
                throw new OperationCanceledException("recentFileToolStripItem can not be null!");

            // check if the file is already in the collection
            int alreadyIn = GetIndexOfRecentFile(fileName);
            if (alreadyIn > 0) // remove it
            {
                Settings.Default.RecentFiles.RemoveAt(alreadyIn);
                if(recentFileToolStripItem.DropDownItems.Count > alreadyIn)
                    recentFileToolStripItem.DropDownItems.RemoveAt(alreadyIn);
            }
            else if (alreadyIn == 0) // it´s the latest file so return
            {
                return;
            }

            // insert the file on top of the list
            Settings.Default.RecentFiles.Insert(0, fileName);
            recentFileToolStripItem.DropDownItems.Insert(0, new FileMenuItem(fileName));

            // remove the last one, if max size is reached
            if (Settings.Default.RecentFiles.Count > MaxRecentFiles)
                Settings.Default.RecentFiles.RemoveAt(MaxRecentFiles);
// throws exception! (Scott Swift 3/13/16)
//            if (Settings.Default.RecentFiles.Count > Settings.Default.RecentFilesMax)
//                this.recentFileToolStripItem.DropDownItems.RemoveAt(Settings.Default.RecentFilesMax);
            if (this.recentFileToolStripItem.DropDownItems.Count > Settings.Default.RecentFilesMax)
                this.recentFileToolStripItem.DropDownItems.RemoveAt(Settings.Default.RecentFilesMax);

            // enable the menu item if it´s disabled
            if (!recentFileToolStripItem.Enabled)
                recentFileToolStripItem.Enabled = true;

            // save the changes
            Settings.Default.Save();
        }

        int GetIndexOfRecentFile(string filename)
        {
            for (int i = 0; i < Settings.Default.RecentFiles.Count; i++)
            {
                string currentFile = Settings.Default.RecentFiles[i];
                if (string.Equals(currentFile, filename, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        ToolStripMenuItem recentFileToolStripItem;

        public ToolStripMenuItem RecentFileToolStripItem
        {
            get { return recentFileToolStripItem; }
            set 
            {
                if (recentFileToolStripItem == value)
                    return;

                recentFileToolStripItem = value;

                ReCreateItems();
            }
        }

        void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RecentFilesMax")
            {
                ReCreateItems();
            }
        }

        void ReCreateItems()
        {
            if (recentFileToolStripItem == null)
                return;

            if (Settings.Default.RecentFiles == null)
                Settings.Default.RecentFiles = new StringCollection();

            recentFileToolStripItem.DropDownItems.Clear();
            recentFileToolStripItem.Enabled = (Settings.Default.RecentFiles.Count > 0);

            int fileItemCount = Math.Min(Settings.Default.RecentFilesMax, Settings.Default.RecentFiles.Count);
            for(int i = 0; i < fileItemCount; i++)
            {
                string file = Settings.Default.RecentFiles[i];
                recentFileToolStripItem.DropDownItems.Add(new FileMenuItem(file));
            }
        }

        public void Clear()
        {
            Settings.Default.RecentFiles.Clear();
            ReCreateItems();
        }
    }
}
