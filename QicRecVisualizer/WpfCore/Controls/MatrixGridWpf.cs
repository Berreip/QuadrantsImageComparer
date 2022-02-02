using System.Windows;
using System.Windows.Controls;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

namespace QicRecVisualizer.WpfCore.Controls
{
    public sealed class MatrixGridWpf : Grid
    {
        /* USED LIKE THAT:
         * <ItemsControl Grid.Row="1" ItemsSource="{Binding MatrixCells, Mode=OneTime}" 
                                                              HorizontalAlignment="Stretch" VerticalAlignment="Stretch" >
                                                    <ItemsControl.ItemsPanel>
                                                        <ItemsPanelTemplate>
                                                            <controls:MatrixGridWpf MatrixInfo="{Binding CurrentMatrixInfo, Mode=OneTime}"/>
                                                        </ItemsPanelTemplate>
                                                    </ItemsControl.ItemsPanel>
                                                    <ItemsControl.ItemContainerStyle>
                                                        <Style >
                                                            <Setter Property="Grid.Row" Value="{Binding RowId, Mode=OneTime}"/>
                                                            <Setter Property="Grid.Column" Value="{Binding ColumnId, Mode=OneTime}"/>
                                                        </Style>
                                                    </ItemsControl.ItemContainerStyle>

                                                    <ItemsControl.ItemTemplate>
                                                        <DataTemplate DataType="{x:Type adapters:CellAdapter}">
                                                            <Rectangle ToolTip="{Binding CurrentCellValue}" Fill="{Binding CellColor, Mode=OneWay}"
                                                                       Stretch="Fill">
                                                            </Rectangle>
                                                        </DataTemplate>
                                                    </ItemsControl.ItemTemplate>
                                                </ItemsControl>
         */
        
        static MatrixGridWpf()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MatrixGridWpf), new FrameworkPropertyMetadata(typeof(MatrixGridWpf)));
        }

        public static readonly DependencyProperty MatrixInfoProperty = DependencyProperty.Register(
            "MatrixInfo", typeof(IMatrixInfo), typeof(MatrixGridWpf), new PropertyMetadata(default(IMatrixInfo), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var wpfGrid = dependencyObject as MatrixGridWpf;
            wpfGrid?.Refresh();
        }

        private void Refresh()
        {
            if (MatrixInfo == null)
            {
                return;
            }
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            for (var i = 0; i < MatrixInfo.RowsCount; i++)
            {
                RowDefinitions.Add(new RowDefinition());
            }
            for (var i = 0; i < MatrixInfo.ColumnsCount; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        public IMatrixInfo MatrixInfo
        {
            get => (IMatrixInfo)GetValue(MatrixInfoProperty);
            set => SetValue(MatrixInfoProperty, value);
        }
    }
    
    public interface IMatrixInfo
    {
        /// <summary>
        /// row count
        /// </summary>
        int RowsCount { get; }
        
        /// <summary>
        /// columns count
        /// </summary>
        int ColumnsCount { get; }

    }

    public sealed class MatrixInfo : IMatrixInfo
    {
        /// <inheritdoc />
        public int RowsCount { get; }

        /// <inheritdoc />
        public int ColumnsCount { get; }

        public MatrixInfo(int rowsCount, int columnsCount)
        {
            ColumnsCount = columnsCount;
            RowsCount = rowsCount;
        }
    }
}