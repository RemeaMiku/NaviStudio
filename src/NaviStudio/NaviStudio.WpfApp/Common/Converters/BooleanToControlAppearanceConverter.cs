﻿using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Common;

namespace NaviStudio.WpfApp.Common.Converters;

[ValueConversion(typeof(bool), typeof(ControlAppearance))]
public class BooleanToControlAppearanceConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool flag
            ? (object)(flag ? (ControlAppearance)parameter : ControlAppearance.Secondary)
            : throw new ArgumentException("value is not bool");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}