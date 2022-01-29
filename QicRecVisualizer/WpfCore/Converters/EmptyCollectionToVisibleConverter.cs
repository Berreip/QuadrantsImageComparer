﻿using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace QicRecVisualizer.WpfCore.Converters
{
    /// <summary>
    /// Rend un élément visible si la liste bindée est vide
    /// </summary>
    public sealed class EmptyCollectionToVisibleConverter : IValueConverter
    {
        /// <summary>
        /// Fait la conversion
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return Visibility.Collapsed; // cachée si null
                case ListCollectionView collectionView:
                    return collectionView.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                case ICollection collection:
                    return collection.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        /// <summary>
        /// Fait la conversion inverse
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}