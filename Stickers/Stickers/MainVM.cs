using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stickers
{
    class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string docText { get; set; }
        public int fontSize { get; set; }
        

        public string DocText
        {
            get { return docText; }
            set
            {
                docText = value;
                OnPropertyChanged("DocText");
            }
        }

        public int FontSize
        {
            get { return fontSize; }
            set
            {
                fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        Cmd openFileCmd;
        Cmd windowLoaded;
        Cmd setFontSize;
        Cmd incrFontSize;
        Cmd decrFontSize;

        public Cmd OpenFileCmd
        {
            get
            {
                return openFileCmd ?? (openFileCmd = new Cmd(obj =>
                {
                    OpenFileDialog opf = new OpenFileDialog
                    {
                        DefaultExt = ".mp3",
                        Multiselect = false,
                        Filter = "Текст|*.txt;"
                    };
                    bool? result = opf.ShowDialog();
                    if (result == true)
                    {
                        Properties.Settings.Default.lastFile = opf.FileName;
                        Properties.Settings.Default.Save();
                        DocText = File.ReadAllText(opf.FileName);
                    }
                }));
            }
        }

        public Cmd WindowLoaded
        {
            get
            {
                return windowLoaded ?? (windowLoaded = new Cmd(obj =>
                {
                    if (Properties.Settings.Default.lastFile != "")
                    {
                        string f = Properties.Settings.Default.lastFile;
                        if (File.Exists(f))
                        {
                            DocText = File.ReadAllText(f);
                        }
                    }
                }));
            }
        }

        public Cmd SetFontSize
        {
            get
            {
                return windowLoaded ?? (windowLoaded = new Cmd(obj =>
                {
                    
                }));
            }
        }

        public Cmd IncrFontSize
        {
            get
            {
                return incrFontSize ?? (incrFontSize = new Cmd(obj =>
                {
                    FontSize++;
                    Properties.Settings.Default.fontSize = FontSize;
                    Properties.Settings.Default.Save();
                }));
            }
        }

        public Cmd DecrFontSize
        {
            get
            {
                return decrFontSize ?? (decrFontSize = new Cmd(obj =>
                {
                    if(FontSize>1)
                        FontSize--;
                    Properties.Settings.Default.fontSize = FontSize;
                    Properties.Settings.Default.Save();
                }));
            }
        }

        public MainVM()
        {
            if (Properties.Settings.Default.lastFile != "")
            {
                FontSize = Properties.Settings.Default.fontSize;
                string f = Properties.Settings.Default.lastFile;
                if (File.Exists(f))
                {
                    DocText = File.ReadAllText(f);
                }
            }
        }

    }
}
