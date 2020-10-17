using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Covid19Analysis.Model;
using Covid19Analysis.View;
using System.Linq;
using Covid19Analysis.IO;
using Covid19Analysis.Utils;

namespace Covid19Analysis
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const int ApplicationHeight = 450;

        /// <summary>
        ///     The application width
        /// </summary>
        public const int ApplicationWidth = 625;

        private int upperBoundInput;
        private int lowerBoundInput;
        private int histogramBinSizeInput;
        private IList<CovidStatisticsError> errorsFound = new List<CovidStatisticsError>();
        private IList<CovidStatistic> currentStatistics = new List<CovidStatistic>();
        private IList<CovidStatistic> incomingStatistics = new List<CovidStatistic>();
        private IList<CovidStatistic> duplicatesFound = new List<CovidStatistic>();


        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Size {Width = ApplicationWidth, Height = ApplicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
            this.upperBoundInput = int.Parse(this.upperBoundTextBox.Text);
            this.lowerBoundInput = int.Parse(this.lowerBoundTextBox.Text);
            this.histogramBinSizeInput = int.Parse(this.histogramBinSize.Text);
        }

        #endregion

        private async void loadFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary, ViewMode = PickerViewMode.Thumbnail
            };
            openPicker.FileTypeFilter.Add(".csv");
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            string text = await FileIO.ReadTextAsync(file);
            if (this.summaryTextBox.Text == "Summary")
            {
                CovidCsvParser parser = new CovidCsvParser();
                this.currentStatistics = parser.ParseText(text).ToList();
                this.errorsFound = parser.Errors.ToList();
                this.setCovidDataToSummaryBox();
            }
            else
            {
                this.incomingStatistics.Clear();
                this.duplicatesFound.Clear();
                CovidCsvParser parser = new CovidCsvParser();
                this.incomingStatistics = parser.ParseText(text).ToList();
                this.errorsFound = parser.Errors.ToList();
                this.displayMergeOption();
            }
        }

        private void setCovidDataToSummaryBox()
        {
            CovidStatisticsReportBuilder stringBuilder = new CovidStatisticsReportBuilder(this.currentStatistics);
            stringBuilder.SetUpperAndLowerBoundLimits(this.upperBoundInput, this.lowerBoundInput);
            stringBuilder.SetHistogramBinSize(this.histogramBinSizeInput);
            this.summaryTextBox.Text = "Summary" + Environment.NewLine + stringBuilder.BuildStringForOutput();
        }

        private async void displayMergeOption()
        {
            MergeContentDialog mergeDialog = new MergeContentDialog();
            var result = await mergeDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                this.findAllDuplicates();
                this.displayApplyToAllDialog();
            }
            else
            {
                this.currentStatistics = this.incomingStatistics;
                this.setCovidDataToSummaryBox();
            }
        }

        private void findAllDuplicates()
        {
            var duplicates = this.incomingStatistics.Where(existingStatistic =>
                this.incomingStatistics.ToList().Exists(newStatistic => existingStatistic.Date == newStatistic.Date)).ToList();
            this.duplicatesFound = duplicates;
        }

        private async void displayApplyToAllDialog()
        {
            var toRemoveFromExisting = new List<CovidStatistic>();
            var toRemoveFromIncoming = new List<CovidStatistic>();
            var duplicatesHandled = new List<CovidStatistic>();
            foreach (var duplicate in this.duplicatesFound)
            {
                var keepOrReplaceDialog = new KeepOrReplaceContentDialog();
                var result = await keepOrReplaceDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    if (keepOrReplaceDialog.CheckBoxStatus)
                    {
                        var remainingDuplicates = this.duplicatesFound.Except(duplicatesHandled);
                        foreach (var remainingDuplicate in remainingDuplicates)
                        {
                            toRemoveFromIncoming.Add(remainingDuplicate);
                        }
                        break;
                    }
                    toRemoveFromIncoming.Add(duplicate);
                    duplicatesHandled.Add(duplicate);
                }
                else
                {
                    if (keepOrReplaceDialog.CheckBoxStatus)
                    {
                        var remainingDuplicates = this.duplicatesFound.Except(duplicatesHandled);
                        foreach (var remainingDuplicate in remainingDuplicates)
                        {
                            toRemoveFromExisting.Add(this.findDuplicateInCurrentStatistics(remainingDuplicate));
                        }
                        break;
                    }
                    toRemoveFromExisting.Add(this.findDuplicateInCurrentStatistics(duplicate));
                    duplicatesHandled.Add(duplicate);
                }
            }
            this.currentStatistics = this.currentStatistics.Except(toRemoveFromExisting).ToList();
            this.incomingStatistics = this.incomingStatistics.Except(toRemoveFromIncoming).ToList();
            var combined = this.currentStatistics.Concat(this.incomingStatistics);
            this.currentStatistics = combined.ToList();
            this.incomingStatistics.Clear();
            this.setCovidDataToSummaryBox();
        }

        private CovidStatistic findDuplicateInCurrentStatistics(CovidStatistic duplicate)
        {
            return this.currentStatistics.ToList().Find(statistic => statistic.Date == duplicate.Date);
        }

        private void upperBound_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.upperBoundTextBox.Text == string.Empty)
            {
                this.upperBoundInput = 0;
                this.setCovidDataToSummaryBox();
            }
            else
            {
                this.upperBoundInput = int.Parse(this.upperBoundTextBox.Text);
                this.setCovidDataToSummaryBox();
            }
        }

        private void lowerBound_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.lowerBoundTextBox.Text == string.Empty)
            {
                this.lowerBoundInput = 0;
                this.setCovidDataToSummaryBox();
            }
            else
            {
                this.lowerBoundInput = int.Parse(this.lowerBoundTextBox.Text);
                this.setCovidDataToSummaryBox();
            }
        }

        private void clear_Data_Click(object sender, RoutedEventArgs e)
        {
            this.duplicatesFound.Clear();
            this.incomingStatistics.Clear();
            this.currentStatistics.Clear();
            this.summaryTextBox.Text = "Summary";
        }

        private async void view_Errors_Click(object sender, RoutedEventArgs e)
        {
            string content = this.ErrorsToString();
            ContentDialog errorsFound = new ContentDialog() {
                Title = "Errors in file most recently added:",
                Content = content,
                CloseButtonText = "Close"
            };
            await errorsFound.ShowAsync();
        }

        /// <summary>
        /// Errors to string.
        /// </summary>
        /// <returns>he list of errors in string format</returns>
        public string ErrorsToString()
        {
            var output = string.Empty;
            if (this.errorsFound.Count == 0)
            {
                output += "No errors found!";
            }
            else
            {
                output = this.errorsFound.Aggregate(output,
                    (current, currentError) => current + ("Line " + currentError.LineErrorWasFound + ": " +
                                                          string.Join(",", currentError.ErrorLineContents) +
                                                          Environment.NewLine));
            }

            return output;
        }

        private void histogramBinSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.histogramBinSize.Text == string.Empty || int.Parse(this.histogramBinSize.Text) < 5)
            {
                this.histogramBinSize.Text = "5";
                this.histogramBinSizeInput = 5;
                this.setCovidDataToSummaryBox();
            }
            else
            {
                this.histogramBinSizeInput = int.Parse(this.histogramBinSize.Text);
                this.setCovidDataToSummaryBox();
            }
        }

        private async void add_Statistic_Click(object sender, RoutedEventArgs e)
        {
            var addStatisticContentDialog = new AddStatisticContentDialog();
            var result = await addStatisticContentDialog.ShowAsync();
           
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    var date = DateTime.ParseExact(addStatisticContentDialog.dateTextBox.Text, "yyyyMMdd", null);
                    var positiveIncrease = int.Parse(addStatisticContentDialog.positiveIncreaseTextBox.Text);
                    var negativeIncrease = int.Parse(addStatisticContentDialog.negativeIncreaseTextBox.Text);
                    var deaths = int.Parse(addStatisticContentDialog.deathsTextBox.Text);
                    var hospitalizations = int.Parse(addStatisticContentDialog.hospitalizationsTextBox.Text);
                    var statistic = new CovidStatistic(date, positiveIncrease, negativeIncrease, deaths,
                        hospitalizations);
                    if (positiveIncrease < 0 || negativeIncrease < 0 || deaths < 0 || hospitalizations < 0)
                    {
                        throw new Exception("Input cannot be negative");
                    }
                    
                    var foundStatistic = this.findDuplicateInCurrentStatistics(statistic);
                    if (foundStatistic != null)
                    {
                        this.incomingStatistics.Add(statistic);
                        this.displayMergeOption();
                    }
                    else
                    {
                        this.currentStatistics.Add(statistic);
                        this.setCovidDataToSummaryBox();

                    }
                }
                catch (Exception)
                {
                    ContentDialog error = new ContentDialog()
                    {
                        Title = "Error: Could Not Add Statistic to List",
                        Content = "Ensure that all values entered are \nnumbers greater than zero.",
                        CloseButtonText = "Close"
                    };
                    await error.ShowAsync();
                }
                
            }
        }

        private async void saveFile_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker {SuggestedStartLocation = PickerLocationId.DocumentsLibrary};
            savePicker.FileTypeChoices.Add("Comma Separated Values File", new List<string>() { ".csv" });
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                CovidCsvSaver statisticsWriter = new CovidCsvSaver(this.currentStatistics);
                    await FileIO.WriteTextAsync(file, statisticsWriter.BuildCsvFileContents());
            }
        }
    }
}