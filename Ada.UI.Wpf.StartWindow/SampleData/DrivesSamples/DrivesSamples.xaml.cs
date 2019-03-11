﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.

namespace Expression.Blend.SampleData.DrivesSamples
{
    #region Namespace Using

    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    #endregion

// To significantly reduce the sample data footprint in your production application, you can set
// the DISABLE_SAMPLE_DATA conditional compilation constant and disable sample data at runtime.
#if DISABLE_SAMPLE_DATA
    internal class DrivesSamples { }
#else

    public class DrivesSamples : INotifyPropertyChanged
    {
        #region  Fields

        #endregion

        #region  Constructors

        public DrivesSamples()
        {
            try
            {
                var resourceUri = new Uri("/Ada.UI.Wpf.StartWindow;component/SampleData/DrivesSamples/DrivesSamples.xaml", UriKind.RelativeOrAbsolute);
                Application.LoadComponent(this, resourceUri);
            }
            catch
            {
            }
        }

        #endregion

        #region Properties

        public Drives Drives { get; } = new Drives();

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class DrivesItem : INotifyPropertyChanged
    {
        #region  Fields

        private string _Drive = string.Empty;

        private bool _Status;

        #endregion

        #region Properties

        public string Drive
        {
            get => _Drive;

            set
            {
                if (_Drive != value)
                {
                    _Drive = value;
                    OnPropertyChanged("Drive");
                }
            }
        }

        public bool Status
        {
            get => _Status;

            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    OnPropertyChanged("Status");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class Drives : ObservableCollection<DrivesItem>
    {
    }
#endif
}