﻿using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace MiraiNavi.WpfApp.Controls;

/// <summary>
/// IconText.xaml 的交互逻辑
/// </summary>
public partial class IconText : TextBlock, IIconControl
{
    public IconText()
    {
        InitializeComponent();
    }



    public bool IconFilled
    {
        get { return (bool)GetValue(IconFilledProperty); }
        set { SetValue(IconFilledProperty, value); }
    }

    public static readonly DependencyProperty IconFilledProperty =
        DependencyProperty.Register("IconFilled", typeof(bool), typeof(IconText), new PropertyMetadata(0));

    public SymbolRegular Icon
    {
        get { return (SymbolRegular)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register("Icon", typeof(SymbolRegular), typeof(IconText), new PropertyMetadata(SymbolRegular.Empty));







}
