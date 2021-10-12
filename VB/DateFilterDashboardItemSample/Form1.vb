Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWin.Localization
Imports DevExpress.DataAccess.Excel
Imports DevExpress.XtraEditors
Imports System

Namespace DateFilterDashboardItemSample

    Public Partial Class Form1
        Inherits XtraForm

        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim excelDataSource As DashboardExcelDataSource = Me.CreateExcelDataSource()
            Dim dBoard As Dashboard = New Dashboard()
            dBoard.DataSources.Add(excelDataSource)
            dBoard.BeginUpdate()
            ' Create dashboard items.
            Dim chart As ChartDashboardItem = Me.CreateChart(excelDataSource)
            dBoard.Items.Add(chart)
            Dim dateFilterItem As DateFilterDashboardItem = Me.CreateDateFilterItem(excelDataSource)
            dBoard.Items.Add(dateFilterItem)
            Dim group As DashboardItemGroup = Me.CreateGroup()
            dBoard.Groups.Add(group)
            group.AddRange(dateFilterItem, chart)
            ' Create the layout tree.
            Dim dateFilterLayoutItem As DashboardLayoutItem = New DashboardLayoutItem(dateFilterItem, 30)
            Dim chartLayoutItem As DashboardLayoutItem = New DashboardLayoutItem(chart, 70)
            Dim groupLayoutItem As DashboardLayoutGroup = New DashboardLayoutGroup(group, 100)
            groupLayoutItem.ChildNodes.AddRange(dateFilterLayoutItem, chartLayoutItem)
            Dim rootGroup As DashboardLayoutGroup = New DashboardLayoutGroup(Nothing, 100)
            rootGroup.ChildNodes.Add(groupLayoutItem)
            rootGroup.Orientation = DashboardLayoutGroupOrientation.Vertical
            dBoard.LayoutRoot = rootGroup
            dBoard.EndUpdate()
            Me.dashboardViewer1.Dashboard = dBoard
        End Sub

        Private Function CreateGroup() As DashboardItemGroup
            Dim group As DashboardItemGroup = New DashboardItemGroup()
            group.Name = "Sales by Date"
            Return group
        End Function

        Private Function CreateDateFilterItem(ByVal excelDataSource As DashboardExcelDataSource) As DateFilterDashboardItem
            Dim dateFilter As DateFilterDashboardItem = New DateFilterDashboardItem()
            dateFilter.Name = String.Empty
            dateFilter.ShowCaption = False
            dateFilter.DataSource = excelDataSource
            dateFilter.Dimension = New Dimension("orderDateId", "OrderDate", DateTimeGroupInterval.DayMonthYear)
            dateFilter.Dimension.DateTimeFormat.DateTimeFormat = DateTimeFormat.[Short]
            dateFilter.ArrangementMode = DateFilterArrangementMode.Vertical
            dateFilter.FilterType = DateFilterType.Between
            dateFilter.DatePickerLocation = DatePickerLocation.Far
            dateFilter.DateTimePeriods.AddRange(DateTimePeriod.CreateLastYear(), DateTimePeriod.CreateNextDays("Next 7 Days", 7), New DateTimePeriod With {.Name = DashboardWinLocalizer.GetString(DashboardWinStringId.PeriodMonthToDate), .Start = New FlowDateTimePeriodLimit With {.Interval = DateTimeInterval.Month, .Offset = 0}, .[End] = New FlowDateTimePeriodLimit With {.Interval = DateTimeInterval.Day, .Offset = 1}}, New DateTimePeriod With {.Name = "Jul-18-2018 - Jan-18-2019", .Start = New FixedDateTimePeriodLimit With {.[Date] = New System.DateTime(2018, 7, 18)}, .[End] = New FixedDateTimePeriodLimit With {.[Date] = New System.DateTime(2019, 1, 18)}})
            Return dateFilter
        End Function

        Private Function CreateChart(ByVal excelDataSource As DashboardExcelDataSource) As ChartDashboardItem
            Dim chart As ChartDashboardItem = New ChartDashboardItem()
            chart.Name = String.Empty
            chart.ShowCaption = False
            chart.DataSource = excelDataSource
            chart.Arguments.Add(New Dimension("OrderDate", DateTimeGroupInterval.DayMonthYear))
            chart.Panes.Add(New ChartPane())
            Dim salesAmountSeries As SimpleSeries = New SimpleSeries(SimpleSeriesType.Line)
            salesAmountSeries.Value = New Measure("Extended Price")
            chart.Panes(CInt((0))).Series.Add(salesAmountSeries)
            Return chart
        End Function

        Private Function CreateExcelDataSource() As DashboardExcelDataSource
            Dim excelDataSource As DashboardExcelDataSource = New DashboardExcelDataSource()
            excelDataSource.FileName = "SalesPerson.xlsx"
            Dim worksheetSettings As ExcelWorksheetSettings = New ExcelWorksheetSettings("Data")
            excelDataSource.SourceOptions = New ExcelSourceOptions(worksheetSettings)
            excelDataSource.Fill()
            Return excelDataSource
        End Function
    End Class
End Namespace
