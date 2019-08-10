using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using OxyPlot.Series;

namespace Linear_equation_solver
{
    public partial class Graph : Form
    {
        public Graph(string first_equation_string, string second_equation_string, float[] first_equation, float[] second_equation)
        {
            InitializeComponent();

            float first_x_intercept, second_x_intercept, first_y_intercept, second_y_intercept, first_slope, second_slope, max_value_x, max_value_y;

            first_x_intercept = first_equation[2] / first_equation[0];// x intercept of the first equation
            first_y_intercept = first_equation[2] / first_equation[1];// y intercept of the first equation

            second_x_intercept = second_equation[2] / second_equation[0];// x intercept of the second equation
            second_y_intercept = second_equation[2] / second_equation[1];// y intercept of the second equation

            first_slope = - first_equation[0] / first_equation[1];// slope of the first equation
            second_slope = -second_equation[0] / second_equation[1];// slope of the second equation

            if(first_x_intercept > second_x_intercept)
                max_value_x = first_x_intercept;
            else
                max_value_x = second_x_intercept;

            if (first_y_intercept > second_y_intercept)
                max_value_y = first_y_intercept;
            else
                max_value_y = second_y_intercept;

            // Create and tweak a PlotView object
            PlotView myPlot = new PlotView();
            myPlot.Location = new System.Drawing.Point(0, 0);
            myPlot.Width = 700;
            myPlot.Height = 700;

            // Create and tweak the PlotModel object
            var myModel = new PlotModel { Title = "Function graph" };
            myModel.PlotType = PlotType.XY;
            myModel.Background = OxyColor.FromRgb(255, 255, 255);
            myModel.TextColor = OxyColor.FromRgb(0, 0, 0);

            // Assign PlotModel to PlotView
            myPlot.Model = myModel;

            //Add plot control to form
            Controls.Add(myPlot);

            // Create equation line series
            var first_equation_line = new LineSeries { Title = first_equation_string, StrokeThickness = 1, Color = OxyColor.FromRgb(255, 0, 0)};
            first_equation_line.Points.Add(new DataPoint(first_x_intercept, 0));
            first_equation_line.Points.Add(new DataPoint(first_x_intercept + 1000, first_slope * 1000));

            first_equation_line.Points.Add(new DataPoint(0, first_y_intercept));
            first_equation_line.Points.Add(new DataPoint(-1000, first_y_intercept - first_slope * 1000));
            

            var second_equation_line = new LineSeries { Title = second_equation_string, StrokeThickness = 1, Color = OxyColor.FromRgb(0, 255, 0)};
            second_equation_line.Points.Add(new DataPoint(second_x_intercept, 0));
            second_equation_line.Points.Add(new DataPoint(second_x_intercept + 1000, second_slope * 1000));

            second_equation_line.Points.Add(new DataPoint(0, second_y_intercept));
            second_equation_line.Points.Add(new DataPoint(-1000, second_y_intercept - second_slope * 1000));

            // Add Series and Axis to plot model
            myModel.Series.Add(first_equation_line);
            myModel.Series.Add(second_equation_line);
            myModel.Axes.Add(
                new LinearAxis 
                { 
                    Position = AxisPosition.Bottom, 
                    PositionAtZeroCrossing = true,
                    Minimum = (int) - max_value_x + 3,
                    Maximum = (int) max_value_x + 3, 
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Crossing,
                    MajorStep = 1
                });

            myModel.Axes.Add(
                new LinearAxis 
                { 
                    Position = AxisPosition.Left, 
                    PositionAtZeroCrossing = true,
                    Minimum = (int) - max_value_y + 3,
                    Maximum = (int) max_value_y + 3, 
                    AxislineStyle = LineStyle.Solid,
                    TickStyle = OxyPlot.Axes.TickStyle.Crossing,
                    MajorStep = 1
                });

        

        }
        }
}
